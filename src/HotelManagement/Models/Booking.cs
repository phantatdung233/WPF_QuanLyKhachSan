using System.Data;

namespace HotelManagement.Models;

public class Booking
{
    public const string StatusReserved = "Đã đặt";
    public const string StatusOccupied = "Đang ở";
    public const string StatusCompleted = "Hoàn thành";
    public const string StatusCancelled = "Đã hủy";

    public int BookingId { get; set; }

    public int CustomerId { get; set; }

    public int RoomId { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    public string Status { get; set; } = StatusReserved;

    public string CustomerName { get; set; } = string.Empty;

    public string RoomNumber { get; set; } = string.Empty;

    public decimal PricePerDay { get; set; }

    public Booking()
    {
    }

    public Booking(DataRow row)
    {
        ArgumentNullException.ThrowIfNull(row);

        BookingId = DataRowReader.GetInt32(row, nameof(BookingId));
        CustomerId = DataRowReader.GetInt32(row, nameof(CustomerId));
        RoomId = DataRowReader.GetInt32(row, nameof(RoomId));
        CheckInDate = DataRowReader.GetDateTime(row, nameof(CheckInDate));
        CheckOutDate = DataRowReader.GetDateTime(row, nameof(CheckOutDate));
        Status = DataRowReader.GetString(row, nameof(Status), StatusReserved);
        CustomerName = DataRowReader.GetOptionalString(row, nameof(CustomerName));
        RoomNumber = DataRowReader.GetOptionalString(row, nameof(RoomNumber));
        PricePerDay = DataRowReader.GetOptionalDecimal(row, nameof(PricePerDay));
    }
}
