# [TASK-03] Màn hình Đăng nhập (Login MVVM) & Phân quyền Administrator / Staff

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-03`  
**GitHub Issue**: [#60](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/60)  
**Thành viên đảm nhiệm**: **Member A (@phantatdung233)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/auth-login`  
**Milestone**: Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (18/11 – 04/12)  
**Khung thời gian dự kiến**: `24/11/2026 – 28/11/2026`  
**Labels**: `auth`, `security`, `m1`, `ready-for-agent`  
**Blocked by**: #59 (TASK-02)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng `UserDAO.cs`, lớp phiên đăng nhập `UserSession` (`CurrentUser`: `UserId`, `Username`, `FullName`, `Role`, `RoleDisplay`), `LoginViewModel.cs` và cửa sổ `LoginWindow.xaml` theo quy trình **2 Pha** trong `CLAUDE.md` và hệ thống token trong `DESIGN.md`.
- Áp dụng phân quyền chặt chẽ dựa trên `GLOSSARY.md`:
  - `Role == "Administrator"` (hiển thị `"Quản trị"`): thấy đủ 8 mục điều hướng.
  - `Role == "Staff"` (hiển thị `"Lễ tân"`): ẩn hoàn toàn 2 mục `Loại phòng` và `Báo cáo` trên Sidebar (còn 6 mục).

---

## 2. Hướng dẫn nghiệp vụ & UI Contract (Theo `DESIGN.md`)
- **Logic xác thực (Pha 1 — Logic & State First)**:
  - Validate đầu vào qua seam `ValidationRules`:
    - Để trống `Username` → lỗi inline `UsernameError = "Vui lòng nhập tên đăng nhập"`.
    - Để trống `Password` → lỗi inline `PasswordError = "Vui lòng nhập mật khẩu"`.
  - Khi hợp lệ: gọi `UserDAO.Instance.Login(username.Trim(), password)` thực thi câu lệnh tham số hóa:
    `SELECT UserId, Username, Password, FullName, Role FROM Users WHERE Username = @username AND Password = @password`.
  - Nếu sai thông tin: gán `ErrorMessage = "Tên đăng nhập hoặc mật khẩu không chính xác!"`.
  - Nếu đúng: gán `UserSession.CurrentUser = user`, khởi tạo `MainWindow` với `MainViewModel` nhận phiên hiện tại, hiển thị `MainWindow` và đóng `LoginWindow`.
  - Đăng xuất từ `MainWindow`: xóa `UserSession.CurrentUser`, mở lại `LoginWindow` sạch trạng thái và đóng `MainWindow`.
- **UI Contract (`LoginWindow` & Shell Account Menu — Pha 2)**:
  - **Dữ liệu hiển thị**: Thương hiệu **`AURA HOTEL`**, trường `Tên đăng nhập`, trường `Mật khẩu`, thông báo lỗi trường/hệ thống, và trên `MainWindow` hiển thị `FullName` + `RoleDisplay` trong nút menu tài khoản kèm mục `Đăng xuất`.
  - **Primitives bắt buộc (`WPF-UI`)**: `ui:FluentWindow` (`ResizeMode="NoResize"`, bo góc `8px`), `ui:TitleBar`, `ui:TextBox`, `ui:PasswordBox`, `ui:InfoBar` (`Severity="Error"`), `ui:ProgressRing`, và **duy nhất 1 Primary CTA** `ui:Button` (`Content="Đăng nhập"`, `Appearance="Primary"`, `IsDefault="True"`). Hệ lưới spacing bội số `4px/8px`.
  - **4 Trạng thái UI bắt buộc**:
    1. *Loading (`IsBusy`)*: Hiện `ui:ProgressRing`, khóa ô nhập và nút `Đăng nhập` chống bấm đúp.
    2. *Empty*: Trạng thái khởi tạo hoặc vừa đăng xuất — các ô nhập trống sạch mật khẩu, chưa hiện lỗi đỏ.
    3. *Error*: Lỗi bỏ trống hiện inline (`InlineErrorTextStyle`) ngay dưới từng ô; lỗi sai tài khoản/kết nối CSDL hiện qua `ui:InfoBar` (`Severity="Error"`).
    4. *Success*: Chuyển thẳng vào `MainWindow` tại màn mặc định `Sơ đồ phòng` với đúng danh sách menu theo `Role`.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Hoàn thành Pha 1 trước: `UserDAO.cs`, `UserSession.cs`, `LoginViewModel.cs` và unit test trong `HotelManagement.Tests` xanh 100%.
- [ ] Hoàn thành Pha 2: `LoginWindow.xaml` tuân thủ `DESIGN.md` (chỉ dùng primitives `WPF-UI`, tối đa 1 Primary CTA, hỗ trợ `Enter`, đủ 4 trạng thái Loading/Empty/Error/Success).
- [ ] Đăng nhập thành công mở `MainWindow` tại màn mặc định `Sơ đồ phòng`, hiển thị đúng `FullName` và `RoleDisplay` trên cụm tài khoản.
- [ ] Phân quyền chuẩn xác: `Administrator` thấy đủ 8 mục menu; `Staff` bị ẩn `Loại phòng` và `Báo cáo` (còn 6 mục).
- [ ] Đăng xuất đưa người dùng về `LoginWindow`, xóa sạch phiên cũ và mật khẩu trên form.
- [ ] Có unit test trong `HotelManagement.Tests` cho `LoginViewModel` (qua interface/delegate xác thực) kiểm tra lỗi bỏ trống, sai mật khẩu, `.Trim()` khoảng trắng đầu/cuối ở `Username`, và phân quyền `Administrator` vs `Staff`.

### Ca biên cần xử lý (Edge Cases):
- Người dùng nhập khoảng trắng thừa đầu/cuối ở ô `Tên đăng nhập` -> tự động `.Trim()` (không `.Trim()` mật khẩu).
- Đóng `LoginWindow` bằng nút X trên TitleBar khi chưa đăng nhập -> kết thúc gọn ứng dụng (`Application.Current.Shutdown()`), không treo tiến trình ngầm.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động (`TDD` seam): `dotnet test` kiểm tra các trạng thái lỗi inline của `LoginViewModel`, khóa bấm đúp `IsBusy` và logic ẩn/hiện menu theo `Role`.
- Trực quan: Đăng nhập `admin`/`123456` (thấy 8 mục, header `Trần Minh Quản (Quản trị)`), đăng xuất rồi đăng nhập `staff`/`123456` (thấy 6 mục, ẩn Loại phòng & Báo cáo, header `Lê Thu Ngân (Lễ tân)`).

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-03.md` lên GitHub Issue #60.*
