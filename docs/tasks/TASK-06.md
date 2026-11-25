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
- Hoàn thiện `ServiceViewModel.cs`, `ServiceView.xaml` và `ServiceDialog.xaml` theo quy trình **2 Pha** (`CLAUDE.md`) và chuẩn `DESIGN.md`.
- Quản lý danh mục dịch vụ phụ trợ (Nước suối Lavie, Ăn sáng buffet, Giặt ủi, Nước ngọt, Thuê xe máy...): Xem/tìm kiếm nhanh, Thêm mới, Cập nhật tên & đơn giá, Xóa dịch vụ chưa phát sinh giao dịch.

---

## 2. Hướng dẫn nghiệp vụ & UI Contract (Theo `DESIGN.md`)
- **Logic & Validation (Pha 1 — Logic & State First)**:
  - `ServiceDAO`: nạp danh sách kèm cờ `UsageCount` từ `BookingServices` để khóa xóa an toàn (tuyệt đối không để ném lỗi SQL Foreign Key ra UI).
  - Validate qua seam `ValidationRules`: `ServiceName` không để trống, không trùng tên; `Price` là số hợp lệ `>= 0` (cho phép `0 đ` cho tiện ích miễn phí).
- **UI Contract (`ServiceView` & `ServiceDialog` — Pha 2)**:
  - **Dữ liệu hiển thị**: Mã DV, Tên dịch vụ, Đơn giá định dạng `#,##0 đ` (dấu chấm phân cách nghìn), Cột Thao tác (`Sửa`, `Xóa`), bộ đếm dòng cuối bảng.
  - **Primitives bắt buộc (`WPF-UI`)**: `ui:TextBox` (ô tìm kiếm + tên dịch vụ), `ui:NumberBox` / `ui:TextBox` (đơn giá), `DataGrid` (`RowHeight="40"`), modal `ui:FluentWindow` / `ui:ContentDialog`, `ui:InfoBar`, `ui:ProgressRing`, tối đa **1 Primary CTA** `ui:Button` (`Thêm dịch vụ` / `Lưu`).
  - **4 Trạng thái UI bắt buộc**:
    1. *Loading*: Hiện `ui:ProgressRing` khi tải danh sách và khóa nút khi đang lưu.
    2. *Empty*: Chưa có dịch vụ nào (`"Chưa có dịch vụ nào"`) hoặc không khớp từ khóa tìm kiếm (`"Không tìm thấy dịch vụ phù hợp"` kèm nút xóa tìm kiếm).
    3. *Error*: Lỗi tên rỗng/trùng hoặc đơn giá âm/chữ hiển thị inline dưới ô nhập (`InlineErrorTextStyle`); lỗi chặn xóa hiện qua `ui:InfoBar`.
    4. *Success*: Định dạng tiền chuẩn `#,##0 đ`, khóa nút `Xóa` kèm `ToolTip` *"Dịch vụ đã phát sinh trong phiếu lưu trú, không thể xóa"* khi `UsageCount > 0`.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] `ServiceDAO.cs` thực thi đầy đủ `GetAll` (kèm cờ `IsInUse` từ `BookingServices`), `Insert`, `Update`, `Delete` bằng truy vấn tham số hóa.
- [ ] `ServiceView.xaml` tuân thủ UI Contract & `DESIGN.md`, định dạng tiền `#,##0 đ`, có tìm kiếm nhanh và đủ 4 trạng thái (`Loading`, `Empty`, `Error`, `Success`).
- [ ] `ServiceDialog.xaml` validate inline Tên dịch vụ và Đơn giá (`>= 0`).
- [ ] Chặn xóa an toàn các dịch vụ đã tồn tại trong `BookingServices`.
- [ ] Có unit test tại seam `ValidationRules` trong `HotelManagement.Tests` cho: tên dịch vụ rỗng/trùng, đơn giá âm/nhập chữ, đơn giá `0` hợp lệ, và quy tắc khóa xóa khi `UsageCount > 0`.

### Ca biên cần xử lý (Edge Cases):
- Đổi đơn giá dịch vụ trong danh mục **không được làm thay đổi ngược** thành tiền của các phòng đã gọi dịch vụ trước đó (vì `BookingServices.UnitPrice` lưu snapshot đơn giá tại thời điểm gọi).
- Nhập đơn giá âm hoặc ký tự chữ -> chặn lưu và báo lỗi inline dưới ô Đơn giá.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động (`TDD` seam): `dotnet test` xác nhận toàn bộ test validate dịch vụ và điều kiện khóa xóa xanh 100%.
- Trực quan: Thêm mới dịch vụ `"Bia Tiger"` giá `25000` -> hiển thị đúng `25.000 đ`, sửa lên `30000` -> hiển thị `30.000 đ` thành công.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-06.md` lên GitHub Issue #79.*
