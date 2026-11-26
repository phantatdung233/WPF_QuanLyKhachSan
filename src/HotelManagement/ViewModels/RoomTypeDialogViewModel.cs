using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.Data.SqlClient;

namespace HotelManagement.ViewModels;

public sealed partial class RoomTypeDialogViewModel : ObservableObject
{
    private readonly Func<IReadOnlyList<RoomType>> _getExistingRoomTypes;
    private readonly Func<bool> _canMutate;
    private readonly Func<RoomType, bool, Task> _persistRoomTypeAsync;

    public event Action? CloseRequested;

    [ObservableProperty]
    private bool _isOpen;

    [ObservableProperty]
    private bool _isEditMode;

    [ObservableProperty]
    private int? _editingRoomTypeId;

    [ObservableProperty]
    private string _title = "Thêm loại phòng";

    [ObservableProperty]
    private string _saveButtonText = "Thêm loại phòng";

    [ObservableProperty]
    private string _typeName = string.Empty;

    [ObservableProperty]
    private string _priceInput = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasTypeNameError))]
    private string? _typeNameError;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasPricePerDayError))]
    private string? _pricePerDayError;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string? _errorMessage;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotSaving))]
    private bool _isSaving;

    public bool HasTypeNameError => !string.IsNullOrWhiteSpace(TypeNameError);

    public bool HasPricePerDayError => !string.IsNullOrWhiteSpace(PricePerDayError);

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    public bool IsNotSaving => !IsSaving;

    public RoomTypeDialogViewModel(
        Func<IReadOnlyList<RoomType>> getExistingRoomTypes,
        Func<bool> canMutate,
        Func<RoomType, bool, Task> persistRoomTypeAsync)
    {
        ArgumentNullException.ThrowIfNull(getExistingRoomTypes);
        ArgumentNullException.ThrowIfNull(canMutate);
        ArgumentNullException.ThrowIfNull(persistRoomTypeAsync);

        _getExistingRoomTypes = getExistingRoomTypes;
        _canMutate = canMutate;
        _persistRoomTypeAsync = persistRoomTypeAsync;
    }

    partial void OnTypeNameChanged(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            TypeNameError = null;
        }

        ErrorMessage = null;
    }

    partial void OnPriceInputChanged(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            PricePerDayError = null;
        }

        ErrorMessage = null;
    }

    partial void OnIsSavingChanged(bool value)
    {
        SaveCommand.NotifyCanExecuteChanged();
        CancelCommand.NotifyCanExecuteChanged();
    }

    public void OpenForCreate()
    {
        IsEditMode = false;
        EditingRoomTypeId = null;
        Title = "Thêm loại phòng";
        SaveButtonText = "Thêm loại phòng";
        TypeName = string.Empty;
        PriceInput = string.Empty;
        ClearErrors();
        IsOpen = true;
    }

    public void OpenForEdit(RoomTypeItemViewModel item)
    {
        ArgumentNullException.ThrowIfNull(item);

        IsEditMode = true;
        EditingRoomTypeId = item.RoomTypeId;
        Title = "Cập nhật loại phòng";
        SaveButtonText = "Lưu thay đổi";
        TypeName = item.TypeName;
        PriceInput = item.PricePerDay.ToString("0.##", CultureInfo.InvariantCulture);
        ClearErrors();
        IsOpen = true;
    }

    private bool CanSave() => !IsSaving && _canMutate();

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync()
    {
        if (IsSaving || !_canMutate())
        {
            return;
        }

        ErrorMessage = null;
        TypeNameError = ValidationRules.ValidateRoomTypeName(
            TypeName,
            _getExistingRoomTypes(),
            IsEditMode ? EditingRoomTypeId : null);
        PricePerDayError = ValidationRules.ValidateRoomTypePrice(PriceInput, out var parsedPrice);

        if (HasTypeNameError || HasPricePerDayError)
        {
            return;
        }

        var payload = new RoomType
        {
            RoomTypeId = EditingRoomTypeId ?? 0,
            TypeName = TypeName.Trim(),
            PricePerDay = parsedPrice
        };

        IsSaving = true;
        try
        {
            await _persistRoomTypeAsync(payload, IsEditMode);
            IsOpen = false;
            ClearErrors();
        }
        catch (SqlException ex) when (ex.Number is 2601 or 2627)
        {
            TypeNameError = "Tên loại phòng đã tồn tại.";
            return;
        }
        catch (KeyNotFoundException ex)
        {
            ErrorMessage = ex.Message;
            return;
        }
        catch (Exception)
        {
            ErrorMessage = "Không thể lưu thông tin loại phòng. Vui lòng kiểm tra kết nối cơ sở dữ liệu và thử lại.";
            return;
        }
        finally
        {
            IsSaving = false;
        }

        CloseRequested?.Invoke();
    }

    [RelayCommand(CanExecute = nameof(IsNotSaving))]
    private void Cancel()
    {
        if (IsSaving)
        {
            return;
        }

        IsOpen = false;
        ClearErrors();
        CloseRequested?.Invoke();
    }

    public void CloseIfOpen()
    {
        if (IsSaving || !IsOpen)
        {
            return;
        }

        IsOpen = false;
        ClearErrors();
    }

    public void NotifyAccessChanged()
    {
        SaveCommand.NotifyCanExecuteChanged();
        CancelCommand.NotifyCanExecuteChanged();
    }

    private void ClearErrors()
    {
        TypeNameError = null;
        PricePerDayError = null;
        ErrorMessage = null;
    }
}
