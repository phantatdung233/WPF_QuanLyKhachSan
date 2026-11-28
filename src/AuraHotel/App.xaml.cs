using System.Windows;
using AuraHotel.Models;
using AuraHotel.Services;
using AuraHotel.ViewModels;
using AuraHotel.Views;

namespace AuraHotel;

public partial class App : Application
{
    private readonly LoginViewModel _loginViewModel = new();

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ShutdownMode = ShutdownMode.OnExplicitShutdown;
        _loginViewModel.Reset();
        ShowLoginWindow();
    }

    private void ShowLoginWindow()
    {
        var shellLaunched = false;
        var loginWindow = new LoginWindow(_loginViewModel);

        void OnLoginSucceeded(User authenticatedUser)
        {
            ShowMainWindow(authenticatedUser);
            shellLaunched = true;
            loginWindow.Close();
        }

        _loginViewModel.LoginSucceeded += OnLoginSucceeded;

        loginWindow.Closed += (_, _) =>
        {
            _loginViewModel.LoginSucceeded -= OnLoginSucceeded;

            if (!shellLaunched)
            {
                _loginViewModel.Cancel();
                Shutdown();
            }
        };

        MainWindow = loginWindow;
        loginWindow.Show();
    }

    private void ShowMainWindow(User authenticatedUser)
    {
        var loggingOut = false;
        var mainViewModel = new MainViewModel(authenticatedUser);
        var mainWindow = new MainWindow(mainViewModel);

        mainViewModel.LogoutRequested += () =>
        {
            loggingOut = true;
            _loginViewModel.Reset();
            ShowLoginWindow();
            mainWindow.Close();
        };

        mainWindow.Closed += (_, _) =>
        {
            if (!loggingOut)
            {
                UserSession.Clear();
                Shutdown();
            }
        };

        MainWindow = mainWindow;
        mainWindow.Show();
    }
}
