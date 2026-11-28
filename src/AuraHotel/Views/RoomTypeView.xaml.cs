using System.Windows;
using System.Windows.Controls;
using AuraHotel.ViewModels;

namespace AuraHotel.Views;

public partial class RoomTypeView : UserControl
{
    private RoomTypeViewModel? _subscribedViewModel;
    private bool _isDialogShowing;

    public RoomTypeView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        DataContextChanged += OnDataContextChanged;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        AttachViewModel(DataContext as RoomTypeViewModel);

        if (_subscribedViewModel is { } viewModel && viewModel.LoadRoomTypesCommand.CanExecute(null))
        {
            await viewModel.LoadRoomTypesCommand.ExecuteAsync(null);
        }
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        DetachViewModel();
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (IsLoaded)
        {
            AttachViewModel(e.NewValue as RoomTypeViewModel);
        }
    }

    private void AttachViewModel(RoomTypeViewModel? viewModel)
    {
        if (ReferenceEquals(_subscribedViewModel, viewModel))
        {
            return;
        }

        DetachViewModel();
        _subscribedViewModel = viewModel;
        if (_subscribedViewModel is not null)
        {
            _subscribedViewModel.DialogRequested += OnDialogRequested;
        }
    }

    private void DetachViewModel()
    {
        if (_subscribedViewModel is null)
        {
            return;
        }

        _subscribedViewModel.DialogRequested -= OnDialogRequested;
        _subscribedViewModel = null;
    }

    private void OnDialogRequested()
    {
        if (_isDialogShowing || _subscribedViewModel is not { } viewModel)
        {
            return;
        }

        _isDialogShowing = true;
        try
        {
            var owner = Window.GetWindow(this);
            var dialog = new RoomTypeDialog(viewModel.Dialog);
            if (owner is not null)
            {
                dialog.Owner = owner;
            }
            else
            {
                dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }

            dialog.ShowDialog();
        }
        finally
        {
            _isDialogShowing = false;
        }
    }
}
