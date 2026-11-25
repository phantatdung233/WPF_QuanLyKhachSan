# [TASK-07] Module Đặt phòng (Booking) & Thuật toán chống trùng lịch

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-07`  
**GitHub Issue**: [#62](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/62)  
**Thành viên đảm nhiệm**: **Member B (@xuanduong29)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/booking-conflict-check`  
**Milestone**: Mốc 2: Nghiệp Vụ Cốt Lõi, Sơ Đồ Phòng & Hóa Đơn (05/12 – 18/12)  
**Khung thời gian dự kiến**: `05/12/2026 – 10/12/2026`  
**Labels**: `booking`, `business-logic`, `m2`, `ready-for-agent`  
**Blocked by**: #78 (TASK-05), #61 (TASK-04)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng `BookingDAO.cs` trong `DAO/` và lớp logic thuần `BookingRules.cs` trong `Services/` (seam kiểm thử tự động TDD).
- Hoàn thiện màn hình danh sách `BookingView.xaml` + hộp thoại `BookingDialog.xaml` và `BookingViewModel.cs` theo quy trình **2 Pha** (`CLAUDE.md`) và chuẩn `DESIGN.md`.
- Cho phép tạo phiếu đặt phòng mới (chọn Khách hàng, chọn Phòng hoạt động, chọn Ngày nhận `CheckInDate` & Ngày trả `CheckOutDate`), xem danh sách booking theo 4 trạng thái (`Đã đặt`, `Đang ở`, `Hoàn thành`, `Đã hủy`) và hủy phiếu đang ở trạng thái `Đã đặt`.
- Áp dụng nghiêm ngặt **Overlap Conflict Rule** trong `GLOSSARY.md` để ngăn chặn tuyệt đối trùng lịch cùng một phòng.

---

## 2. Hướng dẫn nghiệp vụ & UI Contract (Theo `DESIGN.md`)
- **Seam `Services/BookingRules.cs` & `BookingDAO.cs` (Pha 1 — Logic & State First, TDD-first)**:
  - Hàm kiểm tra khoảng ngày hợp lệ: `CheckOutDate.Date > CheckInDate.Date` (nếu `<=` trả về lỗi: `"Ngày trả phòng phải sau ngày nhận phòng ít nhất 1 ngày"`).
  - Cảnh báo không chặn khi `CheckInDate.Date < DateTime.Today`.
  - Hàm kiểm tra xung đột lịch theo **Overlap Conflict Rule** (`GLOSSARY.md`):
    Với mọi phiếu đặt hiện có của cùng `RoomId` đang ở trạng thái hoạt động (`Đã đặt` hoặc `Đang ở`), xung đột xảy ra khi và chỉ khi:
    `NewCheckIn.Date < ExistingCheckOut.Date && ExistingCheckIn.Date < NewCheckOut.Date`
  - Tính tạm tính tiền phòng ngay trên form: `EstimatedNights = (CheckOutDate.Date - CheckInDate.Date).Days`, `EstimatedRoomCharge = EstimatedNights * PricePerDay`.
  - Truy vấn kiểm tra trên SQL Server (`BookingDAO.cs`):
    ```sql
    SELECT BookingId, CheckInDate, CheckOutDate
    FROM Bookings
    WHERE RoomId = @roomId
      AND Status IN (N'Đã đặt', N'Đang ở')
      AND (@newCheckIn < CheckOutDate AND CheckInDate < @newCheckOut)
    ```
  - Nếu hợp lệ (`0` xung đột): thêm bản ghi `Bookings` với `Status = N'Đã đặt'`; nếu phòng đang `Trống`, cập nhật `Rooms.Status = N'Đã đặt'`.
  - **Hủy đặt phòng (`Cancelled`)**: chỉ cho phép khi `Booking.Status == "Đã đặt"` → chuyển `Bookings.Status = N'Đã hủy'`, và nếu phòng không còn booking hoạt động nào khác thì trả `Rooms.Status` về `N'Trống'`.
- **UI Contract (`BookingView` & `BookingDialog` — Pha 2)**:
  - **Dữ liệu hiển thị**:
    - `BookingView` (màn chính khi bấm Sidebar `Đặt phòng`): bộ lọc Trạng thái & Phòng, bảng danh sách gồm Mã BK, Khách hàng, Phòng, Khoảng ngày (`dd/MM/yyyy`), Badge `BookingStatus`, Cột Thao tác theo trạng thái.
    - `BookingDialog`: chọn/tìm Khách hàng, chọn Phòng (ẩn/khóa phòng `Bảo trì`), `CheckInDate`, `CheckOutDate`, khung Tạm tính (`Số đêm × Đơn giá = Tổng tạm tính` theo `#,##0 đ`).
  - **Primitives bắt buộc (`WPF-UI`)**: `ComboBox`, `ui:TextBox` (tìm khách), `DatePicker`, `DataGrid` (`RowHeight="40"`), modal `ui:FluentWindow` / `ui:ContentDialog`, `ui:InfoBar` (`Severity="Error"` cho trùng lịch, `Severity="Warning"` cho ngày quá khứ), `ui:ProgressRing`, tối đa **1 Primary CTA** `ui:Button` (`Đặt phòng` trên màn danh sách; `Xác nhận đặt phòng` trên dialog).
  - **4 Trạng thái UI bắt buộc**:
    1. *Loading*: Hiện `ui:ProgressRing` khi tải danh sách/kiểm tra trùng lịch và khóa nút `Xác nhận đặt phòng` khi đang ghi DB.
    2. *Empty*: `"Chưa có đặt phòng nào"` hoặc `"Không có đặt phòng phù hợp bộ lọc"` kèm nút `"Xóa bộ lọc"`.
    3. *Error*: Lỗi thiếu khách/phòng hoặc `CheckOutDate <= CheckInDate` hiện inline; lỗi trùng lịch hiển thị rõ qua `ui:InfoBar` (`Severity="Error"`): *"Phòng này đã có người đặt trong khoảng thời gian đã chọn ([CheckIn] – [CheckOut])! Vui lòng chọn khoảng ngày khác hoặc phòng khác."* Cảnh báo ngày quá khứ dùng `ui:InfoBar` (`Severity="Warning"`).
    4. *Success*: Cập nhật trực tiếp tiền tạm tính khi đổi ngày/phòng, lưu thành công cập nhật bảng và đổi trạng thái phòng sang `Đã đặt`.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Cài đặt `Services/BookingRules.cs` bằng `/tdd` với đầy đủ unit test trong `HotelManagement.Tests` cho mọi ca biên của **Overlap Conflict Rule**, lỗi ngày và tính tiền tạm tính.
- [ ] `BookingDAO.cs` truy vấn tham số hóa danh sách booking, tìm booking xung đột, tạo mới booking và hủy booking.
- [ ] `BookingDialog.xaml` có chọn/tìm nhanh Khách hàng, chọn Phòng (loại trừ phòng `Bảo trì`), chọn ngày nhận/trả, hiển thị **tiền phòng tạm tính**, cảnh báo vàng ngày quá khứ và banner lỗi đỏ chỉ rõ khoảng ngày phòng đang bận.
- [ ] `BookingView.xaml` hiển thị danh sách booking với lọc theo 4 trạng thái (`Đã đặt`, `Đang ở`, `Hoàn thành`, `Đã hủy`) và hỗ trợ mở sẵn phòng khi được điều hướng từ Sơ đồ phòng (`TASK-09`).

### Ca biên cần xử lý (Edge Cases):
- `CheckOutDate <= CheckInDate`: chặn lưu và báo lỗi `"Ngày trả phòng phải sau ngày nhận phòng ít nhất 1 ngày"`.
- Hai lượt khách nối tiếp liền kề (Khách 1 trả `15/10`, Khách 2 nhận `15/10`): hợp lệ theo công thức `<` nghiêm ngặt (`15/10 < 15/10` là `false`).
- Đơn cũ ở trạng thái `Đã hủy` hoặc `Hoàn thành` trên cùng khoảng ngày: **không** tính là xung đột, cho phép đặt mới bình thường.
- Phòng đang `Bảo trì`: không cho tạo phiếu đặt mới.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động (`TDD` seam): `dotnet test` kiểm chứng ít nhất 8 kịch bản lịch đặt (lồng nhau, giao đầu, giao đuôi, trùng khít, nối tiếp liền kề `15/10`, đơn đã hủy, đơn hoàn thành, ngày trả `<=` ngày nhận).
- Trực quan: Phòng 103 đang có lịch đặt `15/12 – 18/12`; thử đặt `16/12 – 17/12` thấy ngay banner lỗi đỏ chỉ rõ khoảng bận; đổi sang `18/12 – 20/12` lưu thành công.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-07.md` lên GitHub Issue #62.*
