# [TASK-06] Module Quản lý Phòng (Room) CRUD & Liên kết Loại phòng

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-06`  
**GitHub Issue**: [#79](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/79)  
**Thành viên đảm nhiệm**: **Member A (@phantatdung233)**  
**Mức độ ưu tiên**: `Medium`  
**Nhánh làm việc**: `feature/room-management`  
**Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)  
**Blocked by**: #78 (TASK-05)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng `RoomView` và `RoomViewModel`.
- Danh sách phòng hiển thị: Mã phòng (P101, P102...), Tên phòng, Loại phòng (từ RoomType), Trạng thái (`RoomStatus`), Ghi chú.
- ComboBox chọn Loại phòng lấy động từ dữ liệu RoomType.
- Form thêm/sửa thông tin phòng.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Sử dụng Enum `RoomStatus` (`Available`, `Reserved`, `Occupied`, `Maintenance`, `Cleaning`) từ GLOSSARY.md.
- Cung cấp bộ lọc tìm kiếm: Lọc theo trạng thái phòng và lọc theo loại phòng.
- Ràng buộc: Mã phòng là duy nhất (Unique).

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] RoomView hiển thị danh sách phòng kèm ComboBox chọn RoomType.
- [ ] Thực hiện CRUD phòng (Mã phòng, Tên phòng, Trạng thái: Trống/Đã đặt/Đang ở/Bảo trì, Ghi chú).
- [ ] Tìm kiếm và lọc danh sách phòng theo trạng thái hoặc loại phòng.
- [ ] Kiểm tra trùng mã phòng và lưu thay đổi vào SQLite thành công.

### Ca biên cần xử lý (Edge Cases):
- Tạo phòng trùng mã phòng đã có trong hệ thống -> báo lỗi rõ ràng.
- Xóa phòng khi phòng đang có Booking đang lưu trú (Occupied) -> từ chối xóa.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Test CRUD phòng và liên kết đúng khóa ngoại với RoomType.
- Test lọc danh sách phòng theo trạng thái trả về đúng số lượng phòng tương ứng.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-06.md` lên GitHub Issue #79.*
