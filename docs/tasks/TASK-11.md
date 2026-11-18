# [TASK-11] Module Báo cáo & Thống kê doanh thu trực quan (LiveCharts2)

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-11`  
**GitHub Issue**: [#64](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/64)  
**Thành viên đảm nhiệm**: **Member C (@ngocthuyen1)**  
**Mức độ ưu tiên**: `Medium`  
**Nhánh làm việc**: `feature/revenue-stats-report`  
**Milestone**: Mốc 3: Báo Cáo Thống Kê & Chốt Demo (19/12 – 30/12)  
**Khung thời gian dự kiến**: `20/12/2026 – 24/12/2026` *(Chốt Feature Freeze toàn dự án vào ngày `24/12/2026`)*  
**Labels**: `stats`, `report`, `ui`, `m3`, `ready-for-agent`  
**Blocked by**: #63 (TASK-10)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Cài đặt package `LiveChartsCore.SkiaSharpView.WPF` (`2.0.5`) cho project `HotelManagement`.
- Xây dựng lớp tổng hợp số liệu thuần `ReportAggregator.cs` trong `Services/` (seam kiểm thử tự động TDD), hoàn thiện `ReportViewModel.cs` và giao diện `ReportView.xaml` (chỉ dành cho `Administrator`).
- **Tuân thủ quy chuẩn Windows UI Kit tại `docs/design/DESIGN-BRIEF.md` mục 2.3, 6.5 & 7.2**:
  - 3 thẻ KPI dùng chung bề mặt sáng trung tính (`Card Surface`), phân cấp bằng kiểu chữ Segoe UI Variable và khoảng cách (điểm nhấn nhỏ `#005FB8` nếu cần — **tuyệt đối không tô 3 nền màu navy / xanh dương / xanh ngọc**).
  - Thanh `ProgressBar` hiển thị tỷ lệ công suất phòng hiện tại (`Số phòng Đang ở / Tổng số phòng`).
  - Biểu đồ đường doanh thu theo ngày (`CartesianChart` + `LineSeries` đơn giản, rõ ràng trên máy chiếu).
  - Bảng `DataGrid` danh sách hóa đơn `Đã thanh toán` trong khoảng ngày lọc.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- **Seam `Services/ReportAggregator.cs` (TDD-first)**:
  - Validate khoảng lọc: `FromDate.Date <= ToDate.Date` (nếu `FromDate > ToDate` trả về lỗi nhắc chọn lại khoảng ngày hợp lệ).
  - Tổng hợp từ danh sách hóa đơn `Đã thanh toán` trong khoảng `[FromDate.Date, ToDate.Date.AddDays(1).AddTicks(-1)]`:
    1. **Tổng doanh thu**: `SUM(TotalAmount)` (`#,##0 đ`).
    2. **Số lượt thuê hoàn tất**: `COUNT(InvoiceId)`.
    3. **Doanh thu dịch vụ**: `SUM(ServiceCharge)` (`#,##0 đ`).
  - Tính **Tỷ lệ lấp đầy phòng hiện tại**: `OccupiedCount / TotalRoomCount * 100` (tránh chia cho 0 khi `TotalRoomCount == 0`).
  - Nhóm dữ liệu biểu đồ đường theo ngày (`PaymentDate.Date`): trả về chuỗi nhãn ngày (`dd/MM`) và tổng doanh thu từng ngày.
- **Giao diện `ReportView.xaml` & Trạng thái rỗng (`DESIGN-BRIEF.md` mục 6.5)**:
  - Bộ lọc trên cùng: `Từ ngày` (`DatePicker`, mặc định ngày đầu tháng hiện tại), `Đến ngày` (`DatePicker`, mặc định hôm nay), nút `Xem báo cáo`.
  - Khi khoảng lọc **không có hóa đơn nào**: 3 thẻ KPI hiển thị `0 đ` / `0 lượt`, khung biểu đồ và bảng hiển thị trạng thái chủ đích `"Không có dữ liệu doanh thu trong khoảng thời gian đã chọn"` (không để khung trắng lỗi hay ném ngoại lệ null).

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Cài đặt `LiveChartsCore.SkiaSharpView.WPF` (2.0.5), `dotnet build` & `dotnet test` xanh 100%.
- [ ] `Services/ReportAggregator.cs` có unit test đầy đủ trong `HotelManagement.Tests` cho: cộng gộp 3 KPI, nhóm doanh thu theo ngày, tính tỷ lệ công suất phòng (kể cả khi 0 phòng), khoảng ngày rỗng (`0 đ`) và lỗi `FromDate > ToDate`.
- [ ] `ReportView.xaml` thiết kế 3 thẻ KPI bề mặt `Card Surface` trung tính (không tô nền đậm), thanh `ProgressBar` công suất phòng, biểu đồ đường `LiveCharts2` và `DataGrid` hóa đơn đã thanh toán.
- [ ] Khoảng lọc bao trọn cả ngày cuối (`ToDate` tính đến `23:59:59` để không bỏ sót hóa đơn thanh toán trong ngày hôm nay).
- [ ] Tài khoản `Staff` không thấy và không truy cập được màn Báo cáo.

### Ca biên cần xử lý (Edge Cases):
- Hóa đơn vừa thanh toán lúc `14:30` ngày hôm nay (`ToDate = Today`): nếu câu SQL dùng `PaymentDate <= @toDate` với `@toDate = 00:00:00` sẽ bị lọt mất hóa đơn trong ngày -> bắt buộc lọc `PaymentDate >= @fromDateStart AND PaymentDate < @toDateExclusiveNextDay`!
- Khoảng ngày không có giao dịch: KPI hiện `0 đ`, biểu đồ và bảng hiển thị trạng thái trống rõ ràng, không crash.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động (`TDD` seam): `dotnet test` kiểm chứng `ReportAggregator` đặc biệt là ca biên hóa đơn có giờ trong ngày cuối cùng (`14:30` ngày `ToDate`) và ca dữ liệu rỗng.
- Trực quan: Vừa thanh toán xong hóa đơn `1,070,000 đ` ở `TASK-10`, mở ngay màn Báo cáo thấy tổng doanh thu cộng thêm đúng `1,070,000 đ` và điểm dữ liệu ngày hôm nay nổi lên trên biểu đồ đường.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-11.md` lên GitHub Issue #64.*
