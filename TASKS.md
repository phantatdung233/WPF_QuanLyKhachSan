# IT008 — Scrum Task Backlog: Quản Lý Khách Sạn (WPF, .NET 8)

Tài liệu Task Backlog đóng vai trò **Single Source of Truth** cho đồ án IT008. Toàn bộ danh sách 12 Task dưới đây được tinh gọn theo chuẩn đồ án thực hành của sinh viên năm 2, kết nối SQL Server (SSMS), ADO.NET thuần DataProvider, MVVM Toolkit không dùng DI phức tạp và 2 tính năng nâng cao ăn điểm tối đa.

---

## 1. Danh Mục Milestones & Quy Ước Nhóm

### 1.1. Lộ trình Milestones (6 Tuần)
- **Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (Tuần 1-3)**: Hoàn thành khung MVVM Toolkit, CSDL SQL Server 8 bảng, DataProvider ADO.NET, Đăng nhập phân quyền, và các danh mục cơ bản (Phòng, Loại phòng, Khách hàng, Dịch vụ).
- **Mốc 2: Nghiệp Vụ Cốt Lõi, Sơ Đồ Phòng & Hóa Đơn (Tuần 3-5)**: Luồng nghiệp vụ khép kín (Đặt phòng chống trùng lịch, Check-in/out, Gọi dịch vụ, Hóa đơn + Xuất file) và Sơ đồ phòng trực quan DataTrigger.
- **Mốc 3: Báo Cáo Thống Kê & Chốt Demo (Tuần 5-6)**: Màn hình Báo cáo & Thống kê doanh thu (KPI cards, Progress bar), đóng gói kịch bản demo, hoàn thiện slide thuyết trình và tổng duyệt rehearsal.

### 1.2. Phân công 3 thành viên (Mỗi thành viên 4 Tasks trọng tâm)
- **Member A (Trưởng nhóm: Khung nền tảng, CSDL & Quản lý Phòng)**:
  - Khởi tạo Solution WPF .NET 8, CommunityToolkit.Mvvm (TASK-01)
  - Thiết kế Database SQL Server 8 bảng & DataProvider.cs ADO.NET (TASK-02)
  - Màn hình Đăng nhập & Phân quyền Admin / Nhân viên (TASK-03)
  - Module Quản lý Phòng & Loại phòng (TASK-04)
- **Member B (Nghiệp vụ cốt lõi: Khách hàng, Dịch vụ & Đặt phòng)**:
  - Module Quản lý Khách hàng CRUD & Tìm kiếm (TASK-05)
  - Module Quản lý Dịch vụ khách sạn CRUD (TASK-06)
  - Module Đặt phòng & Thuật toán chống trùng lịch (TASK-07)
  - Module Check-in, Check-out & Thêm dịch vụ phòng (TASK-08)
- **Member C (Giao diện trực quan, Thanh toán, Báo cáo & Demo)**:
  - Module Sơ đồ phòng trực quan DataTrigger [Nâng cao 1] (TASK-09)
  - Module Hóa đơn, Thanh toán & Xuất file in ấn [Nâng cao 2] (TASK-10)
  - Module Báo cáo & Thống kê doanh thu thuần XAML (TASK-11)
  - Tổng hợp kiểm thử E2E, Slide thuyết trình & Kịch bản Rehearsal (TASK-12)

### 1.3. Quy ước Git & Phối hợp
- Nhánh bảo vệ: `main` (phiên bản ổn định dùng để nộp bài và demo).
- Nhánh tích hợp: `develop`.
- Nhánh tính năng: `feature/<task-name>` rẽ nhánh từ `develop`.
- Merge vào `develop` sau khi thành viên kiểm tra chéo chạy thử tính năng thành công trên máy tính cá nhân.

---

## 2. Chi Tiết Danh Sách Tasks

### [TASK-01] Khởi tạo Solution WPF .NET 8 & Kiến trúc MVVM Toolkit
- **Assignee**: Member A
- **Priority**: High
- **Labels**: `core`, `architecture`, `m1`
- **Branch**: `feature/core-mvvm-setup`
- **Milestone**: Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (Tuần 1-3)
- **Definition of Done**:
  - [ ] Solution tạo bằng .NET 8 WPF, chạy thành công không có lỗi/cảnh báo.
  - [ ] Cài đặt package CommunityToolkit.Mvvm (8.x).
  - [ ] Tổ chức cấu trúc thư mục chuẩn: Models/, Views/, ViewModels/, DAO/, Resources/.
  - [ ] ViewModelBase kế thừa ObservableObject từ MVVM Toolkit hỗ trợ [ObservableProperty] và [RelayCommand].
  - [ ] MainWindow cấu hình cơ bản gồm Sidebar menu và ContentControl hiển thị View động theo thuộc tính CurrentView trong MainViewModel.

---

### [TASK-02] Thiết kế CSDL SQL Server 8 bảng & Lớp dùng chung DataProvider.cs
- **Assignee**: Member A
- **Priority**: High
- **Labels**: `database`, `core`, `m1`
- **Branch**: `feature/database-sqlserver-setup`
- **Milestone**: Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (Tuần 1-3)
- **Definition of Done**:
  - [ ] Tạo file script Database/HotelDB.sql gồm lệnh CREATE DATABASE, CREATE TABLE cho 8 bảng: Users, RoomTypes, Rooms, Customers, Bookings, Services, BookingServices, Invoices.
  - [ ] Nạp sẵn dữ liệu mẫu (INSERT INTO) trong script: 2 Users (Admin, Staff), 4 loại phòng, 12 phòng, 5 khách hàng, 5 dịch vụ.
  - [ ] Chạy file script thành công trên SSMS (SQL Server Management Studio / SQL Server Express).
  - [ ] Viết lớp DataProvider.cs (ADO.NET) kết nối chuỗi .\SQLEXPRESS với 3 phương thức: ExecuteQuery, ExecuteNonQuery, ExecuteScalar có truyền tham số chống SQL Injection.
  - [ ] Viết các class Models cơ bản (User, RoomType, Room) có constructor nhận DataRow.

---

### [TASK-03] Màn hình Đăng nhập (Login) & Phân quyền Admin / Nhân viên
- **Assignee**: Member A
- **Priority**: High
- **Labels**: `auth`, `security`, `m1`
- **Branch**: `feature/auth-login`
- **Milestone**: Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (Tuần 1-3)
- **Definition of Done**:
  - [ ] Màn hình LoginWindow.xaml đẹp mắt với TextBox Username, PasswordBox và nút Đăng nhập.
  - [ ] UserDAO.cs truy vấn kiểm tra tài khoản trong bảng Users theo username và mật khẩu plaintext.
  - [ ] Đăng nhập đúng: Đóng LoginWindow, mở MainWindow, hiển thị Tên người dùng và Chức vụ ở thanh Header.
  - [ ] Phân quyền theo cột Role: Nếu là Nhân viên (Staff), tự động ẩn hoặc vô hiệu hóa các nút Quản lý loại phòng và Báo cáo doanh thu trên Sidebar menu.
  - [ ] Hỗ trợ nút Đăng xuất (Logout) quay trở lại màn hình đăng nhập.

---

### [TASK-04] Module Quản lý Phòng & Loại phòng (Room & RoomType CRUD)
- **Assignee**: Member A
- **Priority**: Medium
- **Labels**: `room`, `crud`, `m1`
- **Branch**: `feature/room-management`
- **Milestone**: Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (Tuần 1-3)
- **Definition of Done**:
  - [ ] RoomDAO.cs và RoomTypeDAO.cs thực hiện các câu lệnh SQL SELECT, INSERT, UPDATE, DELETE vào bảng Rooms và RoomTypes.
  - [ ] RoomView.xaml: Hiển thị danh sách phòng bằng DataGrid, có ComboBox lọc theo Loại phòng và Trạng thái (Trống, Đang ở, Đã đặt, Bảo trì).
  - [ ] Dialog Thêm/Sửa phòng: Nhập Số phòng, chọn Loại phòng từ ComboBox, chọn Trạng thái. Validate không được để trống số phòng hoặc trùng lặp số phòng.
  - [ ] Quản lý danh mục loại phòng: Thêm loại phòng mới, cập nhật Đơn giá ngày (PricePerDay > 0), không cho xóa loại phòng đang có phòng liên kết.

---

### [TASK-05] Module Quản lý Khách hàng (Customer CRUD & Tìm kiếm)
- **Assignee**: Member B
- **Priority**: High
- **Labels**: `customer`, `crud`, `m1`
- **Branch**: `feature/customer-management`
- **Milestone**: Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (Tuần 1-3)
- **Definition of Done**:
  - [ ] CustomerDAO.cs: Viết các hàm lấy danh sách, thêm, sửa, tìm kiếm khách hàng từ bảng Customers.
  - [ ] CustomerView.xaml: DataGrid hiển thị danh sách khách hàng gồm: Mã KH, Họ tên, CMND/CCCD, Số điện thoại, Email.
  - [ ] Thanh tìm kiếm nhanh khách hàng theo Họ tên hoặc CCCD/SĐT cập nhật danh sách ngay khi gõ.
  - [ ] Dialog Thêm/Sửa khách hàng: Validate bắt buộc nhập Họ tên, CCCD (độ dài 9 hoặc 12 số) và Số điện thoại hợp lệ.

---

### [TASK-06] Module Quản lý Dịch vụ khách sạn (Service CRUD)
- **Assignee**: Member B
- **Priority**: Medium
- **Labels**: `service`, `crud`, `m1`
- **Branch**: `feature/service-management`
- **Milestone**: Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (Tuần 1-3)
- **Definition of Done**:
  - [ ] ServiceDAO.cs: Truy vấn lấy danh mục dịch vụ từ bảng Services.
  - [ ] ServiceView.xaml: Hiển thị bảng danh mục dịch vụ (Tên dịch vụ, Đơn giá).
  - [ ] Hỗ trợ thêm mới dịch vụ (ví dụ: Giặt ủi, Nước suối, Ăn sáng, Thuê xe máy).
  - [ ] Chỉnh sửa thông tin và đơn giá dịch vụ (kiểm tra đơn giá phải >= 0).
  - [ ] Xóa dịch vụ khi không còn phục vụ.

---

### [TASK-07] Module Đặt phòng (Booking) & Thuật toán chống trùng lịch
- **Assignee**: Member B
- **Priority**: High
- **Labels**: `booking`, `business-logic`, `m2`
- **Branch**: `feature/booking-conflict-check`
- **Milestone**: Mốc 2: Nghiệp Vụ Cốt Lõi, Sơ Đồ Phòng & Hóa Đơn (Tuần 3-5)
- **Definition of Done**:
  - [ ] BookingDAO.cs: Thao tác thêm, cập nhật, truy vấn bảng Bookings.
  - [ ] BookingView.xaml: Giao diện tạo phiếu đặt phòng gồm: Chọn Khách hàng (từ ComboBox hoặc tìm kiếm), chọn Phòng, chọn Ngày nhận (CheckInDate) và Ngày trả (CheckOutDate).
  - [ ] Cài đặt thuật toán kiểm tra xung đột thời gian: Hai khoảng thời gian [A, B] và [C, D] trùng nhau khi và chỉ khi A < D và C < B.
  - [ ] Hiển thị thông báo lỗi rõ ràng nếu phòng đã có người đặt trong khoảng ngày đã chọn.
  - [ ] Nếu hợp lệ, lưu phiếu vào bảng Bookings với trạng thái Đã đặt và cập nhật trạng thái phòng.

---

### [TASK-08] Module Check-in, Check-out & Thêm dịch vụ phòng
- **Assignee**: Member B
- **Priority**: High
- **Labels**: `booking`, `service`, `workflow`, `m2`
- **Branch**: `feature/checkin-checkout-services`
- **Milestone**: Mốc 2: Nghiệp Vụ Cốt Lõi, Sơ Đồ Phòng & Hóa Đơn (Tuần 3-5)
- **Definition of Done**:
  - [ ] Nút Check-in: Đổi trạng thái Booking sang Đang ở, đổi trạng thái Phòng sang Đang ở.
  - [ ] Giao diện Thêm dịch vụ phòng: Chọn phòng đang ở, chọn dịch vụ từ ComboBox Services, nhập số lượng, tự động nhân thành tiền và lưu vào bảng BookingServices.
  - [ ] Hiển thị danh sách các dịch vụ mà phòng đó đã gọi kèm nút xóa nếu nhập nhầm.
  - [ ] Nút Check-out: Kích hoạt luồng chuyển tiếp sang màn hình Hóa đơn để tính tiền và thanh toán.

---

### [TASK-09] Module Sơ đồ phòng trực quan (Room Matrix với XAML DataTrigger)
- **Assignee**: Member C
- **Priority**: High
- **Labels**: `advanced`, `ui`, `room-map`, `m2`
- **Branch**: `feature/room-matrix-trigger`
- **Milestone**: Mốc 2: Nghiệp Vụ Cốt Lõi, Sơ Đồ Phòng & Hóa Đơn (Tuần 3-5)
- **Definition of Done**:
  - [ ] RoomMapView.xaml: Hiển thị danh sách tất cả các phòng dạng thẻ (Card/Button) xếp theo lưới tự co giãn (WrapPanel hoặc UniformGrid).
  - [ ] Áp dụng Style.Triggers hoặc DataTrigger trong XAML để tự động đổi màu thẻ phòng theo trạng thái: Xanh lá (Trống), Đỏ (Đang ở), Cam (Đã đặt), Xám (Dọn dẹp/Bảo trì).
  - [ ] Thẻ phòng hiển thị rõ: Số phòng, Loại phòng, Trạng thái.
  - [ ] Click vào thẻ phòng: Xem nhanh thông tin chi tiết; click đúp để mở form Đặt phòng hoặc Check-out nhanh.

---

### [TASK-10] Module Hóa đơn, Thanh toán & Xuất hóa đơn ra file
- **Assignee**: Member C
- **Priority**: High
- **Labels**: `advanced`, `invoice`, `billing`, `m2`
- **Branch**: `feature/invoice-export`
- **Milestone**: Mốc 2: Nghiệp Vụ Cốt Lõi, Sơ Đồ Phòng & Hóa Đơn (Tuần 3-5)
- **Definition of Done**:
  - [ ] InvoiceDAO.cs: Thao tác với bảng Invoices.
  - [ ] InvoiceView.xaml: Hiển thị chi tiết hóa đơn thanh toán gồm: Tiền phòng (Math.Max(1, số ngày) × Đơn giá) + Tiền dịch vụ (tổng tiền các dịch vụ từ BookingServices).
  - [ ] Chọn phương thức thanh toán (Tiền mặt / Chuyển khoản), bấm nút Thanh toán cập nhật trạng thái hóa đơn sang Đã thanh toán, cập nhật Booking sang Hoàn thành và Phòng về trạng thái Trống.
  - [ ] Tính năng nâng cao: Nút Xuất hóa đơn ghi toàn bộ chi tiết hóa đơn ra file văn bản .txt hoặc mở cửa sổ In hóa đơn định dạng chuẩn đẹp.

---

### [TASK-11] Module Báo cáo & Thống kê doanh thu thuần XAML
- **Assignee**: Member C
- **Priority**: Medium
- **Labels**: `stats`, `report`, `ui`, `m3`
- **Branch**: `feature/revenue-stats-report`
- **Milestone**: Mốc 3: Báo Cáo Thống Kê & Chốt Demo (Tuần 5-6)
- **Definition of Done**:
  - [ ] ReportView.xaml: Có bộ lọc khoảng thời gian (Từ ngày - Đến ngày).
  - [ ] Thiết kế 3 thẻ KPI số liệu nổi bật: Tổng doanh thu (VNĐ), Tổng số lượt khách thuê, Tổng tiền dịch vụ phát sinh.
  - [ ] Thanh ProgressBar trực quan hiển thị tỷ lệ công suất phòng (Số phòng đang có khách / Tổng số phòng).
  - [ ] Bảng DataGrid chi tiết danh sách các hóa đơn đã thanh toán trong khoảng thời gian lọc.
  - [ ] Không sử dụng thư viện ngoài, đảm bảo 100% build mượt mà, không giật lag.

---

### [TASK-12] Tổng hợp kiểm thử E2E, Soạn Slide & Kịch bản Rehearsal Demo
- **Assignee**: Member C
- **Priority**: High
- **Labels**: `docs`, `presentation`, `demo`, `qa`, `m3`
- **Branch**: `feature/demo-rehearsal-slides`
- **Milestone**: Mốc 3: Báo Cáo Thống Kê & Chốt Demo (Tuần 5-6)
- **Definition of Done**:
  - [ ] Kiểm thử thông suốt chuỗi kịch bản Demo: Đăng nhập -> Sơ đồ phòng -> Đặt phòng -> Check-in -> Thêm 2 dịch vụ -> Check-out xuất hóa đơn -> Xem thống kê doanh thu.
  - [ ] Đảm bảo file HotelDB.sql chạy nạp lại dữ liệu ban đầu chỉ với 1 click trong SSMS nếu cần reset trước buổi demo.
  - [ ] Hoàn thành bộ Slide thuyết trình giới thiệu đề tài, mô hình kiến trúc WPF MVVM, CSDL và các tính năng nổi bật.
  - [ ] Soạn kịch bản phân vai nói chi tiết cho 3 thành viên và tổ chức rehearsal tối thiểu 2 lần trước ngày báo cáo.
