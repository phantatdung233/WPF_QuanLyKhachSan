using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelManagement.Services;

namespace HotelManagement.ViewModels;

public sealed record PreviewSession(string FullName, string Role);

public partial class RolePreviewViewModel : ViewModelBase
{
    public event Action<PreviewSession>? SessionSelected;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsStaffSelected))]
    [NotifyPropertyChangedFor(nameof(SelectedRole))]
    [NotifyPropertyChangedFor(nameof(SelectedFullName))]
    [NotifyPropertyChangedFor(nameof(SelectedScopeDescription))]
    private bool _isAdministratorSelected = true;

    public bool IsStaffSelected
    {
        get => !IsAdministratorSelected;
        set => IsAdministratorSelected = !value;
    }

    public string SelectedRole =>
        IsAdministratorSelected ? RolePermissions.Administrator : RolePermissions.Staff;

    public string SelectedFullName =>
        IsAdministratorSelected ? "Quản trị viên" : "Lễ tân trực ca";

    public string SelectedScopeDescription =>
        IsAdministratorSelected
            ? "Toàn quyền 8 mục điều hướng, bao gồm Loại phòng và Báo cáo doanh thu."
            : "6 mục vận hành tiếp tân hàng ngày, tự động ẩn Loại phòng và Báo cáo.";

    [RelayCommand]
    private void EnterShell()
    {
        if (IsLoading)
        {
            return;
        }

        SessionSelected?.Invoke(new PreviewSession(SelectedFullName, SelectedRole));
    }
}
