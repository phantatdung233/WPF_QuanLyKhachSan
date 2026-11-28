using AuraHotel.Services;

namespace AuraHotel.ViewModels;

public partial class ReportViewModel : ViewModelBase
{
    public override NavigationKey ModuleKey => NavigationKey.Report;

    public override string ModuleSubtitle => "Phân tích chỉ số KPI doanh thu, công suất phòng và xu hướng theo ngày.";

    public ReportViewModel()
    {
        IsEmpty = true;
        EmptyStateTitle = "Chưa có dữ liệu doanh thu";
        EmptyStateSubtitle = "Chỉ số KPI, công suất phòng và biểu đồ doanh thu theo ngày sẽ hiển thị tại đây.";
    }
}
