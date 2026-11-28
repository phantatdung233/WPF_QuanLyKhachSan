using AuraHotel.Models;
using AuraHotel.Services;

namespace AuraHotel.ViewModels;

public sealed class RoomTypeItemViewModel
{
    public RoomType Model { get; }

    public int RoomTypeId => Model.RoomTypeId;

    public string TypeName => Model.TypeName;

    public decimal PricePerDay => Model.PricePerDay;

    public string PricePerDayDisplay { get; }

    public int RoomCount => Model.RoomCount;

    public bool CanDelete { get; }

    public string DeleteToolTip { get; }

    public RoomTypeItemViewModel(RoomType model)
    {
        ArgumentNullException.ThrowIfNull(model);
        Model = model;
        PricePerDayDisplay = CurrencyFormatter.FormatVnd(model.PricePerDay);
        CanDelete = ValidationRules.CanDeleteRoomType(model.RoomCount);
        DeleteToolTip = ValidationRules.GetRoomTypeDeleteLockReason(model.RoomCount) ?? "Xóa loại phòng";
    }
}
