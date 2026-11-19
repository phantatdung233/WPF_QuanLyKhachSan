using System.Windows;
using System.ComponentModel;
using HotelManagement.Services;
using CommunityToolkit.Mvvm.Input;
using HotelManagement.ViewModels;
using Wpf.Ui.Controls;

namespace HotelManagement.Views;

public partial class MainWindow : FluentWindow
{
    private readonly MainViewModel viewModel;
    private readonly Dictionary<NavigationPage, NavigationViewItem> navigationItems = new();

    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        DataContext = viewModel;
        string? previousGroup = null;
        foreach (var item in viewModel.NavigationItems)
        {
            var group = item.Page switch
            {
                NavigationPage.RoomMap or NavigationPage.Booking => "VẬN HÀNH",
                NavigationPage.Report => "BÁO CÁO",
                _ => "DANH MỤC"
            };
            if (group != previousGroup)
            {
                Sidebar.MenuItems.Add(new NavigationViewItemHeader
                {
                    Text = group,
                    FontSize = 12,
                    Margin = new Thickness(28, 12, 12, 4)
                });
                previousGroup = group;
            }
            var navigationItem = new NavigationViewItem
            {
                Content = item.Title,
                Height = 40,
                Margin = new Thickness(4, 0, 4, 4),
                Icon = item.Page == NavigationPage.Booking
                    ? new ImageIcon { Source = (System.Windows.Media.ImageSource)FindResource("BookingCalendarIcon"), Width = 16, Height = 16 }
                    : new SymbolIcon { Symbol = item.Page switch
                {
                    NavigationPage.RoomMap => SymbolRegular.Grid24,
                    NavigationPage.Room => SymbolRegular.Building24,
                    NavigationPage.RoomType => SymbolRegular.BuildingMultiple24,
                    NavigationPage.Customer => SymbolRegular.People24,
                    NavigationPage.Service => SymbolRegular.Food24,
                    NavigationPage.Invoice => SymbolRegular.Receipt24,
                    _ => SymbolRegular.ChartMultiple24
                } },
                Tag = item.Page,
                IsActive = item == viewModel.SelectedItem,
                Command = new RelayCommand(() => viewModel.Navigate(item.Page))
            };
            navigationItems.Add(item.Page, navigationItem);
            Sidebar.MenuItems.Add(navigationItem);
        }
        viewModel.LoggedOut += OnLoggedOut;
        viewModel.PropertyChanged += OnViewModelChanged;
        Closed += (_, _) =>
        {
            viewModel.LoggedOut -= OnLoggedOut;
            viewModel.PropertyChanged -= OnViewModelChanged;
        };
    }

    private void OnViewModelChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(MainViewModel.SelectedItem)) return;
        foreach (var (page, item) in navigationItems)
            item.IsActive = viewModel.SelectedItem?.Page == page;
    }

    private void OpenAccountMenu(object sender, RoutedEventArgs e)
    {
        var button = (FrameworkElement)sender;
        button.ContextMenu.PlacementTarget = button;
        button.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
        button.ContextMenu.IsOpen = true;
    }

    private void Logout(object sender, RoutedEventArgs e) => viewModel.LogoutCommand.Execute(null);

    private void OnLoggedOut(object? sender, EventArgs e)
    {
        var login = new LoginWindow();
        Application.Current.MainWindow = login;
        login.Show();
        Close();
    }
}
