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
- Hoàn thiện màn hình `BookingView.xaml` (`BookingList` + `BookingDialog.xaml`) và `BookingViewModel.cs` bám sát quy chuẩn trạng thái tại `docs/design/DESIGN-BRIEF.md` mục 6.4 & `WIREFRAMES.md` mục 8–9.
- Cho phép tạo phiếu đặt phòng mới (chọn Khách hàng, chọn Phòng hoạt động, chọn Ngày nhận `CheckInDate` & Ngày trả `CheckOutDate`), xem danh sách booking theo 4 trạng thái (`Đã đặt`, `Đang ở`, `Hoàn thành`, `Đã hủy`) và hủy phiếu đang ở trạng thái `Đã đặt`.
- Áp dụng nghiêm ngặt **Overlap Conflict Rule** trong `GLOSSARY.md` để ngăn chặn tuyệt đối trùng lịch cùng một phòng.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- **Seam `Services/BookingRules.cs` (TDD-first)**:
  - Hàm kiểm tra khoảng ngày hợp lệ: `CheckOutDate.Date > CheckInDate.Date` (nếu `<=` trả về lỗi: `"Ngày trả phòng phải sau ngày nhận phòng ít nhất 1 ngày"`).
  - Cảnh báo không chặn khi `CheckInDate.Date < DateTime.Today` (tông vàng nhắc lễ tân kiểm tra lại ngày).
  - Hàm kiểm tra xung đột lịch theo **Overlap Conflict Rule** (`GLOSSARY.md`):
    Với mọi phiếu đặt hiện có của cùng `RoomId` đang ở trạng thái hoạt động (`Đã đặt` hoặc `Đang ở`), xung đột xảy ra khi và chỉ khi:
    `NewCheckIn.Date < ExistingCheckOut.Date && ExistingCheckIn.Date < NewCheckOut.Date`
  - Tính tạm tính tiền phòng ngay trên form: `EstimatedNights = (CheckOutDate.Date - CheckInDate.Date).Days`, `EstimatedRoomCharge = EstimatedNights * PricePerDay`.
- **Truy vấn kiểm tra trên SQL Server (`BookingDAO.cs`)**:
  ```sql
  SELECT BookingId, CheckInDate, CheckOutDate
  FROM Bookings
  WHERE RoomId = @roomId
    AND Status IN (N'Đã đặt', N'Đang ở')
    AND (@newCheckIn < CheckOutDate AND CheckInDate < @newCheckOut)
  ```
  - Nếu có bản ghi trùng: hiển thị banner lỗi đỏ nổi bật ngay trong form (`DESIGN-BRIEF.md` mục 6.4):
    *"Phòng này đã có người đặt trong khoảng thời gian đã chọn ([CheckIn] – [CheckOut])! Vui lòng chọn khoảng ngày khác hoặc phòng khác."*
  - Nếu hợp lệ (`0` xung đột): thêm bản ghi `Bookings` với `Status = N'Đã đặt'`; nếu phòng đang ở trạng thái `Trống`, cập nhật `Rooms.Status = N'Đã đặt'`.
- **Hủy đặt phòng (`Cancelled`)**:
  - Chỉ cho phép hủy khi `Booking.Status == "Đã đặt"`: chuyển `Bookings.Status = N'Đã hủy'`, và nếu phòng không còn booking hoạt động nào khác thì trả `Rooms.Status` về `N'Trống'`.

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
