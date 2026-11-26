namespace HotelManagement.ViewModels;

public partial class ReportViewModel : ViewModelBase
{
    public ReportViewModel()
    {
        IsEmpty = true;
        EmptyStateTitle = "Chưa có dữ liệu doanh thu";
        EmptyStateSubtitle = "Chỉ số KPI, công suất phòng và biểu đồ doanh thu theo ngày sẽ hiển thị tại đây.";
    }
}
