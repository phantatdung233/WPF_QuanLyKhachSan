# [TASK-08] Module Check-in, Check-out & Gọi dịch vụ phòng

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-08`  
**GitHub Issue**: [#80](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/80)  
**Thành viên đảm nhiệm**: **Member B (@xuanduong29)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/checkin-checkout-services`  
**Milestone**: Mốc 2: Nghiệp Vụ Cốt Lõi, Sơ Đồ Phòng & Hóa Đơn (05/12 – 18/12)  
**Khung thời gian dự kiến**: `11/12/2026 – 15/12/2026` *(bàn giao đúng hạn 15/12 để mở khóa `TASK-10`)*  
**Labels**: `booking`, `service`, `workflow`, `m2`, `ready-for-agent`  
**Blocked by**: #62 (TASK-07), #79 (TASK-06)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Mở rộng các thao tác chuyển trạng thái lưu trú đồng bộ giữa `BookingDAO.cs` và `RoomDAO.cs`.
- Xây dựng `BookingServiceDAO.cs` trong `DAO/` thao tác với bảng `BookingServices` (dùng model `BookingService.cs` từ `TASK-02`).
- Hoàn thiện hộp thoại `AddServiceDialog.xaml` theo `docs/design/DELIVERY-PLAN.md` Vòng 3 & `WIREFRAMES.md` mục 10, cùng các nút hành động ngữ cảnh trên danh sách Booking / Sơ đồ phòng:
  1. **Check-in**: chuyển cả `Booking` và `Room` sang trạng thái `Đang ở`.
  2. **Gọi dịch vụ phòng**: ghi nhận dịch vụ phát sinh kèm số lượng và snapshot `UnitPrice` cho booking đang `Đang ở`, hỗ trợ xóa dòng nhập nhầm.
  3. **Check-out**: khởi tạo hóa đơn chờ thanh toán (nếu chưa có) và điều hướng sang màn hình Hóa đơn (`InvoiceViewModel` — `TASK-10`) với đúng `BookingId`.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- **Quy trình Check-in**:
  - Áp dụng cho đơn đặt phòng đang ở trạng thái `Đã đặt` (và phòng hiện không đang có khách khác `Đang ở`).
  - Thực hiện cập nhật đồng bộ (ưu tiên trong `SqlTransaction`):
    - `Bookings.Status = N'Đang ở'`
    - `Rooms.Status = N'Đang ở'`
- **Hộp thoại Gọi dịch vụ phòng (`AddServiceDialog.xaml`)**:
  - Chỉ mở được cho booking/phòng đang có trạng thái `Đang ở`.
  - Chọn dịch vụ từ `ComboBox` (nạp từ `ServiceDAO`), nhập `Quantity` (số nguyên `>= 1`), hiển thị tức thời `Thành tiền = Quantity * UnitPrice`.
  - Bấm `Thêm dịch vụ`: lưu vào `BookingServices (BookingId, ServiceId, Quantity, UnitPrice)` — lưu cứng `UnitPrice` tại thời điểm gọi.
  - Bên dưới hiển thị bảng kê tất cả dịch vụ phòng đó đã gọi kèm nút `Xóa` trên từng dòng và tổng tiền dịch vụ cập nhật tức thời.
- **Khởi tạo Check-out**:
  - Áp dụng cho booking đang `Đang ở`: bấm `Check-out` sẽ chuyển ngữ cảnh `BookingId` sang `MainViewModel.NavigateToInvoice(bookingId)` để lễ tân đối soát tiền phòng + tiền dịch vụ và xác nhận thanh toán ở `TASK-10` (chỉ khi thanh toán xong ở `TASK-10` mới hoàn tất `Booking` và giải phóng `Room` theo **Settlement Invariant** trong `GLOSSARY.md`).

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Thao tác `Check-in` đổi đồng bộ `Bookings.Status = N'Đang ở'` và `Rooms.Status = N'Đang ở'` (an toàn giao dịch).
- [ ] `BookingServiceDAO.cs` hỗ trợ lấy bảng kê dịch vụ theo `BookingId` (JOIN tên dịch vụ), thêm dòng dịch vụ và xóa dòng nhập nhầm.
- [ ] `AddServiceDialog.xaml` validate inline `Quantity >= 1`, tự tính thành tiền từng dòng và tổng tiền dịch vụ toàn phòng theo thời gian thực.
- [ ] Thao tác `Check-out` truyền chính xác `BookingId` sang luồng Hóa đơn (`InvoiceViewModel`) mà chưa vội giải phóng phòng khi hóa đơn chưa `Đã thanh toán`.
- [ ] Có unit test trong `HotelManagement.Tests` cho quy tắc chuyển trạng thái hợp lệ (chỉ `Đã đặt` mới Check-in được; chỉ `Đang ở` mới gọi dịch vụ / Check-out được) và tính tổng tiền bảng kê `BookingServices`.

### Ca biên cần xử lý (Edge Cases):
- Nhập số lượng dịch vụ `<= 0` hoặc chữ thập phân -> chặn lưu và hiển thị lỗi inline ngay dưới ô số lượng.
- Không cho gọi dịch vụ vào phòng `Trống`, `Đã đặt` (chưa check-in) hoặc `Bảo trì`.
- Phòng đang có khách lưu trú khác chưa trả (`Rooms.Status == "Đang ở"`): chặn Check-in đè và thông báo rõ phòng chưa hoàn tất trả phòng.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động (`TDD` seam): `dotnet test` kiểm chứng máy trạng thái `BookingStatus` / `RoomStatus` và phép cộng bảng kê dịch vụ (thêm, xóa dòng, snapshot `UnitPrice`).
- Trực quan: Check-in một booking `Đã đặt` -> trạng thái đổi sang `Đang ở`; mở `AddServiceDialog` thêm 2 Nước suối Lavie (`10.000 đ`) + 1 Ăn sáng buffet (`50.000 đ`) -> tổng tiền dịch vụ hiển thị đúng `70.000 đ`.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-08.md` lên GitHub Issue #80.*
