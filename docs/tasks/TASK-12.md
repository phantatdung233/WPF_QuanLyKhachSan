# [TASK-12] Module Check-in / Check-out & Vòng đời trạng thái phòng

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-12`  
**GitHub Issue**: [#65](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/65)  
**Thành viên đảm nhiệm**: **Member B (@xuanduong29)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/checkin-checkout`  
**Milestone**: Mốc 2: Ghép Luồng E2E & Nâng Cao (Tuần 4-5)  
**Blocked by**: #64 (TASK-11)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Màn hình quản lý các đơn đặt phòng và thao tác chuyển trạng thái lưu trú.
- Nút thao tác **Check-in**:
  - Chuyển trạng thái `Booking` sang `Occupied`.
  - Cập nhật trạng thái `Room` sang `Occupied`.
- Nút thao tác **Check-out**:
  - Kích hoạt chuyển sang màn hình xuất hóa đơn thanh toán (`InvoiceView`).
  - Sau khi thanh toán xong, chuyển `Booking` sang `Completed`.
  - Chuyển `Room` sang `Cleaning` hoặc `Available`.
- Nút **Hủy đặt phòng (Cancel)**:
  - Chuyển `Booking` sang `Cancelled`.
  - Hoàn trả trạng thái `Room` về `Available`.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Đảm bảo tính nhất quán dữ liệu (Transaction): Cả Booking và Room đều phải được cập nhật cùng lúc trong CSDL.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Nút thao tác Check-in: Chuyển trạng thái Booking sang Occupied, cập nhật trạng thái Room sang Đang có khách.
- [ ] Nút thao tác Check-out: Tính toán tổng chi phí, kích hoạt chuyển sang tạo Invoice, chuyển trạng thái Booking sang Completed và Room sang Đang dọn dẹp/Trống.
- [ ] Xử lý hủy đặt phòng (Cancel) hoàn trả trạng thái phòng về Trống.

### Ca biên cần xử lý (Edge Cases):
- Không cho phép Check-in nếu phòng đang có khách ở chưa check-out.
- Không cho phép Check-out nếu phòng chưa từng check-in.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Test luồng Check-in cập nhật đồng thời Booking và Room sang Occupied.
- Test luồng Cancel trả phòng về Available.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-12.md` lên GitHub Issue #65.*
