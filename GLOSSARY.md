# Hotel Management System (Quản Lý Khách Sạn)

The domain model for the WPF .NET 10 hotel management system (IT008). Defines canonical concepts, entity states, and operational rules across all modules.

## Roles & Authentication

**User**:
An account authorized to access the system, possessing a username, password, and role.
_Avoid_: Account, login, employee

**Role**:
The permission level of a User, strictly partitioned into Administrator and Staff.
_Avoid_: Access level, group, type

**Administrator**:
A Role granting full access, including room-category management and revenue reporting.
_Avoid_: Admin (in prose), manager, superuser

**Staff**:
A Role limited to reception duties, with administrative and revenue features hidden.
_Avoid_: Employee, receptionist, nhân viên

## Rooms & Facilities

**RoomType**:
A category of hotel rooms sharing common physical features, occupancy capacity, and base nightly rate.
_Avoid_: Category, class, tier

**Room**:
An individual lodging unit identified by a unique room code, belonging to a RoomType, and maintaining an operational state.
_Avoid_: Chamber, unit

**RoomStatus**:
The operational lifecycle state of a Room: `Available` (ready for check-in), `Reserved` (booked in advance), `Occupied` (guest staying), and `Maintenance` (out of order).
_Avoid_: Trạng thái phòng, condition

## Customers & Services

**Customer**:
A guest registered in the system identified by their Citizen ID (CCCD/CMND), full name, and phone number.
_Avoid_: Client, guest (as entity name), buyer

**Service**:
An amenity or add-on offered by the hotel (such as laundry, food, minibar items, or airport transit) with a fixed unit price.
_Avoid_: Item, product, amenity

## Bookings & Operations

**Booking**:
A reservation record linking a Customer and a Room over a defined time interval from Check-in date to Check-out date.
_Avoid_: Reservation, order, phiếu thuê

**BookingStatus**:
The lifecycle state of a Booking: `Reserved` (created in advance), `Occupied` (checked in), `Completed` (checked out and settled), and `Cancelled` (revoked prior to stay).
_Avoid_: State, stage

**BookingService**:
A junction entity linking an active Booking to a consumed Service, recording the quantity and unit price at time of use.
_Avoid_: Order detail, service usage, dịch vụ phát sinh

**Check-in**:
The operational transition when a guest receives access to the room, changing Booking status to `Occupied` and Room status to `Occupied`.
_Avoid_: Nhận phòng

**Check-out**:
The operational transition when a guest departs, triggering invoice settlement, changing Booking status to `Completed`, and releasing Room to `Available`.
_Avoid_: Trả phòng

## Billing & Finance

**Invoice**:
The financial statement generated upon check-out detailing room charges and all associated service charges.
_Avoid_: Bill, receipt, hóa đơn

**InvoiceStatus**:
The payment state of an Invoice: `Pending` (unpaid) and `Paid` (settled via cash or bank transfer).
_Avoid_: Thanh toán

**Settlement Invariant**:
A `Paid` Invoice implies its Booking is `Completed` and its Room is back to `Available`; a Booking is never `Completed` while its Invoice is `Pending`. All three transition together.
_Avoid_: Marking an invoice paid without completing the booking

**Room Charge Policy**:
The billing formula where room cost equals nightly base rate multiplied by the number of overnight stays: `Math.Max(1, (CheckOut.Date - CheckIn.Date).Days) * UnitPrice`.
_Avoid_: Hourly billing, fractional day billing

**Overlap Conflict Rule**:
The scheduling invariant preventing overbooking on the same room: an overlap exists if and only if `NewCheckIn < ExistingCheckOut && ExistingCheckIn < NewCheckOut` against any active booking.
_Avoid_: Single-point date collision check
