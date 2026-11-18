# [TASK-06] Module Quản lý Dịch vụ khách sạn (Service CRUD)

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-06`  
**GitHub Issue**: [#79](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/79)  
**Thành viên đảm nhiệm**: **Member B (@xuanduong29)**  
**Mức độ ưu tiên**: `Medium`  
**Nhánh làm việc**: `feature/service-management`  
**Milestone**: Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (18/11 – 04/12)  
**Khung thời gian dự kiến**: `30/11/2026 – 04/12/2026`  
**Labels**: `service`, `crud`, `m1`, `ready-for-agent`  
**Blocked by**: #59 (TASK-02)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng `ServiceDAO.cs` trong `DAO/` sử dụng model `Service.cs` (đã tạo ở `TASK-02`).
- Hoàn thiện `ServiceViewModel.cs`, `ServiceView.xaml` và `ServiceDialog.xaml` theo cùng khung bảng Windows UI Kit với các màn danh mục khác (`docs/design/DESIGN-BRIEF.md` mục 6.1 & `WIREFRAMES.md` mục 7).
- Quản lý danh mục dịch vụ phụ trợ (Nước suối Lavie, Ăn sáng buffet, Giặt ủi, Nước ngọt, Thuê xe máy...): Xem/tìm kiếm nhanh, Thêm mới, Cập nhật tên & đơn giá, Xóa dịch vụ chưa phát sinh giao dịch.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- **`ServiceView.xaml`**:
  - Thanh công cụ: ô tìm kiếm theo tên dịch vụ + nút `Thêm dịch vụ mới`.
  - Bảng `DataGrid`: Mã DV, Tên dịch vụ, Đơn giá định dạng tiền tệ VNĐ (`#,##0 đ`), Cột Thao tác (`Sửa`, `Xóa`).
  - **Khóa xóa an toàn**: truy vấn kiểm tra số lần xuất hiện trong `BookingServices`. Nếu dịch vụ đã từng được sử dụng (`UsageCount > 0`), khóa nút `Xóa` kèm tooltip *"Dịch vụ đã phát sinh trong phiếu lưu trú, không thể xóa"* (tuyệt đối không để quăng lỗi SQL Foreign Key ra giao diện).
  - Hỗ trợ trạng thái rỗng và không tìm thấy kết quả.
- **`ServiceDialog.xaml` & Seam `ValidationRules`**:
  - Validate inline dưới ô nhập:
    - `ServiceName`: không để trống, không trùng tên dịch vụ đã có.
    - `Price`: phải là số hợp lệ `>= 0` (cho phép `0 đ` đối với tiện ích miễn phí kèm phòng).

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] `ServiceDAO.cs` thực thi đầy đủ `GetAll` (kèm cờ `IsInUse` từ `BookingServices`), `Insert`, `Update`, `Delete` bằng truy vấn tham số hóa.
- [ ] `ServiceView.xaml` dùng chung khung thiết kế bảng của Windows UI Kit, định dạng tiền `#,##0 đ`, có tìm kiếm nhanh và trạng thái rỗng.
- [ ] `ServiceDialog.xaml` validate inline Tên dịch vụ và Đơn giá (`>= 0`).
- [ ] Chặn xóa an toàn các dịch vụ đã tồn tại trong `BookingServices`.
- [ ] Có unit test tại seam `ValidationRules` trong `HotelManagement.Tests` cho: tên dịch vụ rỗng/trùng, đơn giá âm/nhập chữ, đơn giá `0` hợp lệ, và quy tắc khóa xóa khi `UsageCount > 0`.

### Ca biên cần xử lý (Edge Cases):
- Đổi đơn giá dịch vụ trong danh mục **không được làm thay đổi ngược** thành tiền của các phòng đã gọi dịch vụ trước đó (vì `BookingServices.UnitPrice` lưu snapshot đơn giá tại thời điểm gọi).
- Nhập đơn giá âm hoặc ký tự chữ -> chặn lưu và báo lỗi inline dưới ô Đơn giá.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động (`TDD` seam): `dotnet test` xác nhận toàn bộ test validate dịch vụ và điều kiện khóa xóa xanh 100%.
- Trực quan: Thêm mới dịch vụ `"Bia Tiger"` giá `25,000` -> hiển thị `25.000 đ` (hoặc `25,000 đ` nhất quán culture), sửa lên `30,000` thành công.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-06.md` lên GitHub Issue #79.*
