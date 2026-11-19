# Wireframes — Hotel Management System (IT008)

**Kèm theo**: [`DESIGN-BRIEF.md`](./DESIGN-BRIEF.md)
**Mục đích**: khoá bố cục sơ bộ trước khi người thiết kế vẽ Figma, và để dev dùng làm chuẩn bố cục khi code XAML.

> Wireframe dưới đây **không phải thiết kế cuối**. Khi dựng Figma, lấy component/biến thể gốc trong [Windows UI Kit — Hotel Management](https://www.figma.com/design/4wEK8iFt0i0RJIfmVYU0iO/Windows-UI-kit--Community-?node-id=172994-2) cho Button, Text Box, ComboBox và Date Picker. Giữ hình dáng, padding, kiểu chữ, viền và trạng thái của kit; không dựng control tương đương bằng shape rời. Có thể sắp xếp lại bố cục nếu không bỏ trạng thái/nghiệp vụ đã ghi.

**Quy tắc ghép màn**: shell, bảng, room card, badge, KPI và dialog nghiệp vụ là mẫu giao diện riêng của ứng dụng nếu kit không có component tương ứng. Ghép theo App Surface/Layer, màu chữ và đường viền trung tính và màu nhấn `#005FB8`; giữ bốn màu trạng thái phòng ở dấu màu và nhãn trạng thái, không tô kín room card. Dùng Segoe UI Variable và Segoe Fluent Icons theo kit; Inter chỉ là phương án thay thế xem trước tạm thời nếu font không tải trong phiên Figma.

Ký hiệu: `[ ]` nút/ô lớn · `( )` combobox · `< >` ô nhập · `[*]` bắt buộc · `⚠` trạng thái lỗi · `🔒` chỉ Admin

---

## 0. Quy ước áp dụng Windows UI Kit

- Dùng instance/biến thể của kit cho Button, Text Box, Password Box, ComboBox, Date Picker, Dialog, Side Nav và icon khi control đó có sẵn; tránh detach/dựng lại bằng shape rời.
- Button accent của kit là xanh `#005FB8`; giữ biến thể trạng thái của kit. Không dùng navy cho sidebar, header, nút hoặc KPI.
- Dùng nền App Surface/Layer sáng, màu chữ và đường viền trung tính và phân cấp bằng khoảng cách/độ nổi nhẹ. Item điều hướng đang chọn có nền trung tính, chữ và biểu tượng màu trung tính và vạch xanh; không đảo cả hàng thành nền navy.
- Chọn đúng trạng thái mặc định, focus, lỗi, vô hiệu hóa, mở và được chọn; chỉ thể hiện trạng thái focus khi cần minh họa, không để các control mẫu vô tình ở trạng thái focus.
- Room card, badge, bảng, KPI và các nội dung nghiệp vụ có thể riêng của ứng dụng; giữ phong cách của kit và ghi rõ cách ghép.
- Các màu phòng theo brief vẫn là mã cố định, nhưng dùng như ô màu, vạch, icon hoặc badge gọn đi kèm nhãn; thân thẻ dùng nền trung tính. Không dùng màu làm tín hiệu duy nhất.
- Geometry tham chiếu: control 4px, dialog/flyout/container cấp cao 8px, cạnh ghép sát 0px. Button và Side Nav trong kit có biến thể cao khoảng 40px; chọn kích thước thực tế theo component thay vì tự ép mọi control bằng nhau.
- Không suy ra mọi thành phần trong wireframe đều là component dựng sẵn của kit; đánh dấu `App-specific` phần tự cấu tạo.

---

## 1. `LoginWindow` — Đăng nhập

Nguồn chuẩn: Figma `173024:783` (LoginWindow / Bình thường), kiểm tra ngày 19/11/2026.

- Cửa sổ 960×524, title bar 48px, hai cột 344px / 616px.
- Cột giới thiệu: logo AURA HOTEL gốc tại x=53, y=168 của cửa sổ, slot 172×48; dòng “Chào mừng / trở lại”, phụ đề “Trang Quản Trị Hệ Thống”.
- Form: lề trái 56px trong cột phải, tiêu đề 28px; nhãn 14px và ô nhập cao 32px; trường Tên đăng nhập và Mật khẩu, action ở cuối bên phải.
- TASK-01 dùng cửa sổ xem trước 680×360 với hai nút Lễ tân và Quản trị. Form đăng nhập, lỗi inline, trạng thái đang kiểm tra và hỗ trợ Enter được triển khai ở TASK-03.
- Giữ các trạng thái lỗi bỏ trống, sai tài khoản và đang kiểm tra theo các frame cùng tên trong Figma.

## 2. `MainWindow-Shell` — Khung chính

Nguồn chuẩn: Figma `173024:784` (Admin), `173024:785` (Staff), kiểm tra ngày 19/11/2026.

- Cửa sổ 1280×800, tối thiểu 1280×720; title bar 48px.
- Pane trái chiếm vùng 240px. Logo gốc AURA HOTEL đặt tại (20,62), slot 200×48.
- Mục điều hướng 208×40, lề trái 12px; có icon Fluent, nền trung tính, trạng thái chọn có dấu accent. Chia nhóm **VẬN HÀNH**, **DANH MỤC**, **BÁO CÁO**. Staff ẩn Loại phòng, Báo cáo và tiêu đề nhóm Báo cáo rỗng.
- Tiêu đề trang tại (256,72), 28px / line-height 36px. Chỉ có một tiêu đề, không lặp trong nội dung placeholder.
- Cụm tài khoản tại (1024,66), slot 232×40: icon, họ tên, vai trò và chevron. Bấm mở menu có Đăng xuất; không đặt chuỗi chào dài bên cạnh nút riêng.
- Vùng nội dung tại (240,132), kích thước 1016×644 ở khung 1280×800; co chiều cao khi xuống 720px. Gutter bên phải và dưới 24px; nội dung module có padding 24px.
- Admin có 8 mục; Staff 6 mục. Vận hành: Sơ đồ phòng, Đặt phòng. Danh mục: Phòng, Loại phòng, Khách hàng, Dịch vụ, Hóa đơn. Báo cáo: Báo cáo.

## 3. `RoomMapView` — Sơ đồ phòng ⭐

Màn trung tâm của app. Phía trên lưới có tiêu đề, thời gian cập nhật, nút Làm mới, bộ lọc và chú thích bốn trạng thái. Lưới bên dưới minh họa một phần trong 12 phòng.

```
┌─────────────────────┬─────────────────────┬─────────────────────┬─────────────────────┐
│ 101                 │ 102                 │ 103                 │ 201                 │
│ VIP                 │ Đơn                 │ Đơn                 │ Đôi                 │
│ ● Trống             │ ● Đang ở            │ ● Đã đặt            │ ● Trống             │
│ 500.000 đ           │ Nguyễn Văn An       │ 15/10–18/10         │ 700.000 đ           │
├─────────────────────┼─────────────────────┼─────────────────────┼─────────────────────┤
│ 202 ...             │ 203 ...             │ 301 ...             │ 302 ...             │
└─────────────────────┴─────────────────────┴─────────────────────┴─────────────────────┘
```

**Mỗi thẻ phòng bắt buộc có**: số phòng, loại phòng, trạng thái bằng chữ và **dấu màu theo trạng thái**. Thân thẻ dùng nền trung tính; mã trạng thái nguyên bản dùng cho ô màu chú thích màu và một vạch/icon/badge nhỏ để giữ màn bình tĩnh, dễ đọc.

Nội dung phụ gợi ý theo trạng thái (giúp lễ tân ra quyết định):
- Trống: đơn giá/đêm.
- Đang ở: tên khách + ngày trả phòng dự kiến.
- Đã đặt: tên khách + khoảng ngày đặt.
- Bảo trì: lý do (nếu có).

**Trạng thái cần vẽ**
- Đủ 4 màu cùng màn.
- Hover thẻ + tooltip: "Click đúp để đặt phòng" (Trống) / "Click đúp để check-out hoặc gọi dịch vụ" (Đang ở). Đã đặt → "Click đúp để check-in".
- Thẻ đang được chọn (click 1 lần) — nếu bạn vẽ panel chi tiết bên phải thì chỉ rõ.
- Stress test 1024px: thẻ xuống dòng, **không có thanh cuộn ngang**. Đây là kiểm tra co giãn tham khảo; mốc hỗ trợ tối thiểu vẫn là 1280×720 theo brief.
- Rỗng: "Chưa có phòng nào".

**Ghi chú nghiệp vụ (annotation bắt buộc)**
- "Dấu trạng thái dùng đúng mã: `#48BB78`, `#F56565`, `#ED8936`, `#A0AEC0` — dev bind theo `Status`; nền thẻ vẫn là nền trung tính."
- "Legend dùng lại đúng 4 mã màu đó."
- "Thẻ phải co giãn tốt, khối lượng phòng có thể thay đổi."

---

## 4. `RoomView` — Quản lý phòng

```
┌───────────────────────────────────────────────────────────────────────────────┐
│ Quản lý phòng                                                                 │
├───────────────────────────────────────────────────────────────────────────────┤
│ ( Tất cả loại ▾ )  ( Tất cả trạng thái ▾ )     [ + Thêm phòng ]               │
│ [ Quản lý loại phòng ]🔒                                                      │
├───────────────────────────────────────────────────────────────────────────────┤
│ Mã  |  Số phòng  |  Loại phòng  |  Đơn giá/ngày  |  Trạng thái  |  Thao tác   │
├───────────────────────────────────────────────────────────────────────────────┤
│ 1   |  101       |  VIP         |  1.200.000 đ   |  ■ Trống     |  Sửa  Xóa   │
│ 2   |  102       |  Đơn         |  500.000 đ     |  ■ Đang ở    |  Sửa  🔒Xóa │
│ 3   |  103       |  Đơn         |  500.000 đ     |  ■ Đã đặt    |  Sửa  Xóa   │
│ 4   |  305       |  VIP         |  1.200.000 đ   |  ■ Bảo trì   |  Sửa  Xóa   │
├───────────────────────────────────────────────────────────────────────────────┤
│ Hiển thị 4 / 12 phòng                          [ < ] [ > ]                    │
└───────────────────────────────────────────────────────────────────────────────┘
```

**Trạng thái cần vẽ**
- Bình thường + 1 dòng đang chọn.
- Lọc theo "Bảo trì" → chỉ 1 dòng.
- Không có kết quả lọc: "Không có phòng nào ở trạng thái Bảo trì".
- Dòng "Đang ở": nút **Xóa vô hiệu hóa** + tooltip "Không thể xóa phòng đang có khách ở".
- Nút "Quản lý loại phòng" chỉ hiện với Admin.

**Dialog: `RoomDialog` (Thêm / Sửa phòng)**
```
┌─────────────────────────────────────────┐
│ Thêm phòng mới                      [x] │
├─────────────────────────────────────────┤
│ Số phòng *                              │
│ < 305                                 > │
│ ⚠ Số phòng không được để trống          │
├─────────────────────────────────────────┤
│ Loại phòng *                            │
│ ( Chọn loại phòng ▾ )                   │
├─────────────────────────────────────────┤
│ Trạng thái ban đầu                      │
│ ( Trống ▾ )                             │
├─────────────────────────────────────────┤
│ [ Hủy ]  [ Lưu ]                        │
└─────────────────────────────────────────┘
```
Trạng thái: bình thường / lỗi để trống / lỗi trùng số phòng ("Số phòng 101 đã tồn tại").

---

## 5. `RoomTypeView` — Loại phòng 🔒

```
┌────────────────────────────────────────────────────────────────────┐
│ Quản lý loại phòng                                                 │
├────────────────────────────────────────────────────────────────────┤
│ [ + Thêm loại phòng ]                                              │
├────────────────────────────────────────────────────────────────────┤
│ Mã  |  Tên loại phòng  |  Đơn giá/ngày  |  Số phòng  |  Thao tác   │
├────────────────────────────────────────────────────────────────────┤
│ 1   |  Đơn             |  500.000 đ     |  4 phòng   |  Sửa  Xóa   │
│ 2   |  Đôi             |  700.000 đ     |  3 phòng   |  Sửa  Xóa   │
│ 3   |  VIP             |  1.200.000 đ   |  3 phòng   |  Sửa  🔒Xóa │
└────────────────────────────────────────────────────────────────────┘
```
- Cột "Số phòng" (số phòng đang thuộc loại này) giúp giải thích vì sao Xóa bị khóa.
- Xóa loại đang có phòng → dialog cảnh báo: "Không thể xóa loại phòng VIP vì đang có 3 phòng sử dụng. Vui lòng chuyển các phòng sang loại khác trước."
- Lỗi validate: đơn giá phải `> 0`, không được âm hoặc chữ.

---

## 6. `CustomerView` — Quản lý khách hàng

```
┌──────────────────────────────────────────────────────────────────────────────┐
│ Quản lý khách hàng                                                           │
├──────────────────────────────────────────────────────────────────────────────┤
│ 🔍 < Tìm theo họ tên, CCCD, SĐT...  >        [ + Thêm khách hàng ]           │
├──────────────────────────────────────────────────────────────────────────────┤
│ Mã  |  Họ và tên      |  CCCD          |  SĐT         |  Email        |  ... │
├──────────────────────────────────────────────────────────────────────────────┤
│ 1   |  Nguyễn Văn An  |  079201001234  |  0901234567  |  an@ex.com    |  Sửa │
│ 2   |  Trần Thị Bích  |  079201005678  |  0912345678  |  bich@ex.com  |  Sửa │
└──────────────────────────────────────────────────────────────────────────────┘
```

**Trạng thái cần vẽ**
- Tìm kiếm gõ dần (real-time) — vẽ lúc đã gõ `079` và kết quả còn 1 dòng.
- Không tìm thấy: "Không tìm thấy khách hàng phù hợp với '079'" + nút "Xóa từ khóa".
- Danh sách rỗng (chưa có ai): "Chưa có khách hàng nào" + [Thêm khách hàng đầu tiên].

**Dialog `CustomerDialog`** — lỗi validation inline:
- Họ và tên * → bắt buộc.
- CCCD * → 9 hoặc 12 chữ số: "CCCD phải là 9 hoặc 12 chữ số".
- Số điện thoại * → 10 số, bắt đầu bằng 0: "Số điện thoại phải có 10 chữ số và bắt đầu bằng 0".
- Email → không bắt buộc, kiểm tra định dạng.
- Trùng CCCD: "CCCD này đã được đăng ký cho khách hàng Trần Thị Bích".

---

## 7. `ServiceView` — Quản lý dịch vụ

```
┌───────────────────────────────────────────────────────┐
│ Quản lý dịch vụ                                       │
├───────────────────────────────────────────────────────┤
│ [ + Thêm dịch vụ ]                                    │
├───────────────────────────────────────────────────────┤
│ Mã DV  |  Tên dịch vụ      |  Đơn giá   |  Thao tác   │
├───────────────────────────────────────────────────────┤
│ DV01   |  Nước suối Lavie  |  10.000 đ  |  Sửa  Xóa   │
│ DV02   |  Ăn sáng buffet   |  50.000 đ  |  Sửa  Xóa   │
│ DV03   |  Giặt ủi          |  30.000 đ  |  Sửa  🔒Xóa │
└───────────────────────────────────────────────────────┘
```
- Xóa dịch vụ đã từng dùng → dialog: "Dịch vụ Giặt ủi đã được sử dụng trong các đặt phòng trước đây, không thể xóa."
- Đơn giá `>= 0`; lỗi khi nhập chữ hoặc số âm.
- Định dạng tiền đúng `#,##0 đ`.

---

## 8. `BookingView` / `BookingDialog` — Đặt phòng ⭐

Có thể là màn riêng trong sidebar, hoặc dialog mở từ Sơ đồ phòng. Cần **cả hai** dạng hoặc chọn một và nói rõ. Đề xuất: màn riêng + dialog khi mở từ thẻ phòng.

```
┌───────────────────────────────────────────────────────────────┐
│ Đặt phòng mới                                                 │
├───────────────────────────────────────────────────────────────┤
│ Khách hàng *                                                  │
│ 🔍 < Tìm khách theo tên/CCCD >   hoặc   ( Chọn khách hàng ▾ ) │
│ → Đã chọn: Nguyễn Văn An · 079201001234 · 0901234567          │
├───────────────────────────────────────────────────────────────┤
│ Phòng *                                                       │
│ ( 101 — VIP — Trống ▾ )                                       │
├───────────────────────────────────────────────────────────────┤
│ Ngày nhận phòng *        Ngày trả phòng *                     │
│ [ 15/10/2026 ▾ ]         [ 18/10/2026 ▾ ]                     │
├───────────────────────────────────────────────────────────────┤
│ Tạm tính                                                      │
│ 3 đêm × 500.000 đ                           1.500.000 đ       │
│ Tổng tạm tính                               1.500.000 đ       │
├───────────────────────────────────────────────────────────────┤
│ [ Hủy ]        [ Xác nhận đặt phòng ]                         │
└───────────────────────────────────────────────────────────────┘
```

**Trạng thái cần vẽ (quan trọng)**
- Bình thường + có tạm tính.
- ⚠ **Lỗi trùng lịch** — khối cảnh báo đỏ nổi bật phía trên nút:
  > "Phòng này đã có người đặt trong khoảng thời gian đã chọn! Vui lòng chọn khoảng ngày khác hoặc phòng khác."
  > Kèm chi tiết: "Phòng 101 đã được đặt từ 15/10/2026 đến 18/10/2026."
- ⚠ **Lỗi ngày**: "Ngày trả phòng phải sau ngày nhận phòng ít nhất 1 ngày".
- ⚠ Cảnh báo vàng: "Ngày nhận phòng đang ở quá khứ" (không chặn).
- Trạng thái khách chưa chọn / phòng chưa chọn.

**Ghi chú nghiệp vụ**
- Đặt liền kề hợp lệ: khách A trả 15/10, khách B nhận 15/10 **vẫn hợp lệ**. Đừng vẽ UI gợi ý là trùng.
- Sau khi đặt thành công, trạng thái phòng chuyển **Đã đặt** (cam).
- Mã đặt phòng hiển thị dạng `B-001` chỉ là **gợi ý** — tài liệu chưa quy định định dạng mã này, dev thống nhất sau.

---

## 9. `BookingList` — Danh sách đặt phòng & Check-in

```
┌─────────────────────────────────────────────────────────────────────────────────────┐
│ Đặt phòng                                                                           │
├─────────────────────────────────────────────────────────────────────────────────────┤
│ ( Tất cả trạng thái ▾ )   ( Tất cả phòng ▾ )    [ + Đặt phòng ]                     │
├─────────────────────────────────────────────────────────────────────────────────────┤
│ Mã BK  |  Khách hàng     |  Phòng  |  Ngày   |  Trạng thái    |  Thao tác           │
├─────────────────────────────────────────────────────────────────────────────────────┤
│ B-001  |  Nguyễn Văn An  |  103    |  15→18  |  ■ Đã đặt      |  [Check-in]         │
│ B-002  |  Trần Thị Bích  |  102    |  12→14  |  ■ Đang ở      |  Dịch vụ  Check-out │
│ B-003  |  Lê Văn Cường   |  201    |  01→03  |  ■ Hoàn thành  |  (xem hóa đơn)      │
│ B-004  |  Phạm Thu Hà    |  302    |  05→07  |  ■ Đã hủy      |  —                  │
└─────────────────────────────────────────────────────────────────────────────────────┘
```

- Nút hành động **phụ thuộc trạng thái** booking:
  - Đã đặt → Check-in / Hủy
  - Đang ở → Thêm dịch vụ / Check-out
  - Hoàn thành → Xem hóa đơn
  - Đã hủy → không có hành động
- Check-in cần dialog xác nhận.
- Trạng thái rỗng: "Chưa có đặt phòng nào trong hệ thống."

---

## 10. `AddServiceDialog` — Gọi dịch vụ phòng

```
┌─────────────────────────────────────────────────────────────────┐
│ Dịch vụ phòng 102 — Trần Thị Bích                       [x]     │
├─────────────────────────────────────────────────────────────────┤
│ Chọn dịch vụ                                                    │
│ ( Nước suối Lavie ▾ )      Số lượng  < 2 >                      │
│ Đơn giá 10.000 đ             Thành tiền  20.000 đ               │
│ [ + Thêm vào ]                                                  │
├─────────────────────────────────────────────────────────────────┤
│ Dịch vụ đã sử dụng                                              │
│ Tên dịch vụ      |  SL  |  Đơn giá   |  Thành tiền  |  Thao tác │
│ Nước suối Lavie  |  2   |  10.000 đ  |  20.000 đ    |  Xóa      │
│ Giặt ủi          |  1   |  30.000 đ  |  30.000 đ    |  Xóa      │
├─────────────────────────────────────────────────────────────────┤
│ Tổng tiền dịch vụ                            50.000 đ           │
│ [ Đóng ] [ Lưu ]                                                │
└─────────────────────────────────────────────────────────────────┘
```

**Trạng thái**
- Bình thường có 2 dòng.
- Rỗng: "Phòng này chưa sử dụng dịch vụ nào."
- ⚠ Số lượng `0` hoặc chữ: "Số lượng phải là số nguyên lớn hơn 0".
- Sau khi xóa 1 dòng: tổng tiền cập nhật ngay (thể hiện bằng annotation).
- **Không cho mở** dialog này với phòng Trống / Bảo trì — nếu bạn vẽ màn chọn phòng trước thì ghi rõ các phòng không hợp lệ bị vô hiệu hóa.

---

## 11. `InvoiceView` — Hóa đơn & thanh toán ⭐

```
┌───────────────────────────────────────────────────────────────────┐
│ Hóa đơn thanh toán          HD-001            [ Chưa thanh toán ] │
├───────────────────────────────────────────────────────────────────┤
│ Khách hàng: Nguyễn Văn An · CCCD 079201001234                     │
│ Phòng: 101 (VIP)             Mã đặt phòng: B-001                  │
│ Nhận phòng: 12/10/2026   →   Trả phòng: 14/10/2026   (2 đêm)      │
├───────────────────────────────────────────────────────────────────┤
│ Chi tiết dịch vụ                                                  │
│ Tên dịch vụ      |  SL  |  Đơn giá   |  Thành tiền                │
│ Nước suối Lavie  |  2   |  10.000 đ  |  20.000 đ                  │
│ Ăn sáng buffet   |  1   |  50.000 đ  |  50.000 đ                  │
├───────────────────────────────────────────────────────────────────┤
│ Tiền phòng   2 đêm × 500.000 đ                 1.000.000 đ        │
│ Tiền dịch vụ                                      70.000 đ        │
├───────────────────────────────────────────────────────────────────┤
│ TỔNG CỘNG THANH TOÁN                           1.070.000 đ        │
├───────────────────────────────────────────────────────────────────┤
│ Phương thức thanh toán                                            │
│ ( • ) Tiền mặt     (   ) Chuyển khoản                             │
├───────────────────────────────────────────────────────────────────┤
│ [ Xác nhận thanh toán ]        [ In / Xuất hóa đơn ]              │
└───────────────────────────────────────────────────────────────────┘
```

**Trạng thái cần vẽ**
1. **Chưa thanh toán** (như trên): badge cam nhạt, nút thanh toán active.
2. **Đã thanh toán**: badge xanh lá "ĐÃ THANH TOÁN", hiện "Phương thức: Chuyển khoản", "Ngày thanh toán: 14/10/2026 14:32", nút thanh toán **vô hiệu hóa**, nút xuất hóa đơn vẫn active.
3. **Không có dịch vụ**: bảng kê hiển thị dòng "Không sử dụng dịch vụ", tiền dịch vụ `0 đ`, tổng = tiền phòng.
4. **Ở đúng 1 đêm** (nhận và trả cùng ngày): hiển thị "1 đêm" và tính 1 × đơn giá — **không được hiển thị 0 đêm hoặc 0 đ**.
5. ⚠ Lỗi: bấm thanh toán khi chưa chọn phương thức → "Vui lòng chọn phương thức thanh toán".

**Dialog xác nhận thanh toán**: "Xác nhận thanh toán 1.070.000 đ bằng Chuyển khoản? Sau khi xác nhận, phòng 101 sẽ trở về trạng thái Trống." → [Hủy] [Xác nhận].

---

## 12. `InvoicePreview` — Xem trước khi xuất `.txt`

```
┌─────────────────────────────────────────────────────────────┐
│ Xuất hóa đơn — HD-001                                   [x] │
├─────────────────────────────────────────────────────────────┤
│ =========================================================   │
│ AURA HOTEL                                      │
│ HÓA ĐƠN THANH TOÁN (RECEIPT)                                │
│ Mã HĐ: HD-001              Ngày lập: 14/10/2026             │
│ Khách hàng: Nguyễn Văn An  Phòng: 101 (VIP)                 │
│ Thời gian: 12/10/2026 -> 14/10/2026 (2 ngày)                │
│ -------------------------------------------------------     │
│ Tiền phòng: 2 ngày x 500,000 đ = 1,000,000 đ                │
├─────────────────────────────────────────────────────────────┤
│ - Nước suối Lavie (x2): 20,000 đ                            │
│ - Ăn sáng buffet (x1): 50,000 đ                             │
│ Tổng tiền dịch vụ: 70,000 đ                                 │
│ -------------------------------------------------------     │
│ TỔNG CỘNG THANH TOÁN: 1,070,000 đ                           │
│ Phương thức: Chuyển khoản                                   │
│ Trạng thái: ĐÃ THANH TOÁN                                   │
│ =========================================================   │
│ CẢM ƠN QUÝ KHÁCH & HẸN GẶP LẠI!                             │
├─────────────────────────────────────────────────────────────┤
│ [ Đóng ] [ Lưu .txt ]                                       │
└─────────────────────────────────────────────────────────────┘
```

- Vùng xem trước dùng **font monospace** để phản ánh đúng file `.txt` sẽ ghi ra.
- Nội dung hóa đơn (khung trên) **phải giữ nguyên cấu trúc** — dev dùng đúng mẫu này trong tài liệu TASK-10.
- Cần nút "Sao chép" (tùy chọn) và nút "Lưu .txt".

---

## 13. `ReportView` — Báo cáo doanh thu 🔒 ⭐

```
┌────────────────────────────────────────────────────────────────────────────────────────────────┐
│ Báo cáo doanh thu                                                                              │
├────────────────────────────────────────────────────────────────────────────────────────────────┤
│ Từ ngày [ 01/10/2026 ▾ ]   Đến ngày [ 23/10/2026 ▾ ]  [Xem báo cáo]                            │
├────────────────────────────────────────────────────────────────────────────────────────────────┤
│ Tổng doanh thu  |  Lượt thuê phòng  |  Doanh thu dịch vụ                                       │
│ bề mặt neutral  |  bề mặt neutral   |  bề mặt neutral                                          │
├────────────────────────────────────────────────────────────────────────────────────────────────┤
│ 12.450.000 đ    |  25 lượt          |  1.230.000 đ                                             │
│ ▲ so với kỳ trước                                                                              │
├────────────────────────────────────────────────────────────────────────────────────────────────┤
│ Tỷ lệ lấp đầy phòng hiện tại: 75% (9/12 phòng)                                                 │
│ ████████████████████████░░░░░░░░░                                                              │
├────────────────────────────────────────────────────────────────────────────────────────────────┤
│ Doanh thu theo ngày                                                                            │
├────────────────────────────────────────────────────────────────────────────────────────────────┤
│ ╱╲      ╱╲                                                                                     │
│ ╱╲╱  ╲╱╲╱    ╲╱╲                                                                               │
│ ╱╲╱╲                     ╲╱                                                                    │
│ 0                                                                                              │
│ 1/10  5/10   10/10   15/10   20/10                                                             │
├────────────────────────────────────────────────────────────────────────────────────────────────┤
│ Mã HĐ   |  Khách hàng     |  Phòng  |  Ngày TT     |  Tiền phòng   |  Dịch vụ   |  Tổng        │
├────────────────────────────────────────────────────────────────────────────────────────────────┤
│ HD-001  |  Nguyễn Văn An  |  101    |  14/10/2026  |  1.000.000 đ  |  70.000 đ  |  1.070.000 đ │
└────────────────────────────────────────────────────────────────────────────────────────────────┘
```

**Quy tắc trình bày KPI (theo TASK-11)**

Ba KPI dùng cùng bề mặt thẻ (Card Surface), viền/độ nổi nhẹ và kiểu chữ thống nhất. Không tô mỗi KPI một nền màu. Dùng `#005FB8` làm điểm nhấn nhỏ (icon, nhãn phụ hoặc điểm nhấn) nếu cần phân cấp; giá trị chính dùng màu chữ trung tính và đúng format. Status color không dùng trang trí cho KPI.

**ProgressBar**: `Value = 75`; dùng track neutral và màu nhấn `#005FB8` theo ProgressBar biến thể/brush của app. Không dùng xanh lá/cam trạng thái phòng cho số liệu KPI. Nhãn đầy đủ dạng "Tỷ lệ lấp đầy phòng hiện tại: 75% (9/12 phòng)".

**Biểu đồ**: đường đơn giản, trục X = ngày thanh toán, trục Y = tổng doanh thu ngày. Có lưới mờ, tooltip khi di chuột (mô tả bằng annotation, không cần vẽ animation).

**Trạng thái cần vẽ**
- Có dữ liệu (như trên).
- **Không có dữ liệu**: 3 KPI = `0 đ` / `0 lượt` / `0 đ`; biểu đồ hiển thị "Không có dữ liệu trong khoảng thời gian này"; bảng trống thân thiện.
- ⚠ **Ngày không hợp lệ** (Từ ngày > Đến ngày): "Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc. Vui lòng chọn lại." Highlight 2 ô ngày.
- Mặc định khi mở: Từ ngày = đầu tháng hiện tại, Đến ngày = hôm nay.

---

## 14. Các dialog dùng chung

Cần vẽ, từ component ở `02-Components`:

| Dialog | Tiêu đề | Nội dung mẫu |
| --- | --- | --- |
| Xác nhận Check-in | "Xác nhận nhận phòng" | "Check-in phòng 103 cho khách Nguyễn Văn An? Phòng sẽ chuyển sang trạng thái Đang ở." |
| Xác nhận Check-out | "Xác nhận trả phòng" | "Check-out phòng 102? Hệ thống sẽ tạo hóa đơn thanh toán." |
| Xác nhận xóa | "Xóa phòng 305" | "Bạn có chắc muốn xóa phòng 305? Thao tác này không thể hoàn tác." |
| Chặn xóa | "Không thể xóa" | "Phòng 102 đang có khách ở, không thể xóa." |
| Thông báo lỗi chung | "Lỗi" | "Tên đăng nhập hoặc mật khẩu không chính xác!" |
| Thành công | Toast | "Đã thanh toán hóa đơn HD-001. Phòng 101 đã trở về trạng thái Trống." |
| Đăng xuất | "Đăng xuất" | "Bạn muốn đăng xuất khỏi hệ thống?" |

**Quy ước nút**: hành động phá hủy (Xóa, Hủy booking) dùng biến thể danger/semantic của kit; hành động xác nhận nghiệp vụ dùng Button màu nhấn `#005FB8`; Hủy/Đóng dùng biến thể secondary/neutral. Dialog là overlay radius 8px, dùng nền trung tính và overlay/shadow vừa đủ. Tiêu đề/nút dùng viết hoa đầu câu; tránh viết hoa toàn dòng.

---
