using System.Data;

namespace HotelManagement.Models;

public class Customer
{
    public int CustomerId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string CitizenId { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }

    public int BookingCount { get; set; }

    public Customer()
    {
    }

    public Customer(DataRow row)
    {
        ArgumentNullException.ThrowIfNull(row);

        CustomerId = DataRowReader.GetInt32(row, nameof(CustomerId));
        FullName = DataRowReader.GetString(row, nameof(FullName));
        CitizenId = DataRowReader.GetString(row, nameof(CitizenId));
        Phone = DataRowReader.GetString(row, nameof(Phone));
        Email = DataRowReader.GetNullableString(row, nameof(Email));
        BookingCount = DataRowReader.GetOptionalInt32(row, nameof(BookingCount));
    }
}
