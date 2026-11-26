namespace HotelManagement.ViewModels;

public partial class ServiceViewModel : ViewModelBase
{
    public ServiceViewModel()
    {
        IsEmpty = true;
        EmptyStateTitle = "Chưa có dịch vụ nào";
        EmptyStateSubtitle = "Danh mục dịch vụ khách sạn và đơn giá niêm yết sẽ hiển thị tại đây.";
    }
}
