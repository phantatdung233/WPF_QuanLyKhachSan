using HotelManagement.Services;

namespace HotelManagement.ViewModels;

public partial class RoomViewModel : ViewModelBase
{
    public override NavigationKey ModuleKey => NavigationKey.Room;

    public override string ModuleSubtitle => "Tra cứu và cập nhật danh sách phòng cùng trạng thái vận hành.";

    public RoomViewModel()
    {
        IsEmpty = true;
        EmptyStateTitle = "Chưa có phòng nào";
        EmptyStateSubtitle = "Danh sách phòng, loại phòng tương ứng và trạng thái vận hành sẽ hiển thị tại đây.";
    }
}
