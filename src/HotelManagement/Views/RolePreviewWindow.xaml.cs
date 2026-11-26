using HotelManagement.ViewModels;
using Wpf.Ui.Controls;

namespace HotelManagement.Views;

public partial class RolePreviewWindow : FluentWindow
{
    public RolePreviewWindow()
        : this(new RolePreviewViewModel())
    {
    }

    public RolePreviewWindow(RolePreviewViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
