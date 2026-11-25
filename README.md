# Hotel Management System — Quản Lý Khách Sạn (AURA HOTEL)

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com)
[![MVVM Toolkit](https://img.shields.io/badge/MVVM-CommunityToolkit-68217A)](https://learn.microsoft.com/dotnet/communitytoolkit/mvvm)
[![WPF-UI](https://img.shields.io/badge/UI%20Shell-WPF--UI-5B5BD6)](https://wpfui.lepo.co)
[![LiveCharts2](https://img.shields.io/badge/Charts-LiveCharts2-2EA043)](https://livecharts.dev)
[![WPF](https://img.shields.io/badge/Platform-WPF-0078D4)](https://learn.microsoft.com/dotnet/desktop/wpf)
[![SQL Server](https://img.shields.io/badge/DB-SQL%20Server-CC2927)](https://www.microsoft.com/sql-server)

Đồ án môn IT008 (Lập trình trực quan): ứng dụng desktop quản lý khách sạn **AURA HOTEL** — quản lý danh mục phòng/khách hàng/dịch vụ, đặt phòng chống trùng lịch, check-in/check-out, hóa đơn thanh toán xuất `.txt` và báo cáo doanh thu.

## Tính năng cốt lõi

| Nhóm | Nội dung |
| ---- | -------- |
| Danh mục | Quản lý loại phòng, phòng, khách hàng, dịch vụ (CRUD + tìm kiếm + khóa xóa an toàn) |
| Đặt phòng | Tạo phiếu đặt phòng, kiểm tra trùng lịch tự động (*Overlap Conflict Rule*) |
| Lưu trú | Check-in nhận phòng, gọi dịch vụ phòng (*snapshot UnitPrice*), khởi tạo check-out |
| Thanh toán | Tính tiền phòng tối thiểu 1 đêm + dịch vụ, thanh toán nguyên tử, xuất biên lai `.txt` UTF-8 |
| Trực quan | Sơ đồ phòng đổi màu chỉ báo theo 4 trạng thái bằng XAML `DataTrigger` |
| Báo cáo | KPI doanh thu, công suất phòng, biểu đồ doanh thu ngày (`LiveCharts2`) |

Luồng nghiệp vụ khép kín: `Đăng nhập → Sơ đồ phòng → Đặt phòng → Check-in → Thêm dịch vụ → Check-out / Hóa đơn (.txt) → Báo cáo doanh thu`.

## Công nghệ

| Lớp | Lựa chọn |
| --- | -------- |
| UI | WPF trên .NET 10 (`net10.0-windows`), giao diện Fluent qua `WPF-UI` 4.3.0 (`FluentWindow`, `NavigationView`) |
| Kiến trúc | MVVM với `CommunityToolkit.Mvvm` 8.4.2 (`ObservableObject`, `[ObservableProperty]`, `[RelayCommand]`) |
| Biểu đồ | `LiveChartsCore.SkiaSharpView.WPF` 2.0.5 (báo cáo doanh thu theo ngày) |
| Cơ sở dữ liệu | SQL Server / SQL Server Express, script 1-click `Database/HotelDB.sql` (8 bảng) |
| Truy cập dữ liệu | ADO.NET thuần qua `DataProvider.cs` + các lớp DAO tham số hóa chống SQL Injection |
| Kiểm thử | `xUnit` (`tests/HotelManagement.Tests`) kiểm thử tự động các seam nghiệp vụ thuần |

## Yêu cầu môi trường

- Visual Studio 2026 (workload .NET Desktop Development, .NET 10 SDK; ghim qua `global.json` ở Issue `#58`)
- SQL Server Express (hoặc LocalDB) + SQL Server Management Studio (SSMS)

## Cài đặt và chạy

1. Clone repository: `git clone https://github.com/phantatdung233/WPF_QuanLyKhachSan.git`
2. Mở `Database/HotelDB.sql` trong SSMS và Execute để tạo database `HotelManagementDb` cùng dữ liệu mẫu chuẩn (tạo ở Issue `#59`).
3. Cấu hình chuỗi kết nối qua biến môi trường `HOTEL_DB_CONNECTION` (mặc định fallback về `.\SQLEXPRESS`).
4. Chạy `dotnet build`, `dotnet test` hoặc nhấn `F5` trong Visual Studio (khởi tạo ở Issue `#58`).

## Tài khoản demo

| Tài khoản | Mật khẩu | Quyền |
| --------- | -------- | ----- |
| `admin` | `123456` | Quản trị (`Administrator`): đầy đủ 8 mục điều hướng, quản lý loại phòng, báo cáo doanh thu |
| `staff` | `123456` | Lễ tân (`Staff`): 6 mục nghiệp vụ hàng ngày, ẩn Loại phòng và Báo cáo |

## Nguồn sự thật duy nhất (Single Sources of Truth)

- **Thuật ngữ & Công thức nghiệp vụ**: [`GLOSSARY.md`](./GLOSSARY.md)
- **Đặc tả hệ thống & Đồ thị 4 Milestones**: [`docs/specs/hotel-management-system.md`](./docs/specs/hotel-management-system.md) ([SPEC #77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77))
- **Design System & Tokens (chuẩn Google `DESIGN.md`)**: [`DESIGN.md`](./DESIGN.md)
- **Quy trình Agent 2 Pha**: [`CLAUDE.md`](./CLAUDE.md), [`AGENTS.md`](./AGENTS.md), [`.cursorrules`](./.cursorrules)
- **Backlog thực thi (14 Vertical-Slice Issues theo 4 Milestones)**: Quản lý trực tiếp trên [GitHub Issues](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues) (`#58`–`#65`, `#78`–`#83`)

## Phân công thành viên

| Thành viên | Phụ trách | Issues |
| ---------- | --------- | ------ |
| Member A (@phantatdung233) | Khung MVVM, CSDL, đăng nhập, loại phòng & phòng | `#58`, `#59`, `#60`, `#82`, `#61` |
| Member B (@xuanduong29) | Khách hàng, dịch vụ, đặt phòng, check-in/out & gọi dịch vụ | `#78`, `#79`, `#62`, `#80`, `#83` |
| Member C (@ngocthuyen1) | Sơ đồ phòng `DataTrigger`, hóa đơn `.txt`, báo cáo & tổng duyệt | `#81`, `#63`, `#64`, `#65` |
