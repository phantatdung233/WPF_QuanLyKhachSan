# [TASK-06] Module Quản lý Dịch vụ khách sạn (Service CRUD)

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-06`  
**GitHub Issue**: [#79](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/79)  
**Thành viên đảm nhiệm**: **Member B (@phantatdung)**  
**Mức độ ưu tiên**: `Medium`  
**Nhánh làm việc**: `feature/service-management`  
**Milestone**: Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (Tuần 1-3)  
**Blocked by**: #59 (TASK-02)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng `ServiceDAO.cs` trong thư mục `DAO/` và `Service.cs` trong `Models/`.
- Xây dựng `ServiceViewModel.cs` và giao diện `ServiceView.xaml`.
- Quản lý danh mục các dịch vụ mà khách sạn cung cấp (ví dụ: Giặt ủi, Nước suối Lavie, Ăn sáng buffet, Nước ngọt, Thuê xe máy).
- Hỗ trợ thêm mới dịch vụ, sửa tên và đơn giá dịch vụ, xóa dịch vụ không còn cung cấp.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Thông tin dịch vụ: `ServiceId`, `ServiceName` (Tên dịch vụ), `Price` (Đơn giá VNĐ).
- `ServiceView.xaml`:
  - Nút "Thêm dịch vụ mới".
  - Bảng `DataGrid`: Hiển thị Mã DV, Tên dịch vụ, Đơn giá định dạng tiền tệ VNĐ (`#,##0 đ`), Cột thao tác (Sửa, Xóa).
- Form Dialog Thêm / Sửa dịch vụ:
  - Nhập Tên dịch vụ.
  - Nhập Đơn giá dịch vụ.
  - Validate: Tên dịch vụ không được để trống; đơn giá phải là số thực >= 0.
- Xử lý xóa dịch vụ:
  - Nếu dịch vụ đã từng được sử dụng trong các đơn đặt phòng cũ (có tồn tại trong `BookingServices`): Không cho xóa hoặc hiển thị cảnh báo để tránh vi phạm khóa ngoại SQL Server.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] ServiceDAO.cs: Truy vấn lấy danh mục dịch vụ từ bảng Services.
- [ ] ServiceView.xaml: Hiển thị bảng danh mục dịch vụ (Tên dịch vụ, Đơn giá).
- [ ] Hỗ trợ thêm mới dịch vụ (ví dụ: Giặt ủi, Nước suối, Ăn sáng, Thuê xe máy).
- [ ] Chỉnh sửa thông tin và đơn giá dịch vụ (kiểm tra đơn giá phải >= 0).
- [ ] Xóa dịch vụ khi không còn phục vụ.

### Ca biên cần xử lý (Edge Cases):
- Nhập đơn giá là số âm hoặc chữ cái -> Validate ngăn chặn và thông báo lỗi.
- Đơn giá hiển thị trên giao diện phải có dấu phân cách phần nghìn rõ ràng (ví dụ: `50,000 đ`).

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Thêm mới dịch vụ "Bia Tiger", giá `25,000`. Dữ liệu xuất hiện ngay trên bảng danh mục.
- Cập nhật giá dịch vụ "Bia Tiger" lên `30,000`: Cơ sở dữ liệu cập nhật chính xác.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-06.md` lên GitHub Issue #79.*
