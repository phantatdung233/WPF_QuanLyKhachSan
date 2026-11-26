namespace HotelManagement.ViewModels;

public partial class BookingViewModel : ViewModelBase
{
    public BookingViewModel()
    {
        IsEmpty = true;
        EmptyStateTitle = "Chưa có lượt đặt phòng nào";
        EmptyStateSubtitle = "Tạo đặt phòng mới để kiểm tra lịch trống và quản lý chu trình nhận — trả phòng.";
    }
}
