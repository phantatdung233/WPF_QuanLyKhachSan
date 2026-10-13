# [TASK-03] Xây dựng Navigation Service & MainWindow Shell

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-03`  
**GitHub Issue**: [#60](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/60)  
**Thành viên đảm nhiệm**: **Member A (@phantatdung233)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/navigation-service`  
**Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)  
**Blocked by**: #58 (TASK-01)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Thiết kế interface `INavigationService` với các phương thức `NavigateTo<TViewModel>()`, `NavigateTo<TViewModel>(object parameter)`, và sự kiện thông báo thay đổi View hiện tại.
- Xây dựng implementation `NavigationService` kết nối với `IServiceProvider` để phân giải ViewModel động.
- Cấu hình cơ chế View-to-ViewModel mapping thông qua WPF `DataTemplate` trong XAML Resources.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Trong `MainWindow.xaml`, sử dụng `ContentControl Content="{Binding CurrentViewModel}"` để hiển thị View tương ứng.
- Đảm bảo `NavigationService` thông báo thuộc tính `CurrentView` cho `MainViewModel`.
- Hỗ trợ truyền tham số (parameters) khi điều hướng (ví dụ: chuyển từ Sơ đồ phòng sang Đặt phòng với RoomId đã chọn).

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Xây dựng interface INavigationService và implement chuyển trang mượt mà giữa các ViewModel.
- [ ] MainWindow chứa khung điều hướng chính (ContentControl hoặc Frame) hiển thị view động theo ViewModel hiện tại.
- [ ] Chuyển đổi qua lại giữa các view không gây rò rỉ bộ nhớ hoặc duplicate ViewModel instances.

### Ca biên cần xử lý (Edge Cases):
- Chuyển trang liên tục không làm tăng tiêu thụ bộ nhớ (tránh tạo mới ViewModel không cần thiết hoặc đăng ký event lặp mà không giải phóng).
- Xử lý khi ViewModel yêu cầu điều hướng nhưng chưa được đăng ký trong DI container.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Unit test `NavigationService` điều hướng thành công từ ViewModel A sang ViewModel B.
- Kiểm tra việc truyền và nhận tham số điều hướng chính xác.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-03.md` lên GitHub Issue #60.*
