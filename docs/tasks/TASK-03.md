# [TASK-03] Màn hình Đăng nhập (Login) & Phân quyền Admin / Nhân viên

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-03`  
**GitHub Issue**: [#60](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/60)  
**Thành viên đảm nhiệm**: **Member A (@phantatdung233)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/auth-login`  
**Milestone**: Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (Tuần 1-3)  
**Blocked by**: #59 (TASK-02)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng cửa sổ `LoginWindow.xaml` với giao diện gọn gàng, trang nhã.
- Xây dựng lớp `UserDAO.cs` trong thư mục `DAO/` chịu trách nhiệm truy vấn bảng `Users`.
- Kiểm tra tài khoản bằng username và mật khẩu plaintext trực tiếp từ SQL Server.
- Quản lý phiên đăng nhập hiện tại (`CurrentLoginUser`): Lưu trữ thông tin tài khoản đang làm việc (UserId, FullName, Role).
- Phân quyền giao diện: Phân biệt rõ quyền `Admin` và quyền `Staff` (Nhân viên lễ tân).

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Giao diện `LoginWindow.xaml`:
  - Ô nhập Tên đăng nhập (`TextBox txtUsername`).
  - Ô nhập Mật khẩu (`PasswordBox txtPassword`).
  - Nút Đăng nhập (`Button btnLogin`) và nút Thoát (`Button btnExit`).
- Trong sự kiện bấm nút Đăng nhập:
  - Lấy chuỗi username và password.
  - Gọi `UserDAO.Instance.Login(username, password)` thực thi câu lệnh SQL:
    `SELECT * FROM Users WHERE Username = @username AND Password = @password`.
  - Nếu đúng: Lưu đối tượng `User` vào biến tĩnh toàn cục (ví dụ `App.CurrentUser`), đóng `LoginWindow` và mở `MainWindow.Show()`.
  - Nếu sai: Hiển thị `MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!", "Lỗi đăng nhập")`.
- Trong `MainWindow.xaml`:
  - Header hiển thị: "Xin chào: [FullName] ([Role])".
  - Nút "Đăng xuất": Đóng `MainWindow`, reset `App.CurrentUser = null` và mở lại `LoginWindow.Show()`.
  - Kiểm tra `App.CurrentUser.Role`: Nếu là `Staff`, ẩn hoặc vô hiệu hóa các nút chức năng quản trị ("Quản lý loại phòng", "Báo cáo thống kê").

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Màn hình LoginWindow.xaml đẹp mắt với TextBox Username, PasswordBox và nút Đăng nhập.
- [ ] UserDAO.cs truy vấn kiểm tra tài khoản trong bảng Users theo username và mật khẩu plaintext.
- [ ] Đăng nhập đúng: Đóng LoginWindow, mở MainWindow, hiển thị Tên người dùng và Chức vụ ở thanh Header.
- [ ] Phân quyền theo cột Role: Nếu là Nhân viên (Staff), tự động ẩn hoặc vô hiệu hóa các nút Quản lý loại phòng và Báo cáo doanh thu trên Sidebar menu.
- [ ] Hỗ trợ nút Đăng xuất (Logout) quay trở lại màn hình đăng nhập.

### Ca biên cần xử lý (Edge Cases):
- Để trống Username hoặc Password khi bấm Đăng nhập -> Hiển thị cảnh báo yêu cầu nhập đầy đủ.
- Tài khoản phân biệt hoa/thường hoặc có khoảng trắng thừa -> Dùng `.Trim()` khi đọc dữ liệu đầu vào.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Thử đăng nhập bằng tài khoản `admin` / `123456`: Đăng nhập thành công, thấy đầy đủ menu.
- Thử đăng nhập bằng tài khoản `staff` / `123456`: Đăng nhập thành công, menu Loại phòng và Báo cáo bị ẩn/khóa.
- Thử nhập sai mật khẩu: Hiển thị hộp thoại báo lỗi thân thiện.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-03.md` lên GitHub Issue #60.*
