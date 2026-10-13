# [TASK-01] Khởi tạo Solution WPF .NET 8 & Kiến trúc MVVM Toolkit

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-01`  
**GitHub Issue**: [#58](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/58)  
**Thành viên đảm nhiệm**: **Member A (@phantatdung233)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/core-mvvm-setup`  
**Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)  
**Blocked by**: None (Có thể bắt đầu ngay)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Khởi tạo Solution .NET 8 với project chính `HotelManagement.Wpf` và project kiểm thử `HotelManagement.Tests`.
- Cài đặt package `CommunityToolkit.Mvvm` (bản 8.x) và `Microsoft.Extensions.DependencyInjection`.
- Tổ chức cấu trúc thư mục chuẩn:
  - `Models/`: Chứa các Domain Entities & DTOs.
  - `ViewModels/`: Kế thừa `ObservableObject` hoặc `ViewModelBase`.
  - `Views/`: Chứa các UserControl và Window.
  - `Services/`: Chứa Interface và Implementation cho các dịch vụ (Navigation, Data, Auth).
  - `Resources/`: Chứa XAML Resource Dictionaries (Colors, Styles, Templates).
- Thiết lập Dependency Injection (DI) trong `App.xaml.cs` (sử dụng `IHost` hoặc `IServiceProvider`).

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- ViewModel cơ sở `ViewModelBase` kế thừa `ObservableObject` của MVVM Toolkit để hỗ trợ `[ObservableProperty]` và `[RelayCommand]`.
- Khởi tạo `App.xaml.cs` với `ConfigureServices`, đăng ký DI cho `MainWindow`, `MainViewModel`, và các dịch vụ cốt lõi.
- Đảm bảo Solution build thành công với `dotnet build`, không có cảnh báo nghiêm trọng.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Solution tạo bằng .NET 8 WPF, chạy thành công không có warning/error.
- [ ] Cài đặt package CommunityToolkit.Mvvm.
- [ ] Tổ chức thư mục chuẩn MVVM: Models/, ViewModels/, Views/, Services/, Resources/.
- [ ] ViewModel cơ sở kế thừa ObservableObject, hỗ trợ RelayCommand và ObservableCollection.
- [ ] Đã có Dependency Injection (DI) hoặc ServiceLocator khởi tạo App container cơ bản.

### Ca biên cần xử lý (Edge Cases):
- Quản lý vòng đời ViewModel đúng cách trong DI container (Transient vs Singleton) tránh rò rỉ bộ nhớ hoặc giữ trạng thái cũ khi chuyển trang.
- Đảm bảo project build được đa nền tảng (hoặc thông báo rõ ràng yêu cầu môi trường Windows desktop khi chạy WPF).

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Kiểm tra việc phân giải phụ thuộc (Dependency Injection container resolution) cho `MainViewModel` và `INavigationService`.
- Kiểm thử `RelayCommand` cơ bản của ViewModel phản hồi đúng khi được kích hoạt.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-01.md` lên GitHub Issue #58.*
