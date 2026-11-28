using AuraHotel.DAO;

namespace AuraHotel.Tests;

public class DataProviderTests
{
    [Fact]
    public void ResolveConnectionString_UsesFallbackSqlExpressWhenEnvironmentVariableIsMissingOrBlank()
    {
        Assert.Equal("AURA_HOTEL_DB_CONNECTION", DataProvider.ConnectionEnvironmentVariable);
        Assert.Equal("HOTEL_DB_CONNECTION", DataProvider.LegacyConnectionEnvironmentVariable);

        Assert.Equal(
            @"Server=.\SQLEXPRESS;Database=AuraHotelDb;Trusted_Connection=True;TrustServerCertificate=True;",
            DataProvider.ResolveConnectionString(null));

        Assert.Equal(
            @"Server=.\SQLEXPRESS;Database=AuraHotelDb;Trusted_Connection=True;TrustServerCertificate=True;",
            DataProvider.ResolveConnectionString("   "));

        Assert.Equal(
            "Server=localhost,1433;Database=AuraHotelDb;TrustServerCertificate=True;",
            DataProvider.ResolveConnectionString("  Server=localhost,1433;Database=AuraHotelDb;TrustServerCertificate=True;  "));

        Assert.Equal(
            "Server=legacy,1433;Database=AuraHotelDb;TrustServerCertificate=True;",
            DataProvider.ResolveConnectionString(
                "   ",
                "  Server=legacy,1433;Database=AuraHotelDb;TrustServerCertificate=True;  "));

        Assert.Equal(
            "Server=primary,1433;Database=AuraHotelDb;TrustServerCertificate=True;",
            DataProvider.ResolveConnectionString(
                "Server=primary,1433;Database=AuraHotelDb;TrustServerCertificate=True;",
                "Server=legacy,1433;Database=AuraHotelDb;TrustServerCertificate=True;"));
    }

    [Fact]
    public void ExtractAndBindParameters_BindsAccuratelyAdjacentToParenthesesCommasEqualsAndNewlines()
    {
        const string sql =
            "INSERT INTO Customers(FullName,CitizenId,Phone,Email)\r\n" +
            "VALUES(@FullName,@CitizenId,\n" +
            "@Phone,@Email)\r\n" +
            "WHERE (@RoomId=@TargetRoomId)";

        var parameters = DataProvider.ExtractParameterNames(sql);
        Assert.Equal(
            ["@FullName", "@CitizenId", "@Phone", "@Email", "@RoomId", "@TargetRoomId"],
            parameters);

        using var command = DataProvider.CreateCommand(
            sql,
            ["Nguyễn Văn An", "079201001234", "0901234567", null, 1, 1]);

        Assert.Equal(6, command.Parameters.Count);
        Assert.Equal("Nguyễn Văn An", command.Parameters["@FullName"].Value);
        Assert.Equal("079201001234", command.Parameters["@CitizenId"].Value);
        Assert.Equal("0901234567", command.Parameters["@Phone"].Value);
        Assert.Equal(DBNull.Value, command.Parameters["@Email"].Value);
        Assert.Equal(1, command.Parameters["@RoomId"].Value);
        Assert.Equal(1, command.Parameters["@TargetRoomId"].Value);
    }

    [Fact]
    public void ExtractParameterNames_DeduplicatesCaseInsensitivelyAndIgnoresGlobalsLiteralsAndComments()
    {
        const string sql = """
            -- Filter by @CommentedOutParam
            /* Multi-line comment with @BlockCommentParam */
            SELECT CustomerId, ISNULL(Email, N'guest@aurahotel.vn') AS Email, @@ROWCOUNT AS RowTotal
            FROM Customers
            WHERE (FullName LIKE @Keyword OR CitizenId LIKE @Keyword OR Phone LIKE @keyword)
              AND Note <> 'it''s@escaped.vn'
              AND CustomerId <> @ExcludeId;
            """;

        var parameters = DataProvider.ExtractParameterNames(sql);

        Assert.Equal(["@Keyword", "@ExcludeId"], parameters);
    }

    [Fact]
    public void CreateCommand_MapsNullToDbNullAndGuardsCountMismatch()
    {
        const string sql = """
            UPDATE Invoices
            SET Status = @Status,
                PaymentMethod = @PaymentMethod,
                PaymentDate = @PaymentDate
            WHERE (InvoiceId = @InvoiceId)
            """;

        using var command = DataProvider.CreateCommand(
            sql,
            ["Chưa thanh toán", null, null, 42]);

        Assert.Equal(4, command.Parameters.Count);
        Assert.Equal("Chưa thanh toán", command.Parameters["@Status"].Value);
        Assert.Equal(DBNull.Value, command.Parameters["@PaymentMethod"].Value);
        Assert.Equal(DBNull.Value, command.Parameters["@PaymentDate"].Value);
        Assert.Equal(42, command.Parameters["@InvoiceId"].Value);

        Assert.Throws<ArgumentException>(() =>
            DataProvider.CreateCommand(sql, ["Chưa thanh toán"]));
    }

    [Fact]
    public void CreateCommand_HandlesParameterlessQueriesAndComparisonOperatorAdjacency()
    {
        using var noParamCommand = DataProvider.CreateCommand("SELECT RoomId, RoomNumber, Status FROM Rooms;");
        Assert.Empty(noParamCommand.Parameters);

        const string overlapSql =
            "SELECT COUNT(*) FROM Bookings WHERE RoomId=@RoomId AND (@NewCheckIn<CheckOutDate AND CheckInDate<@NewCheckOut)";
        var extracted = DataProvider.ExtractParameterNames(overlapSql);
        Assert.Equal(["@RoomId", "@NewCheckIn", "@NewCheckOut"], extracted);
    }
}
