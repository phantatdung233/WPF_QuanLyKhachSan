using HotelManagement.Services;

namespace HotelManagement.ViewModels;

public partial class CustomerViewModel : ViewModelBase
{
    public override NavigationKey ModuleKey => NavigationKey.Customer;

    public override string ModuleSubtitle => "Lưu trữ và tìm kiếm hồ sơ khách lưu trú theo CCCD/CMND hoặc số điện thoại.";

    public CustomerViewModel()
    {
        IsEmpty = true;
        EmptyStateTitle = "Chưa có khách hàng nào";
        EmptyStateSubtitle = "Thêm hồ sơ khách hàng với họ tên, CCCD/CMND và số điện thoại để phục vụ đặt phòng.";
    }
}
