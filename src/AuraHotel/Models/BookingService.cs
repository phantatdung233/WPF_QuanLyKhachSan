using System.Data;

namespace AuraHotel.Models;

public class BookingService
{
    public int BookingServiceId { get; set; }

    public int BookingId { get; set; }

    public int ServiceId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public string ServiceName { get; set; } = string.Empty;

    public BookingService()
    {
    }

    public BookingService(DataRow row)
    {
        ArgumentNullException.ThrowIfNull(row);

        BookingServiceId = DataRowReader.GetInt32(row, nameof(BookingServiceId));
        BookingId = DataRowReader.GetInt32(row, nameof(BookingId));
        ServiceId = DataRowReader.GetInt32(row, nameof(ServiceId));
        Quantity = DataRowReader.GetInt32(row, nameof(Quantity));
        UnitPrice = DataRowReader.GetDecimal(row, nameof(UnitPrice));
        ServiceName = DataRowReader.GetOptionalString(row, nameof(ServiceName));
    }
}
