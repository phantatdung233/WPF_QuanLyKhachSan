using CommunityToolkit.Mvvm.Input;

namespace HotelManagement.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private object currentView;

    public MainViewModel()
    {
        // View mặc định khi mở app (ca biên TASK-01: không để cửa sổ trống).
        CurrentView = new RoomMapViewModel();
    }

    [RelayCommand]
    private void ShowRoomMap() => CurrentView = new RoomMapViewModel();

    [RelayCommand]
    private void ShowBooking() => CurrentView = new BookingViewModel();

    [RelayCommand]
    private void ShowRooms() => CurrentView = new RoomViewModel();

    [RelayCommand]
    private void ShowCustomers() => CurrentView = new CustomerViewModel();

    [RelayCommand]
    private void ShowServices() => CurrentView = new ServiceViewModel();

    [RelayCommand]
    private void ShowInvoices() => CurrentView = new InvoiceViewModel();

    [RelayCommand]
    private void ShowReports() => CurrentView = new ReportViewModel();

    [RelayCommand]
    private void Logout()
    {
        // TASK-03 sẽ đấu dây đăng xuất thật (đóng MainWindow, mở LoginWindow).
    }
}
