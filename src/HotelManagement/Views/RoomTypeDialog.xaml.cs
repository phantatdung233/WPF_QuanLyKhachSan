using System.ComponentModel;
using System.Windows;
using HotelManagement.ViewModels;
using Wpf.Ui.Controls;

namespace HotelManagement.Views;

public partial class RoomTypeDialog : FluentWindow
{
    private readonly RoomTypeViewModel _viewModel;
    private bool _isClosingFromViewModel;

    public RoomTypeDialog(RoomTypeViewModel viewModel)
    {
        ArgumentNullException.ThrowIfNull(viewModel);
        _viewModel = viewModel;

        InitializeComponent();
        DataContext = _viewModel;

        _viewModel.DialogCloseRequested += OnDialogCloseRequested;
        Loaded += OnLoaded;
        Closing += OnClosing;
        Closed += OnClosed;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        TypeNameInput.Focus();
        TypeNameInput.CaretIndex = TypeNameInput.Text?.Length ?? 0;
    }

    private void OnClosing(object? sender, CancelEventArgs e)
    {
        if (!_isClosingFromViewModel && _viewModel.IsSaving)
        {
            e.Cancel = true;
        }
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        _viewModel.DialogCloseRequested -= OnDialogCloseRequested;
        Loaded -= OnLoaded;
        Closing -= OnClosing;
        Closed -= OnClosed;
        _viewModel.CloseDialogIfOpen();
    }

    private void OnDialogCloseRequested()
    {
        if (_isClosingFromViewModel)
        {
            return;
        }

        _isClosingFromViewModel = true;
        Close();
    }
}
