# [TASK-10] Xây dựng bộ Seed Data mẫu chuẩn demo (10 phòng, 10 khách, booking mẫu)

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-10`  
**GitHub Issue**: [#63](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/63)  
**Thành viên đảm nhiệm**: **Member B (@xuanduong29)**  
**Mức độ ưu tiên**: `Medium`  
**Nhánh làm việc**: `feature/seed-data`  
**Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)  
**Blocked by**: #79 (TASK-06), #80 (TASK-08), #81 (TASK-09)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng lớp `DbInitializer` để nạp dữ liệu mẫu tự động khi CSDL chưa có dữ liệu.
- Dữ liệu chuẩn bao gồm:
  - 2 Users: 1 Admin (`admin` / `Admin@123`), 1 Staff (`staff` / `Staff@123`).
  - 3 RoomTypes: Phòng Tiêu Chuẩn (Standard - 300,000 đ), Phòng Cao Cấp (Deluxe - 500,000 đ), Phòng Tổng Thống (Suite - 1,200,000 đ).
  - 10 Rooms: Trải đều các tầng 1, 2, 3 với các trạng thái đại diện (Available, Reserved, Occupied, Maintenance).
  - 10 Customers: Thông tin họ tên và CCCD thực tế.
  - 5 Services: Nước suối (15,000 đ), Giặt ủi (50,000 đ), Ăn sáng (100,000 đ), Nước ngọt (20,000 đ), Đưa đón sân bay (200,000 đ).
  - Sample Bookings: Đại diện đủ các trạng thái để phục vụ demo trực quan.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Hàm `Initialize(HotelDbContext context)` kiểm tra `if (!context.Users.Any())` trước khi nạp dữ liệu.
- Cung cấp nút hoặc phương thức `ResetDatabaseToDefault()` hỗ trợ khôi phục dữ liệu ban đầu chỉ với 1 click.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Viết lớp DbInitializer hoặc EF Core Seed Data nạp dữ liệu ban đầu nếu DB trống.
- [ ] Cung cấp tối thiểu: 2 Users (1 Admin, 1 Nhân viên), 3 Loại phòng, 10 Phòng mẫu trải đều các tầng.
- [ ] Cung cấp 10 Khách hàng mẫu có thông tin thực tế, 5 Dịch vụ khách sạn.
- [ ] Cung cấp sẵn các Booking mẫu đại diện đủ các trạng thái để phục vụ demo.

### Ca biên cần xử lý (Edge Cases):
- Đảm bảo mã băm mật khẩu của tài khoản mẫu khớp với thuật toán băm của `AuthService`.
- Khóa ngoại liên kết chính xác giữa RoomTypeId, CustomerId và RoomId.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Test chạy `DbInitializer` trên database trống, kiểm tra số lượng bản ghi tạo ra đúng chỉ tiêu.
- Test chạy lại khi đã có dữ liệu không gây nhân đôi bản ghi.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-10.md` lên GitHub Issue #63.*
