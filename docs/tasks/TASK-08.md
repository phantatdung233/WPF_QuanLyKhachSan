# [TASK-08] Module Check-in, Check-out & Thêm dịch vụ phòng

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-08`  
**GitHub Issue**: [#80](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/80)  
**Thành viên đảm nhiệm**: **Member B (@xuanduong29)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/checkin-checkout-services`  
**Milestone**: Mốc 2: Nghiệp Vụ Cốt Lõi, Sơ Đồ Phòng & Hóa Đơn (Tuần 3-5)  
**Blocked by**: #62 (TASK-07), #79 (TASK-06)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng các hàm xử lý chuyển đổi trạng thái lưu trú trong `BookingDAO.cs` và `RoomDAO.cs`.
- Xây dựng `BookingServiceDAO.cs` quản lý các dịch vụ phát sinh trong thời gian lưu trú (bảng `BookingServices`).
- Thao tác **Check-in**: Khách đến nhận phòng, chuyển trạng thái Booking thành `Đang ở`, chuyển trạng thái Phòng thành `Đang ở`.
- Thao tác **Gọi dịch vụ phòng**: Lễ tân ghi nhận các dịch vụ khách sạn mà khách trong phòng sử dụng (tên dịch vụ, số lượng, đơn giá tại thời điểm dùng).
- Thao tác **Check-out**: Khởi tạo quá trình trả phòng, tổng hợp tiền phòng và tiền dịch vụ để chuyển sang màn hình Hóa đơn (Task 10).

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Quy trình Check-in:
  - Chọn một đơn đặt phòng đang ở trạng thái `Đã đặt` (hoặc khách vãng lai nhận phòng ngay).
  - Bấm nút "Check-in":
    - Cập nhật `Bookings.Status = N'Đang ở'`.
    - Cập nhật `Rooms.Status = N'Đang ở'`.
- Thêm dịch vụ cho phòng lưu trú:
  - Mở danh sách các phòng đang có khách ở (`Status = 'Đang ở'`).
  - Chọn phòng cần gọi dịch vụ -> mở popup `AddServiceDialog.xaml`.
  - Chọn dịch vụ từ ComboBox (lấy từ bảng `Services`).
  - Nhập số lượng (`Quantity >= 1`). Tự động tính Thành tiền = `Quantity * Price`.
  - Bấm "Lưu dịch vụ": Lưu dòng dữ liệu vào bảng `BookingServices` (`BookingId`, `ServiceId`, `Quantity`, `UnitPrice`).
  - Hiển thị danh sách bảng kê các dịch vụ đã sử dụng của phòng đó kèm nút xóa nếu nhập nhầm.
- Chuẩn bị Check-out:
  - Khi khách báo trả phòng, bấm nút "Check-out" trên phòng đó -> Chuyển thông tin `BookingId` sang màn hình tạo Hóa đơn thanh toán.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Nút Check-in: Đổi trạng thái Booking sang Đang ở, đổi trạng thái Phòng sang Đang ở.
- [ ] Giao diện Thêm dịch vụ phòng: Chọn phòng đang ở, chọn dịch vụ từ ComboBox Services, nhập số lượng, tự động nhân thành tiền và lưu vào bảng BookingServices.
- [ ] Hiển thị danh sách các dịch vụ mà phòng đó đã gọi kèm nút xóa nếu nhập nhầm.
- [ ] Nút Check-out: Kích hoạt luồng chuyển tiếp sang màn hình Hóa đơn để tính tiền và thanh toán.

### Ca biên cần xử lý (Edge Cases):
- Nhập số lượng dịch vụ <= 0 hoặc không phải số nguyên -> Validate chặn và nhắc nhở.
- Xóa một dòng dịch vụ khỏi phòng: Cập nhật lại tổng tiền dịch vụ ngay lập tức.
- Không cho phép gọi dịch vụ vào các phòng đang trống hoặc đang bảo trì.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Thực hiện Check-in cho phòng 201: Cả Booking và Room đều đổi sang trạng thái `Đang ở`.
- Thêm 2 lon nước suối (10,000 đ/lon) và 1 gói giặt ủi (30,000 đ) vào phòng 201: Bảng kê hiển thị tổng cộng 50,000 đ tiền dịch vụ.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-08.md` lên GitHub Issue #80.*
