# [TASK-07] Module Đặt phòng (Booking) & Thuật toán chống trùng lịch

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-07`  
**GitHub Issue**: [#62](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/62)  
**Thành viên đảm nhiệm**: **Member B (@phantatdung)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/booking-conflict-check`  
**Milestone**: Mốc 2: Nghiệp Vụ Cốt Lõi, Sơ Đồ Phòng & Hóa Đơn (Tuần 3-5)  
**Blocked by**: #78 (TASK-05), #61 (TASK-04)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng `BookingDAO.cs` trong thư mục `DAO/` và `Booking.cs` trong `Models/`.
- Xây dựng `BookingViewModel.cs` và giao diện `BookingView.xaml`.
- Quản lý tạo mới đơn đặt phòng: Cho phép chọn Khách hàng, chọn Phòng, chọn Ngày đến (Check-in) và Ngày đi (Check-out).
- Cài đặt thuật toán kiểm tra xung đột thời gian (Overlap check) để tuyệt đối không xảy ra tình trạng trùng lịch (overbooking) cùng một phòng.
- Lưu trữ phiếu đặt phòng vào bảng `Bookings` với trạng thái ban đầu là `Đã đặt` (Reserved).

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Thông tin `Booking`: `BookingId`, `CustomerId`, `RoomId`, `CheckInDate`, `CheckOutDate`, `Status` (Đã đặt, Đang ở, Hoàn thành, Đã hủy), `CreatedDate`.
- `BookingView.xaml`:
  - Chọn Khách hàng từ ComboBox (hoặc có ô tìm kiếm nhanh).
  - Chọn Phòng từ ComboBox (chỉ hiển thị các phòng đang hoạt động).
  - Chọn Ngày nhận (`DatePicker dpCheckIn`) và Ngày trả (`DatePicker dpCheckOut`).
- Thuật toán kiểm tra trùng lịch (Overlap Check):
  - Giả sử đơn đặt mới có khoảng thời gian `[NewCheckIn, NewCheckOut]`.
  - Một phòng bị trùng lịch nếu tồn tại đơn đặt cũ của phòng đó (có `Status` là `Đã đặt` hoặc `Đang ở`) thoả mãn điều kiện:
    **`NewCheckIn < ExistingCheckOut && ExistingCheckIn < NewCheckOut`**
  - Trong SQL Server, câu truy vấn kiểm tra:
    ```sql
    SELECT COUNT(*) FROM Bookings 
    WHERE RoomId = @roomId 
      AND Status IN (N'Đã đặt', N'Đang ở')
      AND (@newCheckIn < CheckOutDate AND CheckInDate < @newCheckOut)
    ```
  - Nếu kết quả trả về `> 0`: Thông báo lỗi "Phòng này đã có người đặt trong khoảng thời gian đã chọn! Vui lòng chọn khoảng ngày khác hoặc phòng khác."
  - Nếu kết quả `= 0`: Tiến hành tạo phiếu đặt phòng, cập nhật trạng thái phòng thành `Đã đặt`.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] BookingDAO.cs: Thao tác thêm, cập nhật, truy vấn bảng Bookings.
- [ ] BookingView.xaml: Giao diện tạo phiếu đặt phòng gồm: Chọn Khách hàng (từ ComboBox hoặc tìm kiếm), chọn Phòng, chọn Ngày nhận (CheckInDate) và Ngày trả (CheckOutDate).
- [ ] Cài đặt thuật toán kiểm tra xung đột thời gian: Hai khoảng thời gian [A, B] và [C, D] trùng nhau khi và chỉ khi A < D và C < B.
- [ ] Hiển thị thông báo lỗi rõ ràng nếu phòng đã có người đặt trong khoảng ngày đã chọn.
- [ ] Nếu hợp lệ, lưu phiếu vào bảng Bookings với trạng thái Đã đặt và cập nhật trạng thái phòng.

### Ca biên cần xử lý (Edge Cases):
- Ngày trả phòng nhỏ hơn hoặc bằng ngày nhận phòng (`CheckOutDate <= CheckInDate`): Validate báo lỗi "Ngày trả phòng phải sau ngày nhận phòng ít nhất 1 ngày".
- Ngày nhận phòng trong quá khứ so với ngày hiện tại: Hiển thị cảnh báo nhắc nhở lễ tân.
- Hai lượt khách liền kề nhau (Khách 1 trả ngày 15/10 lúc 12h, Khách 2 nhận ngày 15/10 lúc 14h): Thuật toán `A < D && C < B` cho phép đặt hợp lệ nếu tính theo mốc ngày liền kề.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Phòng 101 đã có khách đặt từ 15/10 đến 18/10.
- Thử đặt phòng 101 từ 16/10 đến 17/10: Hệ thống lập tức báo lỗi trùng lịch.
- Thử đặt phòng 101 từ 19/10 đến 21/10: Hệ thống cho phép đặt thành công.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-07.md` lên GitHub Issue #62.*
