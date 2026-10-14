# [TASK-04] Module Quản lý Phòng & Loại phòng (Room & RoomType CRUD)

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-04`  
**GitHub Issue**: [#61](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/61)  
**Thành viên đảm nhiệm**: **Member A (@phantatdung233)**  
**Mức độ ưu tiên**: `Medium`  
**Nhánh làm việc**: `feature/room-management`  
**Milestone**: Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (Tuần 1-3)  
**Blocked by**: #59 (TASK-02)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng `RoomDAO.cs` và `RoomTypeDAO.cs` trong thư mục `DAO/`.
- Xây dựng `RoomViewModel.cs` kế thừa `ViewModelBase` và giao diện `RoomView.xaml`.
- Quản lý danh sách phòng: Xem danh sách, Thêm phòng mới, Cập nhật thông tin phòng, Xóa phòng.
- Quản lý danh mục loại phòng: Xem, Thêm mới, Sửa đơn giá ngày theo loại phòng.
- Bộ lọc danh sách phòng theo Loại phòng và Trạng thái phòng (Trống, Đang ở, Đã đặt, Bảo trì).

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- `RoomView.xaml`:
  - Thanh công cụ trên cùng:
    - ComboBox chọn Loại phòng để lọc (Tất cả, Đơn, Đôi, VIP...).
    - ComboBox chọn Trạng thái để lọc (Tất cả, Trống, Đang ở, Đã đặt, Bảo trì).
    - Nút "Thêm phòng mới" và nút "Quản lý loại phòng" (chỉ Admin thao tác).
  - Bảng chính `DataGrid`: Hiển thị Mã phòng, Số phòng, Tên loại phòng, Đơn giá/ngày, Trạng thái hiện tại. Cột thao tác chứa nút "Sửa" và "Xóa".
- Form Dialog Thêm / Sửa phòng (`RoomDialog.xaml`):
  - Nhập Số phòng (ví dụ: `101`, `202`).
  - Chọn Loại phòng từ ComboBox (load danh sách từ `RoomTypeDAO`).
  - Chọn Trạng thái phòng ban đầu.
  - Validation: Số phòng không được rỗng, không được trùng với số phòng đã có trong cơ sở dữ liệu.
- Quản lý Loại phòng:
  - Cho phép thêm loại phòng mới, cập nhật Đơn giá ngày (`PricePerDay > 0`).
  - Khi xóa loại phòng: Kiểm tra nếu có phòng nào đang thuộc loại phòng này thì không cho xóa và báo lỗi cho người dùng.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] RoomDAO.cs và RoomTypeDAO.cs thực hiện các câu lệnh SQL SELECT, INSERT, UPDATE, DELETE vào bảng Rooms và RoomTypes.
- [ ] RoomView.xaml: Hiển thị danh sách phòng bằng DataGrid, có ComboBox lọc theo Loại phòng và Trạng thái (Trống, Đang ở, Đã đặt, Bảo trì).
- [ ] Dialog Thêm/Sửa phòng: Nhập Số phòng, chọn Loại phòng từ ComboBox, chọn Trạng thái. Validate không được để trống số phòng hoặc trùng lặp số phòng.
- [ ] Quản lý danh mục loại phòng: Thêm loại phòng mới, cập nhật Đơn giá ngày (PricePerDay > 0), không cho xóa loại phòng đang có phòng liên kết.

### Ca biên cần xử lý (Edge Cases):
- Phòng đang có khách ở (`Status = 'Đang ở'`): Khóa chức năng xóa phòng để tránh mất dữ liệu liên kết.
- Nhập đơn giá phòng là số âm hoặc chữ cái -> Validate bắt lỗi ngay trên giao diện trước khi lưu xuống CSDL.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Thêm mới một phòng `305` thuộc loại phòng `VIP`: Phòng xuất hiện ngay trên bảng danh sách.
- Sửa trạng thái phòng `305` sang `Bảo trì`: Dữ liệu lưu thành công xuống SQL Server.
- Lọc theo trạng thái `Trống`: Bảng chỉ hiển thị các phòng đang có trạng thái `Trống`.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-04.md` lên GitHub Issue #61.*
