# Specification: Hotel Management System (WPF .NET 8 MVVM)

**Issue Tracker Reference**: [Issue #77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Status**: `ready-for-agent`

---

## Problem Statement

Quản lý khách sạn truyền thống bằng sổ sách hoặc bảng tính Excel dễ dẫn đến sai sót: xung đột trùng lịch phòng (overbooking), thất thoát chi phí dịch vụ phát sinh khi khách lưu trú, tính sai hóa đơn khi trả phòng, và khó nắm bắt tổng quan tình trạng phòng theo thời gian thực. Đồ án IT008 cần một ứng dụng desktop chuyên nghiệp xây dựng trên nền tảng .NET 8 WPF MVVM, đáp ứng đầy đủ chu trình vận hành khách sạn khép kín, trực quan và tin cậy.

## Solution

Phát triển ứng dụng WPF Quản lý Khách sạn chuẩn kiến trúc MVVM Toolkit với cơ sở dữ liệu SQLite Code First gồm 8 bảng thực thể. Ứng dụng cung cấp:
- Hệ thống xác thực và phân quyền (Admin và Nhân viên).
- Quản lý danh mục độc lập (Loại phòng, Phòng, Khách hàng, Dịch vụ).
- Thuật toán tự động ngăn chặn trùng lịch đặt phòng.
- Chu trình lưu trú khép kín (Đặt phòng → Check-in nhận phòng → Gọi dịch vụ phòng → Check-out thanh toán hóa đơn).
- Sơ đồ phòng trực quan đổi màu theo trạng thái bằng DataTrigger.
- Báo cáo thống kê doanh thu và công suất phòng dạng biểu đồ LiveCharts2 với xử lý bất đồng bộ (async).

## User Stories

1. As a Hotel Administrator, I want to log into the system with my credentials, so that I can access administrative settings, manage users, and view revenue analytics.
2. As a Hotel Staff member, I want to log in with my account, so that I can perform daily reception operations without having access to restricted administrative settings.
3. As a User, I want password inputs to be masked and securely hashed in the database, so that unauthorized persons cannot retrieve plaintext passwords.
4. As a User, I want a persistent session indicator displaying my name and role with a Logout button, so that I can exit securely when ending my shift.
5. As a Hotel Administrator, I want to create, view, update, and delete Room Types with base nightly prices, so that rooms can be categorized and priced consistently.
6. As a Hotel Administrator, I want the system to reject deleting a Room Type if active rooms are linked to it, so that database integrity is protected.
7. As a Hotel Staff member, I want to view a list of rooms with their room code, name, type, and current status, so that I can know room readiness immediately.
8. As a Hotel Staff member, I want to filter and search rooms by status or room type, so that I can quickly locate matching rooms for guests.
9. As a Hotel Staff member, I want to create and update Customer records (Full Name, Citizen ID / CCCD, Phone, Email, Address), so that customer profiles are accurately captured.
10. As a Hotel Staff member, I want Citizen ID (9 or 12 digits) and Phone Number formats to be validated in real time, so that invalid input is caught before saving.
11. As a Hotel Staff member, I want to search for existing customers by Name or Citizen ID with live results, so that returning guests can be identified instantly.
12. As a Hotel Staff member, I want to create a Hotel Service with a name and unit price (e.g., Laundry, Breakfast, Drinks), so that add-on amenities can be billed accurately.
13. As a Hotel Staff member, I want to create a room reservation (Booking) by selecting a Customer, Room, Check-in Date, and Check-out Date, so that guest accommodation is booked in advance.
14. As a Hotel Staff member, I want the system to execute an overlap check (`CheckIn < ExistingCheckOut && ExistingCheckIn < CheckOut`), so that double-booking the same room is strictly prevented.
15. As a Hotel Staff member, I want to execute a Check-in action for a booked or walk-in guest, so that the booking transitions to Occupied and the room status turns to Occupied.
16. As a Hotel Staff member, I want to record extra services used by a currently occupied room with itemized quantities, so that service consumption is linked to the active booking.
17. As a Hotel Staff member, I want to adjust or remove service line items before checkout, so that accidental entries can be corrected.
18. As a Hotel Staff member, I want to perform a Check-out action that calculates total room charges (`Math.Max(1, days) * UnitPrice`) plus total service charges, so that a final invoice is generated.
19. As a Hotel Staff member, I want to record the payment method (Cash or Bank Transfer) and mark the invoice as Paid, so that the booking completes and the room changes to Cleaning/Available.
20. As a Hotel Staff member, I want to view an interactive Room Matrix where each room card automatically changes color based on status (Green for Available, Yellow for Reserved, Red for Occupied, Gray for Maintenance), so that room statuses are visible at a glance.
21. As a Hotel Staff member, I want to click any room card on the Room Matrix to view its current occupant or immediately initiate a booking, so that workflow friction is minimized.
22. As a Hotel Administrator, I want to view revenue trends over time via LiveCharts2 column/line charts and occupancy breakdown via pie charts, so that business performance can be analyzed.
23. As a Hotel Administrator, I want reports to load asynchronously with a progress indicator, so that the user interface never freezes while aggregating large datasets.
24. As a Developer/Tester, I want a single-click sample seed database (10 rooms, 10 customers, 5 services, 2 users, demo bookings), so that demonstrations can be reset reliably.

## Implementation Decisions

- **Architectural Pattern**: MVVM using `CommunityToolkit.Mvvm` (`ObservableObject`, `[ObservableProperty]`, `[RelayCommand]`).
- **Database & Persistence**: EF Core 8 with SQLite database provider (`Microsoft.EntityFrameworkCore.Sqlite`). Code-First approach with 8 core entities: `User`, `RoomType`, `Room`, `Customer`, `Booking`, `Service`, `BookingService`, `Invoice`.
- **Navigation Structure**: Single-Window Shell (`MainWindow`) with sidebar menu and top header, driving a central dynamic View/ViewModel switcher via `INavigationService`.
- **Domain Status Representations**:
  - `RoomStatus` stored as Enum: `Available`, `Reserved`, `Occupied`, `Maintenance`, `Cleaning`.
  - `BookingStatus` stored as Enum: `Reserved`, `Occupied`, `Completed`, `Cancelled`.
  - `InvoiceStatus` stored as Enum: `Pending`, `Paid`.
  - Localized to Vietnamese on UI using WPF `IValueConverter` / Display Helpers.
- **Booking Overlap Logic**: Enforces time interval disjointness: Conflict exists if and only if `NewCheckIn < ExistingCheckOut && ExistingCheckIn < NewCheckOut` for any non-cancelled/completed booking on the same room.
- **Pricing Policy**: Room charge calculated on a nightly basis: `TotalDays = Math.Max(1, (CheckOut.Date - CheckIn.Date).Days); RoomCost = TotalDays * RoomPrice`. Total Invoice = `RoomCost + SUM(BookingService.Quantity * Service.Price)`.
- **Visual Room Matrix**: Implemented using an `ItemsControl` / `UniformGrid` with WPF `DataTemplate` and XAML `DataTrigger` bound to `Room.Status`.
- **Charting Library**: `LiveChartsCore.SkiaSharpView.WPF` with asynchronous data retrieval via `Task.Run` and EF Core async queries.

## Testing Decisions

- **Test Philosophy**: Behavioral testing only. No testing of internal private methods or XAML markup rendering details. Tests assert on publicly observable state and database side effects.
- **Primary Seam**: ViewModel and Service layer driven by xUnit with an in-memory SQLite context.
- **Test Suite Scenarios**:
  - `AuthServiceTests`: Login validation, password verification, session state handling.
  - `BookingConflictTests`: Exact date overlap, partial overlap, adjacent booking non-overlap, same-day booking.
  - `LifecycleWorkflowTests`: Complete sequence from Reservation → Check-in → Add Services → Check-out → Invoice payment.
  - `InvoiceCalculationTests`: Zero services, multiple services, minimum 1-night billing rules.

## Out of Scope

- Multi-hotel / multi-branch tenancy.
- Online booking integrations (OTAs like Agoda/Booking.com) or external webhook APIs.
- Payment gateway integrations (VNPay, Momo, Stripe) — only recording Cash/Bank Transfer offline.
- Hardware device integrations (keycard encoders, thermal receipt printers).

## Sub-Issues / Task Graph

The complete breakdown of 23 implementation tasks linked as sub-issues to this specification is maintained in `docs/tasks/` and tracked on GitHub Issues `#58` through `#81`.
