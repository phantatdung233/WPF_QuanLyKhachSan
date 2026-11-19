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
- Xây dựng cửa sổ `LoginWindow.xaml` và `LoginViewModel.cs` chuẩn MVVM (`CommunityToolkit.Mvvm`), bám sát bố cục Windows UI Kit tại `docs/design/WIREFRAMES.md` mục 1 (cửa sổ gọn `960×524`, bo góc 8px, nền sáng trung tính, nút chính `#005FB8`, không dùng mảng nền navy).
- Xây dựng `UserDAO.cs` trong `DAO/` truy vấn tham số hóa bảng `Users`.
- Quản lý phiên đăng nhập qua lớp `UserSession` (`CurrentUser`: `UserId`, `Username`, `FullName`, `Role`, `RoleDisplay`).
- Áp dụng phân quyền chặt chẽ dựa trên `GLOSSARY.md`:
  - `Role == "Administrator"` (hiển thị `"Quản trị"`): thấy đủ 8 mục điều hướng.
  - `Role == "Staff"` (hiển thị `"Lễ tân"`): ẩn hoàn toàn 2 mục `Loại phòng` và `Báo cáo` trên Sidebar (còn 6 mục).

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- **`LoginWindow.xaml` & `LoginViewModel.cs`**:
  - Gồm thương hiệu **"AURA HOTEL"** bên trái và form đăng nhập bên phải: ô `Tên đăng nhập`, ô `Mật khẩu`, vùng hiển thị lỗi inline ngay trên form (`ErrorMessage`, không lạm dụng `MessageBox.Show`), nút `Đăng nhập` (Primary, khóa nút khi `IsBusy == true` để chống bấm đúp) và nút `Thoát` (Secondary).
  - Validate đầu vào qua seam `ValidationRules`:
    - Để trống `Username` → lỗi inline: `"Vui lòng nhập tên đăng nhập"`.
    - Để trống `Password` → lỗi inline: `"Vui lòng nhập mật khẩu"`.
  - Khi hợp lệ: gọi `UserDAO.Instance.Login(username.Trim(), password)` thực thi câu lệnh tham số hóa:
    `SELECT UserId, Username, Password, FullName, Role FROM Users WHERE Username = @username AND Password = @password`.
  - Nếu sai thông tin: gán `ErrorMessage = "Tên đăng nhập hoặc mật khẩu không chính xác!"`.
  - Nếu đúng: gán `UserSession.CurrentUser = user`, khởi tạo `MainWindow` với `MainViewModel` nhận phiên hiện tại, hiển thị `MainWindow` và đóng `LoginWindow`.
- **Phiên làm việc & Đăng xuất trên `MainWindow`**:
  - Header hiển thị họ tên và vai trò trên hai dòng trong nút tài khoản; bấm mở menu có `Đăng xuất`.
  - Nút `Đăng xuất`: xóa `UserSession.CurrentUser`, mở lại `LoginWindow` sạch trạng thái và đóng `MainWindow`.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] `LoginWindow.xaml` + `LoginViewModel.cs` đúng chuẩn Windows UI Kit (`WIREFRAMES.md` mục 1), hỗ trợ phím `Enter` để Đăng nhập (`IsDefault="True"`), hiển thị lỗi inline và khóa nút khi đang kiểm tra.
- [ ] `UserDAO.cs` truy vấn tham số hóa bảng `Users`, chống SQL Injection.
- [ ] Đăng nhập thành công mở `MainWindow` tại màn mặc định `Sơ đồ phòng`, hiển thị đúng `[FullName] và [RoleDisplay] trong nút tài khoản`.
- [ ] Phân quyền chuẩn xác: `Administrator` thấy đủ 8 mục menu; `Staff` bị ẩn `Loại phòng` và `Báo cáo` (còn 6 mục).
- [ ] Đăng xuất đưa người dùng về `LoginWindow`, xóa sạch phiên cũ và mật khẩu trên form.
- [ ] Có unit test trong `HotelManagement.Tests` cho `LoginViewModel` (qua interface/delegate xác thực) kiểm tra lỗi bỏ trống, sai mật khẩu, `.Trim()` khoảng trắng đầu/cuối ở `Username`, và phân quyền `Administrator` vs `Staff`.

### Ca biên cần xử lý (Edge Cases):
- Người dùng nhập khoảng trắng thừa đầu/cuối ở ô `Tên đăng nhập` -> tự động `.Trim()` (không `.Trim()` mật khẩu).
- Đóng `LoginWindow` bằng nút X hoặc nút `Thoát` khi chưa đăng nhập -> kết thúc gọn ứng dụng (`Application.Current.Shutdown()`), không treo tiến trình ngầm.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động (`TDD` seam): `dotnet test` kiểm tra các trạng thái lỗi inline của `LoginViewModel`, khóa bấm đúp `IsBusy` và logic ẩn/hiện menu theo `Role`.
- Trực quan: Đăng nhập `admin`/`123456` (thấy 8 mục, header `Trần Minh Quản (Quản trị)`), đăng xuất rồi đăng nhập `staff`/`123456` (thấy 6 mục, ẩn Loại phòng & Báo cáo, header `Lê Thu Ngân (Lễ tân)`).

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-03.md` lên GitHub Issue #60.*

### Đồng bộ Figma — 19/11/2026

Chuẩn trực quan: `docs/design/FIGMA-ALIGNMENT.md`. Dùng AURA HOTEL, title bar 48px, pane trái 240px, menu nhóm có icon, header tài khoản dạng menu. Login 960×524 chia cột 344/616px. Xác thực thật thuộc TASK-03; TASK-01 giữ nhãn xem trước.
