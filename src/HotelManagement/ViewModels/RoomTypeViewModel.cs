using HotelManagement.Services;

namespace HotelManagement.ViewModels;

public partial class RoomTypeViewModel : ViewModelBase
{
    public override NavigationKey ModuleKey => NavigationKey.RoomType;

    public override string ModuleSubtitle => "Thiết lập hạng phòng, sức chứa tiêu chuẩn và đơn giá cơ bản theo đêm.";

    public RoomTypeViewModel()
    {
        IsEmpty = true;
        EmptyStateTitle = "Chưa có loại phòng nào";
        EmptyStateSubtitle = "Quản trị viên có thể cấu hình danh mục loại phòng, sức chứa và đơn giá theo đêm.";
    }
}
