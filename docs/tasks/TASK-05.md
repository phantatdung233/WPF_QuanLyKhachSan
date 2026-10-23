# [TASK-05] Module Quản lý Khách hàng (Customer CRUD & Tìm kiếm)

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-05`  
**GitHub Issue**: [#78](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/78)  
**Thành viên đảm nhiệm**: **Member B (@xuanduong29)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/customer-management`  
**Milestone**: Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (Tuần 1-3)  
**Labels**: `customer`, `crud`, `m1`  
**Blocked by**: #59 (TASK-02)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng `CustomerDAO.cs` trong thư mục `DAO/` và `Customer.cs` trong `Models/`.
- Xây dựng `CustomerViewModel.cs` và giao diện `CustomerView.xaml`.
- Hỗ trợ xem danh sách toàn bộ khách hàng bằng `DataGrid`.
- Hỗ trợ thêm mới thông tin khách hàng, cập nhật thông tin khách hàng.
- Thanh tìm kiếm nhanh thời gian thực theo Họ tên hoặc CMND/CCCD/SĐT.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Thông tin khách hàng gồm: `CustomerId`, `FullName` (Họ và tên), `CitizenId` (CMND/CCCD), `Phone` (Số điện thoại), `Email`.
- `CustomerView.xaml`:
  - Ô tìm kiếm (`TextBox`): Khi người dùng gõ từ khóa, ViewModel lọc danh sách qua câu lệnh:
    `SELECT * FROM Customers WHERE FullName LIKE '%' + @kw + '%' OR CitizenId LIKE '%' + @kw + '%' OR Phone LIKE '%' + @kw + '%'`.
  - Nút "Thêm khách hàng": Mở `CustomerDialog.xaml`.
  - Bảng danh sách `DataGrid`: Hiển thị thông tin khách hàng kèm nút "Sửa".
- Form Dialog Thêm / Sửa khách hàng:
  - Bắt buộc nhập Họ và tên.
  - Kiểm tra định dạng CCCD: Phải là chuỗi số có độ dài 9 hoặc 12 ký tự.
  - Kiểm tra Số điện thoại: Phải bắt đầu bằng số 0 và có 10 ký tự số.
  - Lưu vào CSDL qua `CustomerDAO.Instance.InsertCustomer(...)` hoặc `UpdateCustomer(...)`.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] CustomerDAO.cs: Viết các hàm lấy danh sách, thêm, sửa, tìm kiếm khách hàng từ bảng Customers.
- [ ] CustomerView.xaml: DataGrid hiển thị danh sách khách hàng gồm: Mã KH, Họ tên, CMND/CCCD, Số điện thoại, Email.
- [ ] Thanh tìm kiếm nhanh khách hàng theo Họ tên hoặc CCCD/SĐT cập nhật danh sách ngay khi gõ.
- [ ] Dialog Thêm/Sửa khách hàng: Validate bắt buộc nhập Họ tên, CCCD (độ dài 9 hoặc 12 số) và Số điện thoại hợp lệ.

### Ca biên cần xử lý (Edge Cases):
- Nhập CCCD hoặc SĐT chứa chữ cái hoặc ký tự đặc biệt -> Hiển thị thông báo nhắc nhở cụ thể.
- Kiểm tra trùng lặp số CCCD giữa các khách hàng khác nhau.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Thêm mới một khách hàng: Nguyễn Văn An, CCCD: `079201001234`, SĐT: `0901234567`.
- Gõ vào ô tìm kiếm: `079` -> Kết quả danh sách chỉ hiển thị khách hàng Nguyễn Văn An.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-05.md` lên GitHub Issue #78.*
