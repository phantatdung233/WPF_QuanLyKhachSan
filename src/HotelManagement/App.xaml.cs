using System.Windows;
using HotelManagement.Models;
using HotelManagement.Services;
using HotelManagement.ViewModels;
using HotelManagement.Views;

namespace HotelManagement;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ShutdownMode = ShutdownMode.OnExplicitShutdown;
        UserSession.Clear();
        ShowLoginWindow();
    }

    private void ShowLoginWindow()
    {
        var shellLaunched = false;
        var loginClosed = false;
        var loginViewModel = new LoginViewModel();
        var loginWindow = new LoginWindow(loginViewModel);

        void OnLoginSucceeded(User authenticatedUser)
        {
            if (loginClosed)
            {
                UserSession.Clear();
                return;
            }

            shellLaunched = true;
            ShowMainWindow(authenticatedUser);
            loginWindow.Close();
        }

        loginViewModel.LoginSucceeded += OnLoginSucceeded;

        loginWindow.Closed += (_, _) =>
        {
            loginClosed = true;
            loginViewModel.LoginSucceeded -= OnLoginSucceeded;

            if (!shellLaunched)
            {
                UserSession.Clear();
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
            UserSession.Clear();
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
