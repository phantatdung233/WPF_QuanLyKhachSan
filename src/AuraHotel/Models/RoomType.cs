using System.Data;

namespace AuraHotel.Models;

public class RoomType
{
    public int RoomTypeId { get; set; }

    public string TypeName { get; set; } = string.Empty;

    public decimal PricePerDay { get; set; }

    public int RoomCount { get; set; }

    public RoomType()
    {
    }

    public RoomType(DataRow row)
    {
        ArgumentNullException.ThrowIfNull(row);

        RoomTypeId = DataRowReader.GetInt32(row, nameof(RoomTypeId));
        TypeName = DataRowReader.GetString(row, nameof(TypeName));
        PricePerDay = DataRowReader.GetDecimal(row, nameof(PricePerDay));
        RoomCount = DataRowReader.GetOptionalInt32(row, nameof(RoomCount));
    }
}
