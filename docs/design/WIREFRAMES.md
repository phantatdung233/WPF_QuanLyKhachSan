# Wireframes — Hotel Management System (IT008)

**Kèm theo**: [`DESIGN-BRIEF.md`](./DESIGN-BRIEF.md)
**Mục đích**: khoá bố cục sơ bộ trước khi designer vẽ Figma, và để dev dùng làm chuẩn layout khi code XAML.

> Wireframe dưới đây **không phải thiết kế cuối**. Designer được phép sắp xếp lại, đổi khoảng cách, dùng component Fluent khác — **miễn là không bỏ mất thành phần hoặc trạng thái nào được ghi chú**.

Ký hiệu: `[ ]` nút/ô lớn · `( )` combobox · `< >` ô nhập · `[*]` bắt buộc · `⚠` trạng thái lỗi · `🔒` chỉ Admin

---

## 1. `LoginWindow` — Đăng nhập

Cửa sổ riêng, nhỏ gọn (≈ 960×600 hoặc nhỏ hơn), **không có sidebar**.

```
┌──────────────────────────────────────────────────────────────┐
│                                                              │
│      ┌───────────────┐        ┌────────────────────────────┐ │
│      │               │        │  Quản Lý Khách Sạn         │ │
│      │   Ảnh/khối    │        │  Đăng nhập để tiếp tục     │ │
│      │   thương hiệu │        │                            │ │
│      │   (navy)      │        │  Tên đăng nhập             │ │
│      │               │        │  <                     >   │ │
│      │  KHÁCH SẠN    │        │                            │ │
│      │   IT008       │        │  Mật khẩu                  │ │
│      │               │        │  <                     >   │ │
│      │               │        │                            │ │
│      │               │        │  [  Đăng nhập         ]    │ │
│      │               │        │  [  Thoát            ]     │ │
│      │               │        │                            │ │
│      └───────────────┘        └────────────────────────────┘ │
└──────────────────────────────────────────────────────────────┘
```

**Thành phần bắt buộc**: ô Tên đăng nhập, ô Mật khẩu (ẩn ký tự), nút Đăng nhập (primary), nút Thoát (secondary).
Tên hiển thị gợi ý: **"Khách sạn IT008"** (lấy từ tên khách sạn trong mẫu hóa đơn ở TASK-10); bạn có thể đề xuất tên khác nhưng phải đồng bộ với màn Hóa đơn và `InvoicePreview`.

**Trạng thái cần vẽ**
- Bình thường.
- ⚠ Lỗi: bấm Đăng nhập khi để trống → lỗi inline dưới ô: "Vui lòng nhập tên đăng nhập" / "Vui lòng nhập mật khẩu".
- ⚠ Lỗi: sai tài khoản → thông báo "Tên đăng nhập hoặc mật khẩu không chính xác!".
- Loading khi đang kiểm tra tài khoản (nút chuyển trạng thái đang tải, không cho bấm 2 lần).

---

## 2. `MainWindow-Shell` — Khung chính

```
┌────────────────┬──────────────────────────────────────────────────────┐
│  ▣ KHÁCH SẠN   │  Sơ đồ phòng        Xin chào: Lê Thu Ngân (Lễ tân)   │
│  IT008         │                              [ Đăng xuất ]           │
├────────────────┼──────────────────────────────────────────────────────┤
│ ◆ Sơ đồ phòng  │                                                      │
│ ○ Đặt phòng    │                                                      │
│ ○ Phòng        │                                                      │
│ ○ Loại phòng🔒 │            <VÙNG NỘI DUNG>                           │
│ ○ Khách hàng   │            (cuộn dọc)                                │
│ ○ Dịch vụ      │                                                      │
│ ○ Hóa đơn      │                                                      │
│ ○ Báo cáo   🔒 │                                                      │
│                │                                                      │
└────────────────┴──────────────────────────────────────────────────────┘
```

**Vẽ 2 frame**: `MainWindow-Shell / Admin` (đủ 8 mục) và `MainWindow-Shell / Staff` (thiếu Loại phòng, Báo cáo).

**Ghi chú**
- Mục đang chọn: nền sáng hơn + chữ trắng + vạch chỉ báo bên trái.
- Sidebar có thể thu gọn về icon — nếu vẽ, cần đảm bảo vẫn nhận ra mục.
- Trạng thái đăng nhập đầu tiên: nội dung mặc định là **Sơ đồ phòng**.
- Min window ≈ 1280×720. Phải vẽ 1 frame ở kích thước này để kiểm tra không vỡ layout.

---

## 3. `RoomMapView` — Sơ đồ phòng ⭐

Màn trung tâm của app.

```
┌────────────────────────────────────────────────────────────────────┐
│  Sơ đồ phòng                                                       │
│  cập nhật lúc 14:32                            [ ] Làm mới  ( ) lọc │
├────────────────────────────────────────────────────────────────────┤
│  Chú thích:  ■ Trống   ■ Đang ở   ■ Đã đặt   ■ Bảo trì             │
├────────────────────────────────────────────────────────────────────┤
│                                                                    │
│  ┌────────────┐ ┌────────────┐ ┌────────────┐ ┌────────────┐       │
│  │ 101        │ │ 102        │ │ 103        │ │ 201        │       │
│  │ VIP        │ │ Đơn        │ │ Đơn        │ │ Đôi        │       │
│  │ Trống      │ │ Đang ở     │ │ Đã đặt     │ │ Trống      │       │
│  │ 500.000 đ  │ │ NV Văn An  │ │ 15/10→18/10│ │ 700.000 đ  │       │
│  └────────────┘ └────────────┘ └────────────┘ └────────────┘       │
│  ┌────────────┐ ┌────────────┐ ┌────────────┐ ┌────────────┐       │
│  │ 202 ...    │ │ 203 ...    │ │ 301 ...    │ │ 302 ...    │       │
│  └────────────┘ └────────────┘ └────────────┘ └────────────┘       │
│                                                                    │
│  (12 thẻ, wrap theo chiều ngang)                                   │
└────────────────────────────────────────────────────────────────────┘
```

**Mỗi thẻ phòng bắt buộc có**: số phòng (lớn), loại phòng, trạng thái, và **nền theo màu trạng thái**.

Nội dung phụ gợi ý theo trạng thái (giúp lễ tân ra quyết định):
- Trống: đơn giá/đêm.
- Đang ở: tên khách + ngày trả phòng dự kiến.
- Đã đặt: tên khách + khoảng ngày đặt.
- Bảo trì: lý do (nếu có).

**Trạng thái cần vẽ**
- Đủ 4 màu cùng màn.
- Hover thẻ + tooltip: "Click đúp để đặt phòng" (Trống) / "Click đúp để check-out hoặc gọi dịch vụ" (Đang ở). Đã đặt → "Click đúp để check-in".
- Thẻ đang được chọn (click 1 lần) — nếu bạn vẽ panel chi tiết bên phải thì chỉ rõ.
- Cửa sổ hẹp 1024px: thẻ xuống dòng, **không có thanh cuộn ngang**.
- Rỗng: "Chưa có phòng nào".

**Ghi chú nghiệp vụ (annotation bắt buộc)**
- "Màu nền thẻ lấy đúng mã: `#48BB78`, `#F56565`, `#ED8936`, `#A0AEC0` — dev dùng `DataTrigger` theo `Status`."
- "Legend dùng lại đúng 4 mã màu đó."
- "Thẻ phải co giãn tốt, khối lượng phòng có thể thay đổi."

---

## 4. `RoomView` — Quản lý phòng

```
┌────────────────────────────────────────────────────────────────────┐
│  Quản lý phòng                                                     │
├────────────────────────────────────────────────────────────────────┤
│  ( Tất cả loại ▾ )  ( Tất cả trạng thái ▾ )     [ + Thêm phòng ]   │
│                                              [ Quản lý loại phòng ]🔒│
├────────────────────────────────────────────────────────────────────┤
│ Mã │ Số phòng │ Loại phòng │ Đơn giá/ngày │ Trạng thái │ Thao tác  │
├────┼──────────┼────────────┼──────────────┼────────────┼───────────┤
│ 1  │ 101      │ VIP        │ 1.200.000 đ  │ ■ Trống    │ Sửa  Xóa  │
│ 2  │ 102      │ Đơn        │   500.000 đ  │ ■ Đang ở   │ Sửa  🔒Xóa│
│ 3  │ 103      │ Đơn        │   500.000 đ  │ ■ Đã đặt   │ Sửa  Xóa  │
│ 4  │ 305      │ VIP        │ 1.200.000 đ  │ ■ Bảo trì  │ Sửa  Xóa  │
├────────────────────────────────────────────────────────────────────┤
│  Hiển thị 4 / 12 phòng                          [ < ] [ > ]        │
└────────────────────────────────────────────────────────────────────┘
```

**Trạng thái cần vẽ**
- Bình thường + 1 dòng đang chọn.
- Lọc theo "Bảo trì" → chỉ 1 dòng.
- Không có kết quả lọc: "Không có phòng nào ở trạng thái Bảo trì".
- Dòng "Đang ở": nút **Xóa disabled** + tooltip "Không thể xóa phòng đang có khách ở".
- Nút "Quản lý loại phòng" chỉ hiện với Admin.

**Dialog: `RoomDialog` (Thêm / Sửa phòng)**
```
┌──────────────────────────────────────────┐
│  Thêm phòng mới                      [x] │
├──────────────────────────────────────────┤
│  Số phòng *                              │
│  < 305                                 > │
│  ⚠ Số phòng không được để trống          │
│                                          │
│  Loại phòng *                            │
│  ( Chọn loại phòng ▾ )                   │
│                                          │
│  Trạng thái ban đầu                      │
│  ( Trống ▾ )                             │
├──────────────────────────────────────────┤
│                    [ Hủy ]  [ Lưu ]      │
└──────────────────────────────────────────┘
```
Trạng thái: bình thường / lỗi để trống / lỗi trùng số phòng ("Số phòng 101 đã tồn tại").

---

## 5. `RoomTypeView` — Loại phòng 🔒

```
┌────────────────────────────────────────────────────────────────────┐
│  Quản lý loại phòng                                                │
├────────────────────────────────────────────────────────────────────┤
│                                          [ + Thêm loại phòng ]    │
├────────────────────────────────────────────────────────────────────┤
│  Mã │ Tên loại phòng │ Đơn giá/ngày │ Số phòng │ Thao tác          │
├─────┼────────────────┼──────────────┼───────────┼──────────────────┤
│  1  │ Đơn            │   500.000 đ  │ 4 phòng   │ Sửa  Xóa         │
│  2  │ Đôi            │   700.000 đ  │ 3 phòng   │ Sửa  Xóa         │
│  3  │ VIP            │ 1.200.000 đ  │ 3 phòng   │ Sửa  🔒Xóa       │
└────────────────────────────────────────────────────────────────────┘
```
- Cột "Số phòng" (số phòng đang thuộc loại này) giúp giải thích vì sao Xóa bị khóa.
- Xóa loại đang có phòng → dialog cảnh báo: "Không thể xóa loại phòng VIP vì đang có 3 phòng sử dụng. Vui lòng chuyển các phòng sang loại khác trước."
- Lỗi validate: đơn giá phải `> 0`, không được âm hoặc chữ.

---

## 6. `CustomerView` — Quản lý khách hàng

```
┌────────────────────────────────────────────────────────────────────┐
│  Quản lý khách hàng                                                │
├────────────────────────────────────────────────────────────────────┤
│  🔍 < Tìm theo họ tên, CCCD, SĐT...  >        [ + Thêm khách hàng ]│
├────────────────────────────────────────────────────────────────────┤
│ Mã │ Họ và tên      │ CCCD         │ SĐT        │ Email      │ ...  │
├────┼────────────────┼──────────────┼────────────┼────────────┼──────┤
│ 1  │ Nguyễn Văn An  │ 079201001234 │ 0901234567 │ an@ex.com  │ Sửa  │
│ 2  │ Trần Thị Bích  │ 079201005678 │ 0912345678 │ bich@ex.com│ Sửa  │
└────────────────────────────────────────────────────────────────────┘
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
┌────────────────────────────────────────────────────────────────────┐
│  Quản lý dịch vụ                                                   │
├────────────────────────────────────────────────────────────────────┤
│                                                [ + Thêm dịch vụ ]  │
├────────────────────────────────────────────────────────────────────┤
│  Mã DV │ Tên dịch vụ        │ Đơn giá      │ Thao tác              │
├────────┼────────────────────┼──────────────┼───────────────────────┤
│  DV01  │ Nước suối Lavie    │    10.000 đ  │ Sửa  Xóa              │
│  DV02  │ Ăn sáng buffet     │    50.000 đ  │ Sửa  Xóa              │
│  DV03  │ Giặt ủi            │    30.000 đ  │ Sửa  🔒Xóa            │
└────────────────────────────────────────────────────────────────────┘
```
- Xóa dịch vụ đã từng dùng → dialog: "Dịch vụ Giặt ủi đã được sử dụng trong các đặt phòng trước đây, không thể xóa."
- Đơn giá `>= 0`; lỗi khi nhập chữ hoặc số âm.
- Định dạng tiền đúng `#,##0 đ`.

---

## 8. `BookingView` / `BookingDialog` — Đặt phòng ⭐

Có thể là màn riêng trong sidebar, hoặc dialog mở từ Sơ đồ phòng. Cần **cả hai** dạng hoặc chọn một và nói rõ. Đề xuất: màn riêng + dialog khi mở từ thẻ phòng.

```
┌────────────────────────────────────────────────────────────────────┐
│  Đặt phòng mới                                                     │
├────────────────────────────────────────────────────────────────────┤
│  Khách hàng *                                                      │
│  🔍 < Tìm khách theo tên/CCCD >   hoặc   ( Chọn khách hàng ▾ )     │
│  → Đã chọn: Nguyễn Văn An · 079201001234 · 0901234567              │
│                                                                    │
│  Phòng *                                                           │
│  ( 101 — VIP — Trống ▾ )                                           │
│                                                                    │
│  Ngày nhận phòng *        Ngày trả phòng *                         │
│  [ 15/10/2026 ▾ ]         [ 18/10/2026 ▾ ]                         │
│                                                                    │
│  ┌── Tạm tính ──────────────────────────────────────────────────┐  │
│  │ 3 đêm × 500.000 đ                           1.500.000 đ       │  │
│  │ Tổng tạm tính                               1.500.000 đ       │  │
│  └──────────────────────────────────────────────────────────────┘  │
│                                                                    │
│                    [ Hủy ]        [ Xác nhận đặt phòng ]          │
└────────────────────────────────────────────────────────────────────┘
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
┌────────────────────────────────────────────────────────────────────┐
│  Đặt phòng                                                         │
├────────────────────────────────────────────────────────────────────┤
│  ( Tất cả trạng thái ▾ )   ( Tất cả phòng ▾ )    [ + Đặt phòng ]   │
├────────────────────────────────────────────────────────────────────┤
│ Mã BK │ Khách hàng   │ Phòng │ Ngày  │ Trạng thái │ Thao tác       │
├───────┼──────────────┼───────┼───────┼────────────┼────────────────┤
│ B-001 │ Nguyễn Văn An│ 103   │ 15→18 │ ■ Đã đặt   │ [Check-in]     │
│ B-002 │ Trần Thị Bích│ 102   │ 12→14 │ ■ Đang ở   │ Dịch vụ  Check-out │
│ B-003 │ Lê Văn Cường │ 201   │ 01→03 │ ■ Hoàn thành│ (xem hóa đơn) │
│ B-004 │ Phạm Thu Hà  │ 302   │ 05→07 │ ■ Đã hủy   │ —              │
└────────────────────────────────────────────────────────────────────┘
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
┌────────────────────────────────────────────────────────────────┐
│  Dịch vụ phòng 102 — Trần Thị Bích                       [x]   │
├────────────────────────────────────────────────────────────────┤
│  Chọn dịch vụ                                                  │
│  ( Nước suối Lavie ▾ )      Số lượng  < 2 >                    │
│  Đơn giá 10.000 đ             Thành tiền  20.000 đ             │
│                                              [ + Thêm vào ]    │
├────────────────────────────────────────────────────────────────┤
│  Dịch vụ đã sử dụng                                            │
│  Tên dịch vụ       │ SL │ Đơn giá   │ Thành tiền │ Thao tác    │
│  Nước suối Lavie   │  2 │  10.000 đ │  20.000 đ  │ Xóa          │
│  Giặt ủi           │  1 │  30.000 đ │  30.000 đ  │ Xóa          │
├────────────────────────────────────────────────────────────────┤
│  Tổng tiền dịch vụ                            50.000 đ         │
│                                        [ Đóng ] [ Lưu ]       │
└────────────────────────────────────────────────────────────────┘
```

**Trạng thái**
- Bình thường có 2 dòng.
- Rỗng: "Phòng này chưa sử dụng dịch vụ nào."
- ⚠ Số lượng `0` hoặc chữ: "Số lượng phải là số nguyên lớn hơn 0".
- Sau khi xóa 1 dòng: tổng tiền cập nhật ngay (thể hiện bằng annotation).
- **Không cho mở** dialog này với phòng Trống / Bảo trì — nếu bạn vẽ màn chọn phòng trước thì ghi rõ các phòng không hợp lệ bị disabled.

---

## 11. `InvoiceView` — Hóa đơn & thanh toán ⭐

```
┌────────────────────────────────────────────────────────────────────┐
│  Hóa đơn thanh toán          HD-001            [ Chưa thanh toán ] │
├────────────────────────────────────────────────────────────────────┤
│  Khách hàng: Nguyễn Văn An · CCCD 079201001234                     │
│  Phòng: 101 (VIP)             Mã đặt phòng: B-001                  │
│  Nhận phòng: 12/10/2026   →   Trả phòng: 14/10/2026   (2 đêm)      │
├────────────────────────────────────────────────────────────────────┤
│  Chi tiết dịch vụ                                                  │
│  Tên dịch vụ        │ SL │ Đơn giá   │ Thành tiền                 │
│  Nước suối Lavie    │  2 │  10.000 đ │  20.000 đ                  │
│  Ăn sáng buffet     │  1 │  50.000 đ │  50.000 đ                  │
├────────────────────────────────────────────────────────────────────┤
│  Tiền phòng   2 đêm × 500.000 đ                 1.000.000 đ        │
│  Tiền dịch vụ                                      70.000 đ        │
│  ─────────────────────────────────────────────────────────────     │
│  TỔNG CỘNG THANH TOÁN                           1.070.000 đ        │
├────────────────────────────────────────────────────────────────────┤
│  Phương thức thanh toán                                            │
│  ( • ) Tiền mặt     (   ) Chuyển khoản                             │
│                                                                    │
│  [ Xác nhận thanh toán ]        [ In / Xuất hóa đơn ]              │
└────────────────────────────────────────────────────────────────────┘
```

**Trạng thái cần vẽ**
1. **Chưa thanh toán** (như trên): badge cam nhạt, nút thanh toán active.
2. **Đã thanh toán**: badge xanh lá "ĐÃ THANH TOÁN", hiện "Phương thức: Chuyển khoản", "Ngày thanh toán: 14/10/2026 14:32", nút thanh toán **disabled**, nút xuất hóa đơn vẫn active.
3. **Không có dịch vụ**: bảng kê hiển thị dòng "Không sử dụng dịch vụ", tiền dịch vụ `0 đ`, tổng = tiền phòng.
4. **Ở đúng 1 đêm** (nhận và trả cùng ngày): hiển thị "1 đêm" và tính 1 × đơn giá — **không được hiển thị 0 đêm hoặc 0 đ**.
5. ⚠ Lỗi: bấm thanh toán khi chưa chọn phương thức → "Vui lòng chọn phương thức thanh toán".

**Dialog xác nhận thanh toán**: "Xác nhận thanh toán 1.070.000 đ bằng Chuyển khoản? Sau khi xác nhận, phòng 101 sẽ trở về trạng thái Trống." → [Hủy] [Xác nhận].

---

## 12. `InvoicePreview` — Xem trước khi xuất `.txt`

```
┌────────────────────────────────────────────────────────────────────┐
│  Xuất hóa đơn — HD-001                                   [x]      │
├────────────────────────────────────────────────────────────────────┤
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │ =========================================================    │  │
│  │              KHÁCH SẠN IT008 LUXURY                          │  │
│  │           HÓA ĐƠN THANH TOÁN (RECEIPT)                       │  │
│  │ Mã HĐ: HD-001              Ngày lập: 14/10/2026              │  │
│  │ Khách hàng: Nguyễn Văn An  Phòng: 101 (VIP)                  │  │
│  │ Thời gian: 12/10/2026 -> 14/10/2026 (2 ngày)                 │  │
│  │ -------------------------------------------------------      │  │
│  │ Tiền phòng: 2 ngày x 500,000 đ = 1,000,000 đ                 │  │
│  │                                                              │  │
│  │   - Nước suối Lavie (x2): 20,000 đ                           │  │
│  │   - Ăn sáng buffet (x1): 50,000 đ                            │  │
│  │ Tổng tiền dịch vụ: 70,000 đ                                  │  │
│  │ -------------------------------------------------------      │  │
│  │ TỔNG CỘNG THANH TOÁN: 1,070,000 đ                            │  │
│  │ Phương thức: Chuyển khoản                                    │  │
│  │ Trạng thái: ĐÃ THANH TOÁN                                    │  │
│  │ =========================================================    │  │
│  │        CẢM ƠN QUÝ KHÁCH & HẸN GẶP LẠI!                       │  │
│  └──────────────────────────────────────────────────────────────┘  │
│                                                                    │
│                                              [ Đóng ] [ Lưu .txt ]│
└────────────────────────────────────────────────────────────────────┘
```

- Vùng xem trước dùng **font monospace** để phản ánh đúng file `.txt` sẽ ghi ra.
- Nội dung hóa đơn (khung trên) **phải giữ nguyên cấu trúc** — dev dùng đúng mẫu này trong tài liệu TASK-10.
- Cần nút "Sao chép" (tùy chọn) và nút "Lưu .txt".

---

## 13. `ReportView` — Báo cáo doanh thu 🔒 ⭐

```
┌────────────────────────────────────────────────────────────────────┐
│  Báo cáo doanh thu                                                 │
├────────────────────────────────────────────────────────────────────┤
│  Từ ngày [ 01/10/2026 ▾ ]   Đến ngày [ 23/10/2026 ▾ ]  [Xem báo cáo]│
├────────────────────────────────────────────────────────────────────┤
│  ┌──────────────────┐ ┌──────────────────┐ ┌──────────────────┐    │
│  │ TỔNG DOANH THU   │ │ LƯỢT THUÊ PHÒNG  │ │ DOANH THU DỊCH VỤ│    │
│  │ nền navy #1E3A5F │ │ nền xanh dương   │ │ nền xanh ngọc    │    │
│  │                  │ │                  │ │                  │    │
│  │  12.450.000 đ    │ │  25 lượt         │ │  1.230.000 đ     │    │
│  │ ▲ so với kỳ trước│ │                  │ │                  │    │
│  └──────────────────┘ └──────────────────┘ └──────────────────┘    │
├────────────────────────────────────────────────────────────────────┤
│  Tỷ lệ lấp đầy phòng hiện tại: 75% (9/12 phòng)                    │
│  ████████████████████████░░░░░░░░░                                 │
├────────────────────────────────────────────────────────────────────┤
│  Doanh thu theo ngày                                               │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │                        ╱╲      ╱╲                            │  │
│  │                    ╱╲╱  ╲╱╲╱    ╲╱╲                          │  │
│  │              ╱╲╱╲                     ╲╱                     │  │
│  │  0 ──┬────┬────┬────┬────┬────┬────┬────┬──                  │  │
│  │     1/10  5/10   10/10   15/10   20/10                       │  │
│  └──────────────────────────────────────────────────────────────┘  │
├────────────────────────────────────────────────────────────────────┤
│  Mã HĐ │ Khách hàng  │ Phòng │ Ngày TT    │ Tiền phòng │ Dịch vụ │ Tổng │
├────────┼─────────────┼───────┼────────────┼────────────┼─────────┼──────┤
│ HD-001 │ Nguyễn Văn An│ 101  │ 14/10/2026 │ 1.000.000 đ│  70.000 đ│1.070.000 đ│
└────────────────────────────────────────────────────────────────────┘
```

**Quy tắc màu thẻ KPI (theo TASK-11)**
| Thẻ | Nền | Nội dung |
| --- | --- | --- |
| Tổng doanh thu | Navy `#1E3A5F` | Số tiền, định dạng `#,##0 đ` |
| Lượt thuê phòng | Xanh dương | Số nguyên + "lượt" |
| Doanh thu dịch vụ | Xanh ngọc | Số tiền, định dạng `#,##0 đ` |

> Nếu "xanh dương" / "xanh ngọc" gần với màu "Trống" `#48BB78` thì đổi cho đủ tương phản — ghi rõ mã bạn chọn.

**ProgressBar**: `Value = 75`, cao ~20px, màu xanh lá hoặc cam. Nhãn đầy đủ dạng "Tỷ lệ lấp đầy phòng hiện tại: 75% (9/12 phòng)".

**Biểu đồ**: đường đơn giản, trục X = ngày thanh toán, trục Y = tổng doanh thu ngày. Có lưới mờ, tooltip khi hover (mô tả bằng annotation, không cần vẽ animation).

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

**Quy ước nút**: hành động phá hủy (Xóa, Hủy booking) dùng nút danger đỏ; hành động xác nhận nghiệp vụ dùng primary navy; Hủy/Đóng dùng secondary.

---

## 15. Checklist đối chiếu nhanh cho designer

Trước khi gửi mỗi vòng, tự kiểm:

- [ ] Có đủ trạng thái rỗng / không kết quả / lỗi cho màn tương ứng chưa?
- [ ] 4 mã màu trạng thái phòng có đúng không?
- [ ] Màn này có bị lộ với Staff không?
- [ ] Số tiền có đúng `1.070.000 đ` không (không phải `1,070,000 VND`)?
- [ ] Ngày có đúng `dd/MM/yyyy` không?
- [ ] Có thành phần nào WPF không làm được không?
- [ ] Đã đặt đúng tên frame chưa?
