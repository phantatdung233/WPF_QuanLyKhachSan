# Specification: Hotel Management System (WPF .NET 10 MVVM)

**Issue Tracker Reference**: [Issue #77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Status**: `ready-for-agent`

---

## Problem Statement

Quản lý khách sạn truyền thống bằng sổ sách hoặc bảng tính Excel dễ dẫn đến sai sót: trùng lịch phòng (overbooking), thất thoát chi phí dịch vụ phát sinh khi khách lưu trú, tính sai hóa đơn khi trả phòng, và khó nắm bắt tổng quan tình trạng phòng theo thời gian thực. Đồ án IT008 (Lập trình trực quan) cần một ứng dụng desktop trực quan, tin cậy xây dựng trên nền tảng .NET 10 WPF MVVM, đáp ứng đầy đủ chu trình vận hành khách sạn khép kín và phù hợp với năng lực triển khai thực tế của nhóm sinh viên năm 2.

## Solution

Phát triển ứng dụng WPF Quản lý Khách sạn chuẩn kiến trúc MVVM (`CommunityToolkit.Mvvm`) kết nối cơ sở dữ liệu Microsoft SQL Server (SSMS & SQL Server Express) qua tầng truy cập dữ liệu ADO.NET thuần (`DataProvider.cs`). Ứng dụng cung cấp:
- Hệ thống xác thực và phân quyền đơn giản (Admin và Nhân viên) dựa trên bảng `Users`.
- Quản lý danh mục cơ bản (Loại phòng, Phòng, Khách hàng, Dịch vụ).
- Thuật toán kiểm tra và ngăn chặn xung đột trùng lịch đặt phòng (`A < D && C < B`).
- Chu trình lưu trú khép kín: Đặt phòng → Check-in nhận phòng → Gọi dịch vụ phòng → Check-out thanh toán hóa đơn.
- **Tính năng nâng cao 1**: Sơ đồ phòng trực quan (Room Matrix) tự động đổi màu thẻ phòng theo 4 trạng thái bằng XAML `DataTrigger` (Xanh lá: Trống, Đỏ: Đang ở, Cam: Đã đặt, Xám: Dọn dẹp/Bảo trì).
- **Tính năng nâng cao 2**: Xuất hóa đơn thanh toán ra file văn bản (.txt) hoặc in ấn khi hoàn tất trả phòng.
- Màn hình Báo cáo & Thống kê doanh thu với các thẻ KPI nổi bật, thanh tiến trình tỷ lệ công suất phòng và bảng chi tiết doanh thu bằng WPF XAML thuần.

## User Stories

1. As a Hotel Administrator, I want to log in with my username and password, so that I can access administrative settings, manage room categories, and view revenue reports.
2. As a Hotel Staff member, I want to log in with my account, so that I can perform reception tasks while administrative and revenue management features are hidden.
3. As a User, I want a persistent session indicator displaying my name and role with a Logout button, so that I can exit securely.
4. As a Hotel Administrator, I want to manage Room Types with base daily prices, so that rooms are categorized and priced consistently.
5. As a Hotel Staff member, I want to view a list of rooms with their room number, type, and current status, so that room availability is immediately clear.
6. As a Hotel Staff member, I want to search and filter rooms by status or room type, so that matching rooms can be found quickly for guests.
7. As a Hotel Staff member, I want to create and update Customer records (Full Name, Citizen ID / CCCD, Phone, Email), so that guest profiles are stored accurately.
8. As a Hotel Staff member, I want Citizen ID and Phone Number formats to be validated, so that invalid input is caught before saving.
9. As a Hotel Staff member, I want to search for existing customers by Name or Citizen ID with real-time results, so that returning guests are recognized quickly.
10. As a Hotel Staff member, I want to manage Hotel Services with a name and unit price (e.g., Laundry, Breakfast, Drinks, Motorcycle Rental).
11. As a Hotel Staff member, I want to create a room reservation (Booking) by selecting a Customer, Room, Check-in Date, and Check-out Date.
12. As a Hotel Staff member, I want the system to execute an overlap check (`CheckIn < ExistingCheckOut && ExistingCheckIn < CheckOut`), so that double-booking the same room is prevented.
13. As a Hotel Staff member, I want to execute a Check-in action for a booked guest, so that the booking transitions to Occupied and the room turns Occupied.
14. As a Hotel Staff member, I want to record extra services used by an occupied room with quantities, so that service costs are linked to the active booking.
15. As a Hotel Staff member, I want to perform a Check-out action calculating room charges (`Math.Max(1, days) * PricePerDay`) plus service charges, so that a final invoice is generated.
16. As a Hotel Staff member, I want to mark the invoice as Paid with Cash or Bank Transfer, completing the booking and resetting room status to Available.
17. As a Hotel Staff member, I want to view an interactive Room Matrix where cards change color based on status (Green for Available, Red for Occupied, Orange for Reserved, Gray for Maintenance).
18. As a Hotel Staff member, I want to click any room card on the Room Matrix to view details or double-click to initiate booking or check-out.
19. As a Hotel Staff member, I want to export or print the itemized invoice to a clean text file upon checkout.
20. As a Hotel Administrator, I want to view revenue KPI metrics, occupancy progress bars, and an itemized paid invoice table over custom date ranges.
21. As a Student / Presenter, I want a single-click SQL script (`HotelDB.sql`) to seed demo data (users, rooms, customers, bookings), so that demonstrations can be reset reliably in SSMS.

## Implementation Decisions

- **Architectural Pattern**: MVVM with `CommunityToolkit.Mvvm` (`ObservableObject`, `[ObservableProperty]`, `[RelayCommand]`). View navigation is managed via `CurrentView` property in `MainViewModel` hosting Views inside a central `ContentControl`. Input forms use `Window.ShowDialog()`.
- **Database & Persistence**: Microsoft SQL Server (SSMS / SQL Server Express). Database schema and seed data are maintained in `Database/HotelDB.sql` with 8 core tables: `Users`, `RoomTypes`, `Rooms`, `Customers`, `Bookings`, `Services`, `BookingServices`, `Invoices`.
- **Data Access Layer**: Pure ADO.NET (`SqlConnection`, `SqlCommand`, `SqlDataReader`, `SqlDataAdapter`) via reusable `DataProvider.cs` and Data Access Objects (`UserDAO`, `RoomDAO`, `CustomerDAO`, `BookingDAO`, `ServiceDAO`, `InvoiceDAO`).
- **Domain Status Representations**:
  - `RoomStatus`: `Trống` (Available), `Đang ở` (Occupied), `Đã đặt` (Reserved), `Bảo trì` (Maintenance).
  - `BookingStatus`: `Đã đặt` (Reserved), `Đang ở` (Occupied), `Hoàn thành` (Completed), `Đã hủy` (Cancelled).
  - `InvoiceStatus`: `Chưa thanh toán` (Pending), `Đã thanh toán` (Paid).
- **Booking Overlap Logic**: Two intervals `[A, B]` and `[C, D]` overlap if and only if `A < D && C < B`.
- **Pricing Policy**: Room charge calculated on a daily basis: `TotalDays = Math.Max(1, (CheckOut.Date - CheckIn.Date).Days); RoomCost = TotalDays * PricePerDay`. Total Invoice = `RoomCost + SUM(Quantity * UnitPrice)`.
- **Visual Room Matrix**: Implemented with WPF `ItemsControl` / `WrapPanel` using XAML `DataTrigger` styles for status-driven background colors.
- **Reporting & Dashboard**: Pure WPF XAML metric KPI cards, `ProgressBar` occupancy meter, and `DataGrid` date-filtered invoice list without third-party charting dependencies.

## Testing Decisions

- **Testing Philosophy**: Functional & Manual Verification testing aligned with course evaluation criteria.
- **Demo Script**: End-to-end user scenario testing covering Login → Room Matrix inspection → Booking → Check-in → Add Services → Check-out & Text Invoice Export → Revenue Report verification.
- **Reset Reliability**: Instant database re-seeding via executing `HotelDB.sql` in SSMS before presentation rehearsals.

## Out of Scope

- Multi-tenant / cloud hosting.
- Online booking API integrations (Agoda, Booking.com).
- Payment gateway integrations (VNPay, MoMo, Stripe) — manual cash and bank transfer recording only.
- Complex cryptographic hardware / keycard integrations.

## Sub-Issues / Task Graph

The project backlog contains 12 streamlined tasks assigned equally across 3 team members, documented in `docs/tasks/TASK-01.md` through `docs/tasks/TASK-12.md` and tracked on GitHub Issues.
