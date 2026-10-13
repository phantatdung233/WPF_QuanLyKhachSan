# [TASK-13] Module Dịch vụ phòng (BookingService) gắn dịch vụ phát sinh

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-13`  
**GitHub Issue**: [#66](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/66)  
**Thành viên đảm nhiệm**: **Member B (@xuanduong29)**  
**Mức độ ưu tiên**: `Medium`  
**Nhánh làm việc**: `feature/booking-service-usage`  
**Milestone**: Mốc 2: Ghép Luồng E2E & Nâng Cao (Tuần 4-5)  
**Blocked by**: #81 (TASK-09), #65 (TASK-12)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Giao diện chọn một phòng đang thuê (`BookingStatus.Occupied`) và thêm dịch vụ phát sinh.
- Chọn dịch vụ từ danh mục `Service`, nhập số lượng sử dụng, tự động tính thành tiền (`Quantity * UnitPrice`).
- Hiển thị danh sách các dịch vụ phòng đó đã gọi kèm thời gian gọi.
- Hỗ trợ xóa hoặc chỉnh sửa số lượng dịch vụ đã gọi trước khi checkout.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Lưu dữ liệu vào bảng `BookingService`. Lưu cứng `UnitPrice` tại thời điểm gọi dịch vụ (tránh trường hợp sau này đổi giá dịch vụ làm sai lệch hóa đơn cũ).

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Giao diện cho phép chọn một phòng đang thuê và thêm dịch vụ phát sinh vào phiếu đặt phòng đó.
- [ ] Chọn dịch vụ từ danh mục Service, nhập số lượng, tự động nhân thành tiền.
- [ ] Hỗ trợ xem danh sách các dịch vụ đã sử dụng của phòng và xóa/chỉnh sửa số lượng.
- [ ] Cập nhật bảng trung gian BookingService trong CSDL.

### Ca biên cần xử lý (Edge Cases):
- Nhập số lượng dịch vụ <= 0 hoặc ký tự không phải số.
- Thêm dịch vụ cho phòng đã check-out hoặc phòng chưa check-in -> chặn thao tác.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Test thêm dịch vụ tính đúng thành tiền.
- Test chỉnh sửa số lượng cập nhật lại tổng tiền chi tiết.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-13.md` lên GitHub Issue #66.*
