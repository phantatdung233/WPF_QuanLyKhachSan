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
- **Tuân thủ tuyệt đối quy trình 2 Pha (`CLAUDE.md`) và hệ thống token trong `DESIGN.md`**:
  - Thân thẻ phòng giữ nền sáng trung tính (`CardSurfaceBrush` `#FFFFFF`), **không tô kín toàn bộ thẻ bằng mảng màu bão hòa**.
  - Áp dụng kỹ thuật XAML `Style.Triggers` / `DataTrigger` theo `Status` để tự động đổi màu **vạch chỉ báo trạng thái (`4px`), chấm màu và badge trạng thái** bằng đúng 4 mã màu cố định trong `Resources/Colors.xaml`:
    - `#48BB78`: `Trống` (`Available`)
    - `#F56565`: `Đang ở` (`Occupied`)
    - `#ED8936`: `Đã đặt` (`Reserved`)
    - `#A0AEC0`: `Bảo trì` (`Maintenance`)
- Tương tác nhanh trên sơ đồ: click đơn để chọn/xem chi tiết phòng, click đúp để kích hoạt thao tác nghiệp vụ tương ứng với trạng thái hiện tại.

---

## 2. Hướng dẫn nghiệp vụ & UI Contract (Theo `DESIGN.md`)
- **Logic điều phối & Lọc sơ đồ (Pha 1 — Logic & State First)**:
  - `RoomMapViewModel`: nạp danh sách phòng kèm thông tin lưu trú/đặt trước hiện hành, tính bộ đếm theo 4 trạng thái cho thanh Legend, lọc tức thời theo Loại phòng & Trạng thái, và quyết định hành động khi click đúp (`Trống` → mở Đặt phòng chọn sẵn `RoomId`; `Đã đặt` → Check-in; `Đang ở` → Gọi dịch vụ / Check-out).
- **UI Contract (`RoomMapView` — Pha 2)**:
  - **Dữ liệu hiển thị**:
    - Thanh điều khiển trên cùng: `ComboBox` lọc Loại phòng, `ComboBox` lọc Trạng thái, Legend 4 màu kèm số lượng từng trạng thái, mốc thời gian cập nhật và nút `Làm mới`.
    - Mỗi thẻ phòng: Số phòng (`101`, `202`...), Loại phòng (`Đơn`, `VIP`...), vạch/chấm/badge `Status` đổi màu qua XAML `DataTrigger`, dòng phụ theo ngữ cảnh (`Trống`: đơn giá `#,##0 đ`; `Đang ở`: tên khách + ngày trả dự kiến; `Đã đặt`: tên khách + khoảng ngày; `Bảo trì`: `"Đang bảo trì"`), cùng `ToolTip` hướng dẫn thao tác.
  - **Primitives bắt buộc (`WPF-UI`)**: `ComboBox`, `ui:Button` (`Làm mới`), `ScrollViewer` + `ItemsControl` (`WrapPanel` tự xuống dòng ở `1280×720` không có thanh cuộn ngang), `Border` (`CardSurfaceBorderStyle`), `ui:InfoBar`, `ui:ProgressRing`.
  - **4 Trạng thái UI bắt buộc**:
    1. *Loading*: Hiện `ui:ProgressRing` khi đang tải/làm mới sơ đồ phòng.
    2. *Empty*: Chưa có phòng trong CSDL (`"Chưa có phòng nào"`) hoặc lọc không có phòng khớp (`"Không có phòng phù hợp bộ lọc"` kèm nút `"Xóa bộ lọc"`).
    3. *Error*: Lỗi mất kết nối CSDL khi tải sơ đồ hiển thị qua `ui:InfoBar` (`Severity="Error"`) kèm nút thử lại.
    4. *Success*: Lưới thẻ phòng trên nền trung tính với đủ 4 dấu màu `DataTrigger` sắc nét, tự động làm mới ngay khi quay lại từ màn Đặt phòng / Check-in / Hóa đơn.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] `RoomMapView.xaml` hiển thị lưới thẻ phòng bằng `ItemsControl` + `WrapPanel` trên nền `CardSurfaceBrush` trung tính theo `DESIGN.md`, có Legend 4 màu, bộ lọc trạng thái/loại phòng và đủ 4 trạng thái UI.
- [ ] Cài đặt XAML `DataTrigger` đổi màu vạch chỉ báo / chấm / badge trạng thái theo đúng 4 mã `#48BB78`, `#F56565`, `#ED8936`, `#A0AEC0` (không tô đặc nền cả thẻ).
- [ ] Hiển thị đủ trạng thái hover, được chọn, tooltip hướng dẫn, và trạng thái rỗng `"Chưa có phòng nào"` khi danh sách trống.
- [ ] Click đơn xem thông tin thẻ đang chọn; click đúp điều hướng đúng hành động theo trạng thái phòng.
- [ ] Có unit test trong `HotelManagement.Tests` cho `RoomMapViewModel` (lọc theo trạng thái/loại phòng, đếm số lượng từng trạng thái cho Legend, và quyết định hành động khi click đúp theo từng `RoomStatus`).

### Ca biên cần xử lý (Edge Cases):
- Cửa sổ thu về kích thước tối thiểu `1280×720`: thẻ tự xuống dòng qua `WrapPanel`, tuyệt đối không bị cắt chữ hay tràn thanh cuộn ngang.
- Khi quay lại Sơ đồ phòng sau khi Đặt phòng / Check-in / Check-out ở màn khác: `RoomMapViewModel.LoadRooms()` tự làm mới để trạng thái màu cập nhật tức thì.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động (`TDD` seam): `dotnet test` kiểm tra bộ lọc phòng, bộ đếm trạng thái Legend và điều phối hành động click đúp trên `RoomMapViewModel`.
- Trực quan: Mở `RoomMapView` với dữ liệu seed 12 phòng (6 Trống, 2 Đang ở, 3 Đã đặt, 1 Bảo trì) xác nhận đủ 4 dấu màu sắc nét trên nền thẻ sáng trung tính, co giãn cửa sổ mượt.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-09.md` lên GitHub Issue #81.*
