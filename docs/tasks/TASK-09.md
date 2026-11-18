# [TASK-09] Module Sơ đồ phòng trực quan (Room Matrix với XAML DataTrigger)

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-09`  
**GitHub Issue**: [#81](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/81)  
**Thành viên đảm nhiệm**: **Member C (@ngocthuyen1)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/room-matrix-trigger`  
**Milestone**: Mốc 2: Nghiệp Vụ Cốt Lõi, Sơ Đồ Phòng & Hóa Đơn (05/12 – 18/12)  
**Khung thời gian dự kiến**: `30/11/2026 – 08/12/2026` *(khởi động sớm ngay sau khi `TASK-04` hoàn thành)*  
**Labels**: `advanced`, `ui`, `room-map`, `m2`, `ready-for-agent`  
**Blocked by**: #61 (TASK-04)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Hoàn thiện `RoomMapViewModel.cs` và giao diện trung tâm `RoomMapView.xaml` (**Tính năng nâng cao 1** — màn hình lễ tân nhìn vào 90% thời gian).
- **Tuân thủ tuyệt đối quy chuẩn Windows UI Kit tại `docs/design/DESIGN-BRIEF.md` mục 2.4 & `WIREFRAMES.md` mục 3**:
  - Thân thẻ phòng giữ nền sáng trung tính (`Card Surface`), **không tô kín toàn bộ thẻ bằng mảng màu bão hòa**.
  - Áp dụng kỹ thuật XAML `Style.Triggers` / `DataTrigger` theo `Status` để tự động đổi màu **vạch chỉ báo trạng thái, chấm màu và badge trạng thái** bằng đúng 4 mã màu cố định trong `Resources/Colors.xaml`:
    - `#48BB78`: `Trống` (`Available`)
    - `#F56565`: `Đang ở` (`Occupied`)
    - `#ED8936`: `Đã đặt` (`Reserved`)
    - `#A0AEC0`: `Bảo trì` (`Maintenance`)
- Tương tác nhanh trên sơ đồ: click đơn để chọn/xem chi tiết phòng, click đúp để kích hoạt thao tác nghiệp vụ tương ứng với trạng thái hiện tại.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- **Thanh điều khiển & Chú thích màu (Header + Legend)**:
  - Bộ lọc nhanh theo Trạng thái / Loại phòng, nút `Làm mới` kèm mốc thời gian cập nhật, và thanh **Legend 4 ô màu + nhãn tiếng Việt** dùng lại đúng 4 brush trong `Colors.xaml`.
- **Lưới thẻ phòng (`ItemsControl` + `WrapPanel`)**:
  - Tự động xuống dòng mượt mà khi co giãn cửa sổ (kiểm tra tới chiều rộng `1024px` không xuất hiện thanh cuộn ngang).
  - Nội dung mỗi thẻ phòng (`WIREFRAMES.md` mục 3):
    1. Số phòng nổi bật (`101`, `202`...), Tên loại phòng (`VIP`, `Đơn`...).
    2. Dải/chấm màu + nhãn `Status` đổi màu tự động qua XAML `DataTrigger`.
    3. Dòng thông tin phụ theo ngữ cảnh:
       - `Trống`: hiển thị đơn giá/đêm (`#,##0 đ`).
       - `Đang ở`: hiển thị tên khách đang lưu trú + ngày trả dự kiến (nếu đã có bảng dữ liệu booking).
       - `Đã đặt`: hiển thị tên khách + khoảng ngày đặt.
       - `Bảo trì`: ghi chú `"Đang bảo trì"`.
    4. Tooltip gợi ý thao tác khi di chuột: `"Click đúp để đặt phòng"` (`Trống`), `"Click đúp để check-in"` (`Đã đặt`), `"Click đúp để gọi dịch vụ / check-out"` (`Đang ở`).
- **Điều hướng nhanh khi Click đúp**:
  - Phòng `Trống`: chuyển sang màn Đặt phòng (`MainViewModel`) với `RoomId` được chọn sẵn.
  - Phòng `Đã đặt` / `Đang ở`: chuyển nhanh sang thao tác Check-in, Gọi dịch vụ hoặc Check-out (dùng callback điều hướng qua `MainViewModel` để ghép mượt với `TASK-07` & `TASK-08`).

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] `RoomMapView.xaml` hiển thị lưới thẻ phòng bằng `ItemsControl` + `WrapPanel` trên nền `Card Surface` trung tính chuẩn Windows UI Kit, có Legend 4 màu và bộ lọc trạng thái/loại phòng.
- [ ] Cài đặt XAML `DataTrigger` đổi màu vạch chỉ báo / chấm / badge trạng thái theo đúng 4 mã `#48BB78`, `#F56565`, `#ED8936`, `#A0AEC0` (không tô đặc nền cả thẻ).
- [ ] Hiển thị đủ trạng thái hover, được chọn, tooltip hướng dẫn, và trạng thái rỗng `"Chưa có phòng nào"` khi danh sách trống.
- [ ] Click đơn xem thông tin thẻ đang chọn; click đúp điều hướng đúng hành động theo trạng thái phòng.
- [ ] Có unit test trong `HotelManagement.Tests` cho `RoomMapViewModel` (lọc theo trạng thái/loại phòng, đếm số lượng từng trạng thái cho Legend, và quyết định hành động khi click đúp theo từng `RoomStatus`).

### Ca biên cần xử lý (Edge Cases):
- Cửa sổ thu hẹp về `1024px` (hoặc `1280×720`): thẻ tự xuống dòng qua `WrapPanel`, tuyệt đối không bị cắt chữ hay tràn thanh cuộn ngang.
- Khi quay lại Sơ đồ phòng sau khi Đặt phòng / Check-in / Check-out ở màn khác: `RoomMapViewModel.LoadRooms()` tự làm mới để trạng thái màu cập nhật tức thì.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động (`TDD` seam): `dotnet test` kiểm tra bộ lọc phòng, bộ đếm trạng thái Legend và điều phối hành động click đúp trên `RoomMapViewModel`.
- Trực quan: Mở `RoomMapView` với dữ liệu seed 12 phòng (6 Trống, 2 Đang ở, 3 Đã đặt, 1 Bảo trì) xác nhận đủ 4 dấu màu sắc nét trên nền thẻ sáng trung tính, co giãn cửa sổ mượt.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-09.md` lên GitHub Issue #81.*
