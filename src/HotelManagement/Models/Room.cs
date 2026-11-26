using System.Data;

namespace HotelManagement.Models;

public class Room
{
    public const string StatusAvailable = "Trống";
    public const string StatusOccupied = "Đang ở";
    public const string StatusReserved = "Đã đặt";
    public const string StatusMaintenance = "Bảo trì";

    public int RoomId { get; set; }

    public string RoomNumber { get; set; } = string.Empty;

    public int RoomTypeId { get; set; }

    public string Status { get; set; } = StatusAvailable;

    public string TypeName { get; set; } = string.Empty;

    public decimal PricePerDay { get; set; }

    public int BookingCount { get; set; }

    public Room()
    {
    }

    public Room(DataRow row)
    {
        ArgumentNullException.ThrowIfNull(row);

        RoomId = DataRowReader.GetInt32(row, nameof(RoomId));
        RoomNumber = DataRowReader.GetString(row, nameof(RoomNumber));
        RoomTypeId = DataRowReader.GetInt32(row, nameof(RoomTypeId));
        Status = DataRowReader.GetString(row, nameof(Status), StatusAvailable);
        TypeName = DataRowReader.GetOptionalString(row, nameof(TypeName));
        PricePerDay = DataRowReader.GetOptionalDecimal(row, nameof(PricePerDay));
        BookingCount = DataRowReader.GetOptionalInt32(row, nameof(BookingCount));
    }
}
