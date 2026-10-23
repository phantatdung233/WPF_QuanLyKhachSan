# Design Brief — Hotel Management System (IT008)

**Dành cho**: Designer thiết kế Figma
**Tham chiếu nguồn**: [`docs/specs/hotel-management-system.md`](../specs/hotel-management-system.md) (Issue #77), [`GLOSSARY.md`](../../GLOSSARY.md), [`docs/tasks/TASK-01..12`](../tasks/)
**Tài liệu liên quan**: [`WIREFRAMES.md`](./WIREFRAMES.md), [`DELIVERY-PLAN.md`](./DELIVERY-PLAN.md)

---

## 0. Đọc mục này trước (30 giây)

> Đây là **đồ án môn IT008 (Lập trình trực quan)** — một **ứng dụng desktop Windows** viết bằng WPF/.NET 10, không phải web, không phải mobile.
>
> Người dùng là **lễ tân khách sạn ngồi dùng liên tục cả ngày**. Ưu tiên số một là **thông tin rõ ràng, thao tác ít click, bảng biểu dễ đọc** — không phải landing page thoáng đãng.
>
> **Ràng buộc quan trọng nhất**: mọi thứ bạn vẽ phải **code được bằng XAML trong 6 tuần bởi 3 sinh viên**, dùng đúng thư viện Fluent UI có sẵn. Nếu thiết kế đẹp nhưng không code được trong thời gian đó thì phải làm lại.

Mục tiêu của bạn: tạo **bộ Figma làm chuẩn giao diện** để team dev bám theo chính xác — không phải mockup để trình bày.

---

## 1. Sản phẩm là gì

| Mục | Nội dung |
| --- | --- |
| Loại ứng dụng | Desktop app, cửa sổ Windows |
| Nền tảng code | WPF trên .NET 10 (`net10.0-windows`) |
| Thư viện UI | **WPF-UI 4.3.0** — shell kiểu Fluent, sidebar `NavigationView`, cửa sổ `FluentWindow` |
| Ngôn ngữ UI | **Tiếng Việt toàn bộ**, có đầy đủ dấu |
| Tiền tệ | VNĐ, định dạng có dấu phân cách nghìn + hậu tố `đ` |
| Vai trò người dùng | **Administrator** (đầy đủ) và **Staff** (lễ tân, bị ẩn menu quản trị) |
| Độ phân giải tối thiểu | 1280×720 (buổi demo dùng máy chiếu); thiết kế ở **1280×800** |
| Số màn hình | **13 màn/khu vực** — xem mục 5 |

### 1.1 Nghiệp vụ tóm tắt (bạn cần hiểu để vẽ đúng)

Người dùng đăng nhập → mở **Sơ đồ phòng** → tạo **Đặt phòng** cho khách → **Check-in** khi khách đến → ghi nhận **dịch vụ phát sinh** trong lúc lưu trú → **Check-out** và **thanh toán hóa đơn** → xem **báo cáo doanh thu**.

Chuỗi này là "vòng đời" chính. Mọi màn hình của bạn phải phục vụ chuỗi này, và **sơ đồ phòng là màn hình trung tâm** — nó là nơi lễ tân nhìn vào 90% thời gian.

### 1.2 Quy tắc nghiệp vụ ảnh hưởng trực tiếp tới UI

Đây là những chỗ designer hay vẽ sai nếu không được cảnh báo:

1. **Phòng có 4 trạng thái, không phải 2.** Không được gộp "Đã đặt" với "Đang ở".
2. **Có 2 biến thể sidebar theo vai trò.** Staff **không thấy** mục "Loại phòng" và "Báo cáo".
3. **Đặt phòng có thể bị từ chối** vì trùng lịch → phải thiết kế **trạng thái lỗi** của form đặt phòng.
4. **Hóa đơn có 2 trạng thái**: chưa thanh toán / đã thanh toán → cần 2 diện mạo khác nhau.
5. **Tính tiền phòng tối thiểu 1 đêm** kể cả khi khách nhận và trả trong cùng ngày — chỗ này cần hiển thị rõ để lễ tân giải thích được với khách.
6. **Phòng đang có khách ở** thì không được xóa → nút Xóa phải có trạng thái bị khóa (disabled).
7. **Dịch vụ / loại phòng đã được dùng** thì không được xóa → cần thông báo chặn, không phải màn hình trắng.
8. **Ngày trả phòng phải sau ngày nhận phòng ít nhất 1 ngày** → validate ngày trên form đặt phòng.
9. **Hóa đơn xuất ra file `.txt`** → cần một màn preview văn bản hóa đơn dạng monospace (xem màn 11).
10. **Dữ liệu mẫu khi demo**: 12 phòng, 4 loại phòng, 5 khách hàng, 5 dịch vụ, 2 tài khoản. Vẽ đúng số lượng này trong mockup để demo trông thật.

---

## 2. Ràng buộc kỹ thuật (bắt buộc)

### 2.1 Những gì ĐƯỢC dùng

| Thành phần | Dùng cho |
| --- | --- |
| Sidebar dạng `NavigationView` | Menu dọc bên trái, có icon + chữ, có mục đang chọn được highlight |
| `FluentWindow` | Khung cửa sổ, có nút minimize/maximize/close chuẩn Fluent |
| Bảng dữ liệu (`DataGrid`) | Mọi màn danh mục. Có header, sắp xếp, chọn dòng, cuộn |
| Bộ lọc `ComboBox` | Lọc theo loại / trạng thái |
| Ô tìm kiếm `TextBox` (có icon kính lúp) | Tìm khách hàng, tìm dịch vụ |
| `DatePicker` | Ngày nhận / trả phòng, khoảng ngày báo cáo |
| `ProgressBar` | Thanh công suất phòng trong báo cáo |
| Biểu đồ đường (LiveCharts2) | Duy nhất trang Báo cáo |
| Hộp thoại `Window.ShowDialog()` | Mọi form thêm/sửa, xác nhận |
| Toast / `MessageBox` | Thông báo thành công, lỗi, cảnh báo |

### 2.2 Những gì KHÔNG được dùng

- ❌ Mobile layout, responsive breakpoint, tablet
- ❌ Dark mode, theme switcher
- ❌ Animation chuyển trang phức tạp, parallax, gradient động
- ❌ Kéo-thả (drag & drop), swipe
- ❌ Multi-window ngoài 1 cửa sổ chính + hộp thoại
- ❌ Biểu đồ tròn 3D, gauge đồng hồ nhiều kim, biểu đồ nhiệt
- ❌ Icon bộ tự vẽ không có trong Fluent Icons
- ❌ Hiệu ứng blur / backdrop / glassmorphism

### 2.3 Nguyên tắc thị giác

- **Phẳng, sạch, viền mảnh.** Bóng đổ rất nhẹ (nếu có), bán kính góc 4–8px.
- **Mật độ thông tin cao nhưng có nhịp**: hàng bảng cao ~40px, padding 12–16px.
- **Màu dùng có mục đích, không trang trí.** Màu mạnh nhất trong app phải dành cho **trạng thái phòng** — vì đó là thứ lễ tân cần nhận ra trong 0.5 giây.
- **Không dùng màu thương hiệu cho badge trạng thái.** Xem 2.4.

### 2.4 Bảng màu

**Màu thương hiệu (đã chốt):**

| Vai trò | Mã | Ghi chú |
| --- | --- | --- |
| Primary — xanh navy | `#1E3A5F` | Sidebar, nút chính, header, tiêu đề |
| Primary — nhạt (hover/selected) | `#2F5280` | Mục menu đang chọn |
| Primary — nền trang | `#F5F7FA` | Nền vùng nội dung |
| Bề mặt thẻ (surface) | `#FFFFFF` | Thẻ, bảng, hộp thoại |
| Chữ chính | `#1A202C` | |
| Chữ phụ / nhãn | `#4A5568` | |
| Viền / đường kẻ | `#E2E8F0` | |
| Thành công | `#2F855A` | Thông báo lưu thành công |

**Màu trạng thái phòng (BẮT BUỘC, không đổi mã):**

| Trạng thái | Nhãn tiếng Việt | Mã | Ý nghĩa nghiệp vụ |
| --- | --- | --- | --- |
| Available | Trống | `#48BB78` | Sẵn sàng đón khách |
| Occupied | Đang ở | `#F56565` | Có khách đang lưu trú |
| Reserved | Đã đặt | `#ED8936` | Đã đặt trước, chưa đến |
| Maintenance | Bảo trì | `#A0AEC0` | Ngừng hoạt động |

> ⚠️ Bốn mã màu trên được quy định cố định trong `docs/tasks/TASK-09.md` và dev sẽ dùng đúng các giá trị đó trong XAML. Đừng "làm mềm" hay đổi tông. Bạn được phép đổi **màu chữ trên nền đó** nếu cần tương phản — hãy kiểm tra badge cam `#ED8936` và xám `#A0AEC0` trước khi dùng chữ trắng.

**Màu ngữ nghĩa khác:** thông báo lỗi dùng tông đỏ đậm hơn `#F56565` để không lẫn với trạng thái "Đang ở" (ví dụ `#C53030`). Trạng thái thanh toán: chưa thanh toán dùng tông cam nhạt, đã thanh toán dùng tông xanh lá đậm.

### 2.5 Kiểu chữ

- Font: **Segoe UI** (có sẵn trên Windows, hỗ trợ tiếng Việt đầy đủ). Nếu bạn dùng font khác trong Figma, phải đảm bảo tiếng Việt hiển thị đúng dấu.
- Thang chữ gợi ý: 12 (nhãn nhỏ) / 13 (nội dung bảng) / 14 (nội dung chuẩn) / 16 (tiêu đề khu vực) / 20–24 (tiêu đề trang) / 28–32 (số liệu KPI).
- Số tiền dùng numeral tabular (chữ số đều bề ngang) để cột tiền trong bảng thẳng hàng.
- **Không** dùng chữ in hoa toàn bộ cho nội dung — chỉ dùng cho nhãn phụ nhỏ.

### 2.6 Định dạng dữ liệu

| Loại | Định dạng | Ví dụ |
| --- | --- | --- |
| Tiền | `#,##0 đ` | `1.070.000 đ` |
| Ngày | `dd/MM/yyyy` | `14/10/2026` |
| Số điện thoại | 10 chữ số, bắt đầu bằng 0 | `0901234567` |
| CCCD | 9 hoặc 12 chữ số | `079201001234` |
| Số phòng | 3 ký tự | `101`, `305` |

---

## 3. Cấu trúc khung ứng dụng (dùng chung mọi màn)

```
┌──────────────┬────────────────────────────────────────────────┐
│              │  Header: [Tiêu đề trang]  Xin chào: Tên (Role) │
│   Sidebar    │                            [Đăng xuất]         │
│  (nav dọc)   ├────────────────────────────────────────────────┤
│              │                                                │
│  • Sơ đồ phòng                                                │
│  • Đặt phòng │        Vùng nội dung (ContentControl)          │
│  • Phòng     │        cuộn dọc nếu dài                        │
│  • Khách hàng│                                                │
│  • Dịch vụ   │                                                │
│  • Hóa đơn   │                                                │
│  • Báo cáo   │                                                │
│              │                                                │
└──────────────┴────────────────────────────────────────────────┘
```

**Kích thước gợi ý**: sidebar rộng 220–240px (có thể thu gọn còn icon), header cao 56–64px, padding nội dung 24px.

**Menu sidebar — Admin (8 mục):** Sơ đồ phòng, Đặt phòng, Phòng, Loại phòng, Khách hàng, Dịch vụ, Hóa đơn, Báo cáo.
**Menu sidebar — Staff (6 mục):** bỏ **Loại phòng** và **Báo cáo**.

> Ghi chú về nguồn: TASK-01 liệt kê menu "Sơ đồ phòng, Đặt phòng, Phòng, Khách hàng, Dịch vụ, Hóa đơn, Báo cáo" (7 mục, chưa có Loại phòng); TASK-04 và TASK-03 yêu cầu mục "Quản lý loại phòng" chỉ Admin thấy. Gộp lại thành 8 mục cho Admin. Nếu bạn thấy 8 mục quá dài, được phép gộp "Phòng" + "Loại phòng" thành một màn có 2 tab — nhưng phải giữ đúng quyền ẩn với Staff.

**Header** phải có: tiêu đề trang hiện tại, "Xin chào: [Họ tên] ([Vai trò])", nút "Đăng xuất".

Bố cục khung này vẽ **một lần** thành component, các màn hình khác tái sử dụng.

---

## 4. Persona & tình huống sử dụng

| Persona | Bối cảnh | Điều họ cần từ UI |
| --- | --- | --- |
| **Lễ tân (Staff)** | Đứng quầy, khách đang chờ trước mặt. Tai nghe điện thoại, tay nhập máy. | Nhìn 1 giây là biết phòng nào trống. Tạo đặt phòng nhanh. Thêm dịch vụ không quá 3 click. |
| **Quản trị (Admin)** | Văn phòng, đầu/cuối ngày. | Xem doanh thu theo khoảng ngày, kiểm tra công suất phòng, quản lý giá loại phòng. |
| **Giảng viên chấm bài** | Xem demo 7–8 phút trên máy chiếu. | Màn hình phải trông "đã hoàn thiện", dữ liệu mẫu trông thật, không có chỗ trống xấu hổ. |

**Kết luận thiết kế:** tối ưu cho lễ tân, nhưng đảm bảo 3 màn "điểm nhấn" của bài demo trông ấn tượng nhất: **Sơ đồ phòng**, **Hóa đơn**, **Báo cáo**.

---

## 5. Danh sách màn hình cần thiết kế

Tên frame trong Figma **phải đặt đúng tên kỹ thuật** dưới đây để dev đối chiếu.

| # | Frame name (Figma) | Màn hình | Vai trò | Mức ưu tiên |
| --- | --- | --- | --- | --- |
| 1 | `LoginWindow` | Đăng nhập | Cả hai | Vòng 1 |
| 2 | `MainWindow-Shell` | Khung chính + sidebar Admin & Staff | Cả hai | Vòng 1 |
| 3 | `RoomMapView` | **Sơ đồ phòng (Room Matrix)** ⭐ | Cả hai | Vòng 1 |
| 4 | `RoomView` | Quản lý phòng | Cả hai | Vòng 2 |
| 5 | `RoomTypeView` | Quản lý loại phòng | Admin | Vòng 2 |
| 6 | `CustomerView` | Quản lý khách hàng | Cả hai | Vòng 2 |
| 7 | `ServiceView` | Quản lý dịch vụ | Cả hai | Vòng 2 |
| 8 | `BookingView` / `BookingDialog` | Đặt phòng ⭐ | Cả hai | Vòng 2 |
| 9 | `BookingList` | Danh sách booking + thao tác Check-in | Cả hai | Vòng 2 |
| 10 | `AddServiceDialog` | Gọi dịch vụ phòng | Cả hai | Vòng 3 |
| 11 | `InvoiceView` | **Hóa đơn & thanh toán** ⭐ | Cả hai | Vòng 3 |
| 12 | `InvoicePreview` | Xem trước / xuất hóa đơn `.txt` | Cả hai | Vòng 3 |
| 13 | `ReportView` | **Báo cáo doanh thu** ⭐ | Admin | Vòng 3 |
| — | `Dialog-*` | Các hộp thoại xác nhận/xóa/lỗi | Cả hai | Vòng 2–3 |

Chi tiết từng màn: xem [`WIREFRAMES.md`](./WIREFRAMES.md).

---

## 6. Trạng thái bắt buộc cho từng loại màn

Designer thường chỉ vẽ trạng thái "đẹp" (có dữ liệu đầy đủ). Đồ án này cần cả trạng thái xấu — vì giảng viên sẽ bấm thử.

### 6.1 Màn danh mục (Phòng / Loại phòng / Khách hàng / Dịch vụ / Booking)
- **Bình thường**: có ~10 dòng dữ liệu, có ít nhất 1 dòng được chọn.
- **Rỗng**: "Chưa có dữ liệu" + nút gọi hành động (Thêm mới). Không phải bảng trắng trơn.
- **Đang tìm kiếm, không có kết quả**: "Không tìm thấy khách hàng phù hợp với '079'". Gợi ý xóa từ khóa.
- **Đang lọc, không có kết quả**: "Không có phòng nào ở trạng thái Bảo trì".
- **Dòng bị khóa thao tác**: nút Xóa disabled + tooltip giải thích lý do (vd: "Phòng đang có khách ở").
- **Lỗi validate trên form**: lỗi hiển thị **ngay dưới ô nhập**, không phải chỉ popup.

### 6.2 Sơ đồ phòng
- Đủ 4 màu cùng lúc (12 phòng).
- Hover + focus một thẻ phòng.
- Tooltip khi hover gợi ý "Click đúp để đặt phòng / check-out".
- Cửa sổ hẹp (khoảng 1024px): thẻ tự xuống dòng, không tràn ngang.
- Trạng thái rỗng: "Chưa có phòng nào" (trường hợp database trống).

### 6.3 Hóa đơn
- Khách có dùng dịch vụ (bảng kê 2–3 dòng).
- Khách không dùng dịch vụ (tiền dịch vụ `0 đ`).
- Ở đúng 1 đêm (nhận và trả trong cùng ngày) → hiển thị "1 đêm" chứ không phải "0".
- Đã thanh toán → các nút thanh toán chuyển disabled, có badge "ĐÃ THANH TOÁN".
- Lỗi: chưa chọn phương thức thanh toán.

### 6.4 Đặt phòng
- **Lỗi trùng lịch**: thông báo đỏ nổi bật trong form, câu chữ: *"Phòng này đã có người đặt trong khoảng thời gian đã chọn! Vui lòng chọn khoảng ngày khác hoặc phòng khác."* Cần chỉ ra phòng nào đang bận và khoảng bận.
- **Lỗi ngày**: "Ngày trả phòng phải sau ngày nhận phòng ít nhất 1 ngày".
- **Cảnh báo ngày trong quá khứ**: tông vàng, không chặn nhưng nhắc.
- **Trạng thái hợp lệ**: có xem trước tiền phòng tạm tính (số đêm × đơn giá).

### 6.5 Báo cáo
- Có dữ liệu: 3 KPI có số, progressbar có thanh, biểu đồ có đường, bảng có dòng.
- **Không có dữ liệu trong khoảng lọc**: KPI hiển thị `0 đ`, bảng trống thân thiện, biểu đồ hiển thị trạng thái "Không có dữ liệu" (không phải khung trắng lỗi).
- **Ngày bắt đầu > ngày kết thúc**: thông báo nhắc chọn lại.

---

## 7. Design System cần bàn giao

### 7.1 Trang `01-Styles`
- Bảng màu (brand + 4 trạng thái + ngữ nghĩa), có mã hex ghi chú bên cạnh.
- Typography scale với tên style: `Display/KPI`, `Heading/Page`, `Heading/Section`, `Body`, `Body/Small`, `Label`.
- Spacing scale: 4 / 8 / 12 / 16 / 24 / 32.
- Bán kính góc, độ dày viền, độ đổ bóng.

### 7.2 Trang `02-Components`
Mỗi component phải có **các biến thể trạng thái**:

| Component | Biến thể |
| --- | --- |
| Button | Primary / Secondary / Danger / Disabled / Loading |
| Text input | Default / Focus / Error / Disabled / có icon |
| Search box | Default / có từ khóa / không có kết quả |
| ComboBox | Closed / Open / Selected |
| DatePicker | Default / Focus / Error |
| Table | Header / Row / Row selected / Row hover / Empty |
| Card KPI | 3 màu nền như mô tả trong wireframe |
| Room card | 4 trạng thái màu × (normal / hover / selected) |
| Badge trạng thái | Trống / Đang ở / Đã đặt / Bảo trì + thanh toán |
| Sidebar item | Normal / Active / Disabled (mục bị ẩn với Staff thì không vẽ) |
| Dialog | Info / Confirm xóa / Lỗi |
| Toast | Thành công / Lỗi / Cảnh báo |
| Legend (chú thích màu) | 4 ô màu + nhãn |

### 7.3 Trang `03-Screens` và `04-Dialogs`
- Mỗi màn một section, frame đặt tên theo bảng ở mục 5.
- Kèm **annotation** (chú thích) cho các chỗ nghiệp vụ đặc biệt, ví dụ:
  - "Mục này chỉ hiển thị với Admin"
  - "Nút Xóa bị khóa khi phòng đang ở"
  - "Màu thẻ theo trạng thái — mã cố định trong Resources/Colors.xaml"
  - "Số tiền định dạng #,##0 đ"

### 7.4 Trang `05-Flows`
Vẽ 2 luồng chính dạng flow nhẹ (không cần đẹp, chỉ cần rõ):
- Luồng đặt phòng → check-in → thêm dịch vụ → check-out → thanh toán.
- Luồng đăng nhập → phân quyền → đăng xuất.

---

## 8. Những điều KHÔNG được làm

1. Không đổi 4 mã màu trạng thái phòng.
2. Không cho Staff thấy màn Báo cáo hay Loại phòng trong bất kỳ mockup nào.
3. Không vẽ biểu đồ/hiệu ứng mà LiveCharts2 hoặc WPF không làm được dễ dàng (biểu đồ 3D, animation vẽ đường, gradient phức tạp trên biểu đồ).
4. Không dùng khoảng trắng lớn kiểu landing page — app này mật độ cao.
5. Không đưa thêm tính năng không có trong tài liệu (chat, thông báo push, phân trang nâng cao, đa ngôn ngữ, avatar upload, in PDF) — nếu bạn thấy cần, hãy **hỏi trước**, đừng tự thêm vào mockup.
6. Không dùng chữ tiếng Anh trên UI. Tên biến/label kỹ thuật trong annotation thì được.
7. Không dùng placeholder kiểu "Lorem ipsum" — dùng đúng dữ liệu mẫu ở mục 9.

---

## 9. Dữ liệu mẫu (dùng chính xác trong mockup)

Dùng đúng dữ liệu này để mockup khớp với bản demo thật và để đối chiếu khi review.

**Tài khoản**
| Username | Mật khẩu | Họ tên | Vai trò |
| --- | --- | --- | --- |
| `admin` | `123456` | (đặt một tên Việt, ví dụ "Trần Minh Quản") | Administrator |
| `staff` | `123456` | (ví dụ "Lê Thu Ngân") | Staff |

**Loại phòng (4)** — ví dụ: Đơn (500.000 đ/đêm), Đôi (700.000 đ/đêm), VIP (1.200.000 đ/đêm), Gia đình (950.000 đ/đêm).

**Phòng (12)** — đánh số 101, 102, 103, 201, 202, 203, 301, 302, 303, 305, 401, 402. Phân bổ trạng thái trong mockup: **6 Trống, 2 Đang ở, 3 Đã đặt, 1 Bảo trì** (để legend trông có ý nghĩa).

**Khách hàng (5)** — gồm ít nhất: Nguyễn Văn An, CCCD `079201001234`, SĐT `0901234567`, email `an.nguyen@example.com`.

**Dịch vụ (5)** — ví dụ: Nước suối Lavie (10.000 đ), Ăn sáng buffet (50.000 đ), Giặt ủi (30.000 đ), Nước ngọt (15.000 đ), Thuê xe máy (80.000 đ).

**Kịch bản hóa đơn mẫu (dùng cho màn 11):**
- Phòng 101 (VIP), 2 đêm, đơn giá 500.000 đ → tiền phòng `1.000.000 đ`
- Dịch vụ: Nước suối Lavie ×2 = `20.000 đ`, Ăn sáng buffet ×1 = `50.000 đ` → `70.000 đ`
- **Tổng cộng: `1.070.000 đ`**, phương thức Chuyển khoản, trạng thái Đã thanh toán.

> Con số `1.070.000 đ` này xuất hiện trong tài liệu kiểm thử của dự án. Dùng đúng nó để giảng viên đối chiếu được.

---

## 10. Quy trình làm việc

Chia **3 vòng**, chi tiết ở [`DELIVERY-PLAN.md`](./DELIVERY-PLAN.md).

| Vòng | Nội dung | Mục đích |
| --- | --- | --- |
| **Vòng 1** | Styles + Components + 3 màn lõi (Đăng nhập, Khung chính, Sơ đồ phòng) | **Chốt phong cách.** Duyệt xong mới làm tiếp |
| **Vòng 2** | 5 màn danh mục + Đặt phòng + các dialog cơ bản | Nhân bản layout đã chốt |
| **Vòng 3** | Gọi dịch vụ, Hóa đơn, Xuất hóa đơn, Báo cáo + polish toàn bộ | Hoàn thiện 3 màn điểm nhấn |

**Quy tắc phản hồi:** mỗi vòng gửi **link Figma có quyền xem** (không xuất ảnh) để dev inspect được khoảng cách và mã màu.

**Câu hỏi phải hỏi trước khi vẽ**, không tự quyết:
- Có màn nào bạn thấy thiếu trong danh sách không?
- Có chỗ nào nghiệp vụ chưa rõ (ví dụ: "Khách vãng lai nhận phòng ngay" thì có cần form riêng không)?
- Có component nào bạn định dùng mà không chắc WPF-UI có sẵn?

---

## 11. Tiêu chí nghiệm thu toàn bộ

Bộ Figma được coi là **đạt** khi:

- [ ] Có đủ 13 màn + các hộp thoại, đặt tên đúng bảng ở mục 5.
- [ ] 4 mã màu trạng thái phòng khớp chính xác, xuất hiện nhất quán ở mọi màn.
- [ ] Có 2 biến thể sidebar (Admin / Staff) và phân quyền đúng.
- [ ] Mỗi màn danh mục có đủ trạng thái: bình thường, rỗng, không có kết quả, dòng bị khóa.
- [ ] Form đặt phòng có trạng thái lỗi trùng lịch và lỗi ngày.
- [ ] Hóa đơn có trạng thái chưa thanh toán / đã thanh toán / không có dịch vụ / ở đúng 1 đêm.
- [ ] Báo cáo có trạng thái có dữ liệu và không có dữ liệu.
- [ ] Toàn bộ chữ là tiếng Việt có dấu, tiền định dạng `#,##0 đ`, ngày `dd/MM/yyyy`.
- [ ] Có trang Styles + Components với đầy đủ biến thể trạng thái.
- [ ] Không có thành phần nằm ngoài danh sách "được dùng" ở mục 2.1.
- [ ] Dev đọc Figma và **không cần hỏi lại** về khoảng cách, màu, kích thước chữ.

---

## 12. Câu chốt để gửi designer

> Đây là app **desktop WPF** cho **lễ tân khách sạn**, không phải web. Mọi thứ bạn vẽ phải **code được bằng XAML trong 6 tuần với 3 sinh viên**, dùng Fluent UI có sẵn. Chúng ta ưu tiên **rõ ràng, mật độ thông tin cao, ít click** hơn là đẹp kiểu landing page.
>
> Màn quan trọng nhất là **Sơ đồ phòng** — vẽ trước, chốt trước. Có 4 màu trạng thái phòng **cố định**, không được đổi. Có **2 vai trò** với menu khác nhau. Toàn bộ UI **tiếng Việt**.
>
> Làm theo 3 vòng: vòng 1 chỉ Styles + Components + 3 màn lõi để chốt phong cách, duyệt xong mới sang vòng 2. Dùng đúng dữ liệu mẫu trong brief.
