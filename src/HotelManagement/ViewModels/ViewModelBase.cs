using CommunityToolkit.Mvvm.ComponentModel;

namespace HotelManagement.ViewModels;

public abstract partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotLoading))]
    [NotifyPropertyChangedFor(nameof(IsEmptyStateVisible))]
    private bool _isLoading;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEmptyStateVisible))]
    private bool _isEmpty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string? _errorMessage;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasSuccessMessage))]
    private string? _successMessage;

    [ObservableProperty]
    private string _loadingMessage = "Đang tải dữ liệu...";

    [ObservableProperty]
    private string _emptyStateTitle = "Chưa có dữ liệu";

    [ObservableProperty]
    private string _emptyStateSubtitle = string.Empty;

    public bool IsNotLoading => !IsLoading;

    public bool IsEmptyStateVisible => IsEmpty && !IsLoading;

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    public bool HasSuccessMessage => !string.IsNullOrWhiteSpace(SuccessMessage);
}
