using HotelManagement.Services;

namespace HotelManagement.ViewModels;

public partial class InvoiceViewModel : ViewModelBase
{
    public override NavigationKey ModuleKey => NavigationKey.Invoice;

    public override string ModuleSubtitle => "Thanh toán hóa đơn trả phòng và xuất biên lai văn bản chuẩn UTF-8.";

    public InvoiceViewModel()
    {
        IsEmpty = true;
        EmptyStateTitle = "Chưa có hóa đơn nào";
        EmptyStateSubtitle = "Hóa đơn tiền phòng và dịch vụ đã sử dụng sẽ được tổng hợp khi khách trả phòng.";
    }
}
