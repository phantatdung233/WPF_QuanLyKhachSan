using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelManagement.DAO;
using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.Data.SqlClient;

namespace HotelManagement.ViewModels;

public partial class RoomTypeViewModel : ViewModelBase
{
    private const string DefaultLoadingMessage = "Đang tải danh mục loại phòng...";

    private readonly Func<Task<IReadOnlyList<RoomType>>> _getAllAsync;
    private readonly Func<RoomType, Task<int>> _insertAsync;
    private readonly Func<RoomType, Task<bool>> _updateAsync;
    private readonly Func<int, Task<bool>> _deleteAsync;
    private readonly string _fallbackRole;
    private List<RoomType> _allRoomTypes = [];

    public event Action? DialogRequested;

    public override NavigationKey ModuleKey => NavigationKey.RoomType;

    public override string ModuleSubtitle =>
        "Thiết lập danh mục loại phòng, đơn giá cơ bản theo đêm và kiểm soát phòng trực thuộc.";

    public RoomTypeDialogViewModel Dialog { get; }

    public ObservableCollection<RoomTypeItemViewModel> FilteredRoomTypes { get; } = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasActiveSearch))]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _footerSummaryText = "Chưa có loại phòng nào trong danh mục.";

    public bool IsAdministratorAccess =>
        RolePermissions.IsAdministrator(UserSession.CurrentUser?.Role ?? _fallbackRole);

    public bool HasActiveSearch => !string.IsNullOrWhiteSpace(SearchText);

    public bool IsTableVisible => !IsLoading && FilteredRoomTypes.Count > 0;

    public bool IsBusy => IsLoading || Dialog.IsSaving;

    public bool IsNotBusy => !IsBusy;

    public RoomTypeViewModel(string fallbackRole = RolePermissions.Administrator)
        : this(
            getAllAsync: static () => Task.Run(RoomTypeDAO.Instance.GetAll),
            insertAsync: static roomType => Task.Run(() => RoomTypeDAO.Instance.Insert(roomType)),
            updateAsync: static roomType => Task.Run(() => RoomTypeDAO.Instance.Update(roomType)),
            deleteAsync: static roomTypeId => Task.Run(() => RoomTypeDAO.Instance.Delete(roomTypeId)),
            fallbackRole: fallbackRole)
    {
    }

    public RoomTypeViewModel(
        Func<Task<IReadOnlyList<RoomType>>> getAllAsync,
        Func<RoomType, Task<int>> insertAsync,
        Func<RoomType, Task<bool>> updateAsync,
        Func<int, Task<bool>> deleteAsync,
        string fallbackRole = RolePermissions.Administrator)
    {
        ArgumentNullException.ThrowIfNull(getAllAsync);
        ArgumentNullException.ThrowIfNull(insertAsync);
        ArgumentNullException.ThrowIfNull(updateAsync);
        ArgumentNullException.ThrowIfNull(deleteAsync);

        _getAllAsync = getAllAsync;
        _insertAsync = insertAsync;
        _updateAsync = updateAsync;
        _deleteAsync = deleteAsync;
        _fallbackRole = fallbackRole;

        Dialog = new RoomTypeDialogViewModel(
            getExistingRoomTypes: () => _allRoomTypes,
            canMutate: () => !IsLoading && IsAdministratorAccess,
            persistRoomTypeAsync: PersistRoomTypeAsync);
        Dialog.PropertyChanged += OnDialogPropertyChanged;

        LoadingMessage = DefaultLoadingMessage;
        ApplyEmptyCopyForFullCatalog();
        IsEmpty = true;
    }

    partial void OnSearchTextChanged(string value)
    {
        ClearSearchCommand.NotifyCanExecuteChanged();
        ErrorMessage = null;
        SuccessMessage = null;
        ApplyFilter();
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName == nameof(IsLoading))
        {
            OnPropertyChanged(nameof(IsBusy));
            OnPropertyChanged(nameof(IsNotBusy));
            OnPropertyChanged(nameof(IsTableVisible));
            NotifyCommandStates();
        }
    }

    private void OnDialogPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(RoomTypeDialogViewModel.IsSaving))
        {
            OnPropertyChanged(nameof(IsBusy));
            OnPropertyChanged(nameof(IsNotBusy));
            NotifyCommandStates();
        }
    }

    private bool CanLoadRoomTypes() => !IsBusy;

    private bool CanExecuteCatalogAction() => !IsBusy && IsAdministratorAccess;

    private bool CanEditRoomTypeRow(RoomTypeItemViewModel? item) =>
        !IsBusy && IsAdministratorAccess && item is not null;

    private bool CanDeleteRoomTypeRow(RoomTypeItemViewModel? item) =>
        !IsBusy && IsAdministratorAccess && item is not null && item.CanDelete;

    [RelayCommand(CanExecute = nameof(CanLoadRoomTypes))]
    private async Task LoadRoomTypesAsync()
    {
        if (IsBusy || !EnsureAdministratorAccess())
        {
            return;
        }

        ErrorMessage = null;
        SuccessMessage = null;
        LoadingMessage = DefaultLoadingMessage;
        IsLoading = true;
        try
        {
            var items = await _getAllAsync();
            _allRoomTypes = items.ToList();
        }
        catch (Exception)
        {
            ErrorMessage = "Không thể tải danh mục loại phòng. Vui lòng kiểm tra kết nối SQL Server và thử lại.";
        }
        finally
        {
            IsLoading = false;
        }

        ApplyFilter();
    }

    [RelayCommand(CanExecute = nameof(HasActiveSearch))]
    private void ClearSearch()
    {
        SearchText = string.Empty;
    }

    [RelayCommand(CanExecute = nameof(CanExecuteCatalogAction))]
    private void OpenCreateDialog()
    {
        if (!EnsureAdministratorAccess())
        {
            return;
        }

        ErrorMessage = null;
        SuccessMessage = null;
        Dialog.OpenForCreate();
        DialogRequested?.Invoke();
    }

    [RelayCommand(CanExecute = nameof(CanEditRoomTypeRow))]
    private void OpenEditDialog(RoomTypeItemViewModel? item)
    {
        if (item is null || !EnsureAdministratorAccess())
        {
            return;
        }

        ErrorMessage = null;
        SuccessMessage = null;
        Dialog.OpenForEdit(item);
        DialogRequested?.Invoke();
    }

    private async Task PersistRoomTypeAsync(RoomType payload, bool isEditMode)
    {
        ErrorMessage = null;
        SuccessMessage = null;

        if (isEditMode)
        {
            var updated = await _updateAsync(payload);
            if (!updated)
            {
                _allRoomTypes = (await _getAllAsync()).ToList();
                ApplyFilter();
                throw new KeyNotFoundException("Loại phòng không còn tồn tại. Danh sách đã được làm mới.");
            }
        }
        else
        {
            payload.RoomTypeId = await _insertAsync(payload);
        }

        _allRoomTypes = (await _getAllAsync()).ToList();
        SuccessMessage = isEditMode
            ? $"Đã cập nhật loại phòng \"{payload.TypeName}\"."
            : $"Đã thêm loại phòng \"{payload.TypeName}\".";
        ApplyFilter();
    }

    [RelayCommand(CanExecute = nameof(CanDeleteRoomTypeRow))]
    private async Task DeleteRoomTypeAsync(RoomTypeItemViewModel? item)
    {
        if (item is null || IsBusy || !EnsureAdministratorAccess())
        {
            return;
        }

        ErrorMessage = null;
        SuccessMessage = null;

        if (!ValidationRules.CanDeleteRoomType(item.RoomCount))
        {
            ErrorMessage = ValidationRules.GetRoomTypeDeleteLockReason(item.RoomCount);
            ApplyFilter();
            return;
        }

        LoadingMessage = "Đang xóa loại phòng...";
        IsLoading = true;
        try
        {
            var deleted = await _deleteAsync(item.RoomTypeId);
            _allRoomTypes = (await _getAllAsync()).ToList();

            if (!deleted)
            {
                var refreshed = _allRoomTypes.FirstOrDefault(rt => rt.RoomTypeId == item.RoomTypeId);
                ErrorMessage = refreshed is { RoomCount: > 0 }
                    ? ValidationRules.GetRoomTypeDeleteLockReason(refreshed.RoomCount)
                    : $"Không thể xóa loại phòng \"{item.TypeName}\" vì dữ liệu đã thay đổi.";
            }
            else
            {
                SuccessMessage = $"Đã xóa loại phòng \"{item.TypeName}\".";
            }
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            ErrorMessage = $"Không thể xóa loại phòng \"{item.TypeName}\" vì đang có phòng trực thuộc.";
        }
        catch (Exception)
        {
            ErrorMessage = "Không thể xóa loại phòng. Vui lòng kiểm tra kết nối cơ sở dữ liệu và thử lại.";
        }
        finally
        {
            LoadingMessage = DefaultLoadingMessage;
            IsLoading = false;
        }

        ApplyFilter();
    }

    private bool EnsureAdministratorAccess()
    {
        if (IsAdministratorAccess)
        {
            return true;
        }

        ErrorMessage = "Bạn không có quyền truy cập chức năng quản lý loại phòng.";
        IsEmpty = false;
        NotifyCommandStates();
        return false;
    }

    private void ApplyFilter()
    {
        var keyword = SearchText?.Trim() ?? string.Empty;
        var matching = keyword.Length == 0
            ? _allRoomTypes
            : _allRoomTypes.Where(item => MatchesSearch(item, keyword)).ToList();

        FilteredRoomTypes.Clear();
        foreach (var roomType in matching)
        {
            FilteredRoomTypes.Add(new RoomTypeItemViewModel(roomType));
        }

        if (FilteredRoomTypes.Count == 0 && !HasError)
        {
            if (keyword.Length > 0)
            {
                EmptyStateTitle = "Không tìm thấy loại phòng phù hợp";
                EmptyStateSubtitle = $"Không có loại phòng nào khớp với từ khóa \"{keyword}\".";
            }
            else
            {
                ApplyEmptyCopyForFullCatalog();
            }

            IsEmpty = true;
        }
        else
        {
            IsEmpty = false;
        }

        var totalRooms = matching.Sum(item => item.RoomCount);
        FooterSummaryText = _allRoomTypes.Count == 0
            ? "Chưa có loại phòng nào trong danh mục."
            : $"Hiển thị {FilteredRoomTypes.Count} / {_allRoomTypes.Count} loại phòng · Tổng {totalRooms} phòng trực thuộc";

        OnPropertyChanged(nameof(IsTableVisible));
        NotifyCommandStates();
    }

    private void ApplyEmptyCopyForFullCatalog()
    {
        EmptyStateTitle = "Chưa có loại phòng nào";
        EmptyStateSubtitle = "Nhấn \"Thêm loại phòng\" để thiết lập loại phòng và đơn giá cơ bản theo đêm đầu tiên.";
    }

    private void NotifyCommandStates()
    {
        LoadRoomTypesCommand.NotifyCanExecuteChanged();
        ClearSearchCommand.NotifyCanExecuteChanged();
        OpenCreateDialogCommand.NotifyCanExecuteChanged();
        OpenEditDialogCommand.NotifyCanExecuteChanged();
        DeleteRoomTypeCommand.NotifyCanExecuteChanged();
        Dialog.NotifyAccessChanged();
    }

    private static bool MatchesSearch(RoomType roomType, string keyword) =>
        roomType.RoomTypeId.ToString(CultureInfo.InvariantCulture)
            .Contains(keyword, StringComparison.OrdinalIgnoreCase)
        || roomType.TypeName.Contains(keyword, StringComparison.OrdinalIgnoreCase);
}
