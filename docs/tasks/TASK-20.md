# [TASK-20] Tích hợp kiểm thử hoàn chỉnh chuỗi nghiệp vụ E2E

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-20`  
**GitHub Issue**: [#72](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/72)  
**Thành viên đảm nhiệm**: **Member A (@phantatdung233)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/e2e-integration-testing`  
**Milestone**: Mốc 2: Ghép Luồng E2E & Nâng Cao (Tuần 4-5)  
**Blocked by**: #69 (TASK-16), #70 (TASK-17), #71 (TASK-18)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Viết kịch bản kiểm thử tích hợp tự động và kiểm thử thủ công chuỗi hành trình nghiệp vụ khép kín:
  1. Đăng nhập tài khoản Nhân viên.
  2. Xem Sơ đồ phòng, chọn phòng trống tạo Đơn đặt phòng cho khách.
  3. Thực hiện Check-in nhận phòng (phòng chuyển sang màu đỏ Occupied).
  4. Thêm 2 dịch vụ phát sinh (nước uống, giặt ủi) vào phòng đang ở.
  5. Thực hiện Check-out, kiểm tra hóa đơn tính đúng tiền phòng và tiền dịch vụ, xác nhận thanh toán.
  6. Kiểm tra Sơ đồ phòng trả về trạng thái dọn dẹp/trống.
  7. Vào màn hình Thống kê kiểm tra doanh thu được cộng thêm đúng số tiền vừa thanh toán.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Ghi nhận lỗi phát sinh giữa các mắt xích và phối hợp xử lý dứt điểm.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Test thông suốt toàn bộ kịch bản nghiệp vụ khép kín.
- [ ] Không phát sinh lỗi null pointer hoặc crash đột ngột trong suốt chuỗi hành vi.

### Ca biên cần xử lý (Edge Cases):
- Đảm bảo không có ngoại lệ NullReferenceException hoặc rò rỉ dữ liệu giữa các bước.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Kịch bản Integration Test chạy từ đầu đến cuối thành công 100%.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-20.md` lên GitHub Issue #72.*
