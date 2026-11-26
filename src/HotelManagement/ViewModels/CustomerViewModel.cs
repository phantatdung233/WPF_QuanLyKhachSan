namespace HotelManagement.ViewModels;

public partial class CustomerViewModel : ViewModelBase
{
    public CustomerViewModel()
    {
        IsEmpty = true;
        EmptyStateTitle = "Chưa có khách hàng nào";
        EmptyStateSubtitle = "Thêm hồ sơ khách hàng với họ tên, CCCD/CMND và số điện thoại để phục vụ đặt phòng.";
    }
}
