# [TASK-09] Module Quản lý Dịch vụ (Service) CRUD

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-09`  
**GitHub Issue**: [#81](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/81)  
**Thành viên đảm nhiệm**: **Member B (@xuanduong29)**  
**Mức độ ưu tiên**: `Medium`  
**Nhánh làm việc**: `feature/service-management`  
**Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)  
**Blocked by**: #59 (TASK-02), #60 (TASK-03)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng `ServiceView` và `ServiceViewModel`.
- Quản lý danh mục dịch vụ phát sinh của khách sạn: Giặt ủi, Nước khoáng, Ăn sáng buffet, Đưa đón sân bay, Nước ngọt, Spa mini.
- Các trường: Tên dịch vụ, Đơn giá, Mô tả, Trạng thái hoạt động (`IsActive`).

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Cho phép thêm mới, sửa đổi tên dịch vụ, đơn giá và bật/tắt phục vụ.
- Validate: Đơn giá >= 0; tên dịch vụ không để trống.
- Định dạng hiển thị tiền tệ VNĐ trên DataGrid (`#,##0 đ`).

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] ServiceView hiển thị danh mục các dịch vụ khách sạn (giặt ủi, nước uống, ăn sáng, đưa đón...).
- [ ] Hỗ trợ thêm mới, sửa đổi tên dịch vụ, đơn giá và trạng thái còn phục vụ hay không.
- [ ] Validate đơn giá dịch vụ >= 0.

### Ca biên cần xử lý (Edge Cases):
- Nhập giá tiền âm hoặc ký tự không hợp lệ.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Test thêm dịch vụ với đơn giá hợp lệ.
- Test validate chặn đơn giá âm.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-09.md` lên GitHub Issue #81.*
