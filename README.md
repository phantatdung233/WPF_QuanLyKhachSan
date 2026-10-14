# Quản Lý Khách Sạn (WPF, .NET 8) — Đồ án IT008

Ứng dụng desktop quản lý khách sạn: danh mục phòng/khách hàng/dịch vụ, đặt phòng chống
trùng lịch, check-in/out, hóa đơn và báo cáo doanh thu.

## Thành viên

| Thành viên | Phụ trách |
| ---------- | --------- |
| Member A (@phantatdung233) | Khung MVVM, CSDL, đăng nhập, quản lý phòng (TASK-01–04) |
| Member B (@xuanduong29) | Khách hàng, dịch vụ, đặt phòng, check-in/out (TASK-05–08) |
| Member C (@ngocthuyen1) | Sơ đồ phòng, hóa đơn, báo cáo, slide demo (TASK-09–12) |

## Công nghệ

- .NET 8 WPF, XAML, `CommunityToolkit.Mvvm`
- SQL Server / SQL Server Express, ADO.NET thuần (`DataProvider.cs`), không dùng EF
- Xem chi tiết: SPEC #77, `TASKS.md`, `docs/specs/hotel-management-system.md`

## Cách chạy

1. Cài Visual Studio 2022 (.NET 8) và SQL Server Express + SSMS.
2. Mở `Database/HotelDB.sql` trong SSMS, Execute để tạo DB và nạp dữ liệu mẫu
   (2 users, 4 loại phòng, 12 phòng, 5 khách hàng, 5 dịch vụ).
3. Kiểm tra chuỗi kết nối trong `DataProvider.cs` khớp tên server локаль (ví dụ `.\SQLEXPRESS`).
4. Mở solution, F5.

Tài khoản demo: `admin` / `123456` (quyền quản trị), `staff` / `123456` (lễ tân).

## Luồng nghiệp vụ chính

Đăng nhập → Sơ đồ phòng → Đặt phòng → Check-in → Thêm dịch vụ →
Check-out / Hóa đơn → Báo cáo doanh thu.

Quy tắc quan trọng:

- Chống trùng lịch: hai khoảng `[A, B]` và `[C, D]` trùng nhau khi `A < D && C < B`.
- Tiền phòng = `Math.Max(1, số ngày) × đơn giá/ngày`; tổng hóa đơn = tiền phòng + tiền dịch vụ.

## Tài liệu

- Backlog (Single Source of Truth): `TASKS.md`
- Hướng dẫn từng task: `docs/tasks/TASK-01.md` … `TASK-12.md`
- Thuật ngữ chuẩn: `GLOSSARY.md`
- Issues GitHub: [#58](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/58)–[#65](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/65), [#78](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/78)–[#81](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/81)
