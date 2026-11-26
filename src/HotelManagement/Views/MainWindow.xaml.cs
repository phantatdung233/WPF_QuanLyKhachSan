using System.ComponentModel;
using System.Windows;
using HotelManagement.Services;
using HotelManagement.ViewModels;
using Wpf.Ui.Controls;

namespace HotelManagement.Views;

public partial class MainWindow : FluentWindow
{
    private readonly MainViewModel _viewModel;
    private readonly Dictionary<NavigationKey, NavigationViewItem> _navigationItemsByKey = [];

    public MainWindow()
        : this(new MainViewModel())
    {
    }

    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        DataContext = _viewModel;

        BuildNavigationItems();
        UpdateActiveNavigationItem();

        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        Closed += OnWindowClosed;
    }

    private void BuildNavigationItems()
    {
        RootNavigationView.MenuItems.Clear();
        _navigationItemsByKey.Clear();

        foreach (var descriptor in _viewModel.NavigationItems)
        {
            var navItem = new NavigationViewItem
            {
                Content = descriptor.Title,
                Tag = descriptor.Key,
                Height = 40,
                Margin = new Thickness(0, 0, 0, 4),
                Icon = new SymbolIcon { Symbol = ResolveSymbol(descriptor.Key) }
            };

            var targetKey = descriptor.Key;
            navItem.Click += (_, _) => _viewModel.NavigateCommand.Execute(targetKey);

            _navigationItemsByKey[targetKey] = navItem;
            RootNavigationView.MenuItems.Add(navItem);
        }
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.SelectedNavigationItem))
        {
            UpdateActiveNavigationItem();
        }
    }

    private void UpdateActiveNavigationItem()
    {
        var activeKey = _viewModel.SelectedNavigationItem?.Key;
        foreach (var (key, item) in _navigationItemsByKey)
        {
            item.IsActive = activeKey == key;
        }
    }

    private void OnWindowClosed(object? sender, EventArgs e)
    {
        _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
        Closed -= OnWindowClosed;
    }

    private static SymbolRegular ResolveSymbol(NavigationKey key) => key switch
    {
        NavigationKey.RoomMap => SymbolRegular.GridDots24,
        NavigationKey.Booking => SymbolRegular.Calendar24,
        NavigationKey.Room => SymbolRegular.Door24,
        NavigationKey.RoomType => SymbolRegular.DoorTag24,
        NavigationKey.Customer => SymbolRegular.PeopleCommunity24,
        NavigationKey.Service => SymbolRegular.DrinkCoffee24,
        NavigationKey.Invoice => SymbolRegular.ReceiptMoney24,
        NavigationKey.Report => SymbolRegular.DataBarVertical24,
        _ => SymbolRegular.GridDots24
    };
}
