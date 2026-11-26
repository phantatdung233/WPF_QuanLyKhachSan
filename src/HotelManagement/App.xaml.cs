using System.Windows;
using HotelManagement.ViewModels;
using HotelManagement.Views;

namespace HotelManagement;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ShutdownMode = ShutdownMode.OnExplicitShutdown;
        ShowRolePreviewWindow();
    }

    private void ShowRolePreviewWindow()
    {
        var shellLaunched = false;
        var previewViewModel = new RolePreviewViewModel();
        var previewWindow = new RolePreviewWindow(previewViewModel);

        previewViewModel.SessionSelected += session =>
        {
            shellLaunched = true;
            ShowMainWindow(session);
            previewWindow.Close();
        };

        previewWindow.Closed += (_, _) =>
        {
            if (!shellLaunched)
            {
                Shutdown();
            }
        };

        MainWindow = previewWindow;
        previewWindow.Show();
    }

    private void ShowMainWindow(PreviewSession session)
    {
        var loggingOut = false;
        var mainViewModel = new MainViewModel(session.FullName, session.Role);
        var mainWindow = new MainWindow(mainViewModel);

        mainViewModel.LogoutRequested += () =>
        {
            loggingOut = true;
            ShowRolePreviewWindow();
            mainWindow.Close();
        };

        mainWindow.Closed += (_, _) =>
        {
            if (!loggingOut)
            {
                Shutdown();
            }
        };

        MainWindow = mainWindow;
        mainWindow.Show();
    }
}
