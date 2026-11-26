namespace HotelManagement.ViewModels;

public partial class RoomMapViewModel : ViewModelBase
{
    public RoomMapViewModel()
    {
        IsEmpty = true;
        EmptyStateTitle = "Chưa có dữ liệu phòng trên sơ đồ";
        EmptyStateSubtitle = "Sơ đồ phòng trực quan sẽ hiển thị danh sách phòng theo 4 trạng thái vận hành.";
    }
}
