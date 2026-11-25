# [TASK-04] Module Quản lý Phòng & Loại phòng (Room & RoomType CRUD)

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-04`  
**GitHub Issue**: [#61](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/61)  
**Thành viên đảm nhiệm**: **Member A (@phantatdung233)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/room-management`  
**Milestone**: Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (18/11 – 04/12)  
**Khung thời gian dự kiến**: `25/11/2026 – 29/11/2026` *(ưu tiên hoàn thành sớm để mở khóa `TASK-09` & `TASK-07`)*  
**Labels**: `room`, `crud`, `m1`, `ready-for-agent`  
**Blocked by**: #59 (TASK-02)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng `RoomDAO.cs` và `RoomTypeDAO.cs` trong `DAO/` thao tác tham số hóa với bảng `Rooms` và `RoomTypes`.
- Hoàn thiện 2 màn hình danh mục theo quy trình **2 Pha** (`CLAUDE.md`) và hệ thống token chuẩn trong `DESIGN.md`:
  - `RoomView.xaml` + `RoomViewModel.cs` + `RoomDialog.xaml`: Xem danh sách phòng, lọc kép theo Loại phòng & Trạng thái (`Trống`, `Đang ở`, `Đã đặt`, `Bảo trì`), Thêm/Sửa/Xóa phòng.
  - `RoomTypeView.xaml` + `RoomTypeViewModel.cs` + `RoomTypeDialog.xaml` (chỉ `Administrator` truy cập): Xem/tìm nhanh danh sách loại phòng kèm cột **Số lượng phòng liên kết**, Thêm/Sửa đơn giá đêm (`PricePerDay > 0`), Xóa loại phòng chưa có phòng gắn kèm.

---

## 2. Hướng dẫn nghiệp vụ & UI Contract (Theo `DESIGN.md`)
- **Logic & Validation (Pha 1 — Logic & State First)**:
  - `RoomViewModel` / `RoomDialog`: lọc kết hợp Loại phòng + Trạng thái; validate `RoomNumber` không rỗng, không trùng lặp; khóa xóa phòng đang `Đang ở` hoặc đã phát sinh `Bookings`.
  - `RoomTypeViewModel` / `RoomTypeDialog` (`Administrator` only): validate `TypeName` không rỗng, không trùng tên, `PricePerDay > 0`; chặn xóa khi số phòng liên kết `RoomCount > 0`.
- **UI Contract (`RoomView`, `RoomTypeView` & Dialogs — Pha 2)**:
  - **Dữ liệu hiển thị**:
    - `RoomView`: Mã phòng, Số phòng (`101`, `202`...), Loại phòng, Đơn giá/đêm (`#,##0 đ`), Badge Trạng thái (4 brush `RoomStatus` cố định từ `Colors.xaml`), Cột Thao tác (`Sửa`, `Xóa`), bộ đếm dòng cuối bảng.
    - `RoomTypeView`: Mã loại, Tên loại phòng, Đơn giá/đêm (`#,##0 đ`), Số phòng trực thuộc, Cột Thao tác (`Sửa`, `Xóa`).
  - **Primitives bắt buộc (`WPF-UI`)**: `ComboBox` (bộ lọc/chọn loại), `ui:TextBox` / `ui:NumberBox`, `DataGrid` (`RowHeight="40"`), modal `ui:FluentWindow` / `ui:ContentDialog`, `ui:InfoBar`, `ui:ProgressRing`, và tối đa **1 Primary CTA** `ui:Button` (`Appearance="Primary"`) trên mỗi màn/dialog (`Thêm phòng`, `Thêm loại phòng`, `Lưu`).
  - **4 Trạng thái UI bắt buộc**:
    1. *Loading*: Hiện `ui:ProgressRing` khi nạp dữ liệu và khóa nút `Lưu` khi đang ghi DB.
    2. *Empty*: Phân biệt rõ chưa có dữ liệu gốc (`"Chưa có phòng nào"` / `"Chưa có loại phòng nào"`) với lọc không khớp (`"Không có phòng phù hợp bộ lọc"` kèm nút `"Xóa bộ lọc"`).
    3. *Error*: Lỗi nhập liệu (`RoomNumber` rỗng/trùng, `PricePerDay <= 0`) hiện inline dưới ô nhập (`InlineErrorTextStyle`); lỗi chặn xóa/CSDL hiện qua `ui:InfoBar`.
    4. *Success*: Bảng hiển thị chuẩn `#,##0 đ`, khóa nút `Xóa` kèm `ToolTip` giải thích rõ khi phòng đang bận hoặc loại phòng còn phòng trực thuộc, cập nhật tức thì sau CRUD.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] `RoomDAO.cs` và `RoomTypeDAO.cs` thực thi đầy đủ `SELECT` (kèm JOIN lấy `TypeName`, `PricePerDay`, đếm số phòng theo loại), `INSERT`, `UPDATE`, `DELETE` có tham số `@param`.
- [ ] `RoomView.xaml` lọc tức thời kết hợp Loại phòng + Trạng thái, hiển thị tiền tệ `#,##0 đ`, có trạng thái rỗng/không kết quả và khóa nút `Xóa` khi phòng đang bận hoặc có booking.
- [ ] `RoomDialog.xaml` validate inline không để trống hoặc trùng `RoomNumber`.
- [ ] `RoomTypeView.xaml` + `RoomTypeDialog.xaml` validate `PricePerDay > 0` và chặn xóa loại phòng đang có phòng trực thuộc.
- [ ] Có unit test tại seam `ValidationRules` trong `HotelManagement.Tests` cho: kiểm tra trùng `RoomNumber`, `PricePerDay <= 0` / nhập chữ, điều kiện khóa xóa phòng (`Status == "Đang ở" || BookingCount > 0`) và khóa xóa loại phòng (`RoomCount > 0`).

### Ca biên cần xử lý (Edge Cases):
- Nhân viên lễ tân (`Staff`) đăng nhập: không nhìn thấy và không thể điều hướng sang màn `Loại phòng`.
- Nhập đơn giá chứa ký tự không phải số hoặc `<= 0` -> chặn ngay trên form kèm thông báo cụ thể dưới ô nhập.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động (`TDD` seam): `dotnet test` chạy bộ test validate phòng, loại phòng và quy tắc khóa xóa.
- Trực quan: Thêm phòng `403` loại `VIP`, lọc trạng thái `Trống` thấy phòng mới; thử xóa phòng đang `Đang ở` hoặc xóa loại phòng `VIP` xác nhận hệ thống khóa/chặn an toàn.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-04.md` lên GitHub Issue #61.*
