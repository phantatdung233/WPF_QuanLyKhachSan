# [TASK-04] Module Quản lý Phòng & Loại phòng (Room & RoomType CRUD)

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-04`  
**GitHub Issue**: [#61](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/61)  
**Thành viên đảm nhiệm**: **Member A (@phantatdung233)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/room-management`  
**Milestone**: Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (18/11 – 04/12)  
**Khung thời gian dự kiến**: `25/11/2026 – 29/11/2026` *(ưu tiên hoàn thành sớm để mở khóa `TASK-09` & `TASK-07`)*  
**Labels**: `room`, `crud`, `m1`, `ready-for-agent`  
**Blocked by**: #59 (TASK-02)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng `RoomDAO.cs` và `RoomTypeDAO.cs` trong `DAO/` thao tác tham số hóa với bảng `Rooms` và `RoomTypes`.
- Hoàn thiện 2 màn hình danh mục đã có placeholder từ `TASK-01`, bám sát bố cục bảng Windows UI Kit tại `docs/design/DESIGN-BRIEF.md` mục 6.1 & `WIREFRAMES.md` mục 4–5:
  - `RoomView.xaml` + `RoomViewModel.cs` + `RoomDialog.xaml`: Xem danh sách phòng, lọc kép theo Loại phòng & Trạng thái (`Trống`, `Đang ở`, `Đã đặt`, `Bảo trì`), Thêm/Sửa/Xóa phòng.
  - `RoomTypeView.xaml` + `RoomTypeViewModel.cs` + `RoomTypeDialog.xaml` (chỉ `Administrator` truy cập): Xem danh sách loại phòng kèm cột **Số lượng phòng liên kết**, Thêm/Sửa đơn giá đêm (`PricePerDay > 0`), Xóa loại phòng chưa có phòng gắn kèm.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- **`RoomView.xaml`**:
  - Thanh công cụ trên cùng: `ComboBox` lọc Loại phòng (`Tất cả` + danh sách từ DB), `ComboBox` lọc Trạng thái (`Tất cả`, `Trống`, `Đang ở`, `Đã đặt`, `Bảo trì`), nút `Thêm phòng mới`.
  - Bảng `DataGrid` (chiều cao dòng ~40px theo kit): Mã phòng, Số phòng (`101`, `202`...), Loại phòng, Đơn giá/đêm (`#,##0 đ`), Badge Trạng thái (dùng đúng 4 mã màu `RoomStatus` làm chấm/nhãn), Cột Thao tác (`Sửa`, `Xóa`).
  - **Khóa xóa an toàn**: Nếu phòng đang `Đang ở` hoặc đã phát sinh lịch sử `Bookings`, vô hiệu hóa nút `Xóa` (kèm tooltip giải thích lý do: *"Không thể xóa phòng đang có khách ở hoặc đã có lịch đặt"*).
  - Hỗ trợ đủ trạng thái rỗng (`"Chưa có phòng nào"`) và trạng thái lọc không có kết quả (`"Không có phòng phù hợp bộ lọc hiện tại"`).
- **`RoomDialog.xaml`**:
  - Nhập Số phòng, chọn Loại phòng (`ComboBox`), chọn Trạng thái (`Trống` / `Bảo trì` khi tạo mới).
  - Lỗi validate hiển thị **inline ngay dưới ô nhập**: Số phòng rỗng hoặc trùng số phòng đã tồn tại trong CSDL.
- **`RoomTypeView.xaml` & `RoomTypeDialog.xaml` (`Administrator` only)**:
  - Hiển thị Mã loại, Tên loại phòng, Đơn giá/đêm (`#,##0 đ`), Số phòng đang thuộc loại này, Cột Thao tác (`Sửa`, `Xóa`).
  - Validate `TypeName` không rỗng, không trùng tên, `PricePerDay > 0`.
  - Chặn xóa (khóa nút `Xóa` / hiển thị cảnh báo rõ ràng) nếu loại phòng đang có số phòng liên kết `> 0`.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] `RoomDAO.cs` và `RoomTypeDAO.cs` thực thi đầy đủ `SELECT` (kèm JOIN lấy `TypeName`, `PricePerDay`, đếm số phòng theo loại), `INSERT`, `UPDATE`, `DELETE` có tham số `@param`.
- [ ] `RoomView.xaml` lọc tức thời kết hợp Loại phòng + Trạng thái, hiển thị tiền tệ `#,##0 đ`, có trạng thái rỗng/không kết quả và khóa nút `Xóa` khi phòng đang bận hoặc có booking.
- [ ] `RoomDialog.xaml` validate inline không để trống hoặc trùng `RoomNumber`.
- [ ] `RoomTypeView.xaml` + `RoomTypeDialog.xaml` validate `PricePerDay > 0` và chặn xóa loại phòng đang có phòng trực thuộc.
- [ ] Có unit test tại seam `ValidationRules` trong `HotelManagement.Tests` cho: kiểm tra trùng `RoomNumber`, `PricePerDay <= 0` / nhập chữ, điều kiện khóa xóa phòng (`Status == "Đang ở"`) và khóa xóa loại phòng (`RoomCount > 0`).

### Ca biên cần xử lý (Edge Cases):
- Nhân viên lễ tân (`Staff`) đăng nhập: không nhìn thấy màn `Loại phòng`, và trên `RoomView` các thao tác quản trị cấu hình loại phòng bị ẩn theo quyền.
- Nhập đơn giá chứa ký tự không phải số hoặc `<= 0` -> chặn ngay trên form kèm thông báo cụ thể dưới ô nhập.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động (`TDD` seam): `dotnet test` chạy bộ test validate phòng, loại phòng và quy tắc khóa xóa.
- Trực quan: Thêm phòng `403` loại `VIP`, lọc trạng thái `Trống` thấy phòng mới; thử xóa phòng đang `Đang ở` hoặc xóa loại phòng `VIP` xác nhận hệ thống khóa/chặn an toàn.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-04.md` lên GitHub Issue #61.*
