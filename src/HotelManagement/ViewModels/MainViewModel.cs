using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelManagement.Services;

namespace HotelManagement.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly Dictionary<NavigationKey, Func<ViewModelBase>> _viewFactories = new()
    {
        [NavigationKey.RoomMap] = static () => new RoomMapViewModel(),
        [NavigationKey.Booking] = static () => new BookingViewModel(),
        [NavigationKey.Room] = static () => new RoomViewModel(),
        [NavigationKey.RoomType] = static () => new RoomTypeViewModel(),
        [NavigationKey.Customer] = static () => new CustomerViewModel(),
        [NavigationKey.Service] = static () => new ServiceViewModel(),
        [NavigationKey.Invoice] = static () => new InvoiceViewModel(),
        [NavigationKey.Report] = static () => new ReportViewModel()
    };

    private readonly Dictionary<NavigationKey, ViewModelBase> _viewCache = [];
    private bool _isUpdatingSelection;

    public event Action? LogoutRequested;

    public string Role { get; }

    public string RoleDisplay { get; }

    public IReadOnlyList<NavigationItemDescriptor> NavigationItems { get; }

    [ObservableProperty]
    private string _fullName;

    [ObservableProperty]
    private string _pageTitle = "Sơ đồ phòng";

    [ObservableProperty]
    private string _pageSubtitle = string.Empty;

    [ObservableProperty]
    private ViewModelBase _currentView = null!;

    [ObservableProperty]
    private NavigationItemDescriptor? _selectedNavigationItem;

    [ObservableProperty]
    private bool _isSessionActive = true;

    public MainViewModel(
        string fullName = "Quản trị viên",
        string role = RolePermissions.Administrator)
    {
        Role = RolePermissions.IsAdministrator(role)
            ? RolePermissions.Administrator
            : RolePermissions.Staff;
        RoleDisplay = RolePermissions.GetRoleDisplay(Role);
        _fullName = string.IsNullOrWhiteSpace(fullName)
            ? (Role == RolePermissions.Administrator ? "Quản trị viên" : "Lễ tân trực ca")
            : fullName.Trim();

        NavigationItems = RolePermissions.GetAllowedNavigationItems(Role);
        Navigate(NavigationKey.RoomMap);
    }

    partial void OnSelectedNavigationItemChanged(NavigationItemDescriptor? value)
    {
        if (_isUpdatingSelection || value is null)
        {
            return;
        }

        Navigate(value.Key);
    }

    [RelayCommand]
    private void Navigate(NavigationKey key)
    {
        if (!RolePermissions.CanAccess(Role, key))
        {
            ErrorMessage = "Bạn không có quyền truy cập chức năng này.";
            return;
        }

        ErrorMessage = null;

        var descriptor = NavigationItems.First(item => item.Key == key);
        if (!_viewCache.TryGetValue(key, out var viewModel))
        {
            viewModel = _viewFactories[key]();
            _viewCache[key] = viewModel;
        }

        CurrentView = viewModel;
        PageTitle = descriptor.Title;
        PageSubtitle = viewModel.ModuleSubtitle;

        _isUpdatingSelection = true;
        SelectedNavigationItem = descriptor;
        _isUpdatingSelection = false;
    }

    [RelayCommand]
    private void Logout()
    {
        if (!IsSessionActive)
        {
            return;
        }

        IsSessionActive = false;
        LogoutRequested?.Invoke();
    }
}
