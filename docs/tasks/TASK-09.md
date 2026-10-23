# [TASK-09] Module Sơ đồ phòng trực quan (Room Matrix với XAML DataTrigger)

**Part of Spec**: [#77](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/77)  
**Mã Task**: `TASK-09`  
**GitHub Issue**: [#81](https://github.com/phantatdung233/WPF_QuanLyKhachSan/issues/81)  
**Thành viên đảm nhiệm**: **Member C (@ngocthuyen1)**  
**Mức độ ưu tiên**: `High`  
**Nhánh làm việc**: `feature/room-matrix-trigger`  
**Milestone**: Mốc 2: Nghiệp Vụ Cốt Lõi, Sơ Đồ Phòng & Hóa Đơn (Tuần 3-5)  
**Labels**: `advanced`, `ui`, `room-map`, `m2`  
**Blocked by**: #61 (TASK-04)  

---

## 1. Bối cảnh & Mục tiêu kỹ thuật (Architecture & Scope)
- Xây dựng `RoomMapViewModel.cs` và giao diện `RoomMapView.xaml`.
- **Điểm nhấn nâng cao 1**: Sơ đồ phòng trực quan (Room Matrix / Dashboard) hiển thị toàn bộ phòng khách sạn dưới dạng lưới các ô/thẻ (Cards).
- Áp dụng kỹ thuật XAML `Style.Triggers` hoặc `DataTrigger` (hoặc `IValueConverter`) để màu nền của mỗi thẻ phòng tự động đổi màu theo trạng thái thời gian thực:
  - 🟢 **Xanh lá cây (`#48BB78`)**: Phòng trống (Available)
  - 🔴 **Đỏ (`#F56565`)**: Đang có khách ở (Occupied)
  - 🟠 **Cam (`#ED8936`)**: Đã đặt trước (Reserved)
  - ⚫ **Xám (`#A0AEC0`)**: Bảo trì (Maintenance)
- Tương tác trực tiếp trên sơ đồ phòng: Click vào thẻ phòng để xem thông tin, click đúp để mở nhanh tác vụ Đặt phòng hoặc Check-in/Check-out.

---

## 2. Hướng dẫn nghiệp vụ & Luồng xử lý chi tiết (Implementation Details)
- `RoomMapView.xaml`:
  - Thanh chú thích màu sắc (Legend) ở trên cùng:
    - Ô màu Xanh: Phòng trống
    - Ô màu Đỏ: Đang ở
    - Ô màu Cam: Đã đặt
    - Ô màu Xám: Bảo trì
  - Danh sách phòng hiển thị qua `ItemsControl`:
    ```xml
    <ItemsControl ItemsSource="{Binding Rooms}">
        <ItemsControl.ItemsPanel>
            <ItemsPanelTemplate>
                <WrapPanel />
            </ItemsPanelTemplate>
        </ItemsControl.ItemsPanel>
        <ItemsControl.ItemTemplate>
            <DataTemplate>
                <Border Style="{StaticResource RoomCardStyle}">
                    <StackPanel>
                        <TextBlock Text="{Binding RoomNumber}" FontSize="18" FontWeight="Bold" />
                        <TextBlock Text="{Binding TypeName}" />
                        <TextBlock Text="{Binding Status}" FontStyle="Italic" />
                    </StackPanel>
                </Border>
            </DataTemplate>
        </ItemsControl.ItemTemplate>
    </ItemsControl>
    ```
  - Trong `Style` của thẻ phòng, định nghĩa `DataTrigger` theo `Status` để đổi thuộc tính `Background`.
  - Tương tác tiện lợi:
    - Nếu phòng màu Xanh (Trống): Click đúp -> Mở màn hình Đặt phòng với số phòng đã chọn sẵn.
    - Nếu phòng màu Đỏ (Đang ở): Click đúp -> Mở popup Gọi thêm dịch vụ hoặc Tiến hành Check-out.

---

## 3. Tiêu chí nghiệm thu & Ca biên (Acceptance Criteria & Edge Cases)
### Checklist Definition of Done:
- [ ] RoomMapView.xaml: Hiển thị danh sách tất cả các phòng dạng thẻ (Card/Button) xếp theo lưới tự co giãn (WrapPanel hoặc UniformGrid).
- [ ] Áp dụng Style.Triggers hoặc DataTrigger trong XAML để tự động đổi màu thẻ phòng theo trạng thái: Xanh lá (Trống), Đỏ (Đang ở), Cam (Đã đặt), Xám (Bảo trì).
- [ ] Thẻ phòng hiển thị rõ: Số phòng, Loại phòng, Trạng thái.
- [ ] Click vào thẻ phòng: Xem nhanh thông tin chi tiết; click đúp để mở form Đặt phòng hoặc Check-out nhanh.

### Ca biên cần xử lý (Edge Cases):
- Co giãn cửa sổ: `WrapPanel` tự động căn chỉnh số lượng thẻ phòng trên mỗi dòng mà không làm tràn ngang gây mất thẩm mỹ.
- Khi một phòng vừa được Check-in hoặc Check-out thành công ở màn hình khác, quay lại Sơ đồ phòng màu sắc phải được cập nhật ngay lập tức.

---

## 4. Chiến lược kiểm thử (Testing Strategy)
- Mở màn hình Sơ đồ phòng: Thấy toàn bộ danh sách 12 phòng hiển thị đầy đủ các màu xanh, đỏ, cam tương ứng với dữ liệu mẫu trong CSDL.
- Thực hiện Check-in một phòng trống -> Màu thẻ phòng lập tức chuyển từ Xanh lá sang Đỏ.

---

*Tài liệu tự động đồng bộ từ `docs/tasks/TASK-09.md` lên GitHub Issue #81.*
