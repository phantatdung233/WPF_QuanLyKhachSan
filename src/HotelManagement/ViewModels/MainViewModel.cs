using CommunityToolkit.Mvvm.Input;
using HotelManagement.Services;

namespace HotelManagement.ViewModels;

public sealed class MainViewModel : ViewModelBase
{
    private readonly Dictionary<NavigationPage, ViewModelBase> views = new();
    private NavigationItem? selectedItem;
    private ViewModelBase? currentView;
    private string sessionDisplay;
    public string FullName { get; private set; }
    public string RoleDisplay { get; private set; }
    private IReadOnlyList<NavigationItem> navigationItems;

    public MainViewModel(string fullName, string role)
    {
        FullName = fullName;
        RoleDisplay = role == "Administrator" ? "Quản trị" : "Lễ tân";
        navigationItems = RolePermissions.GetNavigationItems(role);
        if (navigationItems.Count == 0)
            throw new ArgumentException("Unsupported role.", nameof(role));
        sessionDisplay = $"Xin chào: {fullName} ({(role == "Administrator" ? "Quản trị" : "Lễ tân")})";
        LogoutCommand = new RelayCommand(Logout);
        Navigate(NavigationPage.RoomMap);
    }

    public IReadOnlyList<NavigationItem> NavigationItems => navigationItems;
    public NavigationItem? SelectedItem => selectedItem;
    public ViewModelBase? CurrentView => currentView;
    public string PageTitle => selectedItem?.Title ?? string.Empty;
    public string SessionDisplay => sessionDisplay;
    public IRelayCommand LogoutCommand { get; }
    public event EventHandler? LoggedOut;

    public bool Navigate(NavigationPage page)
    {
        var item = navigationItems.FirstOrDefault(item => item.Page == page);
        if (item is null) return false;
        if (!views.TryGetValue(page, out var view))
        {
            view = page switch
            {
                NavigationPage.RoomMap => new RoomMapViewModel(),
                NavigationPage.Booking => new BookingViewModel(),
                NavigationPage.Room => new RoomViewModel(),
                NavigationPage.RoomType => new RoomTypeViewModel(),
                NavigationPage.Customer => new CustomerViewModel(),
                NavigationPage.Service => new ServiceViewModel(),
                NavigationPage.Invoice => new InvoiceViewModel(),
                NavigationPage.Report => new ReportViewModel(),
                _ => throw new ArgumentOutOfRangeException(nameof(page))
            };
            views.Add(page, view);
        }
        SetProperty(ref selectedItem, item, nameof(SelectedItem));
        SetProperty(ref currentView, view, nameof(CurrentView));
        OnPropertyChanged(nameof(PageTitle));
        return true;
    }

    private void Logout()
    {
        FullName = string.Empty;
        RoleDisplay = string.Empty;
        OnPropertyChanged(nameof(FullName));
        OnPropertyChanged(nameof(RoleDisplay));
        navigationItems = Array.Empty<NavigationItem>();
        views.Clear();
        SetProperty(ref selectedItem, null, nameof(SelectedItem));
        SetProperty(ref currentView, null, nameof(CurrentView));
        SetProperty(ref sessionDisplay, string.Empty, nameof(SessionDisplay));
        OnPropertyChanged(nameof(NavigationItems));
        OnPropertyChanged(nameof(PageTitle));
        LoggedOut?.Invoke(this, EventArgs.Empty);
    }
}
