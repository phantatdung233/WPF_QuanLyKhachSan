using AuraHotel.Models;

namespace AuraHotel.Services;

public static class ValidationRules
{
    public const int MaxRoomTypeNameLength = 100;
    public const decimal MaxRoomTypePricePerDay = 9_999_999_999_999_999m;

    public static string? ValidateRoomTypeName(
        string? typeName,
        IEnumerable<RoomType> existingRoomTypes,
        int? editingRoomTypeId = null)
    {
        ArgumentNullException.ThrowIfNull(existingRoomTypes);

        var normalized = typeName?.Trim() ?? string.Empty;
        if (normalized.Length == 0)
        {
            return "Vui lòng nhập tên loại phòng.";
        }

        if (normalized.Length > MaxRoomTypeNameLength)
        {
            return $"Tên loại phòng không được vượt quá {MaxRoomTypeNameLength} ký tự.";
        }

        var isDuplicate = existingRoomTypes.Any(item =>
            item.RoomTypeId != editingRoomTypeId
            && string.Equals(item.TypeName.Trim(), normalized, StringComparison.OrdinalIgnoreCase));

        return isDuplicate ? "Tên loại phòng đã tồn tại." : null;
    }

    public static string? ValidateRoomTypePrice(string? rawPrice, out decimal pricePerDay)
    {
        pricePerDay = 0m;
        if (string.IsNullOrWhiteSpace(rawPrice))
        {
            return "Vui lòng nhập đơn giá/đêm.";
        }

        if (!CurrencyFormatter.TryParseVndAmount(rawPrice, out pricePerDay))
        {
            return "Đơn giá/đêm không hợp lệ.";
        }

        return ValidateRoomTypePricePerDay(pricePerDay);
    }

    public static string? ValidateRoomTypePricePerDay(decimal pricePerDay)
    {
        if (pricePerDay <= 0m)
        {
            return "Đơn giá/đêm phải lớn hơn 0.";
        }

        if (decimal.Truncate(pricePerDay) != pricePerDay)
        {
            return "Đơn giá/đêm phải là số nguyên VNĐ.";
        }

        if (pricePerDay > MaxRoomTypePricePerDay)
        {
            return "Đơn giá/đêm vượt quá giới hạn cho phép.";
        }

        return null;
    }

    public static bool CanDeleteRoomType(int roomCount) => roomCount <= 0;

    public static string? GetRoomTypeDeleteLockReason(int roomCount) =>
        roomCount > 0
            ? $"Không thể xóa loại phòng đang có {roomCount} phòng trực thuộc."
            : null;
}
