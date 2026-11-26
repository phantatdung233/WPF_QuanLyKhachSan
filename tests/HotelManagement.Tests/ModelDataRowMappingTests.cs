using System.Data;
using HotelManagement.Models;

namespace HotelManagement.Tests;

public class ModelDataRowMappingTests
{
    [Fact]
    public void User_MapsDataRowAndRejectsMissingRequiredColumns()
    {
        var table = new DataTable();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("Username", typeof(string));
        table.Columns.Add("Password", typeof(string));
        table.Columns.Add("FullName", typeof(string));
        table.Columns.Add("Role", typeof(string));

        var row = table.Rows.Add(2, "staff", "123456", "Lê Thu Ngân", "Staff");

        var user = new User(row);

        Assert.Equal(2, user.UserId);
        Assert.Equal("staff", user.Username);
        Assert.Equal("123456", user.Password);
        Assert.Equal("Lê Thu Ngân", user.FullName);
        Assert.Equal("Staff", user.Role);

        var incompleteTable = new DataTable();
        incompleteTable.Columns.Add("UserId", typeof(int));
        var incompleteRow = incompleteTable.Rows.Add(1);
        Assert.Throws<ArgumentException>(() => new User(incompleteRow));
    }

    [Fact]
    public void RoomTypeAndRoom_MapBaseColumnsAndOptionalJoinedProjections()
    {
        var roomTypeTable = new DataTable();
        roomTypeTable.Columns.Add("RoomTypeId", typeof(int));
        roomTypeTable.Columns.Add("TypeName", typeof(string));
        roomTypeTable.Columns.Add("PricePerDay", typeof(decimal));

        var baseTypeRow = roomTypeTable.Rows.Add(1, "Đơn", 500000m);
        var roomType = new RoomType(baseTypeRow);

        Assert.Equal(1, roomType.RoomTypeId);
        Assert.Equal("Đơn", roomType.TypeName);
        Assert.Equal(500000m, roomType.PricePerDay);
        Assert.Equal(0, roomType.RoomCount);

        var roomTable = new DataTable();
        roomTable.Columns.Add("RoomId", typeof(int));
        roomTable.Columns.Add("RoomNumber", typeof(string));
        roomTable.Columns.Add("RoomTypeId", typeof(int));
        roomTable.Columns.Add("Status", typeof(string));
        roomTable.Columns.Add("TypeName", typeof(string));
        roomTable.Columns.Add("PricePerDay", typeof(decimal));
        roomTable.Columns.Add("BookingCount", typeof(long));

        var roomRow = roomTable.Rows.Add(1, "101", 1, Room.StatusAvailable, "Đơn", 500000m, 2L);
        var room = new Room(roomRow);

        Assert.Equal(1, room.RoomId);
        Assert.Equal("101", room.RoomNumber);
        Assert.Equal(1, room.RoomTypeId);
        Assert.Equal("Trống", room.Status);
        Assert.Equal("Đơn", room.TypeName);
        Assert.Equal(500000m, room.PricePerDay);
        Assert.Equal(2, room.BookingCount);
    }

    [Fact]
    public void CustomerAndService_HandleNullableEmailDbNullWithoutInvalidCastException()
    {
        var customerTable = new DataTable();
        customerTable.Columns.Add("CustomerId", typeof(int));
        customerTable.Columns.Add("FullName", typeof(string));
        customerTable.Columns.Add("CitizenId", typeof(string));
        customerTable.Columns.Add("Phone", typeof(string));
        customerTable.Columns.Add("Email", typeof(string));

        var nullEmailRow = customerTable.Rows.Add(3, "Lê Hoàng Nam", "079199009012", "0987654321", DBNull.Value);
        var populatedEmailRow = customerTable.Rows.Add(1, "Nguyễn Văn An", "079201001234", "0901234567", "an.nguyen@gmail.com");

        var customerWithoutEmail = new Customer(nullEmailRow);
        var customerWithEmail = new Customer(populatedEmailRow);

        Assert.Null(customerWithoutEmail.Email);
        Assert.Equal("Lê Hoàng Nam", customerWithoutEmail.FullName);
        Assert.Equal("an.nguyen@gmail.com", customerWithEmail.Email);

        var serviceTable = new DataTable();
        serviceTable.Columns.Add("ServiceId", typeof(int));
        serviceTable.Columns.Add("ServiceName", typeof(string));
        serviceTable.Columns.Add("Price", typeof(decimal));
        serviceTable.Columns.Add("UsageCount", typeof(int));

        var serviceRow = serviceTable.Rows.Add(2, "Ăn sáng buffet", 50000m, DBNull.Value);
        var service = new Service(serviceRow);

        Assert.Equal(2, service.ServiceId);
        Assert.Equal("Ăn sáng buffet", service.ServiceName);
        Assert.Equal(50000m, service.Price);
        Assert.Equal(0, service.UsageCount);
    }

    [Fact]
    public void BookingAndBookingService_MapStayDatesAndSnapshotUnitPrice()
    {
        var bookingTable = new DataTable();
        bookingTable.Columns.Add("BookingId", typeof(int));
        bookingTable.Columns.Add("CustomerId", typeof(int));
        bookingTable.Columns.Add("RoomId", typeof(int));
        bookingTable.Columns.Add("CheckInDate", typeof(DateTime));
        bookingTable.Columns.Add("CheckOutDate", typeof(DateTime));
        bookingTable.Columns.Add("Status", typeof(string));
        bookingTable.Columns.Add("CustomerName", typeof(string));
        bookingTable.Columns.Add("RoomNumber", typeof(string));
        bookingTable.Columns.Add("PricePerDay", typeof(decimal));

        var checkIn = new DateTime(2026, 11, 24, 14, 0, 0);
        var checkOut = new DateTime(2026, 11, 26, 12, 0, 0);
        var bookingRow = bookingTable.Rows.Add(
            10, 1, 1, checkIn, checkOut, Booking.StatusOccupied, "Nguyễn Văn An", "101", 500000m);

        var booking = new Booking(bookingRow);

        Assert.Equal(10, booking.BookingId);
        Assert.Equal(1, booking.CustomerId);
        Assert.Equal(1, booking.RoomId);
        Assert.Equal(checkIn, booking.CheckInDate);
        Assert.Equal(checkOut, booking.CheckOutDate);
        Assert.Equal("Đang ở", booking.Status);
        Assert.Equal("Nguyễn Văn An", booking.CustomerName);
        Assert.Equal("101", booking.RoomNumber);
        Assert.Equal(500000m, booking.PricePerDay);

        var bookingServiceTable = new DataTable();
        bookingServiceTable.Columns.Add("BookingServiceId", typeof(int));
        bookingServiceTable.Columns.Add("BookingId", typeof(int));
        bookingServiceTable.Columns.Add("ServiceId", typeof(int));
        bookingServiceTable.Columns.Add("Quantity", typeof(int));
        bookingServiceTable.Columns.Add("UnitPrice", typeof(decimal));
        bookingServiceTable.Columns.Add("ServiceName", typeof(string));

        var lineRow = bookingServiceTable.Rows.Add(5, 10, 1, 2, 10000m, "Nước suối Lavie");
        var bookingService = new BookingService(lineRow);

        Assert.Equal(5, bookingService.BookingServiceId);
        Assert.Equal(10, bookingService.BookingId);
        Assert.Equal(1, bookingService.ServiceId);
        Assert.Equal(2, bookingService.Quantity);
        Assert.Equal(10000m, bookingService.UnitPrice);
        Assert.Equal("Nước suối Lavie", bookingService.ServiceName);
    }

    [Fact]
    public void Invoice_HandlesNullablePaymentDateAndPaymentMethodDbNullWithoutInvalidCastException()
    {
        var invoiceTable = new DataTable();
        invoiceTable.Columns.Add("InvoiceId", typeof(int));
        invoiceTable.Columns.Add("BookingId", typeof(int));
        invoiceTable.Columns.Add("RoomCharge", typeof(decimal));
        invoiceTable.Columns.Add("ServiceCharge", typeof(decimal));
        invoiceTable.Columns.Add("TotalAmount", typeof(decimal));
        invoiceTable.Columns.Add("Status", typeof(string));
        invoiceTable.Columns.Add("PaymentMethod", typeof(string));
        invoiceTable.Columns.Add("PaymentDate", typeof(DateTime));
        invoiceTable.Columns.Add("RoomNumber", typeof(string));
        invoiceTable.Columns.Add("CustomerName", typeof(string));

        var pendingRow = invoiceTable.Rows.Add(
            1, 10, 1000000m, 70000m, 1070000m, Invoice.StatusPending, DBNull.Value, DBNull.Value, "101", "Nguyễn Văn An");

        var paidTimestamp = new DateTime(2026, 11, 26, 11, 30, 0);
        var paidRow = invoiceTable.Rows.Add(
            2, 11, 1000000m, 70000m, 1070000m, Invoice.StatusPaid, Invoice.PaymentMethodBankTransfer, paidTimestamp, "101", "Nguyễn Văn An");

        var pendingInvoice = new Invoice(pendingRow);
        var paidInvoice = new Invoice(paidRow);

        Assert.Equal(1, pendingInvoice.InvoiceId);
        Assert.Equal(10, pendingInvoice.BookingId);
        Assert.Equal(1000000m, pendingInvoice.RoomCharge);
        Assert.Equal(70000m, pendingInvoice.ServiceCharge);
        Assert.Equal(1070000m, pendingInvoice.TotalAmount);
        Assert.Equal("Chưa thanh toán", pendingInvoice.Status);
        Assert.Null(pendingInvoice.PaymentMethod);
        Assert.Null(pendingInvoice.PaymentDate);

        Assert.Equal("Đã thanh toán", paidInvoice.Status);
        Assert.Equal("Chuyển khoản", paidInvoice.PaymentMethod);
        Assert.Equal(paidTimestamp, paidInvoice.PaymentDate);
        Assert.Equal("101", paidInvoice.RoomNumber);
        Assert.Equal("Nguyễn Văn An", paidInvoice.CustomerName);
    }
}
