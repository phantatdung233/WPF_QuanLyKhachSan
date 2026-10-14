# [TASK-01] Khởi tạo Solution WPF .NET 8 & Kiến trúc MVVM Toolkit

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-01`  
**GitHub Issue**: [#58](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/58)  
**Thành viên đảm nhiệm**: **Member A (@phantatdung233)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/core-mvvm-setup`  
**Milestone**: Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (Tuần 1-3)  
**Blocked by**: None (Bắt đầu ngay)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Khởi tạo Solution .NET 8 WPF mang tên `HotelManagement`.
- Cài đặt package duy nhất: `CommunityToolkit.Mvvm` (phiên bản 8.x) qua NuGet.
- Tổ chức cấu trúc thư mục rõ ràng theo mô hình MVVM sinh viên:
  - `Models/`: Chứa các lớp thực thể (`User.cs`, `Room.cs`, `Customer.cs`...).
  - `DAO/`: Chứa các lớp Data Access Object và `DataProvider.cs` kết nối ADO.NET.
  - `ViewModels/`: Kế thừa `ObservableObject` hoặc `ViewModelBase`.
  - `Views/`: Chứa `MainWindow.xaml`, `LoginWindow.xaml` và các `UserControl` chức năng.
  - `Resources/`: Chứa `Colors.xaml`, `Styles.xaml` dùng chung cho toàn bộ ứng dụng.
- Xây dựng `ViewModelBase` kế thừa `ObservableObject` để hỗ trợ sinh tự động thuộc tính `[ObservableProperty]` và lệnh `[RelayCommand]`.
- Thiết lập cơ chế điều hướng giao diện chính trong `MainViewModel`: Thuộc tính `CurrentView` kiểu `object` bind vào `ContentControl` trong `MainWindow.xaml`.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Cấu hình `MainWindow.xaml`:
  - Bên trái: Menu Sidebar chứa các nút điều hướng (Sơ đồ phòng, Đặt phòng, Phòng, Khách hàng, Dịch vụ, Hóa đơn, Báo cáo).
  - Phía trên: Header hiển thị tiêu đề, tên nhân viên đăng nhập và nút Đăng xuất.
  - Trung tâm: `<ContentControl Content="{Binding CurrentView}" />`.
- Khi người dùng click nút menu, `RelayCommand` trong `MainViewModel` sẽ khởi tạo ViewModel tương ứng và gán vào `CurrentView`.
- Cấu hình `DataTemplate` trong `App.xaml` hoặc `MainWindow.Resources` ánh xạ từ ViewModel sang View tương ứng (ví dụ: `<DataTemplate DataType="{x:Type vm:RoomViewModel}"> <views:RoomView /> </DataTemplate>`).

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Solution tạo bằng .NET 8 WPF, chạy thành công không có lỗi/cảnh báo.
- [ ] Cài đặt package CommunityToolkit.Mvvm (8.x).
- [ ] Tổ chức cấu trúc thư mục chuẩn: Models/, Views/, ViewModels/, DAO/, Resources/.
- [ ] ViewModelBase kế thừa ObservableObject từ MVVM Toolkit hỗ trợ [ObservableProperty] và [RelayCommand].
- [ ] MainWindow cấu hình cơ bản gồm Sidebar menu và ContentControl hiển thị View động theo thuộc tính CurrentView trong MainViewModel.

### Ca biên cần xử lý (Edge Cases):
- Đảm bảo khi khởi chạy ứng dụng lần đầu, `CurrentView` có giá trị mặc định (ví dụ trỏ vào `RoomMapViewModel` hoặc hiển thị trang chủ chào mừng).
- Giao diện MainWindow có kích thước tối thiểu (`MinWidth`, `MinHeight`) để tránh bị vỡ layout khi co kéo cửa sổ.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Kiểm tra chạy F5 ứng dụng hiển thị MainWindow với Sidebar và thanh Header.
- Bấm thử các nút trên Sidebar để xác nhận `ContentControl` chuyển đổi View trơn tru.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-01.md` lên GitHub Issue #58.*
