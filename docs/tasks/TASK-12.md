# [TASK-12] Tổng hợp kiểm thử E2E, Soạn Slide & Kịch bản Rehearsal Demo

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-12`  
**GitHub Issue**: [#65](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/65)  
**Thành viên đảm nhiệm**: **Member C (@ngocthuyen1)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/demo-rehearsal-slides`  
**Milestone**: Mốc 3: Báo Cáo Thống Kê & Chốt Demo (Tuần 5-6)  
**Blocked by**: #81 (TASK-09), #63 (TASK-10), #64 (TASK-11)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Chuẩn bị toàn diện cho buổi báo cáo và chấm điểm đồ án môn học IT008 trước hội đồng giảng viên.
- Kiểm thử tích hợp thông suốt (End-to-End) toàn bộ chu trình nghiệp vụ khách sạn trên một máy tính sạch.
- Đóng gói file script CSDL `Database/HotelDB.sql` để có thể reset lại dữ liệu demo chuẩn chỉ trong 30 giây bằng SSMS nếu có sự cố.
- Soạn thảo Slide báo cáo thuyết trình (PowerPoint/Canva) gồm các nội dung cốt lõi: Đặt vấn đề, Công nghệ sử dụng, Kiến trúc CSDL & WPF MVVM, Demo tính năng và Điểm nhấn nâng cao.
- Soạn kịch bản Demo chi tiết từng phút cho cả 3 thành viên và tổ chức tổng duyệt (Rehearsal) ít nhất 2 lần.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Kịch bản chuỗi nghiệp vụ E2E cần kiểm tra:
  1. Đăng nhập tài khoản Nhân viên (`staff`): Kiểm tra quyền hạn, thấy bị ẩn menu Quản trị.
  2. Đăng xuất, đăng nhập tài khoản Quản trị (`admin`): Kiểm tra quyền hạn đầy đủ.
  3. Mở **Sơ đồ phòng trực quan**: Thấy 12 phòng hiển thị 4 màu trực quan.
  4. Chọn 1 phòng màu Xanh (Trống) -> Tạo phiếu **Đặt phòng** cho khách mới. Sơ đồ phòng đổi sang màu Vàng (Đã đặt).
  5. Khách đến -> Bấm **Check-in**: Sơ đồ phòng đổi sang màu Đỏ (Đang ở).
  6. Khách gọi đồ -> Bấm **Thêm dịch vụ phòng** (2 lon nước, 1 giặt ủi).
  7. Khách trả phòng -> Bấm **Check-out**: Kiểm tra màn hình Hóa đơn tính chuẩn tiền phòng + tiền dịch vụ.
  8. Bấm **Thanh toán & Xuất hóa đơn**: Xuất file `.txt` cho giảng viên xem. Sơ đồ phòng tự động quay về màu Xanh (Trống).
  9. Mở màn hình **Báo cáo & Thống kê**: Xác nhận số tiền vừa thanh toán đã được cập nhật vào thẻ KPI doanh thu.
- Slide thuyết trình:
  - Slide 1: Tên đề tài, Giảng viên hướng dẫn, Danh sách 3 thành viên và vai trò.
  - Slide 2: Mục tiêu đồ án & Bài toán nghiệp vụ khách sạn.
  - Slide 3: Kiến trúc kỹ thuật (.NET 8 WPF, CommunityToolkit.Mvvm, ADO.NET SQL Server).
  - Slide 4: Sơ đồ thực thể CSDL (ERD 8 bảng).
  - Slide 5: Hai điểm nhấn nổi bật (Sơ đồ phòng DataTrigger & Xuất hóa đơn ra file).
  - Slide 6: Kết luận & Hướng phát triển tương lai.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Kiểm thử thông suốt chuỗi kịch bản Demo: Đăng nhập -> Sơ đồ phòng -> Đặt phòng -> Check-in -> Thêm 2 dịch vụ -> Check-out xuất hóa đơn -> Xem thống kê doanh thu.
- [ ] Đảm bảo file HotelDB.sql chạy nạp lại dữ liệu ban đầu chỉ với 1 click trong SSMS nếu cần reset trước buổi demo.
- [ ] Hoàn thành bộ Slide thuyết trình giới thiệu đề tài, mô hình kiến trúc WPF MVVM, CSDL và các tính năng nổi bật.
- [ ] Soạn kịch bản phân vai nói chi tiết cho 3 thành viên và tổ chức rehearsal tối thiểu 2 lần trước ngày báo cáo.

### Ca biên cần xử lý (Edge Cases):
- Phòng ngừa sự cố quên mật khẩu demo hoặc máy chiếu trường bị đổi độ phân giải màn hình -> Thiết lập sẵn độ phân giải tối thiểu 1280x720 và ghi nhớ tài khoản mặc định `admin`/`123456`.
- Chuẩn bị sẵn 1 bản video quay màn hình thao tác đề phòng máy tính gặp trục trặc khi demo trực tiếp.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Chạy thử nghiệm kịch bản demo 2 lần liên tiếp không vấp, thời gian trình bày chuẩn 7 - 8 phút.
- Mỗi thành viên tự tin trình bày phần việc của mình và trả lời được các câu hỏi phản biện của giảng viên.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-12.md` lên GitHub Issue #65.*
