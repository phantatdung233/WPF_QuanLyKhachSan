namespace HotelManagement.ViewModels;

public partial class RoomViewModel : ViewModelBase
{
    public RoomViewModel()
    {
        IsEmpty = true;
        EmptyStateTitle = "Chưa có phòng nào";
        EmptyStateSubtitle = "Danh sách phòng, loại phòng tương ứng và trạng thái vận hành sẽ hiển thị tại đây.";
    }
}
