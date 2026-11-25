# Specification: Hotel Management System (WPF .NET 10 MVVM)

**Issue Tracker Reference**: [Issue #77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Design System Reference**: [`DESIGN.md`](../../DESIGN.md)  
**Agent Workflow Reference**: [`CLAUDE.md`](../../CLAUDE.md), [`AGENTS.md`](../../AGENTS.md)  
**Status**: `ready-for-agent`

---

## Problem Statement

Quản lý khách sạn truyền thống bằng sổ sách hoặc bảng tính Excel dễ dẫn đến sai sót: trùng lịch phòng (overbooking), thất thoát chi phí dịch vụ phát sinh khi khách lưu trú, tính sai hóa đơn khi trả phòng, và khó nắm bắt tổng quan tình trạng phòng theo thời gian thực. Đồ án IT008 (Lập trình trực quan) cần một ứng dụng desktop trực quan, tin cậy xây dựng trên nền tảng .NET 10 WPF MVVM dưới thương hiệu **AURA HOTEL**, đáp ứng đầy đủ chu trình vận hành khách sạn khép kín cho nhóm 3 thành viên.

## Solution

Phát triển ứng dụng WPF Quản lý Khách sạn chuẩn kiến trúc MVVM (`CommunityToolkit.Mvvm`) với giao diện Fluent dựa trên `WPF-UI` theo quy chuẩn [`DESIGN.md`](../../DESIGN.md), kết nối cơ sở dữ liệu Microsoft SQL Server (SSMS & SQL Server Express) qua tầng truy cập dữ liệu ADO.NET thuần (`DataProvider.cs`). Ứng dụng cung cấp:

- Hệ thống xác thực và phân quyền rõ ràng (`Administrator` và `Staff`) dựa trên bảng `Users`, ẩn các mục quản trị đối với lễ tân.
- Quản lý danh mục cơ bản (Loại phòng, Phòng, Khách hàng, Dịch vụ) với đầy đủ 4 trạng thái UI bắt buộc (`Loading`, `Empty`, `Error`, `Success`), tìm kiếm/lọc thời gian thực và khóa xóa an toàn.
- Thuật toán kiểm tra và ngăn chặn xung đột trùng lịch đặt phòng (theo *Overlap Conflict Rule* trong `GLOSSARY.md`).
- Chu trình lưu trú khép kín: Đặt phòng → Check-in nhận phòng → Gọi dịch vụ phòng → Check-out thanh toán hóa đơn.
- **Tính năng nâng cao 1**: Sơ đồ phòng trực quan (Room Matrix) giữ nền thẻ sáng trung tính (`CardSurfaceBrush`) và tự động đổi màu vạch/nhãn chỉ báo theo 4 trạng thái phòng bằng XAML `DataTrigger` (Xanh lá `#48BB78`: Trống, Đỏ `#F56565`: Đang ở, Cam `#ED8936`: Đã đặt, Xám `#A0AEC0`: Bảo trì).
- **Tính năng nâng cao 2**: Xem trước biên lai monospace và xuất hóa đơn thanh toán chuẩn UTF-8 ra file văn bản (`.txt`) khi hoàn tất trả phòng.
- Màn hình Báo cáo & Thống kê doanh thu với 3 thẻ KPI bề mặt trung tính, thanh tiến trình tỷ lệ công suất phòng (`ProgressBar`), biểu đồ đường doanh thu theo ngày (`LiveCharts2`) và bảng chi tiết hóa đơn đã thanh toán.

## User Stories

1. As a Hotel Administrator, I want to log in with my username and password, so that I can access administrative settings, manage room categories, and view revenue reports.
2. As a Hotel Staff member, I want to log in with my account, so that I can perform reception tasks while administrative and revenue management features are hidden.
3. As a User, I want a persistent session indicator displaying my name and role with a Logout button, so that I can exit securely.
4. As a Hotel Administrator, I want to manage Room Types with base daily prices, so that rooms are categorized and priced consistently.
5. As a Hotel Staff member, I want to view a list of rooms with their room number, type, and current status, so that room availability is immediately clear.
6. As a Hotel Staff member, I want to search and filter rooms by status or room type, so that matching rooms can be found quickly for guests.
7. As a Hotel Staff member, I want to create and update Customer records (Full Name, Citizen ID / CCCD, Phone, Email), so that guest profiles are stored accurately.
8. As a Hotel Staff member, I want Citizen ID and Phone Number formats to be validated inline, so that invalid input is caught before saving.
9. As a Hotel Staff member, I want to search for existing customers by Name, Citizen ID, or Phone with real-time results, so that returning guests are recognized quickly.
10. As a Hotel Staff member, I want to manage Hotel Services with a name and unit price (e.g., Laundry, Breakfast, Drinks, Motorcycle Rental).
11. As a Hotel Staff member, I want to create a room reservation (Booking) by selecting a Customer, Room, Check-in Date, and Check-out Date, with a live preview of estimated room charges.
12. As a Hotel Staff member, I want the system to execute an overlap check (`NewCheckIn < ExistingCheckOut && ExistingCheckIn < NewCheckOut`), so that double-booking the same room is prevented with a clear conflict message.
13. As a Hotel Staff member, I want to execute a Check-in action for a booked guest, so that the booking transitions to `Occupied` (`Đang ở`) and the room turns `Occupied` (`Đang ở`).
14. As a Hotel Staff member, I want to record extra services used by an occupied room with quantities, so that service costs are linked to the active booking.
15. As a Hotel Staff member, I want to perform a Check-out action calculating room charges (per the *Room Charge Policy* in `GLOSSARY.md`) plus service charges, so that a final invoice is generated.
16. As a Hotel Staff member, I want to mark the invoice as `Paid` (`Đã thanh toán`) with Cash (`Tiền mặt`) or Bank Transfer (`Chuyển khoản`), completing the booking and resetting room status to `Available` (`Trống`).
17. As a Hotel Staff member, I want to view an interactive Room Matrix where neutral cards display status-driven `DataTrigger` indicator bars/badges (`#48BB78` Available, `#F56565` Occupied, `#ED8936` Reserved, `#A0AEC0` Maintenance).
18. As a Hotel Staff member, I want to click any room card on the Room Matrix to view details or double-click to initiate booking, check-in, or check-out.
19. As a Hotel Staff member, I want to preview and export the itemized invoice to a clean UTF-8 text file (`.txt`) upon checkout.
20. As a Hotel Administrator, I want to view revenue KPI metrics, occupancy progress bars, a revenue-by-day line chart, and an itemized paid invoice table over custom date ranges.
21. As a Student / Presenter, I want a single-click SQL script (`Database/HotelDB.sql`) to seed deterministic demo data (users, room types, rooms, customers, services, bookings), so that demonstrations can be reset reliably in SSMS.

## Implementation Decisions

- **Architectural Pattern & 2-Phase Execution**: MVVM with `CommunityToolkit.Mvvm` 8.4.2 (`ObservableObject`, `[ObservableProperty]`, `[RelayCommand]`), executed strictly in two phases per [`CLAUDE.md`](../../CLAUDE.md) (Phase 1: Logic, DAO, TDD Seams & 4-state ViewModels passing `dotnet test`; Phase 2: XAML assembly from `DESIGN.md`). View navigation is managed via `CurrentView` in `MainViewModel` hosting Views inside a central `ContentControl` mapped by `DataTemplate`s in `App.xaml`.
- **Solution & Cross-Platform Buildability**: Solution `HotelManagement.slnx` hosts `src/HotelManagement/HotelManagement.csproj` (`net10.0-windows`, `<UseWPF>true</UseWPF>`, `<EnableWindowsTargeting>true</EnableWindowsTargeting>`) and `tests/HotelManagement.Tests/HotelManagement.Tests.csproj` (`xUnit`), pinned via `global.json` (`10.0.100`, `rollForward: latestFeature`).
- **UI Framework & Design System**: `WPF-UI` 4.3.0 on `net10.0-windows`, governed by [`DESIGN.md`](../../DESIGN.md) at repo root (Google `DESIGN.md` format: light neutral surfaces `AppSurfaceBrush` / `CardSurfaceBrush`, Windows accent `#005FB8`, Segoe UI Variable typography, 4px control radius / 8px container radius, minimum window `1280×720`, mandatory 4 UI states `Loading` / `Empty` / `Error` / `Success`).
  - **Admin Sidebar (8 items)**: `Sơ đồ phòng`, `Đặt phòng`, `Phòng`, `Loại phòng`, `Khách hàng`, `Dịch vụ`, `Hóa đơn`, `Báo cáo`.
  - **Staff Sidebar (6 items)**: hides `Loại phòng` and `Báo cáo`.
- **Charting**: `LiveChartsCore.SkiaSharpView.WPF` 2.0.5, used exclusively by the reporting module (`#64`) for a revenue-by-day `CartesianChart` (`LineSeries`).
- **Database & Persistence**: Microsoft SQL Server (SSMS / SQL Server Express / LocalDB). Schema and deterministic seed data live in `Database/HotelDB.sql` with 8 core tables: `Users`, `RoomTypes`, `Rooms`, `Customers`, `Bookings`, `Services`, `BookingServices`, `Invoices`.
- **Data Access Layer**: Pure ADO.NET (`Microsoft.Data.SqlClient`) via shared `DataProvider.cs` (`HOTEL_DB_CONNECTION` env var with fallback to `.\SQLEXPRESS`) and parameterized DAOs (`UserDAO`, `RoomTypeDAO`, `RoomDAO`, `CustomerDAO`, `ServiceDAO`, `BookingDAO`, `BookingServiceDAO`, `InvoiceDAO`). All 8 entity classes in `Models/` are created up front in `#59` alongside the schema to prevent cross-branch model drift.
- **Domain Status & Role Representations (`GLOSSARY.md`)**:
  - `Users.Role`: `'Administrator'` (`Quản trị`) and `'Staff'` (`Lễ tân`).
  - `Rooms.Status` (`RoomStatus`): `Trống`, `Đang ở`, `Đã đặt`, `Bảo trì`.
  - `Bookings.Status` (`BookingStatus`): `Đã đặt`, `Đang ở`, `Hoàn thành`, `Đã hủy`.
  - `Invoices.Status` (`InvoiceStatus`): `Chưa thanh toán`, `Đã thanh toán`.
  - Currency format: dot thousands separator `#,##0 đ` (`500.000 đ`, `1.070.000 đ`).

## Testing Decisions

- **What Makes a Good Test**: Automated tests verify observable domain behavior and invariants through public interfaces, never private helpers or live SQL Server state. Manual E2E tests verify XAML layout, SQL Server persistence, and projector readiness.
- **Pre-Agreed Automated Testing Seams (`tests/HotelManagement.Tests` via `xUnit`, run with `dotnet test`)**:
  1. **Scheduling & Booking Seam (`BookingRules`)**: verifies the *Overlap Conflict Rule* (partial overlap, full containment, identical dates, adjacent same-day check-out/check-in allowed, `Đã hủy`/`Hoàn thành` bookings ignored), state transition guards, and date range validation (`CheckOutDate.Date > CheckInDate.Date`).
  2. **Billing, Receipt & Reporting Seam (`InvoiceCalculator` / `ReportAggregator`)**: verifies *Room Charge Policy* (same-day check-in/out billed as 1 night minimum, multi-night × `PricePerDay` + service lines = `1.070.000 đ` canonical case), *Settlement Invariant* state transitions, UTF-8 `.txt` receipt output formatting, and KPI / daily revenue grouping (including empty date ranges returning `0 đ`).
  3. **Input Validation & Role Seam (`ValidationRules` / `RolePermissions`)**: verifies CCCD (9 or 12 digits), Phone (`0` + 9 digits), `PricePerDay > 0`, `Service.Price >= 0`, `BookingService.Quantity >= 1`, delete-lock predicates, and `Administrator` (8 nav items) vs `Staff` (6 nav items) visibility.
- **End-to-End Demo & Reset Verification (`#65`)**: Full user scenario rehearsal (`Login → Room Matrix → Booking → Check-in → Add Services → Check-out & .txt Export → Revenue Report`) on a clean Windows machine (`1280×720`), backed by 1-click re-seeding via `Database/HotelDB.sql` in SSMS.

## Out of Scope

- Multi-tenant / multi-branch / cloud hosting.
- Online OTA booking API integrations (Agoda, Booking.com).
- Payment gateway integrations (VNPay, MoMo, Stripe) — manual cash (`Tiền mặt`) and bank transfer (`Chuyển khoản`) recording only.
- Keycard hardware, thermal printer drivers, PDF rendering libraries, or dark-mode theme switching.

## Sub-Issues & Goal-Oriented Milestones (Single Source of Truth on GitHub)

All 14 vertical-slice tickets (`#58`–`#65`, `#78`–`#83`) are tracked directly as GitHub sub-issues of `#77` with native `blocked_by` dependencies across **4 Goal-Oriented Demoable Milestones** (no arbitrary fixed calendar deadlines — each milestone closes when its demoable increment is complete):

### Mốc 1: Khung Nền Tảng & Đăng Nhập Phân Quyền (Walking Skeleton)
- `#58` — Khởi tạo Solution WPF .NET 10, Test Project & Khung Điều Hướng Shell *(Blocked by: None)*
- `#59` — Thiết kế CSDL SQL Server 8 bảng, Seed Data chuẩn, 8 Models & DataProvider *(Blocked by: `#58`)*
- `#60` — Đăng nhập xác thực SQL (Login MVVM) & Phiên phân quyền Administrator / Staff *(Blocked by: `#59`)*

### Mốc 2: Danh Mục Cơ Bản & Sơ Đồ Phòng Trực Quan
- `#82` — Quản lý danh mục Loại phòng (RoomType CRUD — Admin) *(Blocked by: `#59`)*
- `#61` — Quản lý danh mục Phòng & Bộ lọc kép (Room CRUD) *(Blocked by: `#82`)*
- `#78` — Quản lý danh mục Khách hàng & Tìm kiếm thời gian thực (Customer CRUD) *(Blocked by: `#59`)*
- `#79` — Quản lý danh mục Dịch vụ khách sạn (Service CRUD) *(Blocked by: `#59`)*
- `#81` — Sơ đồ phòng trực quan với XAML DataTrigger (Room Matrix) *(Blocked by: `#61`)*

### Mốc 3: Chu Trình Lưu Trú & Thanh Toán Hóa Đơn
- `#62` — Đặt phòng mới, Danh sách Booking & Thuật toán chống trùng lịch *(Blocked by: `#61`, `#78`)*
- `#80` — Vận hành nhận phòng (Check-in) & Khởi tạo trả phòng (Check-out) *(Blocked by: `#62`)*
- `#83` — Gọi dịch vụ phòng khi lưu trú (BookingServices & AddServiceDialog) *(Blocked by: `#80`, `#79`)*
- `#63` — Thanh toán Hóa đơn nguyên tử & Xem trước / Xuất biên lai (.txt) *(Blocked by: `#80`, `#83`)*

### Mốc 4: Báo Cáo Doanh Thu & Kiểm Thử Tổng Duyệt
- `#64` — Báo cáo KPI, Công suất phòng & Biểu đồ doanh thu ngày (LiveCharts2) *(Blocked by: `#63`)*
- `#65` — Kiểm thử tích hợp E2E, Soạn Slide & Tổng duyệt Demo *(`ready-for-human`, Blocked by: `#60`, `#81`, `#63`, `#64`)*

### Team Allocation
- **Member A (@phantatdung233) — Platform, Auth & Room Infrastructure (5 issues)**: `#58`, `#59`, `#60`, `#82`, `#61` (+ integration PR review).
- **Member B (@xuanduong29) — Customer, Service & Stay Lifecycle (5 issues)**: `#78`, `#79`, `#62`, `#80`, `#83`.
- **Member C (@ngocthuyen1) — Visual Room Matrix, Billing, Analytics & Demo (4 issues)**: `#81`, `#63`, `#64`, `#65`.

### Git Conventions
- Protected branch: `main` (stable build used for submission and demo).
- Integration branch: `develop` (kept fast-forwarded with `main` before cutting feature branches).
- Feature branches: `feature/<slice-name>`, cut from `develop`.
- Merge into `develop` only after both Phase 1 (`dotnet build` + `dotnet test`) and Phase 2 (`DESIGN.md` check) pass and a teammate reviews the PR.
