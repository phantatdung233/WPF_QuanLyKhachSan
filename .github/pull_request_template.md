## 📌 Mô Tả Thay Đổi & Issue Liên Quan

- **Liên kết Issue**: Closes #
- **Nhánh làm việc**: `feature/...` ➔ `develop`
- **Thành viên thực hiện**: Member ...
- **Reviewer được chỉ định (Bắt buộc theo quy tắc vòng tròn)**:
  - [ ] **Member B** (nếu tác giả PR là **Member A**)
  - [ ] **Member C** (nếu tác giả PR là **Member B**)
  - [ ] **Member A** (nếu tác giả PR là **Member C**)

---

## 🛠 Tóm Tắt Kỹ Thuật

<!-- Mô tả ngắn gọn các thay đổi trong mã nguồn (ViewModel, View, Entity, Service, Resources, v.v.) -->
- 

---

## ✅ Checklist Kiểm Thử Trước Khi Yêu Cầu Review (Tác giả tích chọn)

- [ ] Mã nguồn tuân thủ kiến trúc **MVVM** (`CommunityToolkit.Mvvm`).
- [ ] Code sạch, không có warning/error, đã format naming convention theo chuẩn C#.
- [ ] Nếu có thay đổi CSDL: đã cập nhật `Database/HotelDB.sql` và chạy thử thành công trên SQL Server cục bộ.
- [ ] Đã chạy thử chức năng và đáp ứng đầy đủ checklist **Definition of Done** của Task tương ứng trong `docs/tasks/`.
- [ ] Không có commit thừa, file rác hoặc thông tin nhạy cảm.

---

## 🔍 Checklist Dành Cho Reviewer

- [ ] Code logic đúng nghiệp vụ (kiểm tra tính toán tiền tệ, trùng lịch đặt phòng theo `GLOSSARY.md`, phân quyền).
- [ ] Xử lý ngoại lệ đầy đủ (không crash khi dữ liệu null hoặc sai định dạng).
- [ ] Data Binding và UI hiển thị đúng trên các độ phân giải màn hình.
- [ ] Đồng ý phê duyệt (Approve) và hợp nhất (Merge) vào nhánh `develop`.
