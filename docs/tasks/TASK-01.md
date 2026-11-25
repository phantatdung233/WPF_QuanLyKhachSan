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
  - `Resources/`: `Colors.xaml`, `Styles.xaml` khai báo trọn bộ Design Tokens khớp 1–1 với `DESIGN.md` tại gốc repo (chuẩn Google `DESIGN.md`).

---

## 2. Hướng dẫn nghiệp vụ & UI Contract (Theo `DESIGN.md`)
- **Thực thi 2 Pha (`CLAUDE.md`)**:
  - **Pha 1 (Logic & State First)**: Hoàn thiện `ViewModelBase`, `RolePermissions`, `MainViewModel` (quản lý `CurrentView`, `PageTitle`, `FullName`, `RoleDisplay`, `NavigationItems`, `LogoutCommand`) và pass `dotnet test` trước khi lắp XAML.
  - **Pha 2 (UI Assembly & Verification)**: Lắp `Colors.xaml`, `Styles.xaml`, `MainWindow.xaml` và `LoginWindow.xaml` (stub chọn vai trò xem trước) theo đúng `DESIGN.md`.
- **UI Contract (`MainWindow` & Shell Tokens)**:
  - **Dữ liệu hiển thị**: Thương hiệu **`AURA HOTEL`**, tiêu đề trang hiện tại (`PageTitle` — chỉ hiển thị 1 lần trên Header shell), danh sách điều hướng theo vai trò (`Administrator` 8 mục: `Sơ đồ phòng`, `Đặt phòng`, `Phòng`, `Loại phòng`, `Khách hàng`, `Dịch vụ`, `Hóa đơn`, `Báo cáo`; `Staff` 6 mục: ẩn `Loại phòng` và `Báo cáo`), cụm tài khoản (`FullName`, `RoleDisplay`, menu `Đăng xuất`) và vùng `<ContentControl Content="{Binding CurrentView}" />`.
  - **Primitives bắt buộc (`WPF-UI 4.3.0`)**: `ui:FluentWindow` (`1280×800`, `MinWidth="1280" MinHeight="720"`), `ui:TitleBar` (`48px`), `ui:NavigationView` (`240px` pane trái, `NavigationViewItem` + `ui:SymbolIcon`), `ui:Button` (tối đa 1 `Appearance="Primary"` trên màn stub; cấm viết lại `ControlTemplate` cho Button/Input). Mọi `Margin`/`Padding` tuân thủ hệ lưới bội số `4px/8px`.
  - **Design Tokens (`Resources/Colors.xaml` & `Styles.xaml`)**: Khóa bề mặt sáng trung tính (`AppSurfaceBrush` `#F3F3F3`, `CardSurfaceBrush` `#FFFFFF`), `AccentBrush` `#005FB8`, semantic status brushes và 4 brush `RoomStatus` cố định (`RoomAvailableBrush` `#48BB78`, `RoomOccupiedBrush` `#F56565`, `RoomReservedBrush` `#ED8936`, `RoomMaintenanceBrush` `#A0AEC0`).
  - **4 Trạng thái UI bắt buộc**:
    1. *Loading*: Khai báo sẵn style/token cho `ui:ProgressRing` dùng chung toàn app.
    2. *Empty*: 8 màn hình placeholder hiển thị trạng thái chờ phát triển gọn gàng trên nền `CardSurfaceBrush`.
    3. *Error*: Khai báo sẵn `InlineErrorTextStyle` (`12px`, `#A42618`) và convention `ui:InfoBar` trong `Styles.xaml`.
    4. *Success*: Điều hướng mượt giữa các mục hợp lệ theo quyền và đăng xuất sạch phiên về màn hình đầu.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Dọn bỏ `src/src.csproj` + `src/src.slnx` cũ; tạo `global.json`, `HotelManagement.slnx`, `src/HotelManagement/HotelManagement.csproj` và `tests/HotelManagement.Tests/HotelManagement.Tests.csproj` chạy `dotnet build` & `dotnet test` xanh 100% không lỗi.
- [ ] Cài đặt đúng `CommunityToolkit.Mvvm` (8.4.2) và `WPF-UI` (4.3.0).
- [ ] Tổ chức đầy đủ thư mục `Models/`, `DAO/`, `Services/`, `ViewModels/`, `Views/`, `Resources/` cùng `Colors.xaml` (accent `#005FB8` + 4 mã màu `RoomStatus`) và `Styles.xaml`.
- [ ] `ViewModelBase` kế thừa `ObservableObject`; `MainViewModel` quản lý `CurrentView`, tiêu đề trang và danh sách mục điều hướng theo `RolePermissions` (Admin 8 mục, Staff 6 mục).
- [ ] Khởi tạo sẵn 8 cặp placeholder View/ViewModel và đăng ký `DataTemplate` trong `App.xaml` để các nhánh `TASK-03..11` chỉ sửa file riêng của từng module, không gây conflict tập trung.
- [ ] Có unit test đầu tiên trong `HotelManagement.Tests` kiểm tra điều hướng `MainViewModel` và phân quyền menu `RolePermissions` (8 mục vs 6 mục).

### Ca biên cần xử lý (Edge Cases):
- Khi mở `MainWindow` lần đầu sau đăng nhập, `CurrentView` mặc định trỏ vào `RoomMapViewModel` (`Sơ đồ phòng`).
- Cửa sổ giữ `MinWidth="1280"` và `MinHeight="720"` để không vỡ bố cục trên máy chiếu hội đồng chấm bài.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động (`TDD` seam): Chạy `dotnet test` xác nhận `MainViewModel` mặc định mở `RoomMapViewModel`, chuyển đổi đúng giữa các màn và lọc đúng 8 mục (`Administrator`) so với 6 mục (`Staff`).
- Trực quan: Chạy ứng dụng trên Windows, bấm lần lượt 8 mục trên Sidebar xác nhận `ContentControl` và tiêu đề Header đổi mượt trên nền sáng chuẩn `DESIGN.md`.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-01.md` lên GitHub Issue #58.*
