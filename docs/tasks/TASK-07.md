# [TASK-07] Quản trị Git Flow, Branch Protection & CI Workflow

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-07`  
**GitHub Issue**: [#62](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/62)  
**Thành viên đảm nhiệm**: **Member A (@phantatdung233)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/ci-workflow-governance`  
**Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)  
**Blocked by**: None (Thực hiện song song)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Thiết lập nhánh `develop` làm nhánh tích hợp trung gian từ `main`.
- Cấu hình Pull Request Template chuẩn (`.github/pull_request_template.md`).
- Xây dựng hoặc hoàn thiện GitHub Actions workflow tự động kiểm tra build solution khi mở PR vào `develop`/`main`.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Thiết lập quy tắc review vòng tròn:
  - Member A PR -> Member B review.
  - Member B PR -> Member C review.
  - Member C PR -> Member A review.
- Đảm bảo nhánh bảo vệ không cho phép merge trực tiếp vào main mà không qua PR.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Thiết lập nhánh develop từ main, cấu hình Pull Request Template.
- [ ] Tích hợp workflow GitHub Actions tự động tạo Milestone, Label và Issue từ TASKS.md.
- [ ] Thực thi quy tắc review vòng tròn cho các PR trong suốt các Sprint.

### Ca biên cần xử lý (Edge Cases):
- Merge conflict khi nhiều thành viên cùng chỉnh sửa `App.xaml` hoặc `HotelDbContext.cs`.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tạo PR thử nghiệm và kiểm tra CI trigger chạy đúng kịch bản.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-07.md` lên GitHub Issue #62.*
