# [TASK-16] Module Hóa đơn & Thanh toán (Invoice)

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-16`  
**GitHub Issue**: [#69](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/69)  
**Thành viên đảm nhiệm**: **Member C (@ngocthuyen1)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/invoice-billing`  
**Milestone**: Mốc 2: Ghép Luồng E2E & Nâng Cao (Tuần 4-5)  
**Blocked by**: #65 (TASK-12), #66 (TASK-13)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng `InvoiceView` và `InvoiceViewModel`.
- Chi tiết hóa đơn thanh toán:
  - Thông tin khách hàng, số phòng, ngày đến, ngày đi thực tế.
  - Tiền phòng: `Math.Max(1, (CheckOut.Date - CheckIn.Date).Days) * UnitPrice`.
  - Tiền dịch vụ: Chi tiết từng món từ `BookingService` (Tên, Số lượng, Đơn giá, Thành tiền).
  - Tổng cộng tiền thanh toán.
- Lựa chọn phương thức thanh toán: Tiền mặt hoặc Chuyển khoản ngân hàng.
- Nút xác nhận thanh toán -> Lưu trạng thái `InvoiceStatus.Paid`, cập nhật ngày thanh toán và hoàn tất chu trình thuê phòng.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Định dạng tiền tệ VNĐ chuẩn (`#,##0 đ`).
- Có nút In hóa đơn hoặc Xem trước phiếu thanh toán (Print Preview / Mock PDF).

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] InvoiceView hiển thị chi tiết hóa đơn: Tiền phòng (Số ngày ở × Đơn giá phòng) + Tiền dịch vụ (Tổng chi tiết từ BookingService).
- [ ] Ghi nhận phương thức thanh toán (Tiền mặt / Chuyển khoản) và thời điểm thanh toán.
- [ ] Cập nhật trạng thái hóa đơn sang Paid và lưu bảng Invoice.
- [ ] Định dạng hiển thị tiền tệ chuẩn VNĐ (#,##0 đ).

### Ca biên cần xử lý (Edge Cases):
- Khách check-in và check-out cùng ngày: Đảm bảo công thức tính tối thiểu 1 ngày tiền phòng.
- Phòng không sử dụng dịch vụ: Tiền dịch vụ hiển thị 0 đ, không bị lỗi null pointer.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Test tính toán tổng tiền hóa đơn với phòng không có dịch vụ.
- Test tính toán tổng tiền với phòng có nhiều dịch vụ phát sinh.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-16.md` lên GitHub Issue #69.*
