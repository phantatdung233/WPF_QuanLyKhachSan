using System.Windows;
using HotelManagement.ViewModels;
using Wpf.Ui.Controls;

namespace HotelManagement.Views;

public partial class LoginWindow : FluentWindow
{
    private readonly LoginViewModel _viewModel;
    private bool _isSynchronizingPassword;

    public LoginWindow()
        : this(new LoginViewModel())
    {
    }

    public LoginWindow(LoginViewModel viewModel)
    {
        ArgumentNullException.ThrowIfNull(viewModel);
        _viewModel = viewModel;

        InitializeComponent();
        DataContext = _viewModel;

        _viewModel.PasswordClearRequested += OnPasswordClearRequested;
        Loaded += OnLoaded;
        Closed += OnClosed;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        UsernameInput.Focus();
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        _viewModel.PasswordClearRequested -= OnPasswordClearRequested;
        Loaded -= OnLoaded;
        Closed -= OnClosed;
    }

    private void OnPasswordInputPasswordChanged(object sender, RoutedEventArgs e)
    {
        if (_isSynchronizingPassword)
        {
            return;
        }

        if (!string.Equals(_viewModel.Password, PasswordInput.Password, StringComparison.Ordinal))
        {
            _viewModel.Password = PasswordInput.Password;
        }
    }

    private void OnPasswordClearRequested()
    {
        _isSynchronizingPassword = true;
        try
        {
            PasswordInput.Password = string.Empty;
            PasswordInput.Clear();
        }
        finally
        {
            _isSynchronizingPassword = false;
        }
    }
}
