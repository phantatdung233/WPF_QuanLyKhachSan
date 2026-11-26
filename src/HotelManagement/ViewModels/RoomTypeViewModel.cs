namespace HotelManagement.ViewModels;

public partial class RoomTypeViewModel : ViewModelBase
{
    public RoomTypeViewModel()
    {
        IsEmpty = true;
        EmptyStateTitle = "Chưa có loại phòng nào";
        EmptyStateSubtitle = "Quản trị viên có thể cấu hình danh mục loại phòng, sức chứa và đơn giá theo đêm.";
    }
}
