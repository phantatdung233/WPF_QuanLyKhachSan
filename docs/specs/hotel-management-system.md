# Specification: Hotel Management System (WPF .NET 10 MVVM)

**Issue Tracker Reference**: [Issue #77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Design System Reference**: [`DESIGN.md`](../../DESIGN.md)  
**Implementation Window**: `18/11/2026` → `30/12/2026` (6 weeks / 42 days)  
**Status**: `ready-for-agent`

---

## Problem Statement

Quản lý khách sạn truyền thống bằng sổ sách hoặc bảng tính Excel dễ dẫn đến sai sót: trùng lịch phòng (overbooking), thất thoát chi phí dịch vụ phát sinh khi khách lưu trú, tính sai hóa đơn khi trả phòng, và khó nắm bắt tổng quan tình trạng phòng theo thời gian thực. Đồ án IT008 (Lập trình trực quan) cần một ứng dụng desktop trực quan, tin cậy xây dựng trên nền tảng .NET 10 WPF MVVM, đáp ứng đầy đủ chu trình vận hành khách sạn khép kín và phù hợp với năng lực triển khai thực tế của nhóm 3 sinh viên trong 6 tuần (`18/11/2026` – `30/12/2026`).

## Solution

Phát triển ứng dụng WPF Quản lý Khách sạn chuẩn kiến trúc MVVM (`CommunityToolkit.Mvvm`) với giao diện Fluent dựa trên `WPF-UI` và quy chuẩn **Windows UI Kit**, kết nối cơ sở dữ liệu Microsoft SQL Server (SSMS & SQL Server Express) qua tầng truy cập dữ liệu ADO.NET thuần (`DataProvider.cs`). Ứng dụng cung cấp:

- Hệ thống xác thực và phân quyền rõ ràng (`Administrator` và `Staff`) dựa trên bảng `Users`, ẩn các mục quản trị đối với lễ tân.
- Quản lý danh mục cơ bản (Loại phòng, Phòng, Khách hàng, Dịch vụ) với đầy đủ trạng thái rỗng, tìm kiếm/lọc và khóa xóa an toàn.
- Thuật toán kiểm tra và ngăn chặn xung đột trùng lịch đặt phòng (theo *Overlap Conflict Rule* trong `GLOSSARY.md`).
- Chu trình lưu trú khép kín: Đặt phòng → Check-in nhận phòng → Gọi dịch vụ phòng → Check-out thanh toán hóa đơn.
- **Tính năng nâng cao 1**: Sơ đồ phòng trực quan (Room Matrix) giữ nền thẻ sáng trung tính (`Card Surface`) và tự động đổi màu vạch/nhãn chỉ báo theo 4 trạng thái phòng bằng XAML `DataTrigger` (Xanh lá `#48BB78`: Trống, Đỏ `#F56565`: Đang ở, Cam `#ED8936`: Đã đặt, Xám `#A0AEC0`: Bảo trì).
- **Tính năng nâng cao 2**: Xem trước biên lai monospace và xuất hóa đơn thanh toán chuẩn UTF-8 ra file văn bản (`.txt`) hoặc in ấn khi hoàn tất trả phòng.
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

- **Architectural Pattern**: MVVM with `CommunityToolkit.Mvvm` 8.4.2 (`ObservableObject`, `[ObservableProperty]`, `[RelayCommand]`). View navigation is managed via `CurrentView` in `MainViewModel` hosting Views inside a central `ContentControl` mapped by `DataTemplate`s in `App.xaml`. Input forms use modal dialogs (`Window.ShowDialog()`) with inline validation errors rather than raw `MessageBox` spam.
- **Solution & Cross-Platform Buildability**: Solution `HotelManagement.slnx` hosts `src/HotelManagement/HotelManagement.csproj` (`net10.0-windows`, `<UseWPF>true</UseWPF>`, `<EnableWindowsTargeting>true</EnableWindowsTargeting>`) and `tests/HotelManagement.Tests/HotelManagement.Tests.csproj` (`xUnit`), pinned via `global.json` (`10.0.100`, `rollForward: latestFeature`). Setting `EnableWindowsTargeting=true` ensures `dotnet build` and `dotnet test` run cleanly across Windows, macOS, and GitHub Actions.
- **UI Framework & Design System**: `WPF-UI` 4.3.0 on `net10.0-windows`, following `DESIGN.md` at repo root (Google `DESIGN.md` format: light neutral surfaces `AppSurfaceBrush` / `CardSurfaceBrush`, Windows accent `#005FB8`, Segoe UI Variable typography, Segoe Fluent Icons, 4px control radius / 8px window-dialog radius, minimum window `1280×720`, mandatory 4 UI states `Loading` / `Empty` / `Error` / `Success`).
  - **Admin Sidebar (8 items)**: `Sơ đồ phòng`, `Đặt phòng`, `Phòng`, `Loại phòng`, `Khách hàng`, `Dịch vụ`, `Hóa đơn`, `Báo cáo`.
  - **Staff Sidebar (6 items)**: hides `Loại phòng` and `Báo cáo`.
- **Charting**: `LiveChartsCore.SkiaSharpView.WPF` 2.0.5, used exclusively by the reporting module (`TASK-11`) for a revenue-by-day `CartesianChart` (`LineSeries`).
- **Database & Persistence**: Microsoft SQL Server (SSMS / SQL Server Express / LocalDB). Schema and deterministic seed data live in `Database/HotelDB.sql` with 8 core tables: `Users`, `RoomTypes`, `Rooms`, `Customers`, `Bookings`, `Services`, `BookingServices`, `Invoices`.
- **Data Access Layer**: Pure ADO.NET (`Microsoft.Data.SqlClient`) via singleton/shared `DataProvider.cs` (reading `HOTEL_DB_CONNECTION` env var with fallback to `.\SQLEXPRESS`) and parameterized DAOs (`UserDAO`, `RoomTypeDAO`, `RoomDAO`, `CustomerDAO`, `ServiceDAO`, `BookingDAO`, `BookingServiceDAO`, `InvoiceDAO`). All 8 entity classes in `Models/` are created up front in `TASK-02` alongside the schema to prevent cross-branch model drift.
- **Domain Status & Role Representations**:
  - `Users.Role` stores canonical English glossary identifiers `'Administrator'` and `'Staff'` in SQL (displayed in Vietnamese UI as `Quản trị` and `Lễ tân`).
  - Status columns store canonical Vietnamese strings defined in `GLOSSARY.md`:
    - `Rooms.Status` (`RoomStatus`): `Trống`, `Đang ở`, `Đã đặt`, `Bảo trì`.
    - `Bookings.Status` (`BookingStatus`): `Đã đặt`, `Đang ở`, `Hoàn thành`, `Đã hủy`.
    - `Invoices.Status` (`InvoiceStatus`): `Chưa thanh toán`, `Đã thanh toán`.
- **Booking Overlap Logic**: implements the *Overlap Conflict Rule* defined in `GLOSSARY.md` (`NewCheckIn < ExistingCheckOut && ExistingCheckIn < NewCheckOut` against bookings in `Đã đặt` or `Đang ở`).
- **Pricing & Settlement Policy**: implements the *Room Charge Policy* (`Math.Max(1, (CheckOut.Date - CheckIn.Date).Days) * PricePerDay` + sum of `BookingServices`) and *Settlement Invariant* in `GLOSSARY.md`.
- **Visual Room Matrix**: Implemented with `ItemsControl` + `WrapPanel` where each card keeps a neutral `Card Surface` background and uses XAML `DataTrigger` on `Status` to bind the status indicator bar/badge brush (`#48BB78`, `#F56565`, `#ED8936`, `#A0AEC0` in `Resources/Colors.xaml`).
- **Reporting & Dashboard**: Neutral `Card Surface` KPI cards (hierarchy via typography, never 3 solid colored card backgrounds), `ProgressBar` occupancy meter, `LiveCharts2` daily revenue line chart, and date-filtered `DataGrid` of paid invoices.

## Testing Decisions

- **What Makes a Good Test**: Automated tests verify observable domain behavior and invariants through public interfaces, never private helpers or live SQL Server state. Manual E2E tests verify XAML layout, SQL Server persistence, and projector readiness.
- **Pre-Agreed Automated Testing Seams (`tests/HotelManagement.Tests` via `xUnit`, run with `dotnet test`)**:
  1. **Scheduling & Booking Seam (`BookingRules`)**: verifies the *Overlap Conflict Rule* (partial overlap, full containment, identical dates, adjacent same-day check-out/check-in allowed, `Đã hủy`/`Hoàn thành` bookings ignored) and date range validation (`CheckOutDate.Date > CheckInDate.Date`).
  2. **Billing, Receipt & Reporting Seam (`InvoiceCalculator` / `ReportAggregator`)**: verifies *Room Charge Policy* (same-day check-in/out billed as 1 night minimum, multi-night × `PricePerDay` + service lines = `1.070.000 đ` canonical case), *Settlement Invariant* state transitions, UTF-8 `.txt` receipt output formatting, and KPI / daily revenue grouping (including empty date ranges returning `0 đ`).
  3. **Input Validation & Role Seam (`ValidationRules` / `RolePermissions`)**: verifies CCCD (9 or 12 digits), Phone (`0` + 9 digits), `PricePerDay > 0`, `Service.Price >= 0`, `BookingService.Quantity >= 1`, delete-lock predicates (cannot delete `Đang ở` room, in-use room type, or in-use service), and `Administrator` (8 nav items) vs `Staff` (6 nav items) visibility.
- **End-to-End Demo & Reset Verification (`TASK-12`)**: Full user scenario rehearsal (`Login → Room Matrix → Booking → Check-in → Add Services → Check-out & .txt Export → Revenue Report`) on a clean Windows machine, backed by 1-click re-seeding via `Database/HotelDB.sql` in SSMS.

## Out of Scope

- Multi-tenant / multi-branch / cloud hosting.
- Online OTA booking API integrations (Agoda, Booking.com).
- Payment gateway integrations (VNPay, MoMo, Stripe) — manual cash (`Tiền mặt`) and bank transfer (`Chuyển khoản`) recording only.
- Keycard hardware, thermal printer drivers, PDF rendering libraries, or dark-mode theme switching.

## Sub-Issues / Task Graph

The backlog comprises 12 tasks (`TASK-01`..`TASK-12`, Issues `#58`–`#65` and `#78`–`#81`) tracked as sub-issues of `#77` with native GitHub blocking edges:

- `TASK-01` (`#58`): Blocked by `None` (frontier root).
- `TASK-02` (`#59`): Blocked by `#58` (`TASK-01`).
- `TASK-03` (`#60`), `TASK-04` (`#61`), `TASK-05` (`#78`), `TASK-06` (`#79`): Blocked by `#59` (`TASK-02`).
- `TASK-07` (`#62`): Blocked by `#61` (`TASK-04`) and `#78` (`TASK-05`).
- `TASK-09` (`#81`): Blocked by `#61` (`TASK-04`).
- `TASK-08` (`#80`): Blocked by `#62` (`TASK-07`) and `#79` (`TASK-06`).
- `TASK-10` (`#63`): Blocked by `#80` (`TASK-08`).
- `TASK-11` (`#64`): Blocked by `#63` (`TASK-10`).
- `TASK-12` (`#65`): Blocked by `#81` (`TASK-09`), `#63` (`TASK-10`), and `#64` (`TASK-11`).

## Delivery Plan (`18/11/2026` → `30/12/2026` — 6 Weeks)

### Milestones & Schedule (Critical-Path Compressed)

- **Sprint 0 — Foundation Unlock (`18/11 – 23/11`, 6 days, inside M1)**:
  - Member A completes `TASK-01` (`20/11`, including 8 navigation placeholders & test project) and `TASK-02` (`23/11`, `HotelDB.sql` + `DataProvider` + all 8 `Models/`) so the entire team is unblocked by end of Week 1.
  - Member B & C review `DESIGN.md` UI contracts and prepare unit test cases.
- **Milestone 1 (`18/11 – 04/12/2026`) — Application Skeleton, Database & Basic Catalogues**:
  - `TASK-01` (`#58`), `TASK-02` (`#59`), `TASK-03` (`#60`), `TASK-04` (`#61`, prioritized by `29/11` to unlock `TASK-09` early), `TASK-05` (`#78`), `TASK-06` (`#79`).
- **Milestone 2 (`05/12 – 18/12/2026`) — Core Business Logic, Room Matrix & Invoices**:
  - `TASK-09` (`#81`, starts early `30/11 – 08/12` right after `TASK-04`), `TASK-07` (`#62`, `05/12 – 10/12`), `TASK-08` (`#80`, `11/12 – 15/12`), `TASK-10` (`#63`, `15/12 – 19/12`).
- **Milestone 3 (`19/12 – 30/12/2026`) — Reporting, Feature Freeze & Demo Finalisation**:
  - `TASK-11` (`#64`, `20/12 – 24/12` — **Feature Freeze on `24/12/2026`**).
  - `TASK-12` (`#65`, `25/12 – 30/12` — 6-day buffer for clean-machine E2E QA, 1-click SQL reset check, slide deck, and 2 rehearsals on `28/12` & `29/12`).

### Team Allocation (4 tasks each)

- **Member A (@phantatdung233) — platform, auth and room management**: `TASK-01` (`#58`), `TASK-02` (`#59`), `TASK-03` (`#60`), `TASK-04` (`#61`), plus PR integration review on `develop` during M2–M3.
- **Member B (@xuanduong29) — customer, service and stay workflow**: `TASK-05` (`#78`), `TASK-06` (`#79`), `TASK-07` (`#62`), `TASK-08` (`#80`).
- **Member C (@ngocthuyen1) — visual room matrix, billing, reporting and demo**: `TASK-09` (`#81`), `TASK-10` (`#63`), `TASK-11` (`#64`), `TASK-12` (`#65`).

### Git Conventions

- Protected branch: `main` (stable build used for submission and demo).
- Integration branch: `develop` (kept fast-forwarded with `main` before cutting feature branches).
- Feature branches: `feature/<task-name>`, cut from `develop`.
- Merge into `develop` only after `dotnet build` + `dotnet test` pass and a teammate verifies the feature.
