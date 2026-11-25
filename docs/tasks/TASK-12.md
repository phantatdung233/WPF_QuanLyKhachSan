# [TASK-12] Tổng hợp kiểm thử E2E, Soạn Slide & Kịch bản Rehearsal Demo

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-12`  
**GitHub Issue**: [#65](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/65)  
**Thành viên đảm nhiệm**: **Member C (@ngocthuyen1)** *(phối hợp toàn nhóm A, B, C)*  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/demo-rehearsal-slides`  
**Milestone**: Mốc 3: Báo Cáo Thống Kê & Chốt Demo (19/12 – 30/12)  
**Khung thời gian dự kiến**: `25/12/2026 – 30/12/2026` *(6 ngày đệm sau Feature Freeze `24/12`)*  
**Labels**: `docs`, `presentation`, `demo`, `qa`, `m3`, `ready-for-agent`  
**Blocked by**: #81 (TASK-09), #63 (TASK-10), #64 (TASK-11)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Hoàn thiện chặng nước rút sau mốc **Feature Freeze (`24/12/2026`)** để sẵn sàng nộp bài và bảo vệ đồ án môn IT008 vào hạn chót **`30/12/2026`**.
- Kiểm thử tích hợp thông suốt (End-to-End) toàn bộ chu trình vận hành khách sạn trên máy Windows sạch và đảm bảo toàn bộ unit test (`dotnet test`) xanh 100%.
- Kiểm định script `Database/HotelDB.sql` reset lại toàn bộ dữ liệu demo chuẩn chỉ trong **1 click (dưới 30 giây)** trên SSMS.
- Soạn Slide báo cáo (6–8 slides gọn gàng) và kịch bản thuyết trình chi tiết từng phút cho 3 thành viên, tổ chức **2 buổi tổng duyệt (Rehearsal)** vào ngày `28/12` và `29/12`, chốt merge `develop` → `main` trước `30/12/2026`.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- **Kịch bản chuỗi nghiệp vụ E2E bắt buộc vượt qua (9 bước liên hoàn)**:
  1. Chạy `Database/HotelDB.sql` trên SSMS để đưa dữ liệu về trạng thái mẫu chuẩn.
  2. Đăng nhập tài khoản Lễ tân (`staff` / `123456`): xác nhận cụm tài khoản hiển thị `Lê Thu Ngân` (`Lễ tân`), menu có `Đăng xuất` và Sidebar chỉ có 6 mục (ẩn `Loại phòng` và `Báo cáo`).
  3. Đăng xuất, đăng nhập tài khoản Quản trị (`admin` / `123456`): xác nhận đủ 8 mục điều hướng.
  4. Mở **Sơ đồ phòng (`RoomMapView`)**: thấy 12 phòng trên nền thẻ trung tính với đủ 4 dấu màu `DataTrigger` (`6 Trống`, `2 Đang ở`, `3 Đã đặt`, `1 Bảo trì`).
  5. Thử tạo **Đặt phòng** trùng vào khoảng ngày đang bận -> xác nhận hệ thống chặn bằng banner `ui:InfoBar` đỏ rõ ràng; sau đó chọn phòng `Trống` đặt lịch hợp lệ -> thẻ phòng chuyển dấu sang màu Cam (`Đã đặt`).
  6. Thực hiện **Check-in** cho khách vừa đặt -> thẻ phòng chuyển dấu sang màu Đỏ (`Đang ở`).
  7. Mở **Gọi dịch vụ phòng** -> thêm 2 Nước suối Lavie (`20.000 đ`) + 1 Ăn sáng buffet (`50.000 đ`) = `70.000 đ`.
  8. Bấm **Check-out** -> kiểm tra màn **Hóa đơn** tính chuẩn tiền phòng + tiền dịch vụ (kịch bản chuẩn `1.070.000 đ`), chọn `Chuyển khoản`, bấm **Xác nhận thanh toán** và **Xuất hóa đơn `.txt`** mở lên đối chiếu UTF-8; quay lại Sơ đồ phòng thấy phòng đã về lại màu Xanh (`Trống`).
  9. Mở **Báo cáo & Thống kê**: xác nhận `1.070.000 đ` vừa thanh toán đã cộng ngay vào KPI Tổng doanh thu, bảng hóa đơn và đường biểu đồ `LiveCharts2`.
- **Cấu trúc Slide báo cáo & Tài liệu kịch bản (`docs/demo/DEMO-SCRIPT.md`)**:
  - Slide 1: Tên đề tài, GVHD, 3 thành viên & phân công.
  - Slide 2: Bài toán thực tế & Chu trình khép kín.
  - Slide 3: Kiến trúc kỹ thuật (`.NET 10 WPF`, `CommunityToolkit.Mvvm`, `WPF-UI` theo `DESIGN.md`, `LiveCharts2`, ADO.NET + Unit Test Seams).
  - Slide 4: Thiết kế CSDL SQL Server (ERD 8 bảng & bất biến nghiệp vụ).
  - Slide 5: Ba màn điểm nhấn (Sơ đồ phòng `DataTrigger`, Xuất biên lai `.txt`, Báo cáo `LiveCharts2`).
  - Slide 6: Demo trực tiếp & Tổng kết hướng phát triển.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] `dotnet build` không cảnh báo lỗi nghiêm trọng và toàn bộ bộ test `dotnet test` trong `HotelManagement.Tests` xanh 100%.
- [ ] Vượt qua trọn vẹn 9 bước kịch bản E2E trên máy Windows ở độ phân giải máy chiếu `1280×720` mà không cần can thiệp tay vào CSDL.
- [ ] `Database/HotelDB.sql` kiểm chứng chạy lại 1-click trong SSMS reset sạch dữ liệu demo dưới 30 giây.
- [ ] Hoàn thiện tài liệu kịch bản phân vai `docs/demo/DEMO-SCRIPT.md`, bộ Slide thuyết trình và 1 video quay màn hình dự phòng sự cố máy chiếu.
- [ ] Hoàn thành 2 buổi Rehearsal (`28/12` & `29/12`, thời lượng khớp 7–8 phút) và merge bản ổn định từ `develop` vào `main` trước hạn `30/12/2026`.

### Ca biên cần xử lý (Edge Cases):
- Độ phân giải máy chiếu hội đồng thấp (`1280×720`, scaling Windows 125%): kiểm tra trước trên màn hình thật để không nút bấm nào bị che khuất.
- Luôn có sẵn 1 bản video demo full-flow lưu offline đề phòng sự cố mất điện / lỗi SQL Server đột xuất trên máy thuyết trình.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động: Chạy `dotnet test --configuration Release` xác nhận toàn bộ unit test xanh trước khi merge vào `main`.
- Tổng duyệt thực địa: Chạy bấm giờ 2 lượt liên tiếp từ lúc chạy `HotelDB.sql` đến lúc xuất file `.txt` và mở biểu đồ doanh thu trong 7–8 phút trơn tru.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-12.md` lên GitHub Issue #65.*
