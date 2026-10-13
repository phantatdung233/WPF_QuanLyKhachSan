# [TASK-18] Module Thống kê & Báo cáo với biểu đồ LiveCharts2

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-18`  
**GitHub Issue**: [#71](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/71)  
**Thành viên đảm nhiệm**: **Member C (@ngocthuyen1)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/stats-livecharts`  
**Milestone**: Mốc 2: Ghép Luồng E2E & Nâng Cao (Tuần 4-5)  
**Blocked by**: #69 (TASK-16)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Cài đặt thư viện `LiveChartsCore.SkiaSharpView.WPF` (phiên bản v2).
- Xây dựng `StatsView` và `StatsViewModel`.
- Biểu đồ 1: Biểu đồ cột/đường doanh thu theo ngày hoặc tháng.
- Biểu đồ 2: Biểu đồ tròn (Pie Chart) tỷ trọng doanh thu giữa Tiền phòng và Tiền dịch vụ, hoặc tỷ lệ công suất phòng.
- Bộ lọc khoảng thời gian: Từ ngày (From Date) - Đến ngày (To Date).

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Truy vấn tổng hợp từ bảng `Invoice` (chỉ tính các hóa đơn đã thanh toán `InvoiceStatus.Paid`).
- Thống kê các chỉ số tổng quan (Cards KPI): Tổng doanh thu, Số lượt thuê phòng, Doanh thu dịch vụ trung bình.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Cài đặt thư viện LiveChartsCore.SkiaSharpView.WPF.
- [ ] Hiển thị biểu đồ cột/đường doanh thu theo ngày/tháng.
- [ ] Hiển thị biểu đồ tròn (Pie chart) tỷ lệ công suất phòng hoặc tỷ trọng doanh thu dịch vụ.
- [ ] Bộ lọc thống kê linh hoạt theo khoảng thời gian (Từ ngày - Đến ngày).

### Ca biên cần xử lý (Edge Cases):
- Khoảng thời gian lọc không có hóa đơn nào: Biểu đồ hiển thị trạng thái trống thân thiện, không crash.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Unit test thuật toán tính tổng doanh thu theo khoảng thời gian trả về kết quả chính xác.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-18.md` lên GitHub Issue #71.*
