# [TASK-19] [Nâng Cao 2] Tối ưu truy vấn thống kê bất đồng bộ (Async Stats)

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-19`  
**GitHub Issue**: [#76](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/76)  
**Thành viên đảm nhiệm**: **Member C (@ngocthuyen1)**  
**Mức độ ưu tiên**: `Medium`  
**Nhánh làm việc**: `feature/advanced-async-stats`  
**Milestone**: Mốc 2: Ghép Luồng E2E & Nâng Cao (Tuần 4-5)  
**Blocked by**: #71 (TASK-18)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Tối ưu toàn bộ logic tổng hợp báo cáo bằng mô hình bất đồng bộ (`async/await`).
- Sử dụng các phương thức EF Core async (`ToListAsync()`, `SumAsync()`, `CountAsync()`) hoặc `Task.Run` cho các phép tính toán tổng hợp nặng.
- Hiển thị `ProgressBar` hoặc loading spinner trong lúc dữ liệu đang được tính toán ngầm.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Đảm bảo UI hoàn toàn không bị đơ giật (freeze) khi tải lượng dữ liệu lớn.
- Khóa (Disable) nút "Xem báo cáo" tạm thời trong khi đang tải để tránh gọi lặp (debouncing).

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Toàn bộ logic tổng hợp thống kê được chạy bất đồng bộ qua async/await và Task.Run hoặc EF Core ToListAsync().
- [ ] Hiển thị ProgressBar hoặc vòng xoay loading khi đang tải dữ liệu báo cáo lớn.
- [ ] Đảm bảo UI hoàn toàn mượt mà, không bị hiện tượng treo (freeze) hoặc giật lag trong quá trình tính toán.

### Ca biên cần xử lý (Edge Cases):
- Hủy tác vụ khi người dùng chuyển trang đột ngột bằng `CancellationToken`.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Test phương thức async hoàn thành đúng dữ liệu mà không gây Deadlock trên UI Dispatcher thread.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-19.md` lên GitHub Issue #76.*
