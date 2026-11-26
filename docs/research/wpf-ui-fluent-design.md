# Nghiên Cứu Hệ Thống Thiết Kế Windows 11 Fluent 2 & Các Ứng Dụng Lớn Dùng `WPF-UI`

Tài liệu tổng hợp từ nguồn chính chủ (**Microsoft Learn — Windows 11 App Design**, mã nguồn **`lepoco/wpfui` v4.3.0**, và các ứng dụng desktop mã nguồn mở quy mô lớn dùng `WPF-UI` / Fluent 2 như **WPF-UI Gallery**, **WinUI 3 Gallery**, **Microsoft PowerToys Settings**, **Lenovo Legion Toolkit**, **Bloxstrap**, **BetterGI**), đối chiếu trực tiếp với các vấn đề hiển thị thực tế của **AURA HOTEL**.

---

## 1. Phân Tích Thực Trạng Giao Diện Hiện Tại (Từ 2 Ảnh Chụp Thực Tế)

1. **Mất tương phản bề mặt (*White-on-White Flattening*) trên `MainWindow`:**
   - Khung nội dung đang bọc toàn bộ `<ContentControl>` trong một thẻ trắng `#FFFFFF` khổng lồ chiếm ~85% màn hình. Vừa tạo cảm giác "mảng trắng chết" trống trải, vừa làm toàn bộ thẻ con bên trong (thẻ KPI, thẻ Phòng, bảng `DataGrid` vốn cũng màu `#FFFFFF`) mất sạch độ nổi khối (`#FFFFFF` đặt trên `#FFFFFF` = tương phản `0%`).
2. **Dùng sai kiến trúc `ui:NavigationView` của `WPF-UI`:**
   - Trong [`NavigationViewCompact.xaml`](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/NavigationView/NavigationViewCompact.xaml) của `WPF-UI` 4.3.0, bản thân `ui:NavigationView` **đã là khung 2 cột toàn cửa sổ** (cột 0 là thanh điều hướng, cột 1 là tấm nội dung bo góc trên-trái `CornerRadius="8,0,0,0"`, viền `BorderThickness="1,1,0,0"`). Việc ép `NavigationView` vào một cột `Width="240"` khiến tấm khung bên phải của chính `NavigationView` bị bẹp thành một vệt viền dọc thừa sát cạnh menu, trong khi cụm thông tin tài khoản (`Quản trị viên` + `Đăng xuất`) lại trôi lơ lửng ở góc trên phải thay vì neo gọn ở đáy thanh điều hướng (`PaneFooter`).
3. **Lỗi hỏng/mất icon ở mục `Đặt phòng` và `Phòng` (`SymbolRegular > 0xFFFF`):**
   - Trong [`SymbolRegular.cs`](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/SymbolRegular.cs), một số mã icon nằm ở vùng Supplementary Private Use Area-A (`>= 0xF0000`), ví dụ `Calendar24 = 0xF0285` và `Door24 = 0xF09BA`.
   - Hàm [`SymbolExtensions.GetString()`](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Extensions/SymbolExtensions.cs) của `WPF-UI` giải mã `int` 4-byte bằng `Encoding.Unicode.GetString(BitConverter.GetBytes((int)icon))` thay vì cặp UTF-16 surrogate (`char.ConvertFromUtf32`), làm `0xF0285` biến thành ký tự lạ `ʅ` (`0x0285`) và `0xF09BA` biến thành ký tự trống! Chỉ các icon thuộc mặt phẳng BMP 16-bit (`<= 0xFFFF` như `CalendarLtr24 = 0xE24F`, `Bed24 = 0xF1DA`, `Tag24 = 0xF77D`, `DataTrending24 = 0xE45E`) mới hiển thị nguyên vẹn 100%.
4. **Lỗi cắt chữ tiếng Việt trong `RolePreviewWindow`:**
   - Cửa sổ hộp thoại bị khóa cứng `Height="384"` kết hợp ép `LineHeight` cố định trên `TextBlock` và `Height="32"` trên `ui:Button`, khiến dòng thứ hai của hộp mô tả bị cắt mất phần dưới và dấu tiếng Việt kép (`ấ`, `ệ`, `ử`, `ỗ`) bị chật theo chiều dọc. Trong [`Typography.xaml`](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Resources/Typography.xaml), `WPF-UI` chủ đích **không ép `LineHeight` cứng** để DirectWrite tự giãn dòng tự nhiên cho chữ có dấu, đồng thời hộp thoại dùng `SizeToContent="Height"` và nút/ô nhập dùng `MinHeight="36"`.

---

## 2. Cách Các Ứng Dụng Lớn Dùng `WPF-UI` & Fluent 2 Thiết Kế

### 2.1. Hệ Phân Lớp Bề Mặt 3 Tầng (3-Tier Material & Elevation Ramp)
Theo chuẩn [Layering and Elevation](https://learn.microsoft.com/en-us/windows/apps/design/signature-experiences/layering), [Mica Material](https://learn.microsoft.com/en-us/windows/apps/design/style/mica) và [`Light.xaml` của `WPF-UI`](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Resources/Theme/Light.xaml):

| Tầng (Z-Order) | Vai trò | Giá trị chuẩn (Light Mode) | Áp dụng trong ứng dụng |
| --- | --- | --- | --- |
| **Layer 0 — Base Backdrop** | Nền gốc cửa sổ, `TitleBar` & thanh `NavigationView` bên trái | `Mica` (fallback đặc `#F3F3F3` `SolidBackgroundFillColorBase`) | `ui:FluentWindow` (`WindowBackdropType="Mica"`), vùng rail điều hướng trái |
| **Layer 1 — Content Sheet** | Tấm nền vùng làm việc chính (bo góc trên-trái `8,0,0,0`, viền `1,1,0,0` `#E5E5E5`) | `#F9F9F9` (`SolidBackgroundFillColorTertiary` / `LayerFillColorDefault` `#80FFFFFF`) | Khung nội dung của `NavigationView` chứa tiêu đề trang và các khối thẻ |
| **Layer 2 — Elevated Cards** | Khối nội dung nổi bật, thanh lọc, thẻ KPI, thẻ Phòng, bảng dữ liệu | `#FFFFFF` (`CardBackgroundFillColorDefault`) + viền `1px` `#E5E5E5` (`CardStrokeColorDefaultSolid`) | `ui:Card`, `ui:CardControl`, thẻ phòng, thẻ KPI, khung bọc `DataGrid`, thẻ Empty State |
| **Transient — Overlays** | Menu ngữ cảnh, hộp thoại modal, thông báo nổi | `#FCFCFC` / `#FFFFFF` + nền mờ `SmokeFillColorDefault` | `ui:ContentDialog`, modal `ui:FluentWindow`, `ui:Flyout` |

> **Nguyên tắc cốt lõi:** Không bao giờ bọc toàn bộ trang vào một hộp `#FFFFFF`. Tấm nền trang ở **Layer 1 (`#F9F9F9`)** tạo độ chênh sáng tinh tế để mọi thẻ **Layer 2 (`#FFFFFF`)** tự khắc nổi khối sắc nét với viền `1px` `#E5E5E5` mà không cần đổ bóng đen nặng nề.

### 2.2. Kiến Trúc Khung `FluentWindow` + `NavigationView` Toàn Cửa Sổ
Đối chiếu mã nguồn của [`WPF-UI Gallery`](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui.Gallery/Views/Windows/MainWindow.xaml), [`Lenovo Legion Toolkit`](https://github.com/BartoszCichecki/LenovoLegionToolkit/blob/master/LenovoLegionToolkit.WPF/Windows/MainWindow.xaml), [`Bloxstrap`](https://github.com/bloxstraplabs/bloxstrap/blob/main/Bloxstrap/UI/Elements/Settings/MainWindow.xaml), [`Microsoft PowerToys Settings`](https://github.com/microsoft/PowerToys/blob/main/src/settings-ui/Settings.UI/SettingsXAML/MainWindow.xaml) và [`NavigationView` Guidelines](https://learn.microsoft.com/en-us/windows/apps/design/controls/navigationview):

1. **Cửa sổ không viền cổ điển (`Chromeless Mica Window`):**
   - `<ui:FluentWindow ExtendsContentIntoTitleBar="True" WindowBackdropType="Mica" WindowCornerPreference="Default">` kết hợp `<ui:TitleBar>` cao `36px`–`48px` hỗ trợ Windows 11 Snap Layouts tự nhiên.
2. **Bố cục 3 vùng dọc trong thanh điều hướng trái (`240px`–`260px`):**
   - **`PaneHeader` (Trên cùng):** Biểu trưng thương hiệu gọn gàng (`Icon` + tên ứng dụng `BodyStrong` + phụ đề `Caption`).
   - **`MenuItems` (Giữa):** Các mục điều hướng cao `40px`, bo góc `4px`, vạch chỉ báo dọc `3px × 16px` màu `#005FB8` khi đang chọn. Có thể phân nhóm nghiệp vụ nhẹ nhàng (ví dụ: *Vận hành*, *Danh mục*, *Quản trị*) hoặc danh sách phẳng dễ quét.
   - **`PaneFooter` (Neo sát đáy trái):** Thẻ thông tin phiên đăng nhập (`FullName` + nhãn vai trò `Quản trị` / `Lễ tân`) kèm nút `Đăng xuất` (`SignOut24`). Neo phiên người dùng ở `PaneFooter` vừa lấp khoảng trống ở đáy sidebar, vừa giải phóng hoàn toàn góc trên-phải của vùng nội dung cho thanh công cụ và nút **Primary CTA** của từng phân hệ!
3. **Nhịp lề trang (`Page Gutters` — `32px` ngang, `24px` dọc):**
   - Theo [Windows App Silhouettes](https://learn.microsoft.com/en-us/windows/apps/design/basics/app-silhouette) và [Alignment, Margin, and Padding](https://learn.microsoft.com/en-us/windows/apps/design/layout/alignment-margin-padding), các ứng dụng nghiệp vụ dữ liệu dày dùng lề trang `24px`–`32px` trên tấm nền `#F9F9F9`:
     - Hàng đầu trang (`Header Row`, `Margin="0,0,0,20"` — hoặc bội số `4px` là `0,0,0,20`/`24`): Bên trái là `PageTitle` (`28px` SemiBold) + dòng mô tả ngữ cảnh ngắn (`CaptionTextStyle`); bên phải là cụm hành động cấp trang (tối đa **1 Primary CTA**).
     - Bên dưới là thanh công cụ tìm kiếm/lọc đặt trong thẻ `#FFFFFF` gọn (`Padding="16,12"`, `Margin="0,0,0,12"`) tách biệt rõ với thẻ bảng `DataGrid` (`#FFFFFF`, `RowHeight="40"`) hoặc lưới thẻ phòng `WrapPanel` bên dưới.

### 2.3. Thang Chữ Tiếng Việt, Chiều Cao Control & Trạng Thái Trỗng (`Empty State`)
- **Không khóa cứng `LineHeight` trên WPF `TextBlock`:** Theo [`Typography.xaml` của `WPF-UI`](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Resources/Typography.xaml) và [Typography in Windows](https://learn.microsoft.com/en-us/windows/apps/design/style/typography), chỉ quy định `FontFamily`, `FontSize`, `FontWeight` để DirectWrite tự tính chiều cao dòng chuẩn cho dấu tiếng Việt.
- **Chiều cao control tương tác `MinHeight="36"`:** Nút bấm (`ui:Button`), ô nhập (`ui:TextBox`, `ui:PasswordBox`, `ui:NumberBox`), `ComboBox` và `DatePicker` dùng `MinHeight="36"` và `Padding="12,6"` (hoặc `Padding="16,8"` cho CTA chính) thay vì ép `Height="32"`, giúp chữ tiếng Việt có dấu trên/dưới thoáng và cân tâm tuyệt đối.
- **Hộp thoại tự co giãn chiều cao (`SizeToContent="Height"`):** Mọi cửa sổ hộp thoại ngắn (`LoginWindow`, `RolePreviewWindow`, form modal) khóa `Width` cố định (`420px`–`520px`), đặt `SizeToContent="Height"` + `ResizeMode="NoResize"` để không bao giờ cắt dòng văn bản khi đổi độ phân giải hoặc tỷ lệ DPI (100% / 125% / 150%).
- **Thẻ `Empty State` có cấu trúc:** Khi phân hệ chưa có dữ liệu, thay vì chữ lơ lửng giữa mảng trắng mênh mông, hiển thị một khối thẻ trung tâm vừa vặn (`MaxWidth="440"`, nền `#FFFFFF`, viền `1px` `#E5E5E5`, bo góc `8px`, `Padding="32"`) gồm huy hiệu icon nền dịu (`48×48`, bo góc `8px`, nền `#F3F3F3`), tiêu đề `SectionTitle`, mô tả `Body` màu phụ và nút hành động gợi ý.

---

## 3. Nguồn Tham Chiếu Chính Thống

1. Microsoft Learn — [Layering and Elevation in Windows 11](https://learn.microsoft.com/en-us/windows/apps/design/signature-experiences/layering)
2. Microsoft Learn — [Mica Material Guidelines](https://learn.microsoft.com/en-us/windows/apps/design/style/mica)
3. Microsoft Learn — [Color in Windows Apps](https://learn.microsoft.com/en-us/windows/apps/design/style/color)
4. Microsoft Learn — [Windows App Silhouettes](https://learn.microsoft.com/en-us/windows/apps/design/basics/app-silhouette)
5. Microsoft Learn — [`NavigationView` Design Guidelines](https://learn.microsoft.com/en-us/windows/apps/design/controls/navigationview)
6. Microsoft Learn — [Typography in Windows (`Segoe UI Variable`)](https://learn.microsoft.com/en-us/windows/apps/design/style/typography)
7. Microsoft Learn — [ContentDialog & InfoBar Guidelines](https://learn.microsoft.com/en-us/windows/apps/design/controls/dialogs-and-flyouts/dialogs)
8. `lepoco/wpfui` — [`Wpf.Ui.Gallery MainWindow.xaml`](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui.Gallery/Views/Windows/MainWindow.xaml), [`Light.xaml`](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Resources/Theme/Light.xaml), [`NavigationViewCompact.xaml`](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/NavigationView/NavigationViewCompact.xaml), [`Typography.xaml`](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Resources/Typography.xaml), [`SymbolRegular.cs`](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/SymbolRegular.cs)
9. Ứng dụng thực tế — [`Lenovo Legion Toolkit`](https://github.com/BartoszCichecki/LenovoLegionToolkit), [`Bloxstrap`](https://github.com/bloxstraplabs/bloxstrap), [`Microsoft PowerToys Settings`](https://github.com/microsoft/PowerToys)
