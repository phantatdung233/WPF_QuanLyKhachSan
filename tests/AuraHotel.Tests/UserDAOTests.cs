using System.Data;
using AuraHotel.DAO;
using AuraHotel.Services;

namespace AuraHotel.Tests;

public class UserDAOTests
{
    [Fact]
    public void Login_ExecutesParameterizedQueryTrimsUsernameOnlyAndMapsUser()
    {
        string? capturedSql = null;
        object?[]? capturedParameters = null;

        var dao = new UserDAO((sql, parameters) =>
        {
            capturedSql = sql;
            capturedParameters = parameters;

            using var command = DataProvider.CreateCommand(sql, parameters);
            Assert.Equal(2, command.Parameters.Count);
            Assert.DoesNotContain("admin", command.CommandText, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("123456", command.CommandText, StringComparison.Ordinal);

            return CreateUsersDataTable(
                (1, "admin", "123456", "Trần Minh Quân", RolePermissions.Administrator));
        });

        var user = dao.Login("  admin  ", "123456");

        Assert.NotNull(capturedSql);
        Assert.NotNull(capturedParameters);
        Assert.Equal(["admin", "123456"], capturedParameters);
        Assert.NotNull(user);
        Assert.Equal(1, user.UserId);
        Assert.Equal("admin", user.Username);
        Assert.Equal("Trần Minh Quân", user.FullName);
        Assert.Equal(RolePermissions.Administrator, user.Role);
    }

    [Fact]
    public void Login_DoesNotTrimPasswordAndRejectsTrailingSpaceOrCaseMismatch()
    {
        object?[]? capturedParameters = null;

        var dao = new UserDAO((sql, parameters) =>
        {
            capturedParameters = parameters;
            using var _ = DataProvider.CreateCommand(sql, parameters);

            // Simulate SQL Server ANSI trailing-space / CI collation returning the '123456' row
            return CreateUsersDataTable(
                (2, "staff", "123456", "Lê Thu Ngân", RolePermissions.Staff));
        });

        var trailingSpaceResult = dao.Login("staff", "123456 ");

        Assert.Equal(["staff", "123456 "], capturedParameters);
        Assert.Null(trailingSpaceResult);
    }

    [Fact]
    public void Login_TreatsSqlMetacharactersAsBoundParameterValuesWithoutConcatenation()
    {
        const string maliciousUsername = "admin' OR 1=1 --";
        string? boundUsername = null;

        var dao = new UserDAO((sql, parameters) =>
        {
            using var command = DataProvider.CreateCommand(sql, parameters);
            Assert.DoesNotContain("1=1", command.CommandText, StringComparison.Ordinal);
            boundUsername = Assert.IsType<string>(command.Parameters["@Username"].Value);
            return CreateUsersDataTable();
        });

        var result = dao.Login(maliciousUsername, "anything");

        Assert.Null(result);
        Assert.Equal(maliciousUsername, boundUsername);
    }

    [Theory]
    [InlineData(null, "123456")]
    [InlineData("", "123456")]
    [InlineData("   ", "123456")]
    [InlineData("admin", null)]
    [InlineData("admin", "")]
    public void Login_SkipsDatabaseCallWhenUsernameIsBlankOrPasswordIsEmpty(string? username, string? password)
    {
        var queryCount = 0;
        var dao = new UserDAO((_, _) =>
        {
            queryCount++;
            return CreateUsersDataTable();
        });

        var result = dao.Login(username, password);

        Assert.Null(result);
        Assert.Equal(0, queryCount);
    }

    private static DataTable CreateUsersDataTable(
        params (int UserId, string Username, string Password, string FullName, string Role)[] rows)
    {
        var table = new DataTable();
        table.Columns.Add("UserId", typeof(int));
        table.Columns.Add("Username", typeof(string));
        table.Columns.Add("Password", typeof(string));
        table.Columns.Add("FullName", typeof(string));
        table.Columns.Add("Role", typeof(string));

        foreach (var row in rows)
        {
            table.Rows.Add(row.UserId, row.Username, row.Password, row.FullName, row.Role);
        }

        return table;
    }
}
