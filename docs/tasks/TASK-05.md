# [TASK-05] Module Quản lý Loại phòng (RoomType) CRUD & Binding

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-05`  
**GitHub Issue**: [#78](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/78)  
**Thành viên đảm nhiệm**: **Member A (@phantatdung233)**  
**Mức độ ưu tiên**: `Medium`  
**Nhánh làm việc**: `feature/roomtype-management`  
**Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)  
**Blocked by**: #59 (TASK-02), #60 (TASK-03)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng `RoomTypeView` và `RoomTypeViewModel`.
- Bảng DataGrid hiển thị danh sách loại phòng: Tên loại phòng, Đơn giá cơ bản theo ngày, Sức chứa tối đa, Mô tả.
- Form nhập liệu thêm mới và chỉnh sửa loại phòng.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Thao tác dữ liệu qua `IRoomTypeService` hoặc trực tiếp từ `HotelDbContext`.
- Sử dụng `ObservableCollection<RoomType>` để tự động cập nhật giao diện khi thêm/sửa/xóa.
- Ràng buộc kiểm tra trước khi xóa: Nếu có ít nhất một `Room` đang liên kết với loại phòng này, từ chối xóa và hiển thị thông báo cảnh báo.
- Validate đầu vào: Tên không để trống, không trùng lặp; đơn giá cơ bản > 0; sức chứa >= 1.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] RoomTypeView và RoomTypeViewModel hỗ trợ xem danh sách, thêm mới, sửa, xóa loại phòng.
- [ ] Validate dữ liệu đầu vào: Tên loại phòng không trùng, đơn giá cơ bản > 0.
- [ ] Tự động cập nhật bảng danh sách ngay lập tức khi thêm/sửa/xóa qua ObservableCollection.
- [ ] Ràng buộc kiểm tra không cho xóa loại phòng nếu đã có phòng đang tham chiếu.

### Ca biên cần xử lý (Edge Cases):
- Nhập giá tiền âm hoặc ký tự chữ.
- Trùng tên loại phòng (phân biệt hoa thường hoặc kiểm tra không phân biệt hoa thường).

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Unit test thêm loại phòng hợp lệ.
- Test validate đơn giá <= 0 trả về lỗi.
- Test xóa loại phòng đang có phòng liên kết bị chặn thành công.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-05.md` lên GitHub Issue #78.*
