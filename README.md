# Hotel Management System — Quản Lý Khách Sạn

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com)
[![MVVM Toolkit](https://img.shields.io/badge/MVVM-CommunityToolkit-68217A)](https://learn.microsoft.com/dotnet/communitytoolkit/mvvm)
[![WPF-UI](https://img.shields.io/badge/UI%20Shell-WPF--UI-5B5BD6)](https://wpfui.lepo.co)
[![LiveCharts2](https://img.shields.io/badge/Charts-LiveCharts2-2EA043)](https://livecharts.dev)

[![WPF](https://img.shields.io/badge/Platform-WPF-0078D4)](https://learn.microsoft.com/dotnet/desktop/wpf)
[![SQL Server](https://img.shields.io/badge/DB-SQL%20Server-CC2927)](https://www.microsoft.com/sql-server)

Đồ án môn IT008 (Lập trình trực quan): ứng dụng desktop quản lý khách sạn —
quản lý danh mục phòng/khách hàng/dịch vụ, đặt phòng chống trùng lịch,
check-in/check-out, hóa đơn thanh toán và báo cáo doanh thu.

## Mục lục

- [Tính năng](#tính-năng)
- [Công nghệ](#công-nghệ)
- [Yêu cầu môi trường](#yêu-cầu-môi-trường)
- [Cài đặt và chạy](#cài-đặt-và-chạy)
- [Tài khoản demo](#tài-khoản-demo)
- [Cấu trúc dự án](#cấu-trúc-dự-án)
- [Quy tắc nghiệp vụ](#quy-tắc-nghiệp-vụ)
- [Tài liệu](#tài-liệu)
- [Thành viên](#thành-viên)

## Tính năng

| Nhóm | Nội dung |
| ---- | -------- |
| Danh mục | Quản lý loại phòng, phòng, khách hàng, dịch vụ (CRUD + tìm kiếm) |
| Đặt phòng | Tạo phiếu đặt phòng, kiểm tra trùng lịch tự động |
| Lưu trú | Check-in, gọi dịch vụ phòng, check-out |
| Thanh toán | Tính tiền phòng + dịch vụ, xuất hóa đơn ra file `.txt` |
| Trực quan | Sơ đồ phòng đổi màu theo trạng thái (DataTrigger) |
| Báo cáo | KPI doanh thu, công suất phòng, danh sách hóa đơn theo khoảng ngày |

Luồng nghiệp vụ khép kín: Đăng nhập → Sơ đồ phòng → Đặt phòng →
Check-in → Thêm dịch vụ → Check-out / Hóa đơn → Báo cáo doanh thu.

## Công nghệ

| Lớp | Lựa chọn |
| --- | -------- |
| UI | WPF trên .NET 10 (`net10.0-windows`), giao diện Fluent qua `WPF-UI` 4.3.0 (`NavigationView`) |
| Kiến trúc | MVVM với `CommunityToolkit.Mvvm` 8.4.2 (`ObservableObject`, `[ObservableProperty]`, `[RelayCommand]`) |
| Biểu đồ | `LiveChartsCore.SkiaSharpView.WPF` 2.0.5 (báo cáo doanh thu theo ngày) |
| Cơ sở dữ liệu | SQL Server / SQL Server Express, script `Database/HotelDB.sql` (8 bảng) |
| Truy cập dữ liệu | ADO.NET thuần qua `DataProvider.cs` + các lớp DAO, tham số hóa chống SQL Injection |

## Yêu cầu môi trường

- Visual Studio 2026 (workload .NET Desktop Development, .NET 10 SDK)
  - Không dùng Visual Studio 2022: VS 2022 không hỗ trợ target `net10.0`.
  - SDK sẽ được ghim qua `global.json` (`10.0.100`, `rollForward: latestFeature`) khi tạo solution ở TASK-01.
- SQL Server Express (hoặc LocalDB) + SQL Server Management Studio (SSMS)

## Cài đặt và chạy

1. Clone repository:
   `git clone https://github.com/phantatdung233/WPF_QuanLyKhachSan.git`
2. Mở `Database/HotelDB.sql` trong SSMS và Execute để tạo database
   `HotelManagementDb` cùng dữ liệu mẫu (2 users, 4 loại phòng, 12 phòng,
   5 khách hàng, 5 dịch vụ).
3. Cấu hình chuỗi kết nối trong `DataProvider.cs` khớp tên SQL Server trên máy
   (ví dụ `.\SQLEXPRESS`). *(`DataProvider.cs` được tạo ở TASK-02 và giải
   thích trong `docs/tasks/TASK-02.md`.)*
4. Mở solution trong Visual Studio, nhấn F5. *(Solution `HotelManagement.sln`
   được tạo ở TASK-01 — xem `docs/tasks/TASK-01.md`.)*

> Hai bước 3–4 áp dụng **sau khi** đã hoàn thành TASK-01 và TASK-02. Repository
> hiện chỉ chứa tài liệu (spec + 12 task guide), chưa có mã nguồn.

Để reset dữ liệu trước buổi demo, chỉ cần chạy lại `HotelDB.sql` trong SSMS.

## Tài khoản demo

| Tài khoản | Mật khẩu | Quyền |
| --------- | -------- | ----- |
| `admin` | `123456` | Quản trị: đầy đủ menu, quản lý loại phòng, báo cáo doanh thu |
| `staff` | `123456` | Lễ tân: nghiệp vụ hàng ngày, menu quản trị bị ẩn |

## Cấu trúc dự án

```text
Database/    Script tạo CSDL và dữ liệu mẫu (HotelDB.sql)
Models/      Thực thể (User, Room, Customer, Booking, Service, Invoice, ...)
DAO/         DataProvider.cs và các lớp truy vấn (UserDAO, RoomDAO, ...)
ViewModels/  ViewModelBase + các ViewModel kế thừa ObservableObject
Views/       LoginWindow, MainWindow và các UserControl chức năng
Resources/   Colors.xaml, Styles.xaml dùng chung
docs/        Spec, hướng dẫn từng task, tài liệu agent
```

## Quy tắc nghiệp vụ

Định nghĩa chuẩn của từng nghiệp vụ (công thức chống trùng lịch, cách tính tiền
phòng, trạng thái phòng) nằm **duy nhất** trong [`GLOSSARY.md`](./GLOSSARY.md)
— xem các mục *Overlap Conflict Rule*, *Room Charge Policy*, *RoomStatus*.

- **Trạng thái phòng:** `Trống` (xanh lá), `Đang ở` (đỏ), `Đã đặt` (cam),
  `Bảo trì` (xám). Xem định nghĩa chuẩn trong `GLOSSARY.md`.

## Tài liệu

- Backlog, Single Source of Truth: `docs/tasks/TASK-01.md` … `TASK-12.md`
- Đặc tả hệ thống: `docs/specs/hotel-management-system.md` (SPEC #77)
- Hướng dẫn chi tiết từng task: `docs/tasks/TASK-01.md` … `TASK-12.md`
- Thuật ngữ chuẩn: `GLOSSARY.md`
- GitHub Issues: [#58](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/58)–[#65](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/65),
  [#78](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/78)–[#81](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/81)

## Thành viên

| Thành viên | Phụ trách |
| ---------- | --------- |
| Member A (@phantatdung233) | Khung MVVM, CSDL, đăng nhập, quản lý phòng (TASK-01–04) |
| Member B (@xuanduong29) | Khách hàng, dịch vụ, đặt phòng, check-in/out (TASK-05–08) |
| Member C (@ngocthuyen1) | Sơ đồ phòng, hóa đơn, báo cáo, slide demo (TASK-09–12) |
