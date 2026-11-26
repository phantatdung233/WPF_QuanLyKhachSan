using HotelManagement.Services;

namespace HotelManagement.ViewModels;

public partial class ServiceViewModel : ViewModelBase
{
    public override NavigationKey ModuleKey => NavigationKey.Service;

    public override string ModuleSubtitle => "Quản lý danh mục dịch vụ đi kèm và đơn giá niêm yết tại khách sạn.";

    public ServiceViewModel()
    {
        IsEmpty = true;
        EmptyStateTitle = "Chưa có dịch vụ nào";
        EmptyStateSubtitle = "Danh mục dịch vụ khách sạn và đơn giá niêm yết sẽ hiển thị tại đây.";
    }
}
