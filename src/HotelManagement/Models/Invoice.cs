using System.Data;

namespace HotelManagement.Models;

public class Invoice
{
    public const string StatusPending = "Chưa thanh toán";
    public const string StatusPaid = "Đã thanh toán";
    public const string PaymentMethodCash = "Tiền mặt";
    public const string PaymentMethodBankTransfer = "Chuyển khoản";

    public int InvoiceId { get; set; }

    public int BookingId { get; set; }

    public decimal RoomCharge { get; set; }

    public decimal ServiceCharge { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = StatusPending;

    public string? PaymentMethod { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string RoomNumber { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public Invoice()
    {
    }

    public Invoice(DataRow row)
    {
        ArgumentNullException.ThrowIfNull(row);

        InvoiceId = DataRowReader.GetInt32(row, nameof(InvoiceId));
        BookingId = DataRowReader.GetInt32(row, nameof(BookingId));
        RoomCharge = DataRowReader.GetDecimal(row, nameof(RoomCharge));
        ServiceCharge = DataRowReader.GetDecimal(row, nameof(ServiceCharge));
        TotalAmount = DataRowReader.GetDecimal(row, nameof(TotalAmount));
        Status = DataRowReader.GetString(row, nameof(Status), StatusPending);
        PaymentMethod = DataRowReader.GetNullableString(row, nameof(PaymentMethod));
        PaymentDate = DataRowReader.GetNullableDateTime(row, nameof(PaymentDate));
        RoomNumber = DataRowReader.GetOptionalString(row, nameof(RoomNumber));
        CustomerName = DataRowReader.GetOptionalString(row, nameof(CustomerName));
    }
}
