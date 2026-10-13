# [TASK-02] Thiết kế HotelDbContext Code First 8 bảng & SQLite Migration

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-02`  
**GitHub Issue**: [#59](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/59)  
**Thành viên đảm nhiệm**: **Member A (@phantatdung233)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/database-efcore-setup`  
**Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)  
**Blocked by**: #58 (TASK-01)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Cài đặt `Microsoft.EntityFrameworkCore.Sqlite` và `Microsoft.EntityFrameworkCore.Design`.
- Định nghĩa đầy đủ 8 Entity trong `Models/`:
  1. `User` (Id, Username, PasswordHash, Role, FullName, CreatedAt)
  2. `RoomType` (Id, Name, BasePrice, Description, MaxOccupancy)
  3. `Room` (Id, RoomCode, RoomName, RoomTypeId, Status, Notes)
  4. `Customer` (Id, FullName, CitizenId, PhoneNumber, Email, Address)
  5. `Booking` (Id, CustomerId, RoomId, CheckInDate, CheckOutDate, Status, TotalAmount, CreatedAt)
  6. `Service` (Id, Name, Price, Description, IsActive)
  7. `BookingService` (Id, BookingId, ServiceId, Quantity, UnitPrice, ConsumedAt)
  8. `Invoice` (Id, BookingId, RoomCharge, ServiceCharge, TotalAmount, PaymentMethod, PaymentDate, Status)
- Xây dựng lớp `HotelDbContext` kế thừa `DbContext`.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Sử dụng Fluent API trong `OnModelCreating` để cấu hình các ràng buộc khóa ngoại (1-N giữa RoomType-Room, Customer-Booking, Room-Booking, Booking-BookingService, Service-BookingService, Booking-Invoice).
- Sử dụng Enum chuẩn từ GLOSSARY.md: `RoomStatus`, `BookingStatus`, `InvoiceStatus`.
- Tự động sinh file SQLite `hotel.db` cục bộ khi khởi chạy app (`Database.Migrate()` hoặc `Database.EnsureCreated()`).

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Cài đặt Microsoft.EntityFrameworkCore.Sqlite và Microsoft.EntityFrameworkCore.Design.
- [ ] Định nghĩa đầy đủ 8 Entity: User, RoomType, Room, Customer, Booking, Service, BookingService, Invoice.
- [ ] Cấu hình Fluent API / Data Annotations đúng mối quan hệ khóa ngoại (1-N, N-N).
- [ ] Khởi tạo Initial Migration thành công và kiểm tra sinh file CSDL SQLite cục bộ tự động khi khởi chạy app.

### Ca biên cần xử lý (Edge Cases):
- Ràng buộc Unique trên `RoomCode`, `Username`, và `CitizenId`.
- Định dạng kiểu dữ liệu số thực cho giá tiền (`decimal` với độ chính xác chuẩn).

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Kiểm thử tạo và xóa dữ liệu trên in-memory SQLite context.
- Kiểm thử ràng buộc khóa ngoại không cho phép thêm Room với RoomTypeId không tồn tại.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-02.md` lên GitHub Issue #59.*
