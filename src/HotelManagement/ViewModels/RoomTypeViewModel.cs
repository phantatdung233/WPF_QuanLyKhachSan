using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;
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
    private bool _suppressSearchSideEffects;

    public event Action? DialogRequested;

    public event Action? DialogCloseRequested;

    public override NavigationKey ModuleKey => NavigationKey.RoomType;

    public override string ModuleSubtitle =>
        "Thiết lập danh mục loại phòng, đơn giá cơ bản theo đêm và kiểm soát phòng trực thuộc.";

    public ObservableCollection<RoomTypeItemViewModel> FilteredRoomTypes { get; } = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasActiveSearch))]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _footerSummaryText = "Chưa có loại phòng nào trong danh mục.";

    [ObservableProperty]
    private bool _isDialogOpen;

    [ObservableProperty]
    private bool _isEditMode;

    [ObservableProperty]
    private int? _editingRoomTypeId;

    [ObservableProperty]
    private string _dialogTitle = "Thêm loại phòng";

    [ObservableProperty]
    private string _dialogSaveButtonText = "Thêm loại phòng";

    [ObservableProperty]
    private string _dialogTypeName = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DialogPricePreview))]
    private string _dialogPriceInput = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasTypeNameError))]
    private string? _typeNameError;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasPricePerDayError))]
    private string? _pricePerDayError;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasDialogError))]
    private string? _dialogErrorMessage;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotSaving))]
    [NotifyPropertyChangedFor(nameof(IsBusy))]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isSaving;

    public bool IsAdministratorAccess =>
        RolePermissions.IsAdministrator(UserSession.CurrentUser?.Role ?? _fallbackRole);

    public bool HasActiveSearch => !string.IsNullOrWhiteSpace(SearchText);

    public bool IsTableVisible => !IsLoading && FilteredRoomTypes.Count > 0;

    public bool IsErrorRetryVisible => !IsLoading && HasError && FilteredRoomTypes.Count == 0;

    public bool HasTypeNameError => !string.IsNullOrWhiteSpace(TypeNameError);

    public bool HasPricePerDayError => !string.IsNullOrWhiteSpace(PricePerDayError);

    public bool HasDialogError => !string.IsNullOrWhiteSpace(DialogErrorMessage);

    public bool IsNotSaving => !IsSaving;

    public bool IsBusy => IsLoading || IsSaving;

    public bool IsNotBusy => !IsBusy;

    public string DialogPricePreview =>
        CurrencyFormatter.TryParseVndAmount(DialogPriceInput, out var parsed) && parsed > 0m
            ? $"Định dạng hiển thị: {CurrencyFormatter.FormatVnd(parsed)}"
            : "Nhập đơn giá lớn hơn 0 (ví dụ: 500000 hoặc 500.000).";

    public RoomTypeViewModel()
        : this(RolePermissions.Administrator)
    {
    }

    public RoomTypeViewModel(string fallbackRole)
        : this(
            getAllAsync: static () => Task.Run(RoomTypeDAO.Instance.GetAll),
            insertAsync: static roomType => Task.Run(() => RoomTypeDAO.Instance.Insert(roomType)),
            updateAsync: static roomType => Task.Run(() => RoomTypeDAO.Instance.Update(roomType)),
            deleteAsync: static roomTypeId => Task.Run(() => RoomTypeDAO.Instance.Delete(roomTypeId)),
            fallbackRole: fallbackRole)
    {
    }

    public RoomTypeViewModel(RoomTypeDAO dao, string fallbackRole = RolePermissions.Administrator)
        : this(
            getAllAsync: () => Task.FromResult((dao ?? throw new ArgumentNullException(nameof(dao))).GetAll()),
            insertAsync: roomType => Task.FromResult(dao.Insert(roomType)),
            updateAsync: roomType => Task.FromResult(dao.Update(roomType)),
            deleteAsync: roomTypeId => Task.FromResult(dao.Delete(roomTypeId)),
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

        LoadingMessage = DefaultLoadingMessage;
        ApplyEmptyCopyForFullCatalog();
        IsEmpty = true;
    }

    partial void OnSearchTextChanged(string value)
    {
        ClearSearchCommand.NotifyCanExecuteChanged();
        if (_suppressSearchSideEffects)
        {
            return;
        }

        SuccessMessage = null;
        ApplyFilter();
    }

    partial void OnDialogTypeNameChanged(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            TypeNameError = null;
        }

        DialogErrorMessage = null;
    }

    partial void OnDialogPriceInputChanged(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            PricePerDayError = null;
        }

        DialogErrorMessage = null;
    }

    partial void OnIsSavingChanged(bool value) => NotifyCommandStates();

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName == nameof(IsLoading))
        {
            OnPropertyChanged(nameof(IsBusy));
            OnPropertyChanged(nameof(IsNotBusy));
            OnPropertyChanged(nameof(IsTableVisible));
            OnPropertyChanged(nameof(IsErrorRetryVisible));
            NotifyCommandStates();
        }
        else if (e.PropertyName == nameof(ErrorMessage))
        {
            OnPropertyChanged(nameof(IsErrorRetryVisible));
        }
    }

    private bool CanLoadRoomTypes() => !IsBusy;

    private bool CanExecuteCatalogAction() => !IsBusy && IsAdministratorAccess;

    private bool CanEditRoomTypeRow(RoomTypeItemViewModel? item) =>
        !IsBusy && IsAdministratorAccess && item is not null;

    private bool CanDeleteRoomTypeRow(RoomTypeItemViewModel? item) =>
        !IsBusy && IsAdministratorAccess && item is not null && item.CanDelete;

    private bool CanSaveRoomType() => !IsBusy && IsAdministratorAccess;

    [RelayCommand(CanExecute = nameof(CanLoadRoomTypes))]
    private async Task LoadRoomTypesAsync()
    {
        if (IsBusy)
        {
            return;
        }

        if (!EnsureAdministratorAccess())
        {
            return;
        }

        ErrorMessage = null;
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
        IsEditMode = false;
        EditingRoomTypeId = null;
        DialogTitle = "Thêm loại phòng";
        DialogSaveButtonText = "Thêm loại phòng";
        DialogTypeName = string.Empty;
        DialogPriceInput = string.Empty;
        ResetDialogErrors();
        IsDialogOpen = true;
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
        IsEditMode = true;
        EditingRoomTypeId = item.RoomTypeId;
        DialogTitle = "Cập nhật loại phòng";
        DialogSaveButtonText = "Lưu thay đổi";
        DialogTypeName = item.TypeName;
        DialogPriceInput = item.PricePerDay.ToString("0.##", CultureInfo.InvariantCulture);
        ResetDialogErrors();
        IsDialogOpen = true;
        DialogRequested?.Invoke();
    }

    [RelayCommand(CanExecute = nameof(CanSaveRoomType))]
    private async Task SaveRoomTypeAsync()
    {
        if (IsBusy || !EnsureAdministratorAccess())
        {
            return;
        }

        DialogErrorMessage = null;
        ErrorMessage = null;
        SuccessMessage = null;

        TypeNameError = ValidationRules.ValidateRoomTypeName(
            DialogTypeName,
            _allRoomTypes,
            IsEditMode ? EditingRoomTypeId : null);
        PricePerDayError = ValidationRules.ValidateRoomTypePrice(DialogPriceInput, out var parsedPrice);

        if (HasTypeNameError || HasPricePerDayError)
        {
            return;
        }

        var normalizedName = DialogTypeName.Trim();
        var wasEditMode = IsEditMode;
        var payload = new RoomType
        {
            RoomTypeId = EditingRoomTypeId ?? 0,
            TypeName = normalizedName,
            PricePerDay = parsedPrice
        };

        IsSaving = true;
        try
        {
            if (wasEditMode)
            {
                var updated = await _updateAsync(payload);
                if (!updated)
                {
                    _allRoomTypes = (await _getAllAsync()).ToList();
                    ApplyFilter();
                    DialogErrorMessage = "Loại phòng không còn tồn tại. Danh sách đã được làm mới.";
                    return;
                }
            }
            else
            {
                payload.RoomTypeId = await _insertAsync(payload);
                if (HasActiveSearch && !MatchesSearch(payload, SearchText.Trim()))
                {
                    _suppressSearchSideEffects = true;
                    SearchText = string.Empty;
                    _suppressSearchSideEffects = false;
                }
            }

            _allRoomTypes = (await _getAllAsync()).ToList();
            IsDialogOpen = false;
            ResetDialogErrors();
            SuccessMessage = wasEditMode
                ? $"Đã cập nhật loại phòng \"{normalizedName}\"."
                : $"Đã thêm loại phòng \"{normalizedName}\".";
            ApplyFilter();
        }
        catch (SqlException ex) when (ex.Number is 2601 or 2627)
        {
            TypeNameError = "Tên loại phòng đã tồn tại.";
            return;
        }
        catch (Exception)
        {
            DialogErrorMessage = "Không thể lưu thông tin loại phòng. Vui lòng kiểm tra kết nối cơ sở dữ liệu và thử lại.";
            return;
        }
        finally
        {
            IsSaving = false;
        }

        DialogCloseRequested?.Invoke();
    }

    [RelayCommand(CanExecute = nameof(IsNotSaving))]
    private void CancelDialog()
    {
        if (IsSaving)
        {
            return;
        }

        IsDialogOpen = false;
        ResetDialogErrors();
        DialogCloseRequested?.Invoke();
    }

    public void CloseDialogIfOpen()
    {
        if (IsSaving || !IsDialogOpen)
        {
            return;
        }

        IsDialogOpen = false;
        ResetDialogErrors();
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

    private void ResetDialogErrors()
    {
        TypeNameError = null;
        PricePerDayError = null;
        DialogErrorMessage = null;
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
        OnPropertyChanged(nameof(IsErrorRetryVisible));
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
        SaveRoomTypeCommand.NotifyCanExecuteChanged();
        CancelDialogCommand.NotifyCanExecuteChanged();
        DeleteRoomTypeCommand.NotifyCanExecuteChanged();
    }

    private static bool MatchesSearch(RoomType roomType, string keyword)
    {
        if (roomType.RoomTypeId.ToString(CultureInfo.InvariantCulture)
            .Contains(keyword, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (roomType.TypeName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return ContainsAsciiOnly(keyword)
            && FoldVietnameseDiacritics(roomType.TypeName)
                .Contains(FoldVietnameseDiacritics(keyword), StringComparison.OrdinalIgnoreCase);
    }

    private static bool ContainsAsciiOnly(string value)
    {
        foreach (var ch in value)
        {
            if (ch > 127)
            {
                return false;
            }
        }

        return true;
    }

    private static string FoldVietnameseDiacritics(string input)
    {
        var decomposed = input.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(decomposed.Length);

        foreach (var ch in decomposed)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(ch) == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            builder.Append(ch switch
            {
                'đ' => 'd',
                'Đ' => 'D',
                _ => ch
            });
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }
}
