# [TASK-08] Module Quản lý Khách hàng (Customer) CRUD & Tìm kiếm

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-08`  
**GitHub Issue**: [#80](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/80)  
**Thành viên đảm nhiệm**: **Member B (@xuanduong29)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/customer-management`  
**Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)  
**Blocked by**: #59 (TASK-02), #60 (TASK-03)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng `CustomerView` và `CustomerViewModel`.
- Quản lý thông tin khách hàng: Họ tên, CMND/CCCD, Số điện thoại, Email, Địa chỉ.
- Thanh tìm kiếm realtime (lọc ngay khi người dùng gõ phím) theo Họ tên, CCCD hoặc SĐT.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Regex validation:
  - CMND/CCCD: Chuỗi đúng 9 hoặc 12 chữ số (`^(\d{9}|\d{12})$`).
  - Số điện thoại: Định dạng số điện thoại Việt Nam hợp lệ (10 chữ số, bắt đầu bằng 0: `^0\d{9}$`).
  - Email: Định dạng email hợp lệ (nếu có nhập).
- Hiển thị thông báo lỗi trực tiếp trên giao diện thông qua `INotifyDataErrorInfo` hoặc IDataErrorInfo của ViewModel.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] CustomerView và CustomerViewModel hỗ trợ CRUD thông tin khách hàng.
- [ ] Trường dữ liệu: Họ tên, CMND/CCCD, Số điện thoại, Email, Địa chỉ.
- [ ] Validate định dạng CCCD/CMND (9 hoặc 12 số) và số điện thoại hợp lệ.
- [ ] Thanh tìm kiếm nhanh khách hàng theo Họ tên hoặc CCCD/SĐT dạng realtime binding.

### Ca biên cần xử lý (Edge Cases):
- Nhập CCCD có chứa ký tự chữ hoặc không đủ 9/12 số.
- Nhập trùng số CCCD với khách hàng đã có trong CSDL.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Test regex validation cho CCCD 9 số, 12 số và các ca không hợp lệ (8 số, 10 số, chứa chữ).
- Test tìm kiếm khách hàng trả về danh sách chính xác khi nhập một phần tên hoặc số CCCD.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-08.md` lên GitHub Issue #80.*
