# [TASK-01] Khởi tạo Solution WPF .NET 10, Test Project & Khung Điều Hướng MVVM

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-01`  
**GitHub Issue**: [#58](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/58)  
**Thành viên đảm nhiệm**: **Member A (@phantatdung233)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/core-mvvm-setup`  
**Milestone**: Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (18/11 – 04/12)  
**Khung thời gian dự kiến**: `18/11/2026 – 20/11/2026` (Sprint 0 — mở khóa toàn nhóm)  
**Labels**: `core`, `architecture`, `m1`, `ready-for-agent`  
**Blocked by**: None (Bắt đầu ngay)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Thay thế template tạm `src/src.slnx` + `src/src.csproj` hiện có bằng Solution chuẩn `HotelManagement.slnx` (kèm `global.json` ghim SDK `10.0.100` với `rollForward: latestFeature`), gồm 2 project:
  - `src/HotelManagement/HotelManagement.csproj`: `.NET 10 WPF` (`net10.0-windows`, `<UseWPF>true</UseWPF>`, `<EnableWindowsTargeting>true</EnableWindowsTargeting>` để `dotnet build` chạy được trên cả Windows, macOS và GitHub Actions).
  - `tests/HotelManagement.Tests/HotelManagement.Tests.csproj`: project kiểm thử tự động `xUnit` (`net10.0-windows` với `EnableWindowsTargeting=true`) phục vụ `/tdd` cho các seam nghiệp vụ ở các task sau.
- Cài đặt NuGet cho `HotelManagement`: `CommunityToolkit.Mvvm` (`8.4.2`) và `WPF-UI` (`4.3.0`).
- Tổ chức cấu trúc thư mục rõ ràng bên trong `src/HotelManagement/`:
  - `Models/`: Thực thể nghiệp vụ (sẽ hoàn thiện đủ 8 lớp ở `TASK-02`).
  - `DAO/`: Tầng truy cập dữ liệu ADO.NET.
  - `Services/`: Các lớp logic/validation thuần phục vụ kiểm thử tự động (`BookingRules`, `InvoiceCalculator`, `ValidationRules`, `RolePermissions`).
  - `ViewModels/`: `ViewModelBase` kế thừa `ObservableObject` + `MainViewModel` và các placeholder ViewModel cho 8 màn hình.
  - `Views/`: `MainWindow.xaml`, `LoginWindow.xaml` (stub) và các placeholder `UserControl` cho 8 màn hình điều hướng để **tránh xung đột merge** ở `App.xaml` / `MainWindow.xaml` khi 3 thành viên code song song `TASK-03..11`.
  - `Resources/`: `Colors.xaml`, `Styles.xaml` bám sát quy chuẩn **Windows UI Kit** tại `docs/design/DESIGN-BRIEF.md`.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- **`Resources/Colors.xaml` & `Styles.xaml`** (bám sát `docs/design/DESIGN-BRIEF.md` mục 2.3–2.5):
  - Bề mặt sáng trung tính (`App Surface`, `Card Surface`), màu nhấn Windows `#005FB8`, kiểu chữ Segoe UI Variable, bo góc 4px cho control và 8px cho container/dialog (tuyệt đối không dùng mảng nền navy đậm).
  - Khai báo cố định 4 brush trạng thái phòng (`RoomStatus`):
    - `RoomAvailableBrush`: `#48BB78` (`Trống`)
    - `RoomOccupiedBrush`: `#F56565` (`Đang ở`)
    - `RoomReservedBrush`: `#ED8936` (`Đã đặt`)
    - `RoomMaintenanceBrush`: `#A0AEC0` (`Bảo trì`)
- **`MainWindow.xaml` & `MainViewModel.cs`** (bám sát `docs/design/WIREFRAMES.md` mục 2):
  - Kế thừa `FluentWindow` của `WPF-UI`, kích thước thiết kế `1280×800`, tối thiểu `MinWidth="1280" MinHeight="720"`.
  - Bên trái: Sidebar điều hướng (`NavigationView`) gồm **8 mục cho `Administrator`**: `Sơ đồ phòng`, `Đặt phòng`, `Phòng`, `Loại phòng`, `Khách hàng`, `Dịch vụ`, `Hóa đơn`, `Báo cáo` (với `Staff`, ẩn 2 mục `Loại phòng` và `Báo cáo` qua seam `RolePermissions`).
  - Phía trên vùng nội dung: Header hiển thị tiêu đề trang hiện tại, cụm phiên làm việc `[Họ tên] và [Vai trò] trong nút tài khoản` ; nút tài khoản mở menu `Đăng xuất`.
  - Trung tâm: `<ContentControl Content="{Binding CurrentView}" />` kèm `DataTemplate` đăng ký sẵn cho cả 8 cặp ViewModel/View placeholder trong `App.xaml`.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [x] Dọn bỏ `src/src.csproj` + `src/src.slnx` cũ; tạo `global.json`, `HotelManagement.slnx`, `src/HotelManagement/HotelManagement.csproj` và `tests/HotelManagement.Tests/HotelManagement.Tests.csproj` chạy `dotnet build` & `dotnet test` xanh 100% không lỗi.
- [x] Cài đặt đúng `CommunityToolkit.Mvvm` (8.4.2) và `WPF-UI` (4.3.0).
- [x] Tổ chức đầy đủ thư mục `Models/`, `DAO/`, `Services/`, `ViewModels/`, `Views/`, `Resources/` cùng `Colors.xaml` (accent `#005FB8` + 4 mã màu `RoomStatus`) và `Styles.xaml`.
- [x] `ViewModelBase` kế thừa `ObservableObject`; `MainViewModel` quản lý `CurrentView`, tiêu đề trang và danh sách mục điều hướng theo `RolePermissions` (Admin 8 mục, Staff 6 mục).
- [x] Khởi tạo sẵn 8 cặp placeholder View/ViewModel và đăng ký `DataTemplate` trong `App.xaml` để các nhánh `TASK-03..11` chỉ sửa file riêng của từng module, không gây conflict tập trung.
- [x] Có unit test đầu tiên trong `HotelManagement.Tests` kiểm tra điều hướng `MainViewModel` và phân quyền menu `RolePermissions` (8 mục vs 6 mục).

### Ca biên cần xử lý (Edge Cases):
- Khi mở `MainWindow` lần đầu sau đăng nhập, `CurrentView` mặc định trỏ vào `RoomMapViewModel` (`Sơ đồ phòng`).
- Cửa sổ giữ `MinWidth="1280"` và `MinHeight="720"` để không vỡ bố cục trên máy chiếu hội đồng chấm bài.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động (`TDD` seam): Chạy `dotnet test` xác nhận `MainViewModel` mặc định mở `RoomMapViewModel`, chuyển đổi đúng giữa các màn và lọc đúng 8 mục (`Administrator`) so với 6 mục (`Staff`).
- Trực quan: Chạy ứng dụng trên Windows, bấm lần lượt 8 mục trên Sidebar xác nhận `ContentControl` và tiêu đề Header đổi mượt trên nền sáng chuẩn Windows UI Kit.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-01.md` lên GitHub Issue #58.*

## Ghi chú triển khai TASK-01

- Đã hoàn thiện khung và test điều hướng/phân quyền; `LoginWindow` là màn xem trước vai trò, được thay bằng xác thực thật ở TASK-03.
- Project test giữ `net10.0-windows` + project reference trên Windows. Trên macOS/Linux dùng `net10.0` và liên kết cùng source thuần ViewModels/Services để chạy test khi không có WindowsDesktop runtime.
- Build WPF và unit test đã kiểm tra trên macOS. **Nghiệm thu trực quan trên Windows còn chờ**: mở hai vai trò, kiểm tra 8/6 menu, chuyển đủ trang, chọn trang bằng bàn phím, đăng xuất và kiểm tra cửa sổ tại 1280×720.

### Đồng bộ Figma — 19/11/2026

Chuẩn trực quan: `docs/design/FIGMA-ALIGNMENT.md`. Dùng AURA HOTEL, title bar 48px, pane trái 240px, menu nhóm có icon, header tài khoản dạng menu. TASK-01 dùng màn xem trước 680×360 với hai nút chọn vai trò. Bố cục đăng nhập hai cột trong Figma dành cho TASK-03.
