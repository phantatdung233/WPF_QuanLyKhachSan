# [TASK-22] Kiểm thử hồi quy, Sửa lỗi chặn (Blocker Bugs) & Hardening

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-22`  
**GitHub Issue**: [#74](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/74)  
**Thành viên đảm nhiệm**: **Member B (@xuanduong29)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/regression-bugfixes`  
**Milestone**: Mốc 3: Chốt Demo & Feature Freeze (Tuần 6)  
**Blocked by**: #72 (TASK-20)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Rà soát toàn bộ các lỗi tiềm ẩn và xử lý ngoại lệ (Hardening):
  - Bọc `try-catch` tại các thao tác nhạy cảm (truy vấn CSDL, lưu file, chuyển đổi kiểu).
  - Hiển thị hộp thoại thông báo lỗi thân thiện (`MessageBox` chuẩn hoặc Custom Dialog), không để ứng dụng crash văng ra desktop.
- Kiểm thử các ca biên khắc nghiệt (Stress/Boundary testing).
- Đảm bảo cơ chế khôi phục CSDL mẫu sạch hoạt động tức thì phục vụ demo.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Xử lý các bug blocker ưu tiên cao nhất.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Kiểm tra các ca biên (edge cases): Đặt phòng cùng ngày, ngày trả trước ngày đến, số lượng dịch vụ âm, nhập CCCD sai định dạng.
- [ ] Xử lý triệt để toàn bộ exception handling (Try-Catch, hiển thị hộp thoại thông báo lỗi thân thiện).
- [ ] Đảm bảo cơ sở dữ liệu SQLite tự khôi phục dữ liệu demo sạch chỉ với 1 thao tác nếu cần reset lại demo.

### Ca biên cần xử lý (Edge Cases):
- Mất kết nối CSDL SQLite (file bị khóa bởi process khác) -> thông báo hướng dẫn người dùng.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Kiểm thử hồi quy đảm bảo các bug đã sửa không bị tái diễn.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-22.md` lên GitHub Issue #74.*
