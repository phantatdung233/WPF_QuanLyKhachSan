# [TASK-02] Thiết kế CSDL SQL Server 8 bảng, Trọn bộ 8 Models & DataProvider.cs

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-02`  
**GitHub Issue**: [#59](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/59)  
**Thành viên đảm nhiệm**: **Member A (@phantatdung233)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/database-sqlserver-setup`  
**Milestone**: Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (18/11 – 04/12)  
**Khung thời gian dự kiến**: `21/11/2026 – 23/11/2026` (Sprint 0 — mở khóa toàn nhóm)  
**Labels**: `database`, `core`, `m1`, `ready-for-agent`  
**Blocked by**: #58 (TASK-01)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Tạo thư mục `Database/` tại gốc repo và script `Database/HotelDB.sql` có khả năng chạy lặp lại (idempotent reset 1-click trong SSMS) để tạo mới cơ sở dữ liệu `HotelManagementDb` gồm đầy đủ **8 bảng quan hệ**:
  1. `Users`: `UserId` (PK), `Username` (UNIQUE), `Password`, `FullName`, `Role` (`'Administrator'` hoặc `'Staff'` theo chuẩn `GLOSSARY.md`).
  2. `RoomTypes`: `RoomTypeId` (PK), `TypeName`, `PricePerDay` (`DECIMAL(18,0)` > 0).
  3. `Rooms`: `RoomId` (PK), `RoomNumber` (UNIQUE), `RoomTypeId` (FK), `Status` (`N'Trống'`, `N'Đang ở'`, `N'Đã đặt'`, `N'Bảo trì'`).
  4. `Customers`: `CustomerId` (PK), `FullName`, `CitizenId` (UNIQUE), `Phone`, `Email`.
  5. `Bookings`: `BookingId` (PK), `CustomerId` (FK), `RoomId` (FK), `CheckInDate`, `CheckOutDate`, `Status` (`N'Đã đặt'`, `N'Đang ở'`, `N'Hoàn thành'`, `N'Đã hủy'`), `CreatedDate`.
  6. `Services`: `ServiceId` (PK), `ServiceName`, `Price` (`DECIMAL(18,0)` >= 0).
  7. `BookingServices`: `Id` (PK), `BookingId` (FK), `ServiceId` (FK), `Quantity`, `UnitPrice`.
  8. `Invoices`: `InvoiceId` (PK), `BookingId` (FK), `RoomCharge`, `ServiceCharge`, `TotalAmount`, `PaymentMethod` (`N'Tiền mặt'`, `N'Chuyển khoản'`), `PaymentDate`, `Status` (`N'Chưa thanh toán'`, `N'Đã thanh toán'`).
- Nạp sẵn dữ liệu mẫu chuẩn nhất quán phục vụ kiểm thử tự động và kịch bản demo E2E.
- Xây dựng lớp `DataProvider.cs` trong `DAO/` bằng ADO.NET thuần (`Microsoft.Data.SqlClient`).
- Xây dựng **trọn bộ 8 lớp thực thể trong `Models/`** (`User.cs`, `RoomType.cs`, `Room.cs`, `Customer.cs`, `Service.cs`, `Booking.cs`, `BookingService.cs`, `Invoice.cs`) ngay tại task này để các nhánh `TASK-03..11` dùng chung thống nhất, không bị thiếu `BookingService.cs` hay đụng độ model.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- **Dữ liệu mẫu chuẩn trong `Database/HotelDB.sql`** (nhất quán toán học với kịch bản kiểm thử `1.070.000 đ`):
  - **2 `Users`**:
    - `admin` / `123456` / `N'Trần Minh Quản'` / `'Administrator'`
    - `staff` / `123456` / `N'Lê Thu Ngân'` / `'Staff'`
  - **4 `RoomTypes`**: Đơn (`500000`), Đôi (`700000`), Gia đình (`950000`), VIP (`1200000`).
  - **12 `Rooms`**: `101, 102, 103, 201, 202, 203, 301, 302, 303, 305, 401, 402` (trong đó Phòng `101` thuộc loại `Đơn` `500.000 đ/đêm` để khớp kịch bản mẫu `2 đêm × 500.000 đ + 70.000 đ dịch vụ = 1.070.000 đ`) với phân bổ trạng thái phục vụ demo Sơ đồ phòng: **6 `Trống`, 2 `Đang ở`, 3 `Đã đặt`, 1 `Bảo trì`** (kèm các bản ghi `Bookings` tương ứng cho phòng `Đang ở` và `Đã đặt`).
  - **5 `Customers`**: có tối thiểu khách mẫu `N'Nguyễn Văn An'`, CCCD `'079201001234'`, SĐT `'0901234567'`, Email `'an.nguyen@example.com'`.
  - **5 `Services`**: Nước suối Lavie (`10000`), Nước ngọt (`15000`), Giặt ủi (`30000`), Ăn sáng buffet (`50000`), Thuê xe máy (`80000`).
- **`DAO/DataProvider.cs`**:
  - Đọc chuỗi kết nối ưu tiên từ biến môi trường `HOTEL_DB_CONNECTION`, mặc định fallback về `Server=.\SQLEXPRESS;Database=HotelManagementDb;Trusted_Connection=True;TrustServerCertificate=True;`.
  - Cung cấp 3 phương thức cốt lõi: `ExecuteQuery`, `ExecuteNonQuery`, `ExecuteScalar` hỗ trợ truyền danh sách tham số `@param` vào `SqlCommand.Parameters` (tách token chuẩn bằng Regex để không cắt nhầm dấu phẩy/ngoặc kế bên `@param`, chống SQL Injection).
- **8 lớp `Models/`**: mỗi lớp có constructor rỗng và constructor nhận `DataRow` (kiểm tra an toàn `row.IsNull(...)` / `DBNull.Value`). `User.RoleDisplay` ánh xạ `'Administrator'` → `"Quản trị"`, `'Staff'` → `"Lễ tân"`.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Script `Database/HotelDB.sql` chạy lại nhiều lần không lỗi (tự ngắt kết nối cũ / tạo mới `HotelManagementDb`), tạo đủ 8 bảng có khóa chính, khóa ngoại và ràng buộc `CHECK` cho các cột `Status` / `Role`.
- [ ] Dữ liệu seed chuẩn: 2 Users (`Administrator`/`Staff`), 4 loại phòng, 12 phòng (6 Trống, 2 Đang ở, 3 Đã đặt, 1 Bảo trì; Phòng 101 loại Đơn `500.000 đ`), 5 khách hàng, 5 dịch vụ và dữ liệu booking/hóa đơn mẫu.
- [ ] Cài đặt `Microsoft.Data.SqlClient` và hoàn thiện `DAO/DataProvider.cs` hỗ trợ tham số hóa an toàn cùng cấu hình `HOTEL_DB_CONNECTION`.
- [ ] Hoàn thiện đủ **8 lớp trong `Models/`**: `User`, `RoomType`, `Room`, `Customer`, `Service`, `Booking`, `BookingService`, `Invoice` kèm mapping `DataRow` chống lỗi `DBNull`.
- [ ] Viết unit test trong `HotelManagement.Tests` kiểm tra mapping `DataTable`/`DataRow` cho cả 8 Models (bao gồm cột nullable như `Email`, `PaymentDate`, `PaymentMethod`) và bộ tách tham số `@param` của `DataProvider`.

### Ca biên cần xử lý (Edge Cases):
- Khác tên SQL Server instance giữa máy các thành viên (`.\SQLEXPRESS`, `localhost`, `(localdb)\MSSQLLocalDB`) → đổi qua biến môi trường `HOTEL_DB_CONNECTION` mà không cần sửa code nguồn.
- Các cột cho phép `NULL` (`Customers.Email`, `Invoices.PaymentDate`, `Invoices.PaymentMethod`) khi map sang C# không được ném `InvalidCastException`.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động (`TDD` seam): `dotnet test` kiểm chứng tạo `DataTable` giả lập cho 8 bảng, khởi tạo cả 8 `Models` từ `DataRow` (kể cả giá trị `DBNull.Value`) và kiểm tra hàm tách tham số `@param` trong câu SQL có dấu ngoặc `(@a, @b)`.
- Thực tế SQL Server: Mở SSMS chạy F5 `Database/HotelDB.sql` 2 lần liên tiếp thành công, kiểm tra `SELECT * FROM Rooms` đủ 12 phòng đúng tỷ lệ 4 trạng thái.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-02.md` lên GitHub Issue #59.*
