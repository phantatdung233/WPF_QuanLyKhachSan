using System.Windows;
using HotelManagement.ViewModels;

namespace HotelManagement.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}
