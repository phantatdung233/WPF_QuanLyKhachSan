# IT008 — Backlog đồ án Quản lý khách sạn (WPF, .NET 8)

## 1) Mục tiêu & phạm vi kỹ thuật bắt buộc
- Nền tảng: **.NET 8 + WPF** theo kiến trúc **MVVM**.
- MVVM Toolkit: **CommunityToolkit.Mvvm** với `ObservableObject`, `RelayCommand`, `ObservableCollection`.
- Dữ liệu: **EF Core Code First + SQLite + Migration**.
- Mô hình dữ liệu gồm đúng 8 bảng: `User`, `RoomType`, `Room`, `Customer`, `Booking`, `Service`, `BookingService`, `Invoice`.
- Quy trình Git: nhánh `main` (ổn định), `develop` (tích hợp), `feature/*` (tính năng theo member).

## 2) Phân rã backlog theo module

### 2.1) Nền tảng hệ thống
- [ ] Khởi tạo solution WPF .NET 8, cấu trúc thư mục theo MVVM (Models/ViewModels/Views/Services/Resources).
- [ ] Cài và cấu hình CommunityToolkit.Mvvm cho toàn bộ ViewModel.
- [ ] Thiết kế `HotelDbContext` + cấu hình SQLite connection string.
- [ ] Tạo migration đầu tiên và script update DB.
- [ ] Tạo dữ liệu seed cơ bản theo kế hoạch demo.
- [ ] Thiết kế điều hướng màn hình (navigation service + shell/MainWindow).
- [ ] Thiết lập cơ chế xác thực và phân quyền theo `User`.

### 2.2) Quản lý danh mục & nghiệp vụ cốt lõi
- [ ] CRUD `RoomType` (mã loại, tên loại, đơn giá cơ bản, trạng thái).
- [ ] CRUD `Room` (mã phòng, loại phòng, tình trạng, ghi chú).
- [ ] CRUD `Customer` (họ tên, CCCD/CMND, SĐT, email, địa chỉ).
- [ ] CRUD `Service` (tên dịch vụ, đơn giá, trạng thái áp dụng).
- [ ] CRUD `Booking` (thông tin khách, phòng, ngày đến/đi, trạng thái).
- [ ] Ràng buộc đặt phòng trùng lịch: chỉ cho phép khi **A < D và C < B**.
- [ ] Check-in/Check-out và cập nhật trạng thái phòng theo vòng đời booking.
- [ ] Quản lý `BookingService` gắn dịch vụ phát sinh vào từng booking.

### 2.3) Thanh toán, thống kê, UI/UX
- [ ] Tính tiền và phát hành `Invoice` từ tiền phòng + dịch vụ.
- [ ] Màn hình thanh toán: xác nhận công nợ, phương thức thanh toán, in/xuất hóa đơn.
- [ ] Dashboard thống kê doanh thu, công suất phòng, top dịch vụ.
- [ ] Tích hợp **LiveCharts2** cho biểu đồ trực quan.
- [ ] Truy vấn thống kê chạy async để UI không bị block.
- [ ] Dùng `DataTrigger` hiển thị sơ đồ phòng theo trạng thái.
- [ ] Hoàn thiện tài nguyên giao diện `Colors.xaml`, `Styles.xaml`, `Templates.xaml`.

## 3) Backlog chi tiết theo thành viên

### Member A — Nền tảng + phòng + quản trị nhánh
- [ ] Thiết lập MVVM nền tảng toàn dự án với CommunityToolkit.Mvvm.
- [ ] Xây dựng `HotelDbContext`, migration, liên kết 8 bảng theo Code First.
- [ ] Hoàn thiện điều hướng trang và khung Main navigation.
- [ ] Chức năng đăng nhập và phân quyền theo vai trò `User`.
- [ ] Chức năng quản lý `RoomType` và `Room` (CRUD + binding).
- [ ] Quản lý nhánh `main/develop`, review merge từ các `feature/*`.

### Member B — Khách hàng + đặt phòng + check-in/out + seed
- [ ] Chức năng quản lý `Customer` (CRUD + tìm kiếm).
- [ ] Chức năng `Booking` với kiểm tra trùng lịch điều kiện **A < D và C < B**.
- [ ] Check-in/Check-out và cập nhật trạng thái `Booking`/`Room` tương ứng.
- [ ] Chức năng `BookingService` thêm/xóa dịch vụ theo booking.
- [ ] Seed dữ liệu: tối thiểu **10 phòng, 10 khách, booking mẫu** để demo E2E.

### Member C — Giao diện + hóa đơn + thống kê + demo
- [ ] Thiết kế `Colors.xaml`, `Styles.xaml`, `Templates.xaml` đồng bộ nhận diện.
- [ ] Hoàn thiện `MainWindow` và layout tổng thể ứng dụng.
- [ ] Chức năng `Invoice`/thanh toán hoàn chỉnh luồng nghiệp vụ.
- [ ] Chức năng `Stats`: biểu đồ LiveCharts2 + lọc thời gian + KPI.
- [ ] Áp dụng `DataTrigger` cho sơ đồ phòng trực quan trạng thái.
- [ ] Tối ưu async cho thống kê và truy vấn dữ liệu lớn.
- [ ] Chuẩn bị slide + kịch bản demo trình bày tuần cuối.

## 4) Milestone theo kế hoạch môn học

### Milestone 1 — Cuối tuần 3: độc lập CRUD/binding
- [ ] Hoàn tất mô hình DB 8 bảng + migration chạy ổn định.
- [ ] Mỗi module chính có CRUD cơ bản và binding WPF hoạt động độc lập.
- [ ] Đăng nhập cơ bản và điều hướng chính đã chạy được.
- [ ] Seed dữ liệu đủ để test thao tác nghiệp vụ đơn lẻ.

### Milestone 2 — Cuối tuần 5: E2E + nâng cao
- [ ] Hoàn chỉnh luồng **Login → Booking → Check-in/Check-out → Invoice → Stats**.
- [ ] Áp dụng kiểm tra trùng lịch booking trong toàn bộ đường đi nghiệp vụ.
- [ ] Tích hợp giao diện nâng cao (DataTrigger sơ đồ phòng, style/template đồng bộ).
- [ ] Tích hợp LiveCharts2 + thống kê async, đảm bảo UX mượt.

### Milestone 3 — Tuần 6: đóng băng tính năng và rehearsal
- [ ] **Feature freeze trước demo 2 ngày**: không thêm tính năng mới.
- [ ] Refactor code, dọn technical debt, chuẩn hóa naming/structure.
- [ ] Bug fixing ưu tiên lỗi chặn luồng E2E và lỗi dữ liệu.
- [ ] Tổ chức **2 lần rehearsal** theo đúng kịch bản trình bày.

## 5) Quy tắc thực thi backlog
- [ ] Mỗi task phải có issue riêng, gán đúng assignee và milestone.
- [ ] Mọi thay đổi đi qua `feature/*` → PR vào `develop` → kiểm thử hồi quy → merge `main`.
- [ ] Không merge khi chưa pass checklist tự test chức năng liên quan.
- [ ] Cập nhật trạng thái backlog hằng ngày (To do / In progress / Done).
