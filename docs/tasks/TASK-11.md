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
- **Tuân thủ quy trình 2 Pha (`CLAUDE.md`) và chuẩn `DESIGN.md`**:
  - 3 thẻ KPI dùng chung bề mặt sáng trung tính (`CardSurfaceBrush` `#FFFFFF`), phân cấp bằng `KpiValueTextStyle` (`24px` SemiBold) và khoảng cách (**tuyệt đối không tô 3 nền màu navy / xanh dương / xanh ngọc**).
  - Thanh `ProgressBar` hiển thị tỷ lệ công suất phòng hiện tại (`Số phòng Đang ở / Tổng số phòng`).
  - Biểu đồ đường doanh thu theo ngày (`CartesianChart` + `LineSeries` đơn giản, rõ ràng trên máy chiếu).
  - Bảng `DataGrid` danh sách hóa đơn `Đã thanh toán` trong khoảng ngày lọc.

---

## 2. Hướng dẫn nghiệp vụ & UI Contract (Theo `DESIGN.md`)
- **Seam `Services/ReportAggregator.cs` (Pha 1 — Logic & State First, TDD-first)**:
  - Validate khoảng lọc: `FromDate.Date <= ToDate.Date` (nếu `FromDate > ToDate` trả về lỗi nhắc chọn lại khoảng ngày hợp lệ).
  - Tổng hợp từ danh sách hóa đơn `Đã thanh toán` trong khoảng `[FromDate.Date, ToDate.Date.AddDays(1))`:
    1. **Tổng doanh thu**: `SUM(TotalAmount)` (`#,##0 đ`).
    2. **Số lượt thuê hoàn tất**: `COUNT(InvoiceId)`.
    3. **Doanh thu dịch vụ**: `SUM(ServiceCharge)` (`#,##0 đ`).
  - Tính **Tỷ lệ lấp đầy phòng hiện tại**: `OccupiedCount / TotalRoomCount * 100` (tránh chia cho 0 khi `TotalRoomCount == 0`).
  - Nhóm dữ liệu biểu đồ đường theo ngày (`PaymentDate.Date`): trả về chuỗi nhãn ngày (`dd/MM`) và tổng doanh thu từng ngày.
- **UI Contract (`ReportView` — `Administrator` only, Pha 2)**:
  - **Dữ liệu hiển thị**: Bộ lọc `Từ ngày` (mặc định đầu tháng) & `Đến ngày` (mặc định hôm nay), 3 thẻ KPI (`Tổng doanh thu`, `Lượt thuê hoàn tất`, `Doanh thu dịch vụ`), nhãn + thanh công suất phòng hiện tại (`"Tỷ lệ lấp đầy phòng hiện tại: X% (Y/Z phòng)"`), biểu đồ đường doanh thu theo ngày và bảng hóa đơn `Đã thanh toán`.
  - **Primitives bắt buộc (`WPF-UI` & `LiveCharts2`)**: `DatePicker`, **duy nhất 1 Primary CTA** `ui:Button` (`Xem báo cáo`, `Appearance="Primary"`), 3 `Border` (`CardSurfaceBorderStyle`), `ProgressBar` (`AccentBrush`), `CartesianChart` (`LineSeries`), `DataGrid` (`RowHeight="40"`), `ui:InfoBar`, `ui:ProgressRing`.
  - **4 Trạng thái UI bắt buộc**:
    1. *Loading*: Hiện `ui:ProgressRing` và khóa nút `Xem báo cáo` khi đang truy vấn tổng hợp dữ liệu.
    2. *Empty*: Khi khoảng lọc không có hóa đơn thanh toán nào, 3 thẻ KPI hiển thị `0 đ` / `0 lượt` / `0 đ`, vùng biểu đồ và bảng hiển thị thông báo chủ đích `"Không có dữ liệu doanh thu trong khoảng thời gian đã chọn"` (không để khung trắng lỗi hay crash).
    3. *Error*: Khi `FromDate > ToDate`, hiển thị `ui:InfoBar` (`Severity="Error"`) `"Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc. Vui lòng chọn lại."` và giữ nguyên dữ liệu hợp lệ trước đó.
    4. *Success*: Hiển thị đầy đủ 3 KPI định dạng `#,##0 đ`, thanh công suất, đường biểu đồ `LiveCharts2` và bảng chi tiết hóa đơn.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Cài đặt `LiveChartsCore.SkiaSharpView.WPF` (2.0.5), `dotnet build` & `dotnet test` xanh 100%.
- [ ] `Services/ReportAggregator.cs` có unit test đầy đủ trong `HotelManagement.Tests` cho: cộng gộp 3 KPI, nhóm doanh thu theo ngày, tính tỷ lệ công suất phòng (kể cả khi 0 phòng), khoảng ngày rỗng (`0 đ`) và lỗi `FromDate > ToDate`.
- [ ] `ReportView.xaml` tuân thủ UI Contract & `DESIGN.md`: 3 thẻ KPI bề mặt `CardSurfaceBrush` trung tính (không tô nền đậm), thanh `ProgressBar` công suất phòng, biểu đồ đường `LiveCharts2`, `DataGrid` hóa đơn đã thanh toán và đủ 4 trạng thái UI.
- [ ] Khoảng lọc bao trọn cả ngày cuối (`ToDate` tính đến `23:59:59` để không bỏ sót hóa đơn thanh toán trong ngày hôm nay).
- [ ] Tài khoản `Staff` không thấy và không truy cập được màn Báo cáo.

### Ca biên cần xử lý (Edge Cases):
- Hóa đơn vừa thanh toán lúc `14:30` ngày hôm nay (`ToDate = Today`): nếu câu SQL dùng `PaymentDate <= @toDate` với `@toDate = 00:00:00` sẽ bị lọt mất hóa đơn trong ngày -> bắt buộc lọc `PaymentDate >= @fromDateStart AND PaymentDate < @toDateExclusiveNextDay`!
- Khoảng ngày không có giao dịch: KPI hiện `0 đ`, biểu đồ và bảng hiển thị trạng thái trống rõ ràng, không crash.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động (`TDD` seam): `dotnet test` kiểm chứng `ReportAggregator` đặc biệt là ca biên hóa đơn có giờ trong ngày cuối cùng (`14:30` ngày `ToDate`) và ca dữ liệu rỗng.
- Trực quan: Vừa thanh toán xong hóa đơn `1.070.000 đ` ở `TASK-10`, mở ngay màn Báo cáo thấy tổng doanh thu cộng thêm đúng `1.070.000 đ` và điểm dữ liệu ngày hôm nay nổi lên trên biểu đồ đường.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-11.md` lên GitHub Issue #64.*
