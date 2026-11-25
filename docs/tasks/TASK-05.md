# [TASK-05] Module Quản lý Khách hàng (Customer CRUD & Tìm kiếm thời gian thực)

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-05`  
**GitHub Issue**: [#78](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/78)  
**Thành viên đảm nhiệm**: **Member B (@xuanduong29)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/customer-management`  
**Milestone**: Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (18/11 – 04/12)  
**Khung thời gian dự kiến**: `24/11/2026 – 29/11/2026`  
**Labels**: `customer`, `crud`, `m1`, `ready-for-agent`  
**Blocked by**: #59 (TASK-02)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng `CustomerDAO.cs` trong `DAO/` sử dụng model `Customer.cs` (đã tạo ở `TASK-02`).
- Hoàn thiện `CustomerViewModel.cs`, `CustomerView.xaml` và hộp thoại `CustomerDialog.xaml` theo quy trình **2 Pha** (`CLAUDE.md`) và chuẩn `DESIGN.md`.
- Hỗ trợ xem danh sách khách hàng, tìm kiếm thời gian thực theo **Họ tên, CCCD/CMND hoặc Số điện thoại**, thêm mới, cập nhật và xóa an toàn hồ sơ khách hàng.

---

## 2. Hướng dẫn nghiệp vụ & UI Contract (Theo `DESIGN.md`)
- **Logic & Validation (Pha 1 — Logic & State First)**:
  - Tìm kiếm thời gian thực khi gõ từ khóa `@kw`:
    `SELECT CustomerId, FullName, CitizenId, Phone, Email FROM Customers WHERE FullName LIKE N'%' + @kw + N'%' OR CitizenId LIKE '%' + @kw + '%' OR Phone LIKE '%' + @kw + '%'`.
  - Validate qua seam `ValidationRules`:
    1. `FullName`: bắt buộc không để trống (`.Trim().Length > 0`).
    2. `CitizenId` (CCCD/CMND): chỉ gồm chữ số, độ dài **đúng 9 hoặc 12 số**, và **không trùng** với khách hàng khác trong CSDL.
    3. `Phone`: chỉ gồm chữ số, bắt đầu bằng số `0` và độ dài **đúng 10 số**.
    4. `Email` (không bắt buộc): nếu có nhập thì kiểm tra định dạng email cơ bản.
- **UI Contract (`CustomerView` & `CustomerDialog` — Pha 2)**:
  - **Dữ liệu hiển thị**: Mã KH, Họ và tên, CCCD/CMND, Số điện thoại, Email (`—` nếu trống), Cột Thao tác (`Sửa`, `Xóa`), bộ đếm số khách hàng.
  - **Primitives bắt buộc (`WPF-UI`)**: `ui:TextBox` (ô tìm kiếm có `Icon` + các trường nhập liệu trong dialog), `DataGrid` (`RowHeight="40"`), modal `ui:FluentWindow` / `ui:ContentDialog`, `ui:InfoBar`, `ui:ProgressRing`, tối đa **1 Primary CTA** `ui:Button` (`Thêm khách hàng` trên bảng; `Lưu` trên dialog).
  - **4 Trạng thái UI bắt buộc**:
    1. *Loading*: Hiện `ui:ProgressRing` khi truy vấn và khóa nút `Lưu` khi đang lưu hồ sơ.
    2. *Empty*: Chưa có khách nào (`"Chưa có khách hàng nào"`) hoặc tìm không khớp (`"Không tìm thấy khách hàng phù hợp với '[từ khóa]'"` kèm nút `"Xóa từ khóa"`).
    3. *Error*: Lỗi Họ tên, CCCD, SĐT, Email, trùng CCCD hiển thị inline ngay dưới từng ô nhập (`InlineErrorTextStyle`); lỗi giao dịch hiện qua `ui:InfoBar`.
    4. *Success*: Danh sách cập nhật tức thì; nút `Xóa` tự động khóa kèm `ToolTip` giải thích nếu khách đã có lịch sử `Bookings`.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] `CustomerDAO.cs` cài đặt đầy đủ `GetAll`, `Search`, `ExistsCitizenId(citizenId, excludeCustomerId)`, `Insert`, `Update`, `Delete` có tham số hóa `@param`.
- [ ] `CustomerView.xaml` tuân thủ UI Contract & `DESIGN.md`, lọc thời gian thực theo Họ tên / CCCD / SĐT và xử lý đủ 4 trạng thái (`Loading`, `Empty`, `Error`, `Success`).
- [ ] `CustomerDialog.xaml` validate inline Họ tên, CCCD (9 hoặc 12 chữ số, không trùng lặp), SĐT (`0` + 9 chữ số) và Email tùy chọn.
- [ ] Có unit test tại seam `ValidationRules` trong `HotelManagement.Tests` bao phủ toàn bộ ca hợp lệ/không hợp lệ của CCCD (8 số, 9 số, 10 số, 12 số, lẫn chữ), SĐT (thiếu số 0 đầu, 9 số, 11 số) và trùng CCCD khi thêm mới vs cập nhật chính khách đó.

### Ca biên cần xử lý (Edge Cases):
- Khi **Sửa** thông tin khách hàng giữ nguyên số CCCD cũ của chính họ -> hàm kiểm tra trùng phải loại trừ `CustomerId` đang sửa (`CitizenId = @cccd AND CustomerId <> @id`).
- Khách hàng để trống `Email` -> lưu `DBNull.Value` / chuỗi rỗng an toàn, hiển thị gọn `—` trên bảng.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động (`TDD` seam): `dotnet test` chạy bộ unit test validate hồ sơ khách hàng.
- Trực quan: Thêm khách mới hoặc tìm `079` thấy ngay `Nguyễn Văn An` (`079201001234`); thử nhập trùng CCCD `079201001234` thấy cảnh báo inline ngay dưới ô CCCD.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-05.md` lên GitHub Issue #78.*
