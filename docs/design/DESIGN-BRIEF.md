# Design Brief — Hotel Management System (IT008)

**Dành cho**: Người thiết kế Figma
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

### 0.1 Quy tắc bám theo Windows UI Kit

Nguồn trực quan là [Windows UI Kit](https://www.figma.com/design/4wEK8iFt0i0RJIfmVYU0iO/Windows-UI-kit--Community-?node-id=2434-129659) và trang ứng dụng [Hotel Management](https://www.figma.com/design/4wEK8iFt0i0RJIfmVYU0iO/Windows-UI-kit--Community-?node-id=172994-2). Kiểm tra trực tiếp mẫu App window, Side Nav, Button, Text Box, ComboBox, Date Picker, Dialog và primitives Surface. Khi cập nhật hoặc mở rộng:

- Dùng **component instance/biến thể của kit** cho Button, Text Box, ComboBox và Date Picker khi có control tương ứng. Không vẽ lại control bằng các shape rời.
- Giữ hình dáng, chiều cao, khoảng đệm, kiểu chữ và đường viền, trạng thái focus, vô hiệu hóa và di chuột của component gốc. Button accent trong kit dùng `#005FB8`; các trạng thái lấy từ biến thể của kit, không tự bịa thêm một sắc navy hoặc màu di chuột. Tránh detach instance.
- Search box, badge, room card, bảng, shell và các hộp thoại nghiệp vụ là pattern của ứng dụng nếu kit không có đúng thành phần. Hãy ghép chúng từ component kit và ghi chú rõ phần riêng của ứng dụng.
- Các màu trạng thái phòng bên dưới là giá trị màu nghiệp vụ cố định, khác với màu nhấn Windows. Dùng đúng mã ở chú thích màu, ô màu, nhãn hoặc vạch chỉ trạng thái; giữ thân thẻ trung tính để giao diện không thành các mảng màu bão hòa. Luôn có chữ/icon đi kèm, không truyền trạng thái chỉ bằng màu.
- Nếu tên/biến thể trong kit khác cách gọi trong tài liệu, dùng biến thể thực tế của kit và ghi mapping vào Figma; không tự tạo thêm một bộ component song song.
- Kit dùng Segoe UI Variable và Segoe Fluent Icons. Dùng text styles/iconography của kit; với UI tiếng Việt hãy kiểm tra dấu và phương án thay thế theo Windows. Nếu phiên Figma không tải được font, Inter chỉ là bản xem trước tạm thời, không phải font chuẩn bàn giao.

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

Đây là những chỗ người thiết kế hay vẽ sai nếu không được cảnh báo:

1. **Phòng có 4 trạng thái, không phải 2.** Không được gộp "Đã đặt" với "Đang ở".
2. **Có 2 biến thể sidebar theo vai trò.** Staff **không thấy** mục "Loại phòng" và "Báo cáo".
3. **Đặt phòng có thể bị từ chối** vì trùng lịch → phải thiết kế **trạng thái lỗi** của form đặt phòng.
4. **Hóa đơn có 2 trạng thái**: chưa thanh toán / đã thanh toán → cần 2 diện mạo khác nhau.
5. **Tính tiền phòng tối thiểu 1 đêm** kể cả khi khách nhận và trả trong cùng ngày — chỗ này cần hiển thị rõ để lễ tân giải thích được với khách.
6. **Phòng đang có khách ở** thì không được xóa → nút Xóa phải có trạng thái bị khóa (vô hiệu hóa).
7. **Dịch vụ / loại phòng đã được dùng** thì không được xóa → cần thông báo chặn, không phải màn hình trắng.
8. **Ngày trả phòng phải sau ngày nhận phòng ít nhất 1 ngày** → validate ngày trên form đặt phòng.
9. **Hóa đơn xuất ra file `.txt`** → cần một màn preview văn bản hóa đơn dạng monospace (xem màn 11).
10. **Dữ liệu mẫu khi demo**: 12 phòng, 4 loại phòng, 5 khách hàng, 5 dịch vụ, 2 tài khoản. Vẽ đúng số lượng này trong mockup để demo trông thật.

---

## 2. Ràng buộc kỹ thuật (bắt buộc)

### 2.1 Những gì ĐƯỢC dùng

| Thành phần | Nguồn / cách dùng |
| --- | --- |
| Button | Instance và các biến thể của Windows UI Kit; map action thành Primary/Secondary/Danger/Disabled theo nhu cầu app. |
| Text Box / Password | Windows UI Kit; dùng cho nhập liệu, mật khẩu và tìm kiếm. Search có thể là riêng của ứng dụng pattern ghép từ Text Box + icon của kit. |
| ComboBox | Instance/biến thể của kit cho bộ lọc và lựa chọn. |
| Date Picker | Instance/biến thể của kit cho ngày nhận/trả và lọc báo cáo. |
| Shell / navigation | Dùng ngôn ngữ thị giác của kit; shell Admin/Staff là component riêng của app, với các mục menu theo quyền. |
| Table / DataGrid | Bảng nghiệp vụ riêng của ứng dụng theo mật độ và kiểu chữ của kit; có header, chọn dòng, di chuột, cuộn và trạng thái rỗng. |
| Room card / status badge | Pattern riêng của ứng dụng; dùng giá trị thiết kế cố định cho bốn trạng thái phòng. |
| Dialog / toast | Ưu tiên component kit nếu có; nếu không, ghép từ bề mặt, kiểu chữ, divider và Button của kit. |
| ProgressBar / chart | Chỉ dùng trên báo cáo; biểu đồ đường đơn giản. |

**Nguyên tắc nguồn**: Windows UI Kit là nguồn cho cách trình bày control cơ bản. Không mặc định kit có sẵn bảng, room card, shell khách sạn, KPI hoặc luồng nghiệp vụ; những phần đó phải được cấu tạo theo cùng hệ giá trị thiết kế và quy tắc của kit. XAML/WPF-UI là đích triển khai, nên các component Figma cần có mapping rõ sang control/style phía code.

### 2.2 Những gì KHÔNG được dùng

- ❌ Mobile/tablet hoặc breakpoint ngoài phạm vi desktop đã chốt; 1024px chỉ là kiểm tra khả năng co giãn theo wireframe.
- ❌ Dark mode/theme switcher trong bản bàn giao này (dù kit có biến thể tối).
- ❌ Animation chuyển trang phức tạp, parallax, gradient trang trí.
- ❌ Kéo-thả (drag & drop), swipe.
- ❌ Multi-window ngoài một cửa sổ chính và các dialog.
- ❌ Biểu đồ tròn 3D, gauge nhiều kim, biểu đồ nhiệt.
- ❌ Icon tự vẽ hoặc emoji; dùng glyph của Segoe Fluent Icons/kit.
- ❌ Glassmorphism hoặc blur trang trí. Mica chỉ dùng ở base/title bar nếu WPF-UI hỗ trợ; Acrylic chỉ cho flyout tạm thời nếu có sẵn trong stack. Nếu không hỗ trợ, dùng nền trung tính thay thế của kit.

### 2.3 Nguyên tắc thị giác

- **Calm và sáng**: nền app/shell dùng bề mặt trung tính kiểu Windows; bề mặt layer phân cấp nội dung bằng khoảng cách, viền nhẹ và độ nổi vừa đủ, không dùng panel màu đậm lớn.
- **Geometry theo kit**: control thường (Button, ComboBox, Text Box, ListView) radius 4px; container cửa sổ/dialog/flyout radius 8px; các cạnh ghép sát giữ radius 0px. Không dùng badge dạng viên thuốc nếu component kit không có.
- **Mật độ cao nhưng dễ quét**: dùng khoảng cách theo nhịp 4/8/12/16/24px; bảng khoảng 40px mỗi dòng; ưu tiên căn trái và nhãn viết hoa đầu câu.
- **Accent vừa đủ**: xanh Windows chỉ nhấn action chính, trạng thái bàn phím và chọn mục và liên kết. Không tô navy toàn sidebar/header, không dùng KPI card có ba nền màu riêng.
- **Trạng thái phòng là ngoại lệ nghiệp vụ**: bốn màu phải nhận ra nhanh qua chú thích màu, dấu màu và nhãn; thân thẻ, bảng và nội dung vẫn dùng nền trung tính.

### 2.4 Bảng màu và vai trò giá trị thiết kế

**Nguồn màu giao diện**: Windows UI Kit. Dùng các primitive/alias theo vai trò (App Surface, Layer, Shell Surface, màu chữ và đường viền trung tính, accent, status) thay cho bảng màu tự đặt kiểu navy. Mẫu App Window của kit dùng nền sáng trung tính, vùng điều hướng trung tính và vạch xanh ở selector; Button accent trong component set có fill `#005FB8`.

| Vai trò | Giá trị/cách chọn | Dùng ở đâu |
| --- | --- | --- |
| Nền cửa sổ | `App Surface` / `Base (Mica)` của kit; nếu không hỗ trợ thì dùng nền trung tính thay thế sáng | Nền cửa sổ và title bar |
| Nền nội dung | `Layer` hoặc `Card Surface` của kit | Vùng nội dung, form, bảng, room card |
| Neutral màu chữ | `Neutral foreground 1/2` từ kit | Chữ chính, nhãn, nội dung phụ |
| Neutral stroke | `Neutral stroke` từ kit | Viền control, divider, bảng |
| Windows accent | `#005FB8` (màu Button accent và selector trong kit) | nút chính, trạng thái bàn phím và chọn mục, progress/link; không phủ diện tích lớn |
| Success/warning/error | Semantic status colors/backgrounds của kit | Toast, validation, thanh toán; chọn nền màu nhạt và màu chữ theo cặp tương phản của kit |

**Màu trạng thái phòng (giá trị màu nghiệp vụ BẮT BUỘC, không đổi mã):**

| Trạng thái | Nhãn tiếng Việt | Mã | Ý nghĩa nghiệp vụ |
| --- | --- | --- | --- |
| Available | Trống | `#48BB78` | Sẵn sàng đón khách |
| Occupied | Đang ở | `#F56565` | Có khách đang lưu trú |
| Reserved | Đã đặt | `#ED8936` | Đã đặt trước, chưa đến |
| Maintenance | Bảo trì | `#A0AEC0` | Ngừng hoạt động |

Bốn mã trên là quy ước dữ liệu của dự án, không phải palette Windows. Giữ nguyên mã trong `Colors.xaml`; trong UI ưu tiên dùng làm ô màu chú thích màu, vạch chỉ báo, icon hoặc badge gọn có nhãn. **Không tô kín cả room card bằng các màu bão hòa**; card giữ `Card Surface` để chữ và dữ liệu vẫn rõ. Không thêm mã tint tự chọn nếu chưa ghi rõ ánh xạ giá trị thiết kế.

**Thanh toán và thông báo**: dùng màu ngữ nghĩa của kit (success/warning/danger) cùng nền tint và màu chữ tương ứng. Lỗi validation không dùng cùng một mảng đỏ lớn như trạng thái phòng Đang ở; dùng thông báo ngay dưới ô nhập, icon và viền báo lỗi.

### 2.5 Kiểu chữ

- Font chuẩn: **Segoe UI Variable** và icon **Segoe Fluent Icons**, theo Windows UI Kit; kiểm tra đầy đủ dấu tiếng Việt. Dùng một họ font xuyên suốt.
- Theo Windows type ramp: Small 12/16 Regular; Text 14/20 Regular; Text Strong 14/20 Semibold; Body Large 18/24; Subtitle 20/28 Semibold; Title 28/36 Semibold. Dùng 40/52 chỉ khi thực sự cần headline; KPI không tự động thành display text lớn.
- Dùng Regular cho đa số nội dung và Semibold cho tiêu đề; căn trái mặc định, viết hoa đầu câu, không dùng toàn chữ hoa.
- Số tiền có thể dùng tabular numerals nếu style trong kit hỗ trợ; ưu tiên khả năng đọc và cột thẳng hàng.

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
│              │  Header: [Tiêu đề trang]  [Tên / Role ▾] │
│   Sidebar    │                            [Menu tài khoản]     │
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

**Kích thước theo Figma**: title bar 48px; vùng pane trái 240px, mục điều hướng 208×40px có icon và nhóm; vùng nội dung bắt đầu tại (240,132), gutter phải/dưới 24px. Title bar hệ thống và header nội dung là hai lớp riêng, không gộp thành một thanh navy.

**Menu sidebar — Admin (8 mục):** Sơ đồ phòng, Đặt phòng, Phòng, Loại phòng, Khách hàng, Dịch vụ, Hóa đơn, Báo cáo.
**Menu sidebar — Staff (6 mục):** bỏ **Loại phòng** và **Báo cáo**.

> Ghi chú về nguồn: TASK-01 liệt kê menu "Sơ đồ phòng, Đặt phòng, Phòng, Khách hàng, Dịch vụ, Hóa đơn, Báo cáo" (7 mục, chưa có Loại phòng); TASK-04 và TASK-03 yêu cầu mục "Quản lý loại phòng" chỉ Admin thấy. Gộp lại thành 8 mục cho Admin. Nếu bạn thấy 8 mục quá dài, được phép gộp "Phòng" + "Loại phòng" thành một màn có 2 tab — nhưng phải giữ đúng quyền ẩn với Staff.

**Header nội dung**: tiêu đề 28px tại (256,72), cụm tài khoản 232×40 tại (1024,66) gồm icon, họ tên, vai trò và chevron mở menu Đăng xuất. Không lặp tiêu đề trong module. Thương hiệu chuẩn là AURA HOTEL; dùng logo gốc xuất từ Figma. Xem thông số đầy đủ tại `WIREFRAMES.md` mục 1–2.

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

Người thiết kế thường chỉ vẽ trạng thái "đẹp" (có dữ liệu đầy đủ). Đồ án này cần cả trạng thái xấu — vì giảng viên sẽ bấm thử.

### 6.1 Màn danh mục (Phòng / Loại phòng / Khách hàng / Dịch vụ / Booking)
- **Bình thường**: có ~10 dòng dữ liệu, có ít nhất 1 dòng được chọn.
- **Rỗng**: "Chưa có dữ liệu" + nút gọi hành động (Thêm mới). Không phải bảng trắng trơn.
- **Đang tìm kiếm, không có kết quả**: "Không tìm thấy khách hàng phù hợp với '079'". Gợi ý xóa từ khóa.
- **Đang lọc, không có kết quả**: "Không có phòng nào ở trạng thái Bảo trì".
- **Dòng bị khóa thao tác**: nút Xóa vô hiệu hóa + tooltip giải thích lý do (vd: "Phòng đang có khách ở").
- **Lỗi validate trên form**: lỗi hiển thị **ngay dưới ô nhập**, không phải chỉ popup.

### 6.2 Sơ đồ phòng
- Đủ 4 màu cùng lúc (12 phòng).
- Hover + được chọn bằng bàn phím một thẻ phòng.
- Tooltip khi di chuột gợi ý "Click đúp để đặt phòng / check-out".
- Cửa sổ hẹp (khoảng 1024px): thẻ tự xuống dòng, không tràn ngang.
- Trạng thái rỗng: "Chưa có phòng nào" (trường hợp database trống).

### 6.3 Hóa đơn
- Khách có dùng dịch vụ (bảng kê 2–3 dòng).
- Khách không dùng dịch vụ (tiền dịch vụ `0 đ`).
- Ở đúng 1 đêm (nhận và trả trong cùng ngày) → hiển thị "1 đêm" chứ không phải "0".
- Đã thanh toán → các nút thanh toán chuyển vô hiệu hóa, có badge "ĐÃ THANH TOÁN".
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
- Ghi rõ Windows UI Kit là nguồn control cơ bản; các giá trị thiết kế dưới đây là lớp sản phẩm áp trên kit, không phải một bộ UI kit mới.
- Bảng màu (brand + 4 trạng thái + ngữ nghĩa), có mã hex ghi chú bên cạnh.
- Typography scale với tên style: `Display/KPI`, `Heading/Page`, `Heading/Section`, `Body`, `Body/Small`, `Label`.
- Spacing scale: 4 / 8 / 12 / 16 / 24 / 32.
- Bán kính góc, độ dày viền, độ đổ bóng.

### 7.2 Trang `02-Components`
Dùng instance gốc của Windows UI Kit cho control cơ bản; tạo component riêng chỉ cho mẫu giao diện nghiệp vụ. Mỗi component phải có **các biến thể trạng thái**:

| Component | Biến thể |
| --- | --- |
| Button | Dùng các biến thể sẵn có của kit cho kiểu nút và trạng thái; Loading là trạng thái riêng của ứng dụng nếu kit không có biến thể tương ứng. |
| Text input | Dùng các biến thể sẵn có của kit cho Default / Focus / Error / Disabled; icon chỉ thêm theo pattern của kit. |
| Search box | Default / có từ khóa / không có kết quả |
| ComboBox | Closed / Open / Selected |
| DatePicker | Default / Focus / Error |
| Table | Header / Row / Row được chọn / Row di chuột / Empty |
| Card KPI | Cùng bề mặt thẻ (Card Surface); hierarchy bằng type và khoảng cách; điểm nhấn nhỏ nếu cần, không tô ba nền màu |
| Room card | Neutral bề mặt × (normal / di chuột / được chọn); 4 status dấu chỉ trạng thái hoặc nhãn màu có nhãn |
| Badge trạng thái | Trống / Đang ở / Đã đặt / Bảo trì + thanh toán; màu semantic/room indicator kèm nhãn, dùng quy tắc bo góc theo kit |
| Sidebar item | Normal / Hover / Active theo Side Nav kit (nền trung tính + accent selector) / Disabled (mục ẩn với Staff thì không vẽ) |
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

**Phòng (12)** — đánh số 101, 102, 103, 201, 202, 203, 301, 302, 303, 305, 401, 402. Phân bổ trạng thái trong mockup: **6 Trống, 2 Đang ở, 3 Đã đặt, 1 Bảo trì** (để chú thích màu trông có ý nghĩa).

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
| **Vòng 2** | 5 màn danh mục + Đặt phòng + các dialog cơ bản | Nhân bản bố cục đã chốt |
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
- [ ] Có trang Styles + Components với đầy đủ biến thể trạng thái; Button, Text Box, ComboBox và Date Picker bám instance/biến thể gốc của Windows UI Kit, không bị vẽ lại hoặc lệch style.
- [ ] Không còn màu navy `#1E3A5F`, nền sidebar đậm hoặc KPI card phủ màu; dùng màu nhấn `#005FB8` và nền trung tính theo kit.
- [ ] Font, viết hoa đầu câu, type ramp, radius 4/8px, được chọn/được chọn bằng bàn phím state và bề mặt độ nổi được đối chiếu với kit.
- [ ] Không có thành phần nằm ngoài danh sách "được dùng" ở mục 2.1.
- [ ] Các control có trong Windows UI Kit dùng đúng instance/biến thể; mẫu giao diện riêng của ứng dụng được ghi nhãn và ghép từ giá trị thiết kế/control của kit.
- [ ] Dev đọc Figma và **không cần hỏi lại** về khoảng cách, màu, kích thước chữ.

---

## 12. Câu chốt để gửi người thiết kế

> Đây là app **desktop WPF** cho **lễ tân khách sạn**, không phải web. Dùng Windows UI Kit làm nguồn giao diện: nền trung tính sáng, Segoe UI Variable, control chuẩn, radius theo quy tắc bo góc Windows và màu nhấn `#005FB8` dùng tiết chế. Không dùng sidebar navy hoặc thẻ KPI tô màu lớn. Các màu phòng cố định chỉ làm dấu màu và nhãn trạng thái trên thân thẻ neutral. Thiết kế phải code được bằng XAML/WPF-UI trong 6 tuần, đồng thời ưu tiên đọc nhanh, mật độ cao và ít click.
>
> Màn quan trọng nhất là **Sơ đồ phòng** — vẽ trước, chốt trước. Có 4 màu trạng thái phòng **cố định**, không được đổi. Có **2 vai trò** với menu khác nhau. Toàn bộ UI **tiếng Việt**.
>
> Làm theo 3 vòng: vòng 1 chỉ Styles + Components + 3 màn lõi để chốt phong cách, duyệt xong mới sang vòng 2. Dùng đúng dữ liệu mẫu trong brief.


## 13. Nguồn tham chiếu phong cách Windows

- [Windows UI Kit — Figma](https://www.figma.com/design/4wEK8iFt0i0RJIfmVYU0iO/Windows-UI-kit--Community-?node-id=2434-129659) (nguồn component/biến thể và mẫu App Window).
- [Windows 11 design principles](https://learn.microsoft.com/en-us/windows/apps/design/design-principles) (calm bề mặts, color hierarchy, layering, quy tắc bo góc, kiểu chữ).
- [Windows quy tắc bo góc](https://learn.microsoft.com/en-us/windows/apps/design/signature-experiences/quy tắc bo góc) (8px top-level/overlay, 4px controls, 0px edges that meet).
- [Windows kiểu chữ](https://learn.microsoft.com/en-us/windows/apps/design/signature-experiences/kiểu chữ) (Segoe UI Variable và type ramp).
- [Fluent color giá trị thiết kếs](https://fluent2.microsoft.design/color-giá trị thiết kếs) (neutral, brand và semantic status aliases).
