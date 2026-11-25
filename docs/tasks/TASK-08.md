# [TASK-08] Module Check-in, Check-out & Gọi dịch vụ phòng

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-08`  
**GitHub Issue**: [#80](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/80)  
**Thành viên đảm nhiệm**: **Member B (@xuanduong29)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/checkin-checkout-services`  
**Milestone**: Mốc 2: Nghiệp Vụ Cốt Lõi, Sơ Đồ Phòng & Hóa Đơn (05/12 – 18/12)  
**Khung thời gian dự kiến**: `11/12/2026 – 15/12/2026` *(bàn giao đúng hạn 15/12 để mở khóa `TASK-10`)*  
**Labels**: `booking`, `service`, `workflow`, `m2`, `ready-for-agent`  
**Blocked by**: #62 (TASK-07), #79 (TASK-06)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Mở rộng các thao tác chuyển trạng thái lưu trú đồng bộ giữa `BookingDAO.cs` và `RoomDAO.cs`.
- Xây dựng `BookingServiceDAO.cs` trong `DAO/` thao tác với bảng `BookingServices` (dùng model `BookingService.cs` từ `TASK-02`).
- Hoàn thiện hộp thoại `AddServiceDialog.xaml` và các thao tác vận hành lưu trú theo quy trình **2 Pha** (`CLAUDE.md`) và chuẩn `DESIGN.md`, tích hợp trên danh sách Booking / Sơ đồ phòng:
  1. **Check-in**: chuyển cả `Booking` và `Room` sang trạng thái `Đang ở`.
  2. **Gọi dịch vụ phòng**: ghi nhận dịch vụ phát sinh kèm số lượng và snapshot `UnitPrice` cho booking đang `Đang ở`, hỗ trợ xóa dòng nhập nhầm.
  3. **Check-out**: khởi tạo hóa đơn chờ thanh toán (nếu chưa có) và điều hướng sang màn hình Hóa đơn (`InvoiceViewModel` — `TASK-10`) với đúng `BookingId`.

---

## 2. Hướng dẫn nghiệp vụ & UI Contract (Theo `DESIGN.md`)
- **Logic lưu trú & Dịch vụ phòng (Pha 1 — Logic & State First)**:
  - **Check-in**: Áp dụng cho đơn `Đã đặt` (phòng không đang `Đang ở`), cập nhật đồng bộ trong `SqlTransaction`: `Bookings.Status = N'Đang ở'` và `Rooms.Status = N'Đang ở'`.
  - **Gọi dịch vụ phòng**: Chỉ mở cho booking đang `Đang ở`; validate `Quantity >= 1` (số nguyên), lưu `BookingServices (BookingId, ServiceId, Quantity, UnitPrice)` với snapshot `UnitPrice` tại thời điểm gọi; hỗ trợ xóa dòng và tính lại tổng tức thời.
  - **Khởi tạo Check-out**: Với booking `Đang ở`, tạo hóa đơn `Chưa thanh toán` (nếu chưa có) và chuyển `BookingId` sang `MainViewModel.NavigateToInvoice(bookingId)` (chưa giải phóng phòng cho đến khi thanh toán xong theo **Settlement Invariant** trong `GLOSSARY.md`).
- **UI Contract (`AddServiceDialog` & Booking Row Actions — Pha 2)**:
  - **Dữ liệu hiển thị**: Tiêu đề phòng & tên khách đang ở, bộ chọn Dịch vụ + Đơn giá (`#,##0 đ`), ô Số lượng, Thành tiền dòng tạm tính, bảng kê dịch vụ đã dùng (`Tên dịch vụ`, `SL`, `Đơn giá`, `Thành tiền`, nút `Xóa`) và `Tổng tiền dịch vụ` (`#,##0 đ`). Hành động trên dòng Booking biến thiên theo trạng thái (`Đã đặt` → Nhận phòng / Hủy; `Đang ở` → Dịch vụ / Trả phòng; `Hoàn thành` → Xem hóa đơn; `Đã hủy` → khóa thao tác).
  - **Primitives bắt buộc (`WPF-UI`)**: Modal `ui:FluentWindow` / `ui:ContentDialog`, `ComboBox`, `ui:NumberBox` / `ui:TextBox`, `DataGrid` (`RowHeight="40"`), `ui:InfoBar`, `ui:ProgressRing`, tối đa **1 Primary CTA** `ui:Button` (`Thêm vào` / `Xác nhận`).
  - **4 Trạng thái UI bắt buộc**:
    1. *Loading*: Hiện `ui:ProgressRing` và khóa nút khi đang thực thi `SqlTransaction` Check-in hoặc thêm/xóa dịch vụ.
    2. *Empty*: Khi phòng chưa dùng dịch vụ nào, hiển thị rõ `"Phòng này chưa sử dụng dịch vụ nào"` (tổng dịch vụ `0 đ`).
    3. *Error*: Số lượng `<= 0` hoặc sai định dạng hiện lỗi inline `"Số lượng phải là số nguyên lớn hơn 0"`; chặn Check-in đè khi phòng đang có khách khác lưu trú qua `ui:InfoBar` (`Severity="Error"`).
    4. *Success*: Bảng kê và tổng tiền dịch vụ cập nhật tức thì sau mỗi lần thêm/xóa; xác nhận Check-in / chuyển Check-out mượt sang `InvoiceView`.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Thao tác `Check-in` đổi đồng bộ `Bookings.Status = N'Đang ở'` và `Rooms.Status = N'Đang ở'` (an toàn giao dịch).
- [ ] `BookingServiceDAO.cs` hỗ trợ lấy bảng kê dịch vụ theo `BookingId` (JOIN tên dịch vụ), thêm dòng dịch vụ và xóa dòng nhập nhầm.
- [ ] `AddServiceDialog.xaml` validate inline `Quantity >= 1`, tự tính thành tiền từng dòng và tổng tiền dịch vụ toàn phòng theo thời gian thực.
- [ ] Thao tác `Check-out` truyền chính xác `BookingId` sang luồng Hóa đơn (`InvoiceViewModel`) mà chưa vội giải phóng phòng khi hóa đơn chưa `Đã thanh toán`.
- [ ] Có unit test trong `HotelManagement.Tests` cho quy tắc chuyển trạng thái hợp lệ (chỉ `Đã đặt` mới Check-in được; chỉ `Đang ở` mới gọi dịch vụ / Check-out được) và tính tổng tiền bảng kê `BookingServices`.

### Ca biên cần xử lý (Edge Cases):
- Nhập số lượng dịch vụ `<= 0` hoặc chữ thập phân -> chặn lưu và hiển thị lỗi inline ngay dưới ô số lượng.
- Không cho gọi dịch vụ vào phòng `Trống`, `Đã đặt` (chưa check-in) hoặc `Bảo trì`.
- Phòng đang có khách lưu trú khác chưa trả (`Rooms.Status == "Đang ở"`): chặn Check-in đè và thông báo rõ phòng chưa hoàn tất trả phòng.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động (`TDD` seam): `dotnet test` kiểm chứng máy trạng thái `BookingStatus` / `RoomStatus` và phép cộng bảng kê dịch vụ (thêm, xóa dòng, snapshot `UnitPrice`).
- Trực quan: Check-in một booking `Đã đặt` -> trạng thái đổi sang `Đang ở`; mở `AddServiceDialog` thêm 2 Nước suối Lavie (`10.000 đ`) + 1 Ăn sáng buffet (`50.000 đ`) -> tổng tiền dịch vụ hiển thị đúng `70.000 đ`.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-08.md` lên GitHub Issue #80.*
