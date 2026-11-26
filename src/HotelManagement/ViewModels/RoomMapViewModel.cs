using HotelManagement.Services;

namespace HotelManagement.ViewModels;

public partial class RoomMapViewModel : ViewModelBase
{
    public override NavigationKey ModuleKey => NavigationKey.RoomMap;

    public override string ModuleSubtitle => "Theo dõi trực quan tình trạng phòng theo thời gian thực.";

    public RoomMapViewModel()
    {
        IsEmpty = true;
        EmptyStateTitle = "Chưa có dữ liệu phòng trên sơ đồ";
        EmptyStateSubtitle = "Sơ đồ phòng trực quan sẽ hiển thị danh sách phòng theo 4 trạng thái vận hành.";
    }
}
