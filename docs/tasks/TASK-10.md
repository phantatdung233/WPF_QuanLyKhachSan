# [TASK-10] Module Hóa đơn, Thanh toán & Xuất hóa đơn ra file

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-10`  
**GitHub Issue**: [#63](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/63)  
**Thành viên đảm nhiệm**: **Member C (@ngocthuyen1)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/invoice-export`  
**Milestone**: Mốc 2: Nghiệp Vụ Cốt Lõi, Sơ Đồ Phòng & Hóa Đơn (Tuần 3-5)  
**Labels**: `advanced`, `invoice`, `billing`, `m2`  
**Blocked by**: #80 (TASK-08)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng `InvoiceDAO.cs` trong thư mục `DAO/` và `Invoice.cs` trong `Models/`.
- Xây dựng `InvoiceViewModel.cs` và giao diện `InvoiceView.xaml`.
- Tính toán tổng viện phí lưu trú:
  - Tiền phòng tính theo **Room Charge Policy** trong `GLOSSARY.md`.
  - Tiền dịch vụ = Tổng tiền tất cả các dịch vụ đã sử dụng trong `BookingServices`.
  - Tổng cộng = Tiền phòng + Tiền dịch vụ.
- Ghi nhận phương thức thanh toán: Tiền mặt hoặc Chuyển khoản ngân hàng.
- Cập nhật trạng thái `Invoices.Status = N'Đã thanh toán'`, `Bookings.Status = N'Hoàn thành'`, và giải phóng `Rooms.Status = N'Trống'`.
- **Điểm nhấn nâng cao 2**: Xuất hóa đơn thanh toán ra file văn bản `.txt` hoặc in ấn trực tiếp, lưu trữ biên lai thanh toán.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- `InvoiceView.xaml`:
  - Thông tin chung: Mã hóa đơn, Tên khách hàng, Số phòng, Ngày nhận phòng, Ngày trả phòng, Số ngày ở.
  - Bảng kê chi tiết dịch vụ: Tên dịch vụ, Số lượng, Đơn giá, Thành tiền.
  - Tổng kết tiền tệ: Tiền phòng, Tiền dịch vụ, Tổng tiền phải thanh toán (VNĐ).
  - Lựa chọn Phương thức thanh toán (Radio Button hoặc ComboBox: "Tiền mặt", "Chuyển khoản").
  - Nút "Xác nhận thanh toán" và nút "In / Xuất hóa đơn".
- Tính năng Xuất hóa đơn ra file:
  - Khi bấm nút "Xuất hóa đơn", mở hộp thoại `SaveFileDialog` (mặc định định dạng `.txt` hoặc `.rtf`).
  - Sử dụng `StreamWriter` định dạng nội dung hóa đơn chỉn chu:
    ```
    =======================================================
                    KHÁCH SẠN IT008 LUXURY
               HÓA ĐƠN THANH TOÁN (RECEIPT)
    Mã HĐ: HD-001               Ngày lập: 14/10/2026
    Khách hàng: Nguyễn Văn An   Phòng: 101 (VIP)
    Thời gian: 12/10/2026 -> 14/10/2026 (2 ngày)
    -------------------------------------------------------
    Tiền phòng: 2 ngày x 500,000 đ = 1,000,000 đ
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

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] InvoiceDAO.cs: Thao tác với bảng Invoices.
- [ ] InvoiceView.xaml: Hiển thị chi tiết hóa đơn thanh toán gồm: Tiền phòng (theo **Room Charge Policy** trong `GLOSSARY.md`) + Tiền dịch vụ (tổng tiền các dịch vụ từ BookingServices).
- [ ] Chọn phương thức thanh toán (Tiền mặt / Chuyển khoản), bấm nút Thanh toán cập nhật trạng thái hóa đơn sang Đã thanh toán, cập nhật Booking sang Hoàn thành và Phòng về trạng thái Trống.
- [ ] Tính năng nâng cao: Nút Xuất hóa đơn ghi toàn bộ chi tiết hóa đơn ra file văn bản .txt hoặc mở cửa sổ In hóa đơn định dạng chuẩn đẹp.

### Ca biên cần xử lý (Edge Cases):
- Khách nhận và trả phòng trong cùng một ngày: **Room Charge Policy** trong `GLOSSARY.md` quy định tính tối thiểu 1 ngày ở.
- Khách không sử dụng dịch vụ nào: Tiền dịch vụ hiển thị `0 đ`, tổng tiền bằng đúng tiền phòng.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Tạo hóa đơn cho phòng 101 (ở 2 ngày đơn giá 500,000 đ, dùng 70,000 đ dịch vụ): Tổng thanh toán chính xác là 1,070,000 đ.
- Bấm "Xuất hóa đơn", mở file `.txt` vừa lưu kiểm tra nội dung đầy đủ, chuẩn font tiếng Việt UTF-8.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-10.md` lên GitHub Issue #63.*
