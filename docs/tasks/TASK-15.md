# [TASK-15] Thiết kế Layout tổng thể MainWindow & Header/Sidebar

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-15`  
**GitHub Issue**: [#68](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/68)  
**Thành viên đảm nhiệm**: **Member C (@ngocthuyen1)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/ui-mainwindow-layout`  
**Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)  
**Blocked by**: #60 (TASK-03), #67 (TASK-14)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Thiết kế layout tổng thể của `MainWindow.xaml`:
  - **Sidebar (Bên trái)**: Logo khách sạn, danh sách nút menu điều hướng (Sơ đồ phòng, Đặt phòng, Khách hàng, Dịch vụ, Hóa đơn, Thống kê, Cài đặt).
  - **Header (Trên cùng)**: Tên màn hình hiện tại, hiển thị thông tin tài khoản đang đăng nhập (Avatar/Icon, Tên, Role), nút Đăng xuất.
  - **Content Area (Trung tâm)**: Chứa `ContentControl` hiển thị View hiện tại theo ViewModel.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Menu sidebar có trạng thái Active (highlight trang đang chọn) và hover trực quan.
- Đảm bảo responsive cơ bản khi phóng to cửa sổ hoặc thay đổi kích thước.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Layout MainWindow chuẩn tỉ lệ: Sidebar bên trái (menu điều hướng), Header trên cùng (thông tin user, nút đăng xuất), Content chính ở giữa.
- [ ] Menu sidebar có icon trực quan, hiệu ứng hover/active rõ ràng.
- [ ] Đảm bảo responsive cơ bản khi phóng to/thu nhỏ kích thước cửa sổ.

### Ca biên cần xử lý (Edge Cases):
- Thu nhỏ cửa sổ không làm vỡ layout hoặc che khuất các nút menu chính.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Kiểm tra click các nút trên Sidebar kích hoạt đúng lệnh điều hướng của `NavigationService`.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-15.md` lên GitHub Issue #68.*
