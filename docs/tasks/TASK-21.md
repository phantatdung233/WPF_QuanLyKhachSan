# [TASK-21] Feature Freeze, Refactoring & Chuẩn hóa Naming Convention

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-21`  
**GitHub Issue**: [#73](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/73)  
**Thành viên đảm nhiệm**: **Member A (@phantatdung233)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/refactor-code-cleanup`  
**Milestone**: Mốc 3: Chốt Demo & Feature Freeze (Tuần 6)  
**Blocked by**: #72 (TASK-20)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Thực hiện Đóng băng tính năng (Feature Freeze) 2 ngày trước hạn nộp/thuyết trình. Không thêm tính năng mới.
- Dọn dẹp mã nguồn toàn bộ Solution:
  - Xóa bỏ dead code, file thừa, các dòng debug `Console.WriteLine`.
  - Chuẩn hóa quy ước đặt tên C# (PascalCase cho Class, Method, Property; camelCase cho biến cục bộ và tham số; `_camelCase` cho private fields).
  - Định dạng format code đồng bộ (Clean Code, Indentation).

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Chạy Rebuild Solution đảm bảo 0 Errors, 0 Warnings nghiêm trọng.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Thực hiện Đóng băng tính năng (Feature Freeze) trước ngày báo cáo chính thức ít nhất 2 ngày.
- [ ] Kiểm tra và xóa toàn bộ dead code, file tạm, console logs thừa.
- [ ] Chuẩn hóa format code, comment và naming convention C# toàn bộ solution.
- [ ] Đảm bảo build Clean/Rebuild không phát sinh warning cảnh báo nghiêm trọng.

### Ca biên cần xử lý (Edge Cases):
- Refactor không làm thay đổi hành vi đã chạy đúng của các tính năng trước đó.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Chạy lại toàn bộ test suite để kiểm tra tính toàn vẹn sau khi refactor.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-21.md` lên GitHub Issue #73.*
