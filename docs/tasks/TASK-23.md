# [TASK-23] Chuẩn bị Slide thuyết trình, Kịch bản Demo & Tập dượt Rehearsal

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-23`  
**GitHub Issue**: [#75](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/75)  
**Thành viên đảm nhiệm**: **Member C (@ngocthuyen1)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/demo-rehearsal-slides`  
**Milestone**: Mốc 3: Chốt Demo & Feature Freeze (Tuần 6)  
**Blocked by**: #73 (TASK-21), #74 (TASK-22)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Soạn thảo Slide thuyết trình chuyên nghiệp:
  - Giới thiệu đề tài & thành viên.
  - Kiến trúc hệ thống: WPF .NET 8, MVVM Toolkit, SQLite EF Core 8 bảng.
  - Điểm nổi bật & tính năng nâng cao (Sơ đồ phòng DataTrigger, Async LiveCharts2, Thuật toán kiểm tra xung đột).
  - Demo sản phẩm trực quan.
- Soạn thảo Kịch bản Demo (Demo Script) từng bước căn thời gian chính xác cho 3 thành viên:
  - Member A: Mở màn, kiến trúc, đăng nhập & phân quyền, quản lý phòng.
  - Member B: Nghiệp vụ khách hàng, đặt phòng, check-in, gọi dịch vụ phòng.
  - Member C: Sơ đồ phòng DataTrigger, check-out hóa đơn, biểu đồ thống kê LiveCharts2, kết luận.
- Tổ chức tối thiểu 2 buổi Rehearsal chạy thử và trả lời câu hỏi phản biện.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Chuẩn bị sẵn kịch bản dự phòng nếu có sự cố kỹ thuật trong buổi demo.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Hoàn thành Slide thuyết trình: Giới thiệu đề tài, Kiến trúc WPF MVVM, CSDL 8 bảng, Demo tính năng chính và Điểm nổi bật nâng cao.
- [ ] Soạn thảo Kịch bản demo từng bước (Demo Script) căn thời gian chính xác cho 3 thành viên.
- [ ] Nhóm tổ chức thành công tối thiểu 2 lần Rehearsal chạy thử toàn bộ bài thuyết trình và trả lời câu hỏi phản biện.

### Ca biên cần xử lý (Edge Cases):
- Căn chỉnh thời lượng thuyết trình không vượt quá thời gian quy định của môn học (10-15 phút).

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Chạy thử demo 2 lần thành công không gặp lỗi kỹ thuật.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-23.md` lên GitHub Issue #75.*
