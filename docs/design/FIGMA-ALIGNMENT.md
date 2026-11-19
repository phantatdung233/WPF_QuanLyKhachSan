# Đồng bộ giao diện theo Figma — 19/11/2026

Nguồn: [Hotel Management](https://www.figma.com/design/4wEK8iFt0i0RJIfmVYU0iO/Windows-UI-kit--Community-?node-id=172994-2).

Các thông số đọc qua Figma MCP thay thế mô tả shell/login cũ trong repo. IT008 vẫn là mã môn học; thương hiệu trong UI và biên lai là **AURA HOTEL**.

| Thành phần | Frame / node nguồn | Quy cách |
| --- | --- | --- |
| Admin shell | 173024:784 | 1280×800, title bar 48px, pane trái 240px |
| Staff shell | 173024:785 | 6 mục, bỏ Loại phòng và Báo cáo |
| Nội dung shell | I173024:1349;173024:1207 | (240,132), 1016×644; co chiều cao ở 720px |
| Tài khoản | I173024:1349;173053:11002 | (1024,66), 232×40; menu Đăng xuất |
| Logo shell | 173062:10483 | (20,62), 200×48; logo gốc `src/HotelManagement/Resources/Images/aura-source.png` |
| Login | 173024:783 | 960×524; cột 344/616px, title bar 48px |
| Logo login | 173141:11027 | slot 172×48, x=53, y=168; `src/HotelManagement/Resources/Images/aura-source.png` |

TASK-01 hoàn thiện shell và màn xem trước hai vai trò. Chưa có đăng nhập SQL hay dữ liệu nghiệp vụ. Các module giữ placeholder, chỉ hiển thị một tiêu đề ở shell. Màn xem trước 680×360 chỉ có hai nút mở vai trò. Form đăng nhập hai cột dành cho TASK-03. Sidebar dùng icon lịch vector cho Đặt phòng. Các trang placeholder hiển thị rõ rằng màn hình đang được phát triển.

Kiểm tra trực quan cuối phải chạy WPF trên Windows tại 1280×800 và 1280×720, kiểm tra chọn menu, mở tài khoản, đăng xuất và focus bàn phím. Build trên macOS không xác nhận giao diện render đúng.

### Logo trong suốt

Dùng PNG gốc 1324×670 có alpha từ Figma, không dùng ảnh export frame đã dính nền trắng. XAML dùng `CroppedBitmap` với `SourceRect="72,156,1130,270"`, tương ứng vùng crop của node logo; resource này dùng chung cho logo shell và màn xem trước, mỗi nơi có slot 200×48 với Stretch=Uniform.
