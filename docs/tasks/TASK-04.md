# [TASK-04] Module Đăng nhập & Phân quyền theo vai trò User

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-04`  
**GitHub Issue**: [#61](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/61)  
**Thành viên đảm nhiệm**: **Member A (@phantatdung233)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/auth-login`  
**Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)  
**Blocked by**: #59 (TASK-02), #60 (TASK-03)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Màn hình `LoginView` và `LoginViewModel` với TextBox nhập Username và PasswordBox nhập mật khẩu.
- Xây dựng helper `PasswordBoxHelper` (Attached Property) để bind password an toàn trong chuẩn MVVM.
- Xây dựng dịch vụ `IAuthService` và `AuthService` xử lý đăng nhập, băm mật khẩu (SHA256 hoặc PBKDF2).
- Quản lý phiên làm việc hiện tại qua `ICurrentUserService` lưu thông tin User và Role đã đăng nhập.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Kiểm tra tài khoản từ bảng `User`. So khớp băm mật khẩu bảo mật (có Salt).
- Đăng nhập thành công: Chuyển hướng sang màn hình chính (`MainViewModel` hoặc `RoomMatrixViewModel`), hiển thị tên và vai trò trên Header.
- Đăng nhập thất bại: Hiển thị thông báo lỗi rõ ràng mà không làm gián đoạn ứng dụng.
- Phân quyền: Cung cấp thuộc tính `IsAdmin` để ẩn/hiện các chức năng quản trị (như Quản lý Loại phòng, Báo cáo Doanh thu).

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Màn hình LoginView với binding mật khẩu bảo mật (PasswordBox binding helper/SecurePassword).
- [ ] AuthService xác thực tài khoản từ bảng User, mật khẩu có băm mã hóa an toàn.
- [ ] Lưu trạng thái phiên làm việc (Current User Session).
- [ ] Ẩn/Hiện các chức năng theo vai trò của người dùng (Admin vs Nhân viên).

### Ca biên cần xử lý (Edge Cases):
- Nhập khoảng trắng, mật khẩu rỗng, hoặc tài khoản không tồn tại.
- Xử lý đăng xuất (Logout) xóa sạch phiên làm việc và quay về `LoginView`.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Test đăng nhập đúng tài khoản/mật khẩu -> trả về User hợp lệ.
- Test đăng nhập sai mật khẩu -> ném ngoại lệ hoặc trả về thất bại kèm thông báo.
- Test phân quyền: Kiểm tra cờ `IsAdmin` đúng với vai trò của user.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-04.md` lên GitHub Issue #61.*
