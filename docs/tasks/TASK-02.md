# [TASK-02] Thiết kế CSDL SQL Server 8 bảng & Lớp dùng chung DataProvider.cs

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-02`  
**GitHub Issue**: [#59](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/59)  
**Thành viên đảm nhiệm**: **Member A (@phantatdung233)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/database-sqlserver-setup`  
**Milestone**: Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (Tuần 1-3)  
**Blocked by**: #58 (TASK-01)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Tạo thư mục `Database/` và file script `Database/HotelDB.sql`.
- Script tạo cơ sở dữ liệu `HotelManagementDb` trên SQL Server gồm đầy đủ 8 bảng quan hệ:
  1. `Users`: `UserId` (PK), `Username`, `Password`, `FullName`, `Role` (Admin/Staff).
  2. `RoomTypes`: `RoomTypeId` (PK), `TypeName`, `PricePerDay`.
  3. `Rooms`: `RoomId` (PK), `RoomNumber`, `RoomTypeId` (FK), `Status` (Trống, Đang ở, Đã đặt, Bảo trì).
  4. `Customers`: `CustomerId` (PK), `FullName`, `CitizenId`, `Phone`, `Email`.
  5. `Bookings`: `BookingId` (PK), `CustomerId` (FK), `RoomId` (FK), `CheckInDate`, `CheckOutDate`, `Status`, `CreatedDate`.
  6. `Services`: `ServiceId` (PK), `ServiceName`, `Price`.
  7. `BookingServices`: `Id` (PK), `BookingId` (FK), `ServiceId` (FK), `Quantity`, `UnitPrice`.
  8. `Invoices`: `InvoiceId` (PK), `BookingId` (FK), `RoomCharge`, `ServiceCharge`, `TotalAmount`, `PaymentMethod`, `PaymentDate`, `Status`.
- Cài đặt sẵn khối lệnh nạp dữ liệu mẫu ban đầu (`INSERT INTO`) trong script để phục vụ demo.
- Cài package `Microsoft.Data.SqlClient` (bản stable mới nhất, hiện tại 7.1.1) qua NuGet: đây là driver ADO.NET chính thức của Microsoft, thay cho `System.Data.SqlClient` đã ngừng phát triển tính năng mới.
- Viết lớp `DataProvider.cs` trong thư mục `DAO/` bằng ADO.NET thuần trên namespace `Microsoft.Data.SqlClient` (`using Microsoft.Data.SqlClient;`).
- Viết các class Models cơ bản (`User.cs`, `RoomType.cs`, `Room.cs`) có constructor nhận tham số `DataRow` để map dữ liệu nhanh chóng.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Cấu hình chuỗi kết nối (Connection String) trong `DataProvider.cs`:
  `Server=.\\SQLEXPRESS;Database=HotelManagementDb;Trusted_Connection=True;TrustServerCertificate=True;` (hoặc cấu hình dễ thay đổi qua file cấu hình).
- Lớp `DataProvider` xây dựng theo mô hình Singleton (`DataProvider.Instance`) gồm 3 hàm cốt lõi:
  - `public DataTable ExecuteQuery(string query, object[] parameter = null)`: Phục vụ các câu lệnh `SELECT`.
  - `public int ExecuteNonQuery(string query, object[] parameter = null)`: Phục vụ `INSERT`, `UPDATE`, `DELETE`.
  - `public object ExecuteScalar(string query, object[] parameter = null)`: Phục vụ lấy giá trị đơn lẻ (`COUNT`, `MAX`, `SCOPE_IDENTITY()`).
- Tách tham số dạng `@param` và gán vào `SqlCommand.Parameters` để ngăn ngừa tấn công SQL Injection.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Tạo file script Database/HotelDB.sql gồm lệnh CREATE DATABASE, CREATE TABLE cho 8 bảng: Users, RoomTypes, Rooms, Customers, Bookings, Services, BookingServices, Invoices.
- [ ] Nạp sẵn dữ liệu mẫu (INSERT INTO) trong script: 2 Users (Admin, Staff), 4 loại phòng, 12 phòng, 5 khách hàng, 5 dịch vụ.
- [ ] Chạy file script thành công trên SSMS (SQL Server Management Studio / SQL Server Express).
- [ ] Viết lớp DataProvider.cs (ADO.NET) kết nối chuỗi .\SQLEXPRESS với 3 phương thức: ExecuteQuery, ExecuteNonQuery, ExecuteScalar có truyền tham số chống SQL Injection.
- [ ] Viết các class Models cơ bản (User, RoomType, Room) có constructor nhận DataRow.

### Ca biên cần xử lý (Edge Cases):
- Kiểm tra tên máy chủ SQL Server khác nhau giữa các thành viên (ví dụ máy dùng `.\SQLEXPRESS`, máy dùng `localhost`, máy dùng `(localdb)\mssqllocaldb`) -> để chuỗi kết nối ở vị trí tập trung dễ sửa.
- Xử lý kiểm tra giá trị `DBNull.Value` khi đọc dữ liệu từ `DataRow` để tránh lỗi ép kiểu (cast exception).

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Mở SSMS, mở file `HotelDB.sql` bấm F5 (Execute) xác nhận tạo đầy đủ 8 bảng và nạp đủ dữ liệu mẫu.
- Chạy hàm kiểm thử gọi `DataProvider.Instance.ExecuteQuery("SELECT * FROM Users")` trả về đúng 2 dòng dữ liệu.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-02.md` lên GitHub Issue #59.*
