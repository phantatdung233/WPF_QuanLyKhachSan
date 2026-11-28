## 📌 Mô Tả Thay Đổi & Issue Liên Quan

- **Liên kết Issue**: Closes #
- **Nhánh làm việc**: `feature/...` ➔ `develop`
- **Thành viên thực hiện**: Member ...
- **Reviewer được chỉ định (Bắt buộc theo quy tắc vòng tròn)**:
  - [ ] **Member B** (nếu tác giả PR là **Member A**)
  - [ ] **Member C** (nếu tác giả PR là **Member B**)
  - [ ] **Member A** (nếu tác giả PR là **Member C**)

---

## 🛠 Tóm Tắt Kỹ Thuật (2-Phase Execution)

- **Pha 1 (Logic, Data & ViewModel State)**:
  - 
- **Pha 2 (UI Assembly theo `DESIGN.md`)**:
  - 

---

## ✅ Checklist Kiểm Thử Trước Khi Yêu Cầu Review (Tác giả tích chọn)

- [ ] **Pha 1 hoàn tất**: `Models` / `DAO` / `Services` (TDD seam) / `ViewModel` chạy `dotnet build` & `dotnet test` xanh 100%, đã bao phủ đủ **4 trạng thái UI** (`Loading`, `Empty`, `Error`, `Success`).
- [ ] **Pha 2 tuân thủ `DESIGN.md`**: Chỉ dùng primitives của `WPF-UI` / `LiveCharts2` + `{StaticResource}` tokens; **0** hex màu hardcode ngoài `Colors.xaml`, **0** `ControlTemplate` viết đè Button/Input cơ bản, spacing theo lưới `4px/8px`, tối đa **1 Primary CTA** mỗi màn/dialog.
- [ ] Nếu có thay đổi CSDL: đã cập nhật `Database/AuraHotelDb.sql` và chạy thử thành công trên SQL Server cục bộ.
- [ ] Đáp ứng đầy đủ **Acceptance Criteria** & **UI Contract** trong GitHub Issue tương ứng (và tôn trọng ranh giới **Out of Scope**).
- [ ] Không có commit thừa, file rác hoặc thông tin nhạy cảm.

---

## 🔍 Checklist Dành Cho Reviewer

- [ ] Code logic đúng bất biến nghiệp vụ trong `GLOSSARY.md` (chống trùng lịch, tính tối thiểu 1 đêm, Settlement Invariant, phân quyền).
- [ ] Xử lý đủ 4 trạng thái (`Loading`, `Empty`, `Error`, `Success`) và khóa xóa an toàn kèm `ToolTip`.
- [ ] Kiểm tra hiển thị ở `1280×800` và `1280×720` không bị cắt chữ, vỡ khung hay cuộn ngang thừa.
- [ ] Đồng ý phê duyệt (Approve) và hợp nhất (Merge) vào nhánh `develop`.
