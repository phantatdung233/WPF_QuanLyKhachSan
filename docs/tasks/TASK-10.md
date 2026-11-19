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
- Hoàn thiện `InvoiceViewModel.cs`, `InvoiceView.xaml` và khung xem trước biên lai `InvoicePreview` (**Tính năng nâng cao 2**) bám sát `docs/design/DESIGN-BRIEF.md` mục 6.3 & `WIREFRAMES.md` mục 11–12:
  - Tính tiền phòng theo **Room Charge Policy** (`Math.Max(1, (CheckOut.Date - CheckIn.Date).Days) * PricePerDay`).
  - Cộng tổng tiền dịch vụ từ `BookingServices`.
  - Thực thi **Settlement Invariant** (`GLOSSARY.md`): khi xác nhận thanh toán (`Tiền mặt` / `Chuyển khoản`), cập nhật đồng bộ `Invoices.Status = N'Đã thanh toán'`, `Bookings.Status = N'Hoàn thành'`, và trả `Rooms.Status = N'Trống'`.
  - Xem trước biên lai dạng font monospace và xuất ra file `.txt` chuẩn mã hóa UTF-8 qua `SaveFileDialog`.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- **Seam `Services/InvoiceCalculator.cs` (TDD-first — Member C có thể code ngay trước khi chờ `TASK-08` xong)**:
  - `CalculateStayNights(DateTime checkIn, DateTime checkOut)` → `Math.Max(1, (checkOut.Date - checkIn.Date).Days)` (nhận và trả trong cùng ngày vẫn hiển thị và tính **1 đêm**, không bao giờ hiện `0 đêm`).
  - `CalculateInvoiceSummary(...)` → trả về `Nights`, `RoomCharge`, `ServiceCharge`, `TotalAmount`.
  - `FormatReceiptText(...)` → sinh chuỗi biên lai `.txt` căn cột thẳng hàng, tên thương hiệu thống nhất với màn đăng nhập (**`AURA HOTEL`**):
    ```text
    =======================================================
                        AURA HOTEL
                   HÓA ĐƠN THANH TOÁN (RECEIPT)
    Mã HĐ: HD-001               Ngày lập: 14/10/2026
    Khách hàng: Nguyễn Văn An   Phòng: 101 (Đơn)
    Thời gian: 12/10/2026 -> 14/10/2026 (2 đêm)
    -------------------------------------------------------
    Tiền phòng: 2 đêm x 500,000 đ = 1,000,000 đ
    Dịch vụ phát sinh:
      - Nước suối Lavie (x2): 20,000 đ
      - Ăn sáng buffet (x1): 50,000 đ
    Tổng tiền dịch vụ: 70,000 đ
    -------------------------------------------------------
    TỔNG CỘNG THANH TOÁN: 1,070,000 đ
    Phương thức: Chuyển khoản
    Trạng thái: ĐÃ THANH TOÁN
    =======================================================
               CẢM ƠN QUÝ KHÁCH & HẸN GẶP LẠI!
    ```
- **5 trạng thái giao diện bắt buộc trên `InvoiceView.xaml` (`DESIGN-BRIEF.md` mục 6.3)**:
  1. Hóa đơn `Chưa thanh toán` có dịch vụ phát sinh (cho chọn `Tiền mặt` / `Chuyển khoản` và bấm `Xác nhận thanh toán`).
  2. Hóa đơn `Đã thanh toán`: hiển thị badge xanh `ĐÃ THANH TOÁN`, khóa (vô hiệu hóa) nút thanh toán để không bấm thanh toán lần 2, vẫn cho phép `Xem trước / Xuất file .txt`.
  3. Khách không dùng dịch vụ nào: hiển thị dòng thân thiện `"Không phát sinh dịch vụ"`, tiền dịch vụ `0 đ`.
  4. Nhận và trả phòng cùng ngày: hiển thị rõ `"1 đêm (tối thiểu 1 đêm)"`.
  5. Lỗi inline khi bấm `Xác nhận thanh toán` mà chưa chọn phương thức thanh toán.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] `Services/InvoiceCalculator.cs` có đầy đủ unit test trong `HotelManagement.Tests` cho kịch bản mẫu `1,070,000 đ`, ca cùng ngày (1 đêm tối thiểu), ca 0 dịch vụ và nội dung chuỗi biên lai `.txt`.
- [ ] `InvoiceDAO.cs` thực hiện thanh toán trong `SqlTransaction` đảm bảo **Settlement Invariant**: cập nhật `Invoices` (`Đã thanh toán`, `PaymentMethod`, `PaymentDate`), `Bookings` (`Hoàn thành`) và `Rooms` (`Trống`) nguyên tử.
- [ ] `InvoiceView.xaml` + `InvoicePreview` thể hiện đủ 5 trạng thái giao diện chuẩn Windows UI Kit.
- [ ] Nút `Xuất hóa đơn (.txt)` mở `SaveFileDialog` và ghi file bằng `UTF8Encoding` đầy đủ dấu tiếng Việt.

### Ca biên cần xử lý (Edge Cases):
- Khách check-in và check-out trong cùng một ngày -> tính đúng **1 đêm** theo `Room Charge Policy`.
- Hóa đơn đã ở trạng thái `Đã thanh toán` -> vô hiệu hóa nút thanh toán và lựa chọn phương thức để tránh ghi đè ngày thanh toán hoặc thanh toán kép.
- Người dùng bấm `Cancel` ở cửa sổ `SaveFileDialog` -> thoát êm, không quăng ngoại lệ.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tự động (`TDD` seam): `dotnet test` xác nhận phép tính tiền phòng, tiền dịch vụ (`1,000,000 + 70,000 = 1,070,000`), bất biến thanh toán và mẫu xuất `.txt`.
- Trực quan: Check-out phòng mẫu -> xác nhận thanh toán bằng `Chuyển khoản` -> badge chuyển sang `ĐÃ THANH TOÁN`, phòng trên Sơ đồ phòng về lại `Trống` (`#48BB78`), xuất file `.txt` mở bằng Notepad đúng chuẩn tiếng Việt.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-10.md` lên GitHub Issue #63.*
