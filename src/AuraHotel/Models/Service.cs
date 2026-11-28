using System.Data;

namespace AuraHotel.Models;

public class Service
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int UsageCount { get; set; }

    public Service()
    {
    }

    public Service(DataRow row)
    {
        ArgumentNullException.ThrowIfNull(row);

        ServiceId = DataRowReader.GetInt32(row, nameof(ServiceId));
        ServiceName = DataRowReader.GetString(row, nameof(ServiceName));
        Price = DataRowReader.GetDecimal(row, nameof(Price));
        UsageCount = DataRowReader.GetOptionalInt32(row, nameof(UsageCount));
    }
}
