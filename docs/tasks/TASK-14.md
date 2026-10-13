# [TASK-14] Bộ tài nguyên Shared Resources đồng bộ giao diện

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-14`  
**GitHub Issue**: [#67](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/67)  
**Thành viên đảm nhiệm**: **Member C (@ngocthuyen1)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/ui-shared-resources`  
**Milestone**: Mốc 1: Module Độc Lập (Tuần 1-3)  
**Blocked by**: #58 (TASK-01)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng các ResourceDictionary trong `Resources/`:
  - `Colors.xaml`: Bảng màu chuẩn hiện đại (Primary: Deep Blue #1E3A8A, Secondary: Amber #F59E0B, Background: #F3F4F6, Surface: #FFFFFF, TextPrimary: #111827, Status colors cho phòng).
  - `Styles.xaml`: Custom Style cho `Button`, `TextBox`, `PasswordBox`, `ComboBox`, `DataGrid`, `DatePicker`.
  - `Templates.xaml`: ControlTemplate bo tròn góc, shadow hiện đại, hiệu ứng hover mượt mà.
- Tích hợp toàn bộ vào `App.xaml` thông qua `MergedDictionaries`.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- Đảm bảo font chữ đồng bộ (Segoe UI hoặc Inter) hiển thị tiếng Việt có dấu rõ nét, không lỗi font.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] Colors.xaml: Định nghĩa bảng màu chủ đạo hiện đại (Primary, Secondary, Background, Text, Status colors).
- [ ] Styles.xaml: Tùy biến Style đồng bộ cho Button, TextBox, ComboBox, DataGrid, DatePicker.
- [ ] Templates.xaml: Định nghĩa ControlTemplates và Card template chuẩn cho ứng dụng.
- [ ] Tích hợp toàn bộ ResourceDictionary vào App.xaml dùng chung toàn hệ thống.

### Ca biên cần xử lý (Edge Cases):
- Đảm bảo không xảy ra xung đột key (StaticResource key) giữa các file từ điển.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Kiểm tra `App.xaml` nạp tài nguyên thành công không gây Crash khi khởi động.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-14.md` lên GitHub Issue #67.*
