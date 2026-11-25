# [TASK-10] Module Hóa đơn, Thanh toán & Xuất biên lai ra file (.txt)

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-10`  
**GitHub Issue**: [#63](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/63)  
**Thành viên đảm nhiệm**: **Member C (@ngocthuyen1)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/invoice-export`  
**Milestone**: Mốc 2: Nghiệp Vụ Cốt Lõi, Sơ Đồ Phòng & Hóa Đơn (05/12 – 18/12)  
**Khung thời gian dự kiến**: `15/12/2026 – 19/12/2026` *(có thể viết trước `InvoiceCalculator` bằng TDD từ 09/12)*  
**Labels**: `advanced`, `invoice`, `billing`, `m2`, `ready-for-agent`  
**Blocked by**: #80 (TASK-08)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng lớp tính toán & định dạng biên lai thuần `InvoiceCalculator.cs` trong `Services/` (seam kiểm thử tự động TDD) và `InvoiceDAO.cs` trong `DAO/`.
- Hoàn thiện `InvoiceViewModel.cs`, `InvoiceView.xaml` và hộp thoại xem trước biên lai `InvoicePreview` (**Tính năng nâng cao 2**) theo quy trình **2 Pha** (`CLAUDE.md`) và chuẩn `DESIGN.md`:
  - Tính tiền phòng theo **Room Charge Policy** (`Math.Max(1, (CheckOut.Date - CheckIn.Date).Days) * PricePerDay`).
  - Cộng tổng tiền dịch vụ từ `BookingServices`.
  - Thực thi **Settlement Invariant** (`GLOSSARY.md`): khi xác nhận thanh toán (`Tiền mặt` / `Chuyển khoản`), cập nhật đồng bộ `Invoices.Status = N'Đã thanh toán'`, `Bookings.Status = N'Hoàn thành'`, và trả `Rooms.Status = N'Trống'`.
  - Xem trước biên lai dạng font monospace và xuất ra file `.txt` chuẩn mã hóa UTF-8 qua `SaveFileDialog`.

---

## 2. Hướng dẫn nghiệp vụ & UI Contract (Theo `DESIGN.md`)
- **Seam `Services/InvoiceCalculator.cs` & `InvoiceDAO.cs` (Pha 1 — Logic & State First, TDD-first)**:
  - `CalculateStayNights(DateTime checkIn, DateTime checkOut)` → `Math.Max(1, (checkOut.Date - checkIn.Date).Days)` (nhận và trả trong cùng ngày vẫn hiển thị và tính **1 đêm**, không bao giờ hiện `0 đêm`).
  - `CalculateInvoiceSummary(...)` → trả về `Nights`, `RoomCharge`, `ServiceCharge`, `TotalAmount`.
  - `FormatReceiptText(...)` → sinh chuỗi biên lai `.txt` căn cột thẳng hàng, định dạng tiền dấu chấm `#,##0 đ` và tên thương hiệu **`AURA HOTEL`**:
    ```text
    =======================================================
                          AURA HOTEL
                   HÓA ĐƠN THANH TOÁN (RECEIPT)
    Mã HĐ: HD-001               Ngày lập: 14/10/2026
    Khách hàng: Nguyễn Văn An   Phòng: 101 (Đơn)
    Thời gian: 12/10/2026 -> 14/10/2026 (2 đêm)
    -------------------------------------------------------
    Tiền phòng: 2 đêm x 500.000 đ = 1.000.000 đ
    Dịch vụ phát sinh:
      - Nước suối Lavie (x2): 20.000 đ
      - Ăn sáng buffet (x1): 50.000 đ
    Tổng tiền dịch vụ: 70.000 đ
    -------------------------------------------------------
    TỔNG CỘNG THANH TOÁN: 1.070.000 đ
    Phương thức: Chuyển khoản
    Trạng thái: ĐÃ THANH TOÁN
    =======================================================
               CẢM ƠN QUÝ KHÁCH & HẸN GẶP LẠI!
    ```
- **UI Contract (`InvoiceView` & `InvoicePreview` — Pha 2)**:
  - **Dữ liệu hiển thị**: Bộ chọn/danh sách hóa đơn (để mở trực tiếp từ Sidebar `Hóa đơn` vẫn chọn xem được hóa đơn mới nhất, hoặc tự chọn đúng hóa đơn khi đi từ `Check-out`), Mã HĐ, Badge trạng thái (`Chưa thanh toán` / `Đã thanh toán`), Khách hàng + CCCD, Phòng + Loại phòng, Ngày nhận/trả + Số đêm (tối thiểu `1 đêm`), bảng kê dịch vụ (hoặc dòng `"Không phát sinh dịch vụ"` / `0 đ`), Tiền phòng, Tiền dịch vụ, `Tổng cộng thanh toán` (`#,##0 đ`), `RadioButton` phương thức (`Tiền mặt` / `Chuyển khoản`), và hộp thoại `InvoicePreview` hiển thị văn bản `Cascadia Mono` chỉ đọc + nút `Lưu .txt`.
  - **Primitives bắt buộc (`WPF-UI`)**: `ComboBox` / `DataGrid` chọn hóa đơn, `DataGrid` chi tiết dịch vụ (`RowHeight="40"`), `RadioButton`, modal `ui:FluentWindow` / `ui:ContentDialog` cho `InvoicePreview` và xác nhận thanh toán, `ui:InfoBar`, `ui:ProgressRing`, tối đa **1 Primary CTA** `ui:Button` (`Xác nhận thanh toán` trên `InvoiceView`; `Lưu .txt` trên `InvoicePreview`, nút `Xem trước / Xuất .txt` dùng `Secondary`).
  - **4 Trạng thái UI bắt buộc**:
    1. *Loading*: Hiện `ui:ProgressRing` khi tải chi tiết hóa đơn và khóa nút khi đang chạy `SqlTransaction` thanh toán.
    2. *Empty*: Khi hệ thống chưa có hóa đơn nào (bấm trực tiếp từ Sidebar lúc DB trống), hiển thị `"Chưa có hóa đơn nào phát sinh"` rõ ràng; khi khách không gọi dịch vụ thì bảng kê hiện `"Không phát sinh dịch vụ"` (`0 đ`).
    3. *Error*: Bấm thanh toán khi chưa chọn phương thức hiển thị lỗi inline / `ui:InfoBar` `"Vui lòng chọn phương thức thanh toán"`; lỗi ghi file `.txt` hoặc giao dịch SQL báo qua `ui:InfoBar` (`Severity="Error"`).
    4. *Success*: Sau khi thanh toán, đổi badge sang `ĐÃ THANH TOÁN`, khóa vĩnh viễn nút `Xác nhận thanh toán` và `RadioButton` phương thức (chống thanh toán kép), giữ nút `Xem trước / Xuất .txt` hoạt động và hiện `ui:InfoBar` (`Severity="Success"`).

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] `Services/InvoiceCalculator.cs` có đầy đủ unit test trong `HotelManagement.Tests` cho kịch bản mẫu `1.070.000 đ`, ca cùng ngày (1 đêm tối thiểu), ca 0 dịch vụ và nội dung chuỗi biên lai `.txt`.
- [ ] `InvoiceDAO.cs` thực hiện thanh toán trong `SqlTransaction` đảm bảo **Settlement Invariant**: cập nhật `Invoices` (`Đã thanh toán`, `PaymentMethod`, `PaymentDate`), `Bookings` (`Hoàn thành`) và `Rooms` (`Trống`) nguyên tử.
- [ ] `InvoiceView.xaml` + `InvoicePreview` tuân thủ UI Contract & `DESIGN.md` (có bộ chọn hóa đơn khi mở từ Sidebar và xử lý đủ 4 trạng thái `Loading`, `Empty`, `Error`, `Success`).
- [ ] Nút `Xuất hóa đơn (.txt)` mở `SaveFileDialog` và ghi file bằng `UTF8Encoding` đầy đủ dấu tiếng Việt.

### Ca biên cần xử lý (Edge Cases):
- Khách check-in và check-out trong cùng một ngày -> tính đúng **1 đêm** theo `Room Charge Policy`.
- Hóa đơn đã ở trạng thái `Đã thanh toán` -> vô hiệu hóa nút thanh toán và lựa chọn phương thức để tránh ghi đè ngày thanh toán hoặc thanh toán kép.
- Người dùng bấm `Cancel` ở cửa sổ `SaveFileDialog` -> thoát êm, không quăng ngoại lệ.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động (`TDD` seam): `dotnet test` xác nhận phép tính tiền phòng, tiền dịch vụ (`1.000.000 + 70.000 = 1.070.000 đ`), bất biến thanh toán và mẫu xuất `.txt`.
- Trực quan: Check-out phòng mẫu -> xác nhận thanh toán bằng `Chuyển khoản` -> badge chuyển sang `ĐÃ THANH TOÁN`, phòng trên Sơ đồ phòng về lại `Trống` (`#48BB78`), xuất file `.txt` mở bằng Notepad đúng chuẩn tiếng Việt.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-10.md` lên GitHub Issue #63.*
