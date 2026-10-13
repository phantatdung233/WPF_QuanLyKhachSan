# [TASK-17] [Nâng Cao 1] Sơ đồ phòng trực quan sử dụng DataTrigger WPF

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-17`  
**GitHub Issue**: [#70](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/70)  
**Thành viên đảm nhiệm**: **Member C (@ngocthuyen1)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/advanced-room-matrix-trigger`  
**Milestone**: Mốc 2: Ghép Luồng E2E & Nâng Cao (Tuần 4-5)  
**Blocked by**: #79 (TASK-06), #67 (TASK-14)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng màn hình Sơ đồ phòng (`RoomMapView`, `RoomMapViewModel`).
- Hiển thị danh sách các phòng dạng lưới ma trận thẻ phòng (UniformGrid hoặc WrapPanel trong ItemsControl).
- Áp dụng kỹ thuật **WPF XAML DataTrigger** để tự động đổi màu sắc và icon của thẻ phòng dựa trên thuộc tính `Room.Status`:
  - 🟢 Xanh lá (#10B981): Phòng trống (`Available`)
  - 🟡 Vàng/Cam (#F59E0B): Đã đặt trước (`Reserved`)
  - 🔴 Đỏ (#EF4444): Đang có khách ở (`Occupied`)
  - ⚪ Xám (#6B7280): Đang dọn dẹp / Bảo trì (`Maintenance` / `Cleaning`)

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Nhấp trực tiếp vào một thẻ phòng trên sơ đồ:
  - Nếu phòng trống: Mở nhanh hộp thoại Tạo đặt phòng cho phòng đó.
  - Nếu phòng đang có khách: Mở nhanh thông tin khách đang ở và tùy chọn Thêm dịch vụ / Check-out.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Màn hình Sơ đồ phòng hiển thị dạng lưới các thẻ phòng (WrapPanel / UniformGrid).
- [ ] Ứng dụng DataTrigger trong XAML để đổi màu sắc thẻ phòng tự động theo trạng thái (Available, Reserved, Occupied, Maintenance).
- [ ] Click trực tiếp vào phòng trên sơ đồ để mở nhanh thông tin chi tiết hoặc tạo đơn đặt phòng.

### Ca biên cần xử lý (Edge Cases):
- Số lượng phòng lớn (hàng chục phòng) vẫn đảm bảo cuộn mượt mà (sử dụng UI Virtualization nếu cần).

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Test kiểm tra thuộc tính trạng thái phòng thay đổi thì ViewModel cập nhật và trigger tương ứng kích hoạt.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-17.md` lên GitHub Issue #70.*
