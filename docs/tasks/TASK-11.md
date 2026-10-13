# [TASK-11] Module Đặt phòng (Booking) & Thuật toán kiểm tra trùng lịch

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-11`  
**GitHub Issue**: [#64](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/64)  
**Thành viên đảm nhiệm**: **Member B (@xuanduong29)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/booking-conflict-check`  
**Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)  
**Blocked by**: #79 (TASK-06), #80 (TASK-08)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng `BookingView` và `BookingViewModel`.
- Màn hình tạo phiếu đặt phòng:
  - Chọn Khách hàng (từ danh sách hoặc tạo nhanh).
  - Chọn Phòng muốn đặt (kèm xem trước giá và loại phòng).
  - Chọn Ngày nhận phòng (Check-in Date) và Ngày trả phòng (Check-out Date).

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Thuật toán kiểm tra trùng lịch: Hai khoảng thời gian [A, B] và [C, D] bị trùng nhau khi và chỉ khi:
  `NewCheckIn < ExistingCheckOut && ExistingCheckIn < NewCheckOut`
  (Chỉ kiểm tra với các Booking của cùng phòng đó có trạng thái `Reserved` hoặc `Occupied`).
- Cho phép khách đặt phòng liền kề (Khách cũ trả ngày X lúc 12h, khách mới nhận ngày X lúc 14h -> không tính là xung đột).
- Khi tạo thành công: Tạo bản ghi `Booking` với trạng thái `BookingStatus.Reserved` và chuyển trạng thái `Room.Status` tương ứng.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Màn hình tạo phiếu đặt phòng chọn Khách hàng, Phòng, Ngày nhận (Check-in), Ngày trả (Check-out).
- [ ] Cài đặt thuật toán kiểm tra xung đột thời gian: Hai khoảng thời gian [A, B] và [C, D] trùng nhau khi và chỉ khi A < D và C < B.
- [ ] Hiển thị thông báo lỗi rõ ràng nếu phòng đã bị đặt trong khoảng thời gian lựa chọn.
- [ ] Lưu thông tin Booking với trạng thái ban đầu Reserved/Booked.

### Ca biên cần xử lý (Edge Cases):
- Chọn ngày trả phòng trước hoặc bằng ngày nhận phòng -> báo lỗi validation ngay tại DatePicker.
- Chọn đặt phòng trong quá khứ -> cảnh báo.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Test xung đột trùng trọn vẹn (khoảng mới nằm trong khoảng cũ).
- Test xung đột chồng lấn một phần (bắt đầu trước, kết thúc giữa chừng hoặc ngược lại).
- Test đặt phòng kế tiếp nhau không bị báo lỗi trùng.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-11.md` lên GitHub Issue #64.*
