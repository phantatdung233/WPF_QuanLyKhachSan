# [TASK-11] Module Báo cáo & Thống kê doanh thu thuần XAML

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-11`  
**GitHub Issue**: [#64](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/64)  
**Thành viên đảm nhiệm**: **Member C (@ngocthuyen1)**  
**Mức độ ưu tiên**: `Medium`  
**Nhánh làm việc**: `feature/revenue-stats-report`  
**Milestone**: Mốc 3: Báo Cáo Thống Kê & Chốt Demo (Tuần 5-6)  
**Blocked by**: #63 (TASK-10)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng `ReportViewModel.cs` và giao diện `ReportView.xaml`.
- Thống kê doanh thu và hoạt động kinh doanh của khách sạn theo khoảng thời gian tùy chọn (Từ ngày - Đến ngày).
- Không sử dụng thư viện biểu đồ bên ngoài (như LiveCharts2 / SkiaSharp) để loại bỏ 100% rủi ro lỗi package hoặc lỗi font native.
- Sử dụng các thành phần giao diện WPF XAML thuần:
  - 3 Thẻ chỉ số KPI nổi bật (Cards KPI): Tổng doanh thu (VNĐ), Tổng số lượt thuê phòng, Tổng tiền dịch vụ phát sinh.
  - Thanh `ProgressBar` trực quan hiển thị tỷ lệ công suất phòng (Số phòng đang có khách / Tổng số phòng).
  - Bảng `DataGrid` chi tiết danh sách các hóa đơn đã thanh toán trong khoảng thời gian lọc.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- `ReportView.xaml`:
  - Thanh điều khiển trên cùng:
    - `DatePicker dpFromDate`: Chọn ngày bắt đầu (mặc định đầu tháng hiện tại).
    - `DatePicker dpToDate`: Chọn ngày kết thúc (mặc định ngày hiện tại).
    - `Button btnFilter`: Bấm "Xem báo cáo".
  - Hàng thẻ KPI:
    - Thẻ 1: **Tổng Doanh Thu** (Nền xanh navy, chữ to, định dạng `#,##0 đ`).
    - Thẻ 2: **Lượt Thuê Phòng** (Nền xanh dương, hiển thị số nguyên, ví dụ: 25 lượt).
    - Thẻ 3: **Doanh Thu Dịch Vụ** (Nền xanh ngọc, định dạng `#,##0 đ`).
  - Khung đo Công suất phòng (Occupancy Rate):
    - TextBlock: "Tỷ lệ lấp đầy phòng hiện tại: 75% (9/12 phòng)".
    - `ProgressBar`: Value = 75, Height = 20, Foreground màu xanh lá cây hoặc cam.
  - Bảng chi tiết `DataGrid`:
    - Cột: Mã HĐ, Tên khách hàng, Số phòng, Ngày thanh toán, Tiền phòng, Tiền dịch vụ, Tổng tiền.
- Câu truy vấn SQL tổng hợp:
  ```sql
  SELECT * FROM Invoices 
  WHERE Status = N'Đã thanh toán' 
    AND PaymentDate >= @fromDate AND PaymentDate <= @toDate
  ```
  Tính tổng doanh thu bằng `DataTable.Compute("SUM(TotalAmount)", "")` hoặc LINQ trên danh sách ViewModel.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] ReportView.xaml: Có bộ lọc khoảng thời gian (Từ ngày - Đến ngày).
- [ ] Thiết kế 3 thẻ KPI số liệu nổi bật: Tổng doanh thu (VNĐ), Tổng số lượt khách thuê, Tổng tiền dịch vụ phát sinh.
- [ ] Thanh ProgressBar trực quan hiển thị tỷ lệ công suất phòng (Số phòng đang có khách / Tổng số phòng).
- [ ] Bảng DataGrid chi tiết danh sách các hóa đơn đã thanh toán trong khoảng thời gian lọc.
- [ ] Không sử dụng thư viện ngoài, đảm bảo 100% build mượt mà, không giật lag.

### Ca biên cần xử lý (Edge Cases):
- Khoảng thời gian lọc không có hóa đơn nào: Các thẻ KPI hiển thị `0 đ`, bảng DataGrid trống thân thiện, không bị crash lỗi null.
- Người dùng chọn Ngày bắt đầu lớn hơn Ngày kết thúc -> Hiển thị thông báo nhắc nhở chọn lại ngày hợp lệ.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Chọn khoảng thời gian chứa 3 hóa đơn đã thanh toán: Tổng doanh thu hiển thị đúng tổng giá trị của 3 hóa đơn đó.
- Thử chọn một khoảng thời gian trong tương lai chưa có giao dịch: Giao diện hiển thị tổng doanh thu 0 đ, không phát sinh ngoại lệ.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-11.md` lên GitHub Issue #64.*
