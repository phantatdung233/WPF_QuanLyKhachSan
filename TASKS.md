# IT008 — Scrum Task Backlog: Quản Lý Khách Sạn (WPF, .NET 8)

Tài liệu Task Backlog đóng vai trò **Single Source of Truth** cho đồ án IT008. Toàn bộ danh sách Task dưới đây được tự động đồng bộ hóa lên GitHub Issues và GitHub Projects thông qua workflow `.github/workflows/create-issues.yml`.

---

## 1. Danh Mục Milestones & Quy Ước Nhóm

### 1.1. Lộ trình Milestones
- **Mốc 1: Module Độc Lập (Tuần 1-3)**: Hoàn thành khung MVVM, cấu trúc DB 8 bảng và 7 module CRUD cơ bản độc lập + data binding.
- **Mốc 2: Ghép Luồng E2E & Nâng Cao (Tuần 4-5)**: Ghép thông suốt luồng nghiệp vụ khép kín (Đăng nhập → Đặt phòng → Check-in/out → Hóa đơn → Thống kê) và 3 tính năng nâng cao (Sơ đồ phòng DataTrigger, LiveCharts2, Async stats).
- **Mốc 3: Chốt Demo & Feature Freeze (Tuần 6)**: Đóng băng tính năng 2 ngày trước hạn demo, refactor code, sửa lỗi và tập dượt thuyết trình 2 lần.

### 1.2. Phân công 3 thành viên ("Lát cắt dọc")
- **Member A (Trưởng nhóm)**: Khung MVVM nền tảng, HotelDbContext, Navigation, Module Đăng nhập/Phân quyền (User), Module Phòng & Loại phòng (Room, RoomType), Quản lý nhánh main/develop.
- **Member B (Nghiệp vụ)**: Module Khách hàng (Customer), Module Đặt phòng (Booking), Module Check-in/Check-out & Dịch vụ phát sinh (BookingService), Seed Data mẫu.
- **Member C (Giao diện & Báo cáo)**: Shared UI Resources (Colors, Styles, Templates), Layout MainWindow, Module Hóa đơn (Invoice), Module Thống kê (Stats), Tính năng nâng cao, Slide & Kịch bản demo.

### 1.3. Quy tắc Git Flow & Review vòng tròn
- Nhánh bảo vệ: `main` (Production/Demo), `develop` (Tích hợp trung gian).
- Nhánh tính năng: `feature/<task-name>` rẽ nhánh từ `develop`.
- **Review vòng tròn bắt buộc**:
  - Member A là tác giả PR ➔ **Member B** duyệt (Reviewer).
  - Member B là tác giả PR ➔ **Member C** duyệt (Reviewer).
  - Member C là tác giả PR ➔ **Member A** duyệt (Reviewer).
- Chỉ merge vào `develop` khi: Có ít nhất 1 approval, vượt qua Definition of Done và không có merge conflict.

---

## 2. Chi Tiết Danh Sách Tasks

### [TASK-01] Khởi tạo Solution WPF .NET 8 & Kiến trúc MVVM Toolkit
- **Assignee**: Member A
- **Priority**: High
- **Labels**: `core`, `architecture`, `m1`
- **Branch**: `feature/core-mvvm-setup`
- **Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)
- **Definition of Done**:
  - [ ] Solution tạo bằng .NET 8 WPF, chạy thành công không có warning/error.
  - [ ] Cài đặt package `CommunityToolkit.Mvvm`.
  - [ ] Tổ chức thư mục chuẩn MVVM: `Models/`, `ViewModels/`, `Views/`, `Services/`, `Resources/`.
  - [ ] ViewModel cơ sở kế thừa `ObservableObject`, hỗ trợ `RelayCommand` và `ObservableCollection`.
  - [ ] Đã có Dependency Injection (DI) hoặc ServiceLocator khởi tạo App container cơ bản.

---

### [TASK-02] Thiết kế HotelDbContext Code First 8 bảng & SQLite Migration
- **Assignee**: Member A
- **Priority**: High
- **Labels**: `core`, `database`, `m1`
- **Branch**: `feature/database-efcore-setup`
- **Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)
- **Definition of Done**:
  - [ ] Cài đặt `Microsoft.EntityFrameworkCore.Sqlite` và `Microsoft.EntityFrameworkCore.Design`.
  - [ ] Định nghĩa đầy đủ 8 Entity: `User`, `RoomType`, `Room`, `Customer`, `Booking`, `Service`, `BookingService`, `Invoice`.
  - [ ] Cấu hình Fluent API / Data Annotations đúng mối quan hệ khóa ngoại (1-N, N-N).
  - [ ] Khởi tạo Initial Migration thành công và kiểm tra sinh file CSDL SQLite cục bộ tự động khi khởi chạy app.

---

### [TASK-03] Xây dựng Navigation Service & MainWindow Shell
- **Assignee**: Member A
- **Priority**: High
- **Labels**: `core`, `ui`, `navigation`, `m1`
- **Branch**: `feature/navigation-service`
- **Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)
- **Definition of Done**:
  - [ ] Xây dựng interface `INavigationService` và implement chuyển trang mượt mà giữa các ViewModel.
  - [ ] MainWindow chứa khung điều hướng chính (ContentControl hoặc Frame) hiển thị view động theo ViewModel hiện tại.
  - [ ] Chuyển đổi qua lại giữa các view không gây rò rỉ bộ nhớ hoặc duplicate ViewModel instances.

---

### [TASK-04] Module Đăng nhập & Phân quyền theo vai trò User
- **Assignee**: Member A
- **Priority**: High
- **Labels**: `auth`, `security`, `m1`
- **Branch**: `feature/auth-login`
- **Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)
- **Definition of Done**:
  - [ ] Màn hình `LoginView` với binding mật khẩu bảo mật (PasswordBox binding helper/SecurePassword).
  - [ ] `AuthService` xác thực tài khoản từ bảng `User`, mật khẩu có băm mã hóa an toàn.
  - [ ] Lưu trạng thái phiên làm việc (Current User Session).
  - [ ] Ẩn/Hiện các chức năng theo vai trò của người dùng (Admin vs Nhân viên).

---

### [TASK-05] Module Quản lý Loại phòng (RoomType) CRUD & Binding
- **Assignee**: Member A
- **Priority**: Medium
- **Labels**: `room`, `crud`, `m1`
- **Branch**: `feature/roomtype-management`
- **Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)
- **Definition of Done**:
  - [ ] `RoomTypeView` và `RoomTypeViewModel` hỗ trợ xem danh sách, thêm mới, sửa, xóa loại phòng.
  - [ ] Validate dữ liệu đầu vào: Tên loại phòng không trùng, đơn giá cơ bản > 0.
  - [ ] Tự động cập nhật bảng danh sách ngay lập tức khi thêm/sửa/xóa qua `ObservableCollection`.
  - [ ] Ràng buộc kiểm tra không cho xóa loại phòng nếu đã có phòng đang tham chiếu.

---

### [TASK-06] Module Quản lý Phòng (Room) CRUD & Liên kết Loại phòng
- **Assignee**: Member A
- **Priority**: Medium
- **Labels**: `room`, `crud`, `m1`
- **Branch**: `feature/room-management`
- **Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)
- **Definition of Done**:
  - [ ] `RoomView` hiển thị danh sách phòng kèm ComboBox chọn `RoomType`.
  - [ ] Thực hiện CRUD phòng (Mã phòng, Tên phòng, Trạng thái: Trống/Đã đặt/Đang ở/Bảo trì, Ghi chú).
  - [ ] Tìm kiếm và lọc danh sách phòng theo trạng thái hoặc loại phòng.
  - [ ] Kiểm tra trùng mã phòng và lưu thay đổi vào SQLite thành công.

---

### [TASK-07] Quản trị Git Flow, Branch Protection & CI Workflow
- **Assignee**: Member A
- **Priority**: High
- **Labels**: `devops`, `git`, `m1`
- **Branch**: `feature/ci-workflow-governance`
- **Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)
- **Definition of Done**:
  - [ ] Thiết lập nhánh `develop` từ `main`, cấu hình Pull Request Template.
  - [ ] Tích hợp workflow GitHub Actions tự động tạo Milestone, Label và Issue từ `TASKS.md`.
  - [ ] Thực thi quy tắc review vòng tròn cho các PR trong suốt các Sprint.

---

### [TASK-08] Module Quản lý Khách hàng (Customer) CRUD & Tìm kiếm
- **Assignee**: Member B
- **Priority**: High
- **Labels**: `customer`, `crud`, `m1`
- **Branch**: `feature/customer-management`
- **Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)
- **Definition of Done**:
  - [ ] `CustomerView` và `CustomerViewModel` hỗ trợ CRUD thông tin khách hàng.
  - [ ] Trường dữ liệu: Họ tên, CMND/CCCD, Số điện thoại, Email, Địa chỉ.
  - [ ] Validate định dạng CCCD/CMND (9 hoặc 12 số) và số điện thoại hợp lệ.
  - [ ] Thanh tìm kiếm nhanh khách hàng theo Họ tên hoặc CCCD/SĐT dạng realtime binding.

---

### [TASK-09] Module Quản lý Dịch vụ (Service) CRUD
- **Assignee**: Member B
- **Priority**: Medium
- **Labels**: `service`, `crud`, `m1`
- **Branch**: `feature/service-management`
- **Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)
- **Definition of Done**:
  - [ ] `ServiceView` hiển thị danh mục các dịch vụ khách sạn (giặt ủi, nước uống, ăn sáng, đưa đón...).
  - [ ] Hỗ trợ thêm mới, sửa đổi tên dịch vụ, đơn giá và trạng thái còn phục vụ hay không.
  - [ ] Validate đơn giá dịch vụ >= 0.

---

### [TASK-10] Xây dựng bộ Seed Data mẫu chuẩn demo (10 phòng, 10 khách, booking mẫu)
- **Assignee**: Member B
- **Priority**: Medium
- **Labels**: `database`, `seed-data`, `m1`
- **Branch**: `feature/seed-data`
- **Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)
- **Definition of Done**:
  - [ ] Viết lớp `DbInitializer` hoặc EF Core Seed Data nạp dữ liệu ban đầu nếu DB trống.
  - [ ] Cung cấp tối thiểu: 2 Users (1 Admin, 1 Nhân viên), 3 Loại phòng, 10 Phòng mẫu trải đều các tầng.
  - [ ] Cung cấp 10 Khách hàng mẫu có thông tin thực tế, 5 Dịch vụ khách sạn.
  - [ ] Cung cấp sẵn các Booking mẫu đại diện đủ các trạng thái để phục vụ demo.

---

### [TASK-11] Module Đặt phòng (Booking) & Thuật toán kiểm tra trùng lịch
- **Assignee**: Member B
- **Priority**: High
- **Labels**: `booking`, `business-logic`, `m1`
- **Branch**: `feature/booking-conflict-check`
- **Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)
- **Definition of Done**:
  - [ ] Màn hình tạo phiếu đặt phòng chọn Khách hàng, Phòng, Ngày nhận (Check-in), Ngày trả (Check-out).
  - [ ] Cài đặt thuật toán kiểm tra xung đột thời gian: Hai khoảng thời gian [A, B] và [C, D] trùng nhau khi và chỉ khi **A < D và C < B**.
  - [ ] Hiển thị thông báo lỗi rõ ràng nếu phòng đã bị đặt trong khoảng thời gian lựa chọn.
  - [ ] Lưu thông tin `Booking` với trạng thái ban đầu `Reserved`/`Booked`.

---

### [TASK-12] Module Check-in / Check-out & Vòng đời trạng thái phòng
- **Assignee**: Member B
- **Priority**: High
- **Labels**: `booking`, `workflow`, `m2`
- **Branch**: `feature/checkin-checkout`
- **Milestone**: Mốc 2: Ghép Luồng E2E & Nâng Cao (Tuần 4-5)
- **Definition of Done**:
  - [ ] Nút thao tác Check-in: Chuyển trạng thái Booking sang `Occupied`, cập nhật trạng thái Room sang `Đang có khách`.
  - [ ] Nút thao tác Check-out: Tính toán tổng chi phí, kích hoạt chuyển sang tạo `Invoice`, chuyển trạng thái Booking sang `Completed` và Room sang `Đang dọn dẹp`/`Trống`.
  - [ ] Xử lý hủy đặt phòng (Cancel) hoàn trả trạng thái phòng về `Trống`.

---

### [TASK-13] Module Dịch vụ phòng (BookingService) gắn dịch vụ phát sinh
- **Assignee**: Member B
- **Priority**: Medium
- **Labels**: `service`, `booking`, `m2`
- **Branch**: `feature/booking-service-usage`
- **Milestone**: Mốc 2: Ghép Luồng E2E & Nâng Cao (Tuần 4-5)
- **Definition of Done**:
  - [ ] Giao diện cho phép chọn một phòng đang thuê và thêm dịch vụ phát sinh vào phiếu đặt phòng đó.
  - [ ] Chọn dịch vụ từ danh mục `Service`, nhập số lượng, tự động nhân thành tiền.
  - [ ] Hỗ trợ xem danh sách các dịch vụ đã sử dụng của phòng và xóa/chỉnh sửa số lượng.
  - [ ] Cập nhật bảng trung gian `BookingService` trong CSDL.

---

### [TASK-14] Bộ tài nguyên Shared Resources đồng bộ giao diện
- **Assignee**: Member C
- **Priority**: High
- **Labels**: `ui`, `theme`, `m1`
- **Branch**: `feature/ui-shared-resources`
- **Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)
- **Definition of Done**:
  - [ ] `Colors.xaml`: Định nghĩa bảng màu chủ đạo hiện đại (Primary, Secondary, Background, Text, Status colors).
  - [ ] `Styles.xaml`: Tùy biến Style đồng bộ cho `Button`, `TextBox`, `ComboBox`, `DataGrid`, `DatePicker`.
  - [ ] `Templates.xaml`: Định nghĩa ControlTemplates và Card template chuẩn cho ứng dụng.
  - [ ] Tích hợp toàn bộ ResourceDictionary vào `App.xaml` dùng chung toàn hệ thống.

---

### [TASK-15] Thiết kế Layout tổng thể MainWindow & Header/Sidebar
- **Assignee**: Member C
- **Priority**: High
- **Labels**: `ui`, `layout`, `m1`
- **Branch**: `feature/ui-mainwindow-layout`
- **Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)
- **Definition of Done**:
  - [ ] Layout MainWindow chuẩn tỉ lệ: Sidebar bên trái (menu điều hướng), Header trên cùng (thông tin user, nút đăng xuất), Content chính ở giữa.
  - [ ] Menu sidebar có icon trực quan, hiệu ứng hover/active rõ ràng.
  - [ ] Đảm bảo responsive cơ bản khi phóng to/thu nhỏ kích thước cửa sổ.

---

### [TASK-16] Module Hóa đơn & Thanh toán (Invoice)
- **Assignee**: Member C
- **Priority**: High
- **Labels**: `invoice`, `billing`, `m2`
- **Branch**: `feature/invoice-billing`
- **Milestone**: Mốc 2: Ghép Luồng E2E & Nâng Cao (Tuần 4-5)
- **Definition of Done**:
  - [ ] `InvoiceView` hiển thị chi tiết hóa đơn: Tiền phòng (Số ngày ở × Đơn giá phòng) + Tiền dịch vụ (Tổng chi tiết từ `BookingService`).
  - [ ] Ghi nhận phương thức thanh toán (Tiền mặt / Chuyển khoản) và thời điểm thanh toán.
  - [ ] Cập nhật trạng thái hóa đơn sang `Paid` và lưu bảng `Invoice`.
  - [ ] Định dạng hiển thị tiền tệ chuẩn VNĐ (`#,##0 đ`).

---

### [TASK-17] [Nâng Cao 1] Sơ đồ phòng trực quan sử dụng DataTrigger WPF
- **Assignee**: Member C
- **Priority**: High
- **Labels**: `advanced`, `ui`, `room-map`, `m2`
- **Branch**: `feature/advanced-room-matrix-trigger`
- **Milestone**: Mốc 2: Ghép Luồng E2E & Nâng Cao (Tuần 4-5)
- **Definition of Done**:
  - [ ] Màn hình Sơ đồ phòng hiển thị dạng lưới các thẻ phòng (WrapPanel / UniformGrid).
  - [ ] Ứng dụng `DataTrigger` trong XAML để đổi màu sắc thẻ phòng tự động theo trạng thái:
    - Xanh lá: Phòng trống (Available)
    - Vàng/Cam: Đã đặt trước (Reserved)
    - Đỏ: Đang có khách ở (Occupied)
    - Xám: Đang dọn dẹp / Bảo trì (Maintenance)
  - [ ] Click trực tiếp vào phòng trên sơ đồ để mở nhanh thông tin chi tiết hoặc tạo đơn đặt phòng.

---

### [TASK-18] Module Thống kê & Báo cáo với biểu đồ LiveCharts2
- **Assignee**: Member C
- **Priority**: High
- **Labels**: `stats`, `chart`, `m2`
- **Branch**: `feature/stats-livecharts`
- **Milestone**: Mốc 2: Ghép Luồng E2E & Nâng Cao (Tuần 4-5)
- **Definition of Done**:
  - [ ] Cài đặt thư viện `LiveChartsCore.SkiaSharpView.WPF`.
  - [ ] Hiển thị biểu đồ cột/đường doanh thu theo ngày/tháng.
  - [ ] Hiển thị biểu đồ tròn (Pie chart) tỷ lệ công suất phòng hoặc tỷ trọng doanh thu dịch vụ.
  - [ ] Bộ lọc thống kê linh hoạt theo khoảng thời gian (Từ ngày - Đến ngày).

---

### [TASK-19] [Nâng Cao 2] Tối ưu truy vấn thống kê bất đồng bộ (Async Stats)
- **Assignee**: Member C
- **Priority**: Medium
- **Labels**: `advanced`, `performance`, `async`, `m2`
- **Branch**: `feature/advanced-async-stats`
- **Milestone**: Mốc 2: Ghép Luồng E2E & Nâng Cao (Tuần 4-5)
- **Definition of Done**:
  - [ ] Toàn bộ logic tổng hợp thống kê được chạy bất đồng bộ qua `async/await` và `Task.Run` hoặc EF Core `ToListAsync()`.
  - [ ] Hiển thị `ProgressBar` hoặc vòng xoay loading khi đang tải dữ liệu báo cáo lớn.
  - [ ] Đảm bảo UI hoàn toàn mượt mà, không bị hiện tượng treo (freeze) hoặc giật lag trong quá trình tính toán.

---

### [TASK-20] Tích hợp kiểm thử hoàn chỉnh chuỗi nghiệp vụ E2E
- **Assignee**: Member A
- **Priority**: High
- **Labels**: `integration`, `e2e-testing`, `m2`
- **Branch**: `feature/e2e-integration-testing`
- **Milestone**: Mốc 2: Ghép Luồng E2E & Nâng Cao (Tuần 4-5)
- **Definition of Done**:
  - [ ] Test thông suốt toàn bộ kịch bản nghiệp vụ:
    - Đăng nhập tài khoản nhân viên.
    - Tìm phòng trống trên Sơ đồ phòng và tạo đơn Đặt phòng cho khách.
    - Thực hiện Check-in nhận phòng.
    - Thêm 2 dịch vụ phát sinh vào phòng.
    - Thực hiện Check-out, xuất hóa đơn tổng tiền phòng + dịch vụ, thanh toán.
    - Kiểm tra dữ liệu doanh thu cập nhật chính xác trên màn hình Thống kê.
  - [ ] Không phát sinh lỗi null pointer hoặc crash đột ngột trong suốt chuỗi hành vi.

---

### [TASK-21] Feature Freeze, Refactoring & Chuẩn hóa Naming Convention
- **Assignee**: Member A
- **Priority**: High
- **Labels**: `refactor`, `clean-code`, `m3`
- **Branch**: `feature/refactor-code-cleanup`
- **Milestone**: Mốc 3: Chốt Demo & Feature Freeze (Tuần 6)
- **Definition of Done**:
  - [ ] Thực hiện Đóng băng tính năng (Feature Freeze) trước ngày báo cáo chính thức ít nhất 2 ngày.
  - [ ] Kiểm tra và xóa toàn bộ dead code, file tạm, console logs thừa.
  - [ ] Chuẩn hóa format code, comment và naming convention C# (PascalCase, camelCase) toàn bộ solution.
  - [ ] Đảm bảo build Clean/Rebuild không phát sinh warning cảnh báo nghiêm trọng.

---

### [TASK-22] Kiểm thử hồi quy, Sửa lỗi chặn (Blocker Bugs) & Hardening
- **Assignee**: Member B
- **Priority**: High
- **Labels**: `qa`, `bugfix`, `hardening`, `m3`
- **Branch**: `feature/regression-bugfixes`
- **Milestone**: Mốc 3: Chốt Demo & Feature Freeze (Tuần 6)
- **Definition of Done**:
  - [ ] Kiểm tra các ca biên (edge cases): Đặt phòng cùng ngày, ngày trả trước ngày đến, số lượng dịch vụ âm, nhập CCCD sai định dạng.
  - [ ] Xử lý triệt để toàn bộ exception handling (Try-Catch, hiển thị hộp thoại thông báo lỗi thân thiện).
  - [ ] Đảm bảo cơ sở dữ liệu SQLite tự khôi phục dữ liệu demo sạch chỉ với 1 thao tác nếu cần reset lại demo.

---

### [TASK-23] Chuẩn bị Slide thuyết trình, Kịch bản Demo & Tập dượt Rehearsal
- **Assignee**: Member C
- **Priority**: High
- **Labels**: `docs`, `presentation`, `demo`, `m3`
- **Branch**: `feature/demo-rehearsal-slides`
- **Milestone**: Mốc 3: Chốt Demo & Feature Freeze (Tuần 6)
- **Definition of Done**:
  - [ ] Hoàn thành Slide thuyết trình: Giới thiệu đề tài, Kiến trúc WPF MVVM, CSDL 8 bảng, Demo tính năng chính và Điểm nổi bật nâng cao.
  - [ ] Soạn thảo Kịch bản demo từng bước (Demo Script) căn thời gian chính xác cho 3 thành viên.
  - [ ] Nhóm tổ chức thành công tối thiểu 2 lần Rehearsal chạy thử toàn bộ bài thuyết trình và trả lời câu hỏi phản biện.
