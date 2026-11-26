using HotelManagement.Services;

namespace HotelManagement.ViewModels;

public partial class BookingViewModel : ViewModelBase
{
    public override NavigationKey ModuleKey => NavigationKey.Booking;

    public override string ModuleSubtitle => "Quản lý đặt phòng chống trùng lịch và vận hành nhận — trả phòng.";

    public BookingViewModel()
    {
        IsEmpty = true;
        EmptyStateTitle = "Chưa có lượt đặt phòng nào";
        EmptyStateSubtitle = "Tạo đặt phòng mới để kiểm tra lịch trống và quản lý chu trình nhận — trả phòng.";
    }
}
