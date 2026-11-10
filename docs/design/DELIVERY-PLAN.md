# Delivery Plan — Giao việc thiết kế Figma theo 3 vòng

**Kèm theo**: [`DESIGN-BRIEF.md`](./DESIGN-BRIEF.md), [`WIREFRAMES.md`](./WIREFRAMES.md)

Mục đích: chia việc cho người thiết kế thành **3 vòng có điểm duyệt rõ ràng**, để nếu sai phong cách thì sửa sớm thay vì phải làm lại 13 màn.

Nguyên tắc: **duyệt xong vòng N mới bắt đầu vòng N+1**. Không giao dồn.

---

## Vòng 1 — Chốt phong cách (quan trọng nhất)

**Mục tiêu**: chốt cách áp dụng Windows UI Kit — nền trung tính sáng, xanh Windows làm accent, kiểu chữ Segoe UI Variable, quy tắc bo góc và mật độ — trước khi nhân bản ra 10 màn còn lại.

**Bàn giao**
| Hạng mục | Chi tiết |
| --- | --- |
| Trang `01-Styles` | Token theo vai trò: Surface/App Surface/Layer, màu chữ trung tính và đường viền, màu nhấn `#005FB8`, bốn màu trạng thái nghiệp vụ; type ramp Segoe UI Variable; khoảng cách, radius 4/8px và độ nổi theo Windows UI Kit. |
| Trang `02-Components` (phần 1) | Dùng instance/component gốc từ Windows UI Kit cho Button, Text Box, ComboBox và Date Picker; bổ sung Search, badge, dialog, toast theo cùng kiểu chữ, viền, chiều cao và trạng thái của kit. Giữ rõ đâu là instance gốc, đâu là pattern riêng của ứng dụng. |
| `LoginWindow` | Gồm trạng thái bình thường + lỗi để trống + sai tài khoản |
| `MainWindow-Shell` | 2 frame: Admin (8 mục) và Staff (6 mục) |
| `RoomMapView` | Đủ 4 màu, chú thích màu, di chuột và chú thích, cửa sổ hẹp 1024px, trạng thái rỗng |

**Điểm duyệt (bạn/tổ trưởng xem)**
- [ ] Shell và nội dung dùng các lớp nền sáng trung tính của kit; không còn sidebar/header/KPI mảng navy lớn.
- [ ] Accent Windows `#005FB8` dùng có tiết chế cho CTA và vạch chọn; item điều hướng được chọn có nền trung tính nhẹ, chữ và biểu tượng màu trung tính và vạch xanh.
- [ ] Bốn mã trạng thái phòng giữ chính xác; thể hiện nhanh bằng chú thích màu, nhãn và vạch/badge màu trên thẻ phòng trung tính, không tô toàn bộ thẻ bằng màu bão hòa.
- [ ] Chữ tiếng Việt hiển thị đúng dấu, không bị cắt.
- [ ] Mật độ thông tin đủ cao — không có khoảng trắng lãng phí.
- [ ] Không có hiệu ứng / component vượt ràng buộc ở mục 2.2 của brief.
- [ ] Dev nhìn vào là biết sẽ code `Colors.xaml` và `Styles.xaml` thế nào.

**Căn cứ Figma**: [Windows UI Kit — Hotel Management page](https://www.figma.com/design/4wEK8iFt0i0RJIfmVYU0iO/Windows-UI-kit--Community-?node-id=172994-2). Vòng 1 đã được dựng trên page này; dùng nó làm mốc đối chiếu khi tiếp tục.

**Thời lượng gợi ý**: 1 tuần (đây là vòng chậm nhất, làm kỹ).

**Sau khi duyệt**: ghi lại 1 trang "style đã chốt" (nếu có thay đổi so với brief) và gửi người thiết kế để vòng 2 không bị trôi. Trong Figma, ưu tiên sửa biến thể và giá trị thiết kế của component kit hoặc instance; không tạo bản vẽ thay thế nếu component tương ứng đã có.

---

## Vòng 2 — Nhân bản bố cục danh mục & luồng đặt phòng

**Quy tắc kế thừa**: lấy shell, kiểu chữ, khoảng cách, Button, Text Box, ComboBox, Date Picker và dialog đã chốt ở vòng 1. Bảng, thẻ phòng và các mẫu giao diện nghiệp vụ có thể là component riêng của app, nhưng phải dùng cùng giá trị thiết kế và trạng thái của Windows UI Kit; không tự đặt lại hệ màu/kiểu control.

**Mục tiêu**: tận dụng tối đa một bố cục bảng đã chốt, chỉ khác cột và hành động.

**Bàn giao**
| Hạng mục | Chi tiết |
| --- | --- |
| Component bổ sung | Table (header, row, được chọn, di chuột, empty), Sidebar item (normal/active), Card |
| `RoomView` + `RoomDialog` | Lọc 2 combobox, cột Thao tác, dòng bị khóa xóa |
| `RoomTypeView` | Cột "Số phòng", dialog chặn xóa |
| `CustomerView` + `CustomerDialog` | Search real-time, 3 trạng thái lỗi validate, trùng CCCD |
| `ServiceView` + `ServiceDialog` | Đơn giá ≥ 0, chặn xóa dịch vụ đã dùng |
| `BookingView` / `BookingDialog` | Bao gồm **lỗi trùng lịch**, lỗi ngày, cảnh báo quá khứ, tạm tính |
| `BookingList` | 4 trạng thái booking, nút hành động thay đổi theo trạng thái |

**Điểm duyệt**
- [ ] 4 màn danh mục dùng **cùng một khung** — chỉ khác cột và nút. Nếu mỗi màn một kiểu, yêu cầu thống nhất.
- [ ] Trạng thái rỗng / không có kết quả có ở cả 4 màn.
- [ ] Dòng bị khóa thao tác có lý do rõ (tooltip hoặc icon khoá).
- [ ] Form đặt phòng chỉ ra được **phòng nào bận và khoảng nào bận**.
- [ ] Không màn nào lộ menu Admin.

**Thời lượng gợi ý**: 1 tuần.

---

## Vòng 3 — Ba màn điểm nhấn + hoàn thiện

**Quy tắc kế thừa**: tiếp tục dùng cùng các component/biến thể đã chốt; chỉ tạo pattern mới khi nghiệp vụ cần và ghi rõ cách ghép từ component kit.

**Mục tiêu**: các màn được giảng viên nhìn kỹ nhất trong buổi demo, cộng với polish toàn bộ.

**Bàn giao**
| Hạng mục | Chi tiết |
| --- | --- |
| `AddServiceDialog` | Bảng kê dịch vụ, tổng tiền cập nhật, validate số lượng |
| `InvoiceView` | 5 trạng thái: chưa TT, đã TT, không dịch vụ, đúng 1 đêm, lỗi chưa chọn phương thức |
| `InvoicePreview` | Khung văn bản monospace đúng mẫu receipt, nút lưu `.txt` |
| `ReportView` | 3 KPI, progressbar, biểu đồ đường, bảng, trạng thái không có dữ liệu, lỗi ngày |
| Bộ dialog chung | Check-in, check-out, xóa, chặn xóa, đăng xuất, toast |
| Trang `05-Flows` | 2 luồng nghiệp vụ |
| Polish pass | Rà lại toàn bộ 13 màn theo checklist nghiệm thu ở brief mục 11 |

**Điểm duyệt**
- [ ] Hóa đơn thể hiện đúng `1.070.000 đ` và đúng cấu trúc receipt.
- [ ] Hóa đơn đã thanh toán có trạng thái khoá rõ ràng (không cho bấm thanh toán lần 2).
- [ ] Báo cáo trông ấn tượng khi chiếu máy chiếu: KPI đọc được từ xa, biểu đồ không rối.
- [ ] Báo cáo không có dữ liệu trông "có chủ đích", không phải màn hình lỗi.
- [ ] Toàn bộ 13 màn dùng chung component, không có màn nào "vẽ riêng".

**Thời lượng gợi ý**: 1 tuần.

---

## Timeline gợi ý theo lịch đồ án

Đồ án 6 tuần; phần UI nên xong trước khi code UI thật để dev không phải thiết kế lại.

| Tuần | Công việc |
| --- | --- |
| Tuần 1 | Gửi brief + wireframe, người thiết kế đọc & hỏi lại |
| Tuần 1–2 | **Vòng 1** → duyệt (chốt style) |
| Tuần 2 | Dev bắt đầu code `Resources/Colors.xaml`, `Styles.xaml`, `MainWindow` theo vòng 1 |
| Tuần 2–3 | **Vòng 2** → duyệt |
| Tuần 3 | Dev code các màn danh mục + booking theo vòng 2 |
| Tuần 3–4 | **Vòng 3** → duyệt |
| Tuần 4–5 | Dev code sơ đồ phòng, hóa đơn, báo cáo; người thiết kế chỉ hỗ trợ chỉnh nhỏ |
| Tuần 5–6 | Chỉ sửa lỗi thị giác, không thêm màn mới |

> Nếu thời gian gấp: ưu tiên tuyệt đối **Vòng 1** (Sơ đồ phòng) và **màn Hóa đơn** — vì đó là 2 điểm nhấn nâng cao của đồ án. Các màn danh mục có thể dùng bố cục mặc định của `DataGrid` nếu buộc phải cắt.

---

## Cách phản hồi để tránh lặp vô hạn

1. **Mỗi vòng chỉ 1 lượt duyệt + 1 lượt sửa.** Nếu sau 2 lượt vẫn lệch, dừng lại họp 15 phút thay vì comment qua lại.
2. **Yêu cầu sửa phải cụ thể và có mã dẫn**: ghi `[RoomMapView] thẻ phòng quá thấp, cần ≥ 96px` — không viết "nhìn chưa đẹp".
3. **Dùng comment trên Figma** (chấm vào đúng vị trí) thay vì nhắn tin rời.
4. **Không đổi brief giữa vòng.** Nếu phát sinh ý mới, ghi vào "backlog design" và xử lý ở vòng sau.
5. **Dev là người nghiệm thu cuối**: trước khi duyệt vòng, cho 1 dev xem 5 phút và hỏi "code cái này mất bao lâu?".

---

## Mẫu tin nhắn giao việc vòng 1

> Chào [tên], gửi bạn 2 file đính kèm: `DESIGN-BRIEF.md` (yêu cầu + ràng buộc + dữ liệu mẫu) và `WIREFRAMES.md` (bố cục sơ bộ từng màn). Đọc mục 0→2 của brief trước, có gì chưa rõ nhắn mình ngay trong ngày để mình trả lời trước khi bạn bắt tay vẽ.
>
> **Vòng 1** chỉ cần 5 thứ:
> 1. Trang Styles (màu, chữ, khoảng cách)
> 2. Components cơ bản (button, input, search, combobox, datepicker, badge, dialog)
> 3. `LoginWindow`
> 4. `MainWindow-Shell` — 2 biến thể Admin / Staff
> 5. `RoomMapView` — đủ 4 màu phòng, có chú thích màu, có di chuột và chú thích, có bản 1024px
>
> Mục tiêu vòng này là **chốt cách áp dụng Windows UI Kit**, nên mình chưa cần 10 màn kia. Dùng component có sẵn làm instance; chỉ tạo component riêng của ứng dụng cho thẻ phòng, shell, badge, bảng hoặc nội dung nghiệp vụ mà kit không cung cấp. Nền sáng trung tính và bề mặt theo kit; xanh `#005FB8` là accent, không dùng navy làm nền điều hướng hoặc nền KPI. Duyệt xong mình giao vòng 2.
>
> Lưu ý: đây là **app desktop WPF**. Dùng Windows UI Kit làm nguồn cho control cơ bản trong Figma và đối chiếu khả năng triển khai bằng XAML/WPF-UI. Các mẫu giao diện nghiệp vụ có thể là component riêng nhưng phải cùng hệ giá trị thiết kế. 4 mã màu trạng thái phòng cố định; UI tiếng Việt 100%.
>
> Deadline vòng 1: [ngày]. Gửi mình link Figma có quyền xem nhé.

---

## Mẫu tin nhắn giao việc vòng 2 và 3

> **Vòng 2**: 4 màn danh mục (`RoomView`, `RoomTypeView`, `CustomerView`, `ServiceView`) + 2 màn đặt phòng (`BookingDialog`, `BookingList`). Dùng đúng component vòng 1, đừng tạo style mới. Quan trọng: mỗi màn có trạng thái rỗng, không có kết quả tìm kiếm, và dòng bị khoá thao tác. Form đặt phòng phải có trạng thái lỗi trùng lịch — chi tiết ở mục 8 của `WIREFRAMES.md`.
>
> **Vòng 3**: 3 màn điểm nhấn `AddServiceDialog`, `InvoiceView`, `InvoicePreview`, `ReportView` + bộ dialog chung + trang Flows. Đây là các màn giảng viên nhìn kỹ nhất — cân nhắc kỹ phần số liệu và biểu đồ. Xong thì rà lại toàn bộ theo checklist mục 11 của brief.
