namespace HotelManagement.ViewModels;

public partial class InvoiceViewModel : ViewModelBase
{
    public InvoiceViewModel()
    {
        IsEmpty = true;
        EmptyStateTitle = "Chưa có hóa đơn nào";
        EmptyStateSubtitle = "Hóa đơn tiền phòng và dịch vụ đã sử dụng sẽ được tổng hợp khi khách trả phòng.";
    }
}
