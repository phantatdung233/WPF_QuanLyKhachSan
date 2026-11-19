using System.Windows;
using HotelManagement.ViewModels;
using Wpf.Ui.Controls;

namespace HotelManagement.Views;

// TASK-01 preview only. TASK-03 replaces this with real authentication.
public partial class LoginWindow : FluentWindow
{
    public LoginWindow() => InitializeComponent();
    private void PreviewAdministrator(object sender, RoutedEventArgs e) =>
        OpenShell("Trần Minh Quản", "Administrator");
    private void PreviewStaff(object sender, RoutedEventArgs e) =>
        OpenShell("Lê Thu Ngân", "Staff");

    private void OpenShell(string fullName, string role)
    {
        var shell = new MainWindow(new MainViewModel(fullName, role));
        Application.Current.MainWindow = shell;
        shell.Show();
        Close();
    }
}
