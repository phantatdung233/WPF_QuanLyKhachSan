# AURA HOTEL — Hệ Thống Quản Lý Khách Sạn

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com)
[![MVVM Toolkit](https://img.shields.io/badge/MVVM-CommunityToolkit%208.4.2-68217A)](https://learn.microsoft.com/dotnet/communitytoolkit/mvvm)
[![WPF-UI](https://img.shields.io/badge/UI%20Shell-WPF--UI%204.3.0-005FB8)](https://wpfui.lepo.co)
[![LiveCharts2](https://img.shields.io/badge/Charts-LiveCharts2%202.0.5-2EA043)](https://livecharts.dev)
[![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-CC2927)](https://www.microsoft.com/sql-server)
[![Tests](https://img.shields.io/badge/Tests-xUnit-25A162)](https://xunit.net)
[![Design System](https://img.shields.io/badge/Design%20System-Google%20DESIGN.md-1A73E8)](./DESIGN.md)

Ứng dụng desktop quản lý khách sạn **AURA HOTEL** xây dựng trên nền tảng **C# .NET 10 WPF chuẩn MVVM** kết hợp **SQL Server ADO.NET** cho đồ án môn **IT008 (Lập trình trực quan)**. Hệ thống giải quyết triệt để bài toán trùng lịch đặt phòng (*overbooking*), thất thoát dịch vụ phát sinh và sai lệch công thức tính hóa đơn thông qua chu trình vận hành khép kín:

> `Đăng nhập phân quyền → Sơ đồ phòng trực quan → Đặt phòng → Check-in → Gọi dịch vụ → Check-out & Xuất hóa đơn (.txt) → Báo cáo doanh thu`

---

## Mục lục

- [Tính năng cốt lõi](#tính-năng-cốt-lõi)
- [Công nghệ sử dụng](#công-nghệ-sử-dụng)
- [Kiến trúc & Cấu trúc thư mục](#kiến-trúc--cấu-trúc-thư-mục)
- [Cài đặt & Khởi chạy](#cài-đặt--khởi-chạy)
- [Tài khoản Demo](#tài-khoản-demo)
- [Kiểm thử & Kiểm định chất lượng](#kiểm-thử--kiểm-định-chất-lượng)
- [Quy trình phát triển & Đóng góp](#quy-trình-phát-triển--đóng-góp)
- [Lộ trình 4 Milestones & Phân công](#lộ-trình-4-milestones--phân-công)
- [Thông tin đồ án](#thông-tin-đồ-án)

---

## Tính năng cốt lõi

| Phân hệ | Mô tả nghiệp vụ |
| --- | --- |
| **Xác thực & Phân quyền** | Đăng nhập SQL Server phân tách phiên `Administrator` (`Quản trị` — 8 mục điều hướng) và `Staff` (`Lễ tân` — 6 mục, ẩn Loại phòng & Báo cáo). |
| **Quản lý danh mục** | CRUD Loại phòng, Phòng, Khách hàng, Dịch vụ với tìm kiếm/lọc thời gian thực, kiểm tra đầu vào inline và khóa xóa an toàn khi có dữ liệu ràng buộc. |
| **Sơ đồ phòng trực quan** *(Nâng cao 1)* | Ma trận thẻ phòng nền sáng trung tính, tự động chuyển màu thanh/nhãn chỉ báo theo 4 trạng thái (`Trống`, `Đang ở`, `Đã đặt`, `Bảo trì`) bằng XAML `DataTrigger`. |
| **Đặt phòng chống trùng lịch** | Kiểm tra xung đột tự động theo *Overlap Conflict Rule* (`NewCheckIn < ExistingCheckOut && ExistingCheckIn < NewCheckOut`) và tính trước tiền phòng dự kiến. |
| **Lưu trú & Dịch vụ phòng** | Vận hành Check-in nhận phòng, ghi nhận dịch vụ phát sinh kèm snapshot `UnitPrice` tại thời điểm gọi và khởi tạo Check-out. |
| **Thanh toán & Xuất biên lai** *(Nâng cao 2)* | Tính tiền phòng tối thiểu 1 đêm + dịch vụ, thanh toán nguyên tử (*Settlement Invariant*), xem trước biên lai monospace và xuất file `.txt` chuẩn UTF-8. |
| **Báo cáo & Thống kê** | Thẻ KPI doanh thu, thanh tiến trình công suất phòng (`ProgressBar`), biểu đồ đường doanh thu theo ngày (`LiveCharts2`) và bảng tra cứu hóa đơn. |

---

## Công nghệ sử dụng

| Tầng | Công nghệ / Thư viện | Vai trò |
| --- | --- | --- |
| **UI Framework** | WPF trên `.NET 10` (`net10.0-windows`) + `WPF-UI` `4.3.0` | Khung cửa sổ Fluent (`FluentWindow`, `NavigationView`, `ContentDialog`, `InfoBar`) |
| **Design System** | Chuẩn Google [`DESIGN.md`](./DESIGN.md) | Hệ token YAML chuẩn hóa màu sắc (`#005FB8`), thang chữ Segoe UI Variable, lưới `4px/8px` & 4 trạng thái UI |
| **Kiến trúc MVVM** | `CommunityToolkit.Mvvm` `8.4.2` | `ObservableObject`, `[ObservableProperty]`, `[RelayCommand]` tách biệt hoàn toàn View và Logic |
| **Biểu đồ** | `LiveChartsCore.SkiaSharpView.WPF` `2.0.5` | Biểu đồ đường doanh thu theo ngày (`CartesianChart`) tại phân hệ Báo cáo |
| **Cơ sở dữ liệu** | Microsoft SQL Server (Express / LocalDB) | 8 bảng quan hệ chuẩn khóa ngoại & ràng buộc `CHECK`, khởi tạo 1-click qua `Database/HotelDB.sql` |
| **Truy cập dữ liệu** | ADO.NET thuần (`Microsoft.Data.SqlClient`) | `DataProvider` dùng chung + các lớp `DAO` truy vấn tham số hóa (`@param`) chống SQL Injection |
| **Kiểm thử tự động** | `xUnit` (`tests/HotelManagement.Tests`) | Unit test kiểm chứng công thức nghiệp vụ, phân quyền, kiểm tra dữ liệu và trạng thái ViewModel |

---

## Kiến trúc & Cấu trúc thư mục

Dự án tổ chức theo kiến trúc **MVVM phân tầng** kết hợp bộ tài liệu *Single Source of Truth* ở thư mục gốc (được khởi tạo hoàn chỉnh qua Mốc 1 `#58`–`#59`):

```text
WPF_QuanLyKhachSan/
├── DESIGN.md                            # Design System & Tokens chuẩn Google DESIGN.md
├── GLOSSARY.md                          # Từ điển thuật ngữ, trạng thái & công thức nghiệp vụ bất biến
├── CLAUDE.md                            # Quy tắc thực thi 2 Pha bắt buộc cho AI Agent & Developer
├── AGENTS.md                            # Chỉ mục ngữ cảnh dự án & liên kết công cụ
├── Database/
│   └── HotelDB.sql                      # Script 1-click tạo CSDL 8 bảng & nạp dữ liệu mẫu chuẩn (#59)
├── docs/
│   ├── agents/                          # Cấu hình Issue Tracker (gh CLI), Triage Labels & Domain Docs
│   └── specs/
│       └── hotel-management-system.md   # Đặc tả hệ thống chi tiết (đồng bộ với SPEC #77)
├── src/
│   └── HotelManagement/                 # Ứng dụng chính WPF (.NET 10, EnableWindowsTargeting=true)
│       ├── Models/                      # 8 lớp thực thể ánh xạ bảng CSDL
│       ├── DAO/                         # Tầng truy cập dữ liệu ADO.NET tham số hóa + DataProvider
│       ├── Services/                    # Các seam nghiệp vụ thuần phục vụ TDD (BookingRules, InvoiceCalculator...)
│       ├── ViewModels/                  # Tầng trạng thái & lệnh MVVM (xử lý đủ Loading/Empty/Error/Success)
│       ├── Views/                       # Cửa sổ & màn hình XAML lắp ghép từ WPF-UI primitives
│       └── Resources/                   # Colors.xaml & Styles.xaml ánh xạ 1-1 từ DESIGN.md
└── tests/
    └── HotelManagement.Tests/           # Dự án kiểm thử tự động xUnit
```

---

## Cài đặt & Khởi chạy

### Yêu cầu môi trường

- **.NET 10 SDK** (`10.0.100` trở lên). Nhờ cấu hình `EnableWindowsTargeting=true`, có thể chạy `dotnet build` và `dotnet test` trên cả **Windows**, **macOS** và **Linux**.
- **Môi trường chạy giao diện WPF (Windows 10/11)**: Visual Studio 2022 (v17.14+) / Visual Studio 2026 (workload *.NET Desktop Development*), JetBrains Rider hoặc VS Code.
- **Cơ sở dữ liệu**: SQL Server Express hoặc SQL Server LocalDB + SQL Server Management Studio (SSMS).

### Các bước thiết lập

1. **Clone kho mã nguồn**:
   ```bash
   git clone https://github.com/phantatdung233/WPF_QuanLyKhachSan.git
   cd WPF_QuanLyKhachSan
   ```

2. **Khởi tạo cơ sở dữ liệu** *(từ Issue `#59`)*:
   - Mở file `Database/HotelDB.sql` bằng SSMS và nhấn **Execute (`F5`)** để tạo mới CSDL `HotelManagementDb` cùng bộ dữ liệu mẫu định sẵn.

3. **Cấu hình chuỗi kết nối** *(tùy chọn — mặc định tự động kết nối `.\SQLEXPRESS`)*:
   - **PowerShell (Windows)**:
     ```powershell
     $env:HOTEL_DB_CONNECTION="Server=.\SQLEXPRESS;Database=HotelManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
     ```
   - **zsh / bash (macOS / Linux kết nối SQL Server Docker)**:
     ```bash
     export HOTEL_DB_CONNECTION="Server=localhost,1433;Database=HotelManagementDb;User Id=sa;Password=<YourPassword>;TrustServerCertificate=True;"
     ```

4. **Biên dịch và khởi chạy ứng dụng**:
   ```bash
   dotnet build
   dotnet run --project src/HotelManagement
   ```

---

## Tài khoản Demo

Dữ liệu mẫu trong `Database/HotelDB.sql` cung cấp sẵn 2 tài khoản để kiểm chứng phân quyền:

| Tài khoản | Mật khẩu | Vai trò (`Users.Role`) | Phạm vi truy cập |
| --- | --- | --- | --- |
| `admin` | `123456` | `Administrator` (`Quản trị`) | Toàn quyền **8 mục điều hướng** (bao gồm *Loại phòng* và *Báo cáo doanh thu*) |
| `staff` | `123456` | `Staff` (`Lễ tân`) | **6 mục vận hành lễ tân** hàng ngày (tự động ẩn *Loại phòng* và *Báo cáo*) |

---

## Kiểm thử & Kiểm định chất lượng

Mọi thay đổi trước khi mở Pull Request đều phải vượt qua các cổng kiểm định tự động:

```bash
# 1. Kiểm tra biên dịch toàn bộ Solution
dotnet build

# 2. Chạy toàn bộ bộ kiểm thử đơn vị xUnit (TDD Seams & ViewModels)
dotnet test

# 3. Kiểm định tính hợp lệ của Design System theo chuẩn Google DESIGN.md
npx -y -p @google/design.md designmd lint DESIGN.md
```

---

## Quy trình phát triển & Đóng góp

### Nguồn sự thật duy nhất (*Single Sources of Truth*)

- **Thuật ngữ & Công thức nghiệp vụ**: [`GLOSSARY.md`](./GLOSSARY.md)
- **Đặc tả hệ thống & User Stories**: [`docs/specs/hotel-management-system.md`](./docs/specs/hotel-management-system.md) ([SPEC `#77`](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77))
- **Hệ thống thiết kế & Chống AI Slop**: [`DESIGN.md`](./DESIGN.md)
- **Quy tắc thực thi Agent & Developer**: [`CLAUDE.md`](./CLAUDE.md), [`AGENTS.md`](./AGENTS.md), [`.cursorrules`](./.cursorrules)
- **Backlog thực thi**: [GitHub Issues](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues) (`gh issue view <number>`)

### Quy tắc thực thi 2 Pha bắt buộc

Mỗi GitHub Issue bắt buộc triển khai tuần tự qua **2 pha tách biệt** (chi tiết trong [`CLAUDE.md`](./CLAUDE.md)):

1. **Pha 1 — Logic, Data & State First**: Hoàn thiện `Models/` → `DAO/` → `Services/` (TDD seam) → `ViewModels/` (bao phủ đủ 4 trạng thái `Loading`, `Empty`, `Error`, `Success`) và chạy xanh 100% `dotnet build` & `dotnet test` **trước khi** căn chỉnh giao diện XAML.
2. **Pha 2 — UI Assembly & Verification**: Đọc [`DESIGN.md`](./DESIGN.md), dựng XAML thuần bằng primitives chuẩn của `WPF-UI` và `{StaticResource}` tokens (không hardcode mã màu hex ngoài `Colors.xaml`, tuân thủ lưới `4px/8px`, tối đa **1 Primary CTA** mỗi màn hình/dialog) và kiểm tra hiển thị ở `1280×800` / `1280×720`.

### Chiến lược nhánh Git & Review vòng tròn

- **`main`**: Nhánh ổn định phục vụ báo cáo và nộp đồ án.
- **`develop`**: Nhánh tích hợp chung của nhóm.
- **`feature/<slice-name>`**: Cắt từ `develop` cho từng GitHub Issue (ví dụ: `git switch -c feature/58-app-shell develop`).
- **Pull Request**: Mở PR từ `feature/*` về `develop` theo mẫu [`.github/pull_request_template.md`](./.github/pull_request_template.md) và phân công **Reviewer vòng tròn** (`Member A → Member B → Member C → Member A`).

---

## Lộ trình 4 Milestones & Phân công

Backlog gồm **14 Vertical-Slice Issues** (`#58`–`#65`, `#78`–`#83`) chia thành **4 Mốc bàn giao hướng kết quả Demo** (hoàn thành trọn vẹn từng chặng không gò bó hạn ngày cứng):

| Milestone | Mục tiêu bàn giao có thể Demo | Issues |
| --- | --- | --- |
| **Mốc 1: Khung Nền Tảng & Đăng Nhập Phân Quyền** | Khung `.NET 10` WPF + `xUnit`, CSDL 8 bảng seed chuẩn, đăng nhập phân quyền `admin` / `staff` | `#58`, `#59`, `#60` |
| **Mốc 2: Danh Mục Cơ Bản & Sơ Đồ Phòng Trực Quan** | CRUD Loại phòng, Phòng, Khách hàng, Dịch vụ + **Sơ đồ phòng `DataTrigger`** | `#82`, `#61`, `#78`, `#79`, `#81` |
| **Mốc 3: Chu Trình Lưu Trú & Thanh Toán Hóa Đơn** | Đặt phòng chống trùng lịch, Check-in/out, Gọi dịch vụ, Thanh toán & **Xuất biên lai `.txt`** | `#62`, `#80`, `#83`, `#63` |
| **Mốc 4: Báo Cáo Doanh Thu & Kiểm Thử Tổng Duyệt** | Báo cáo KPI & biểu đồ `LiveCharts2` + Kiểm thử tích hợp E2E & tổng duyệt Demo | `#64`, `#65` |

### Phân công thành viên

| Thành viên | Mảng phụ trách chính | Issues đảm nhận |
| --- | --- | --- |
| **Member A** ([@phantatdung233](https://github.com/phantatdung233)) | Nền tảng MVVM, CSDL & DataProvider, Xác thực phân quyền, Loại phòng & Phòng | `#58`, `#59`, `#60`, `#82`, `#61` |
| **Member B** ([@xuanduong29](https://github.com/xuanduong29)) | Khách hàng, Dịch vụ, Đặt phòng chống trùng lịch, Check-in/out & Gọi dịch vụ phòng | `#78`, `#79`, `#62`, `#80`, `#83` |
| **Member C** ([@ngocthuyen1](https://github.com/ngocthuyen1)) | Sơ đồ phòng `DataTrigger`, Thanh toán hóa đơn `.txt`, Báo cáo doanh thu & Tổng duyệt | `#81`, `#63`, `#64`, `#65` |

---

## Thông tin đồ án

- **Môn học**: IT008 — Lập trình trực quan (*Visual Programming*).
- **Mục đích sử dụng**: Mã nguồn và tài liệu thiết kế phục vụ mục đích học tập, giảng dạy và nghiên cứu học thuật.
