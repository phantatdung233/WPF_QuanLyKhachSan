using System.Data;
using AuraHotel.DAO;
using AuraHotel.Models;

namespace AuraHotel.Tests;

public class RoomTypeDAOTests
{
    [Fact]
    public void GetAll_ExecutesLeftJoinRoomsCountQueryAndMapsRoomTypes()
    {
        string? capturedSql = null;

        var dao = new RoomTypeDAO(
            executeQuery: (sql, parameters) =>
            {
                capturedSql = sql;
                using var command = DataProvider.CreateCommand(sql, parameters);
                Assert.Empty(command.Parameters);
                Assert.Contains("LEFT JOIN", command.CommandText, StringComparison.OrdinalIgnoreCase);
                Assert.Contains("dbo.Rooms", command.CommandText, StringComparison.OrdinalIgnoreCase);
                Assert.Contains("COUNT(", command.CommandText, StringComparison.OrdinalIgnoreCase);

                return CreateRoomTypesDataTable(
                    (1, "Đơn", 500000m, 3),
                    (5, "Penthouse", 2500000m, 0));
            },
            executeNonQuery: static (_, _) => 0,
            executeScalar: static (_, _) => null);

        var items = dao.GetAll();

        Assert.NotNull(capturedSql);
        Assert.Equal(2, items.Count);
        Assert.Equal(1, items[0].RoomTypeId);
        Assert.Equal("Đơn", items[0].TypeName);
        Assert.Equal(500000m, items[0].PricePerDay);
        Assert.Equal(3, items[0].RoomCount);
        Assert.Equal(5, items[1].RoomTypeId);
        Assert.Equal("Penthouse", items[1].TypeName);
        Assert.Equal(2500000m, items[1].PricePerDay);
        Assert.Equal(0, items[1].RoomCount);
    }

    [Fact]
    public void Insert_BindsParameterizedValuesTrimsTypeNameAndReturnsInsertedId()
    {
        const string specialName = "  Suite 'Royal' --  ";
        object?[]? capturedParameters = null;

        var dao = new RoomTypeDAO(
            executeQuery: static (_, _) => new DataTable(),
            executeNonQuery: static (_, _) => 0,
            executeScalar: (sql, parameters) =>
            {
                capturedParameters = parameters;
                using var command = DataProvider.CreateCommand(sql, parameters);
                Assert.Equal(2, command.Parameters.Count);
                Assert.DoesNotContain("Royal", command.CommandText, StringComparison.OrdinalIgnoreCase);
                Assert.Equal("Suite 'Royal' --", command.Parameters["@TypeName"].Value);
                Assert.Equal(1800000m, command.Parameters["@PricePerDay"].Value);
                return 5;
            });

        var newId = dao.Insert(new RoomType
        {
            TypeName = specialName,
            PricePerDay = 1800000m
        });

        Assert.Equal(5, newId);
        Assert.NotNull(capturedParameters);
        Assert.Equal(["Suite 'Royal' --", 1800000m], capturedParameters);
    }

    [Theory]
    [InlineData("", 500000)]
    [InlineData("   ", 500000)]
    [InlineData("VIP", 0)]
    [InlineData("VIP", -100)]
    public void Insert_ThrowsForInvalidRoomTypeWithoutCallingDatabase(string typeName, int price)
    {
        var dbCalls = 0;
        var dao = new RoomTypeDAO(
            executeQuery: static (_, _) => new DataTable(),
            executeNonQuery: static (_, _) => 0,
            executeScalar: (_, _) =>
            {
                dbCalls++;
                return 1;
            });

        Assert.ThrowsAny<ArgumentException>(() =>
        {
            _ = dao.Insert(new RoomType
            {
                TypeName = typeName,
                PricePerDay = price
            });
        });
        Assert.Equal(0, dbCalls);
    }

    [Fact]
    public void Update_BindsParametersTrimsTypeNameAndReturnsAffectedState()
    {
        var dao = new RoomTypeDAO(
            executeQuery: static (_, _) => new DataTable(),
            executeNonQuery: (sql, parameters) =>
            {
                using var command = DataProvider.CreateCommand(sql, parameters);
                Assert.Equal(3, command.Parameters.Count);
                Assert.Equal("Gia đình cao cấp", command.Parameters["@TypeName"].Value);
                Assert.Equal(1050000m, command.Parameters["@PricePerDay"].Value);
                Assert.Equal(3, command.Parameters["@RoomTypeId"].Value);
                return 1;
            },
            executeScalar: static (_, _) => null);

        var updated = dao.Update(new RoomType
        {
            RoomTypeId = 3,
            TypeName = "  Gia đình cao cấp  ",
            PricePerDay = 1050000m
        });

        Assert.True(updated);
    }

    [Theory]
    [InlineData(1, 0, false)]
    [InlineData(5, 1, true)]
    public void Delete_ExecutesSafeNotExistsRoomsParameterizedDelete(
        int roomTypeId,
        int affectedRows,
        bool expectedDeleted)
    {
        var dao = new RoomTypeDAO(
            executeQuery: static (_, _) => new DataTable(),
            executeNonQuery: (sql, parameters) =>
            {
                using var command = DataProvider.CreateCommand(sql, parameters);
                Assert.Single(command.Parameters);
                Assert.Contains("NOT EXISTS", command.CommandText, StringComparison.OrdinalIgnoreCase);
                Assert.Contains("dbo.Rooms", command.CommandText, StringComparison.OrdinalIgnoreCase);
                Assert.Equal(roomTypeId, command.Parameters["@RoomTypeId"].Value);
                return affectedRows;
            },
            executeScalar: static (_, _) => null);

        var deleted = dao.Delete(roomTypeId);

        Assert.Equal(expectedDeleted, deleted);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Delete_SkipsDatabaseCallWhenRoomTypeIdIsNonPositive(int invalidRoomTypeId)
    {
        var dbCalls = 0;
        var dao = new RoomTypeDAO(
            executeQuery: static (_, _) => new DataTable(),
            executeNonQuery: (_, _) =>
            {
                dbCalls++;
                return 1;
            },
            executeScalar: static (_, _) => null);

        Assert.False(dao.Delete(invalidRoomTypeId));
        Assert.Equal(0, dbCalls);
    }

    private static DataTable CreateRoomTypesDataTable(
        params (int RoomTypeId, string TypeName, decimal PricePerDay, int RoomCount)[] rows)
    {
        var table = new DataTable();
        table.Columns.Add("RoomTypeId", typeof(int));
        table.Columns.Add("TypeName", typeof(string));
        table.Columns.Add("PricePerDay", typeof(decimal));
        table.Columns.Add("RoomCount", typeof(int));

        foreach (var row in rows)
        {
            table.Rows.Add(row.RoomTypeId, row.TypeName, row.PricePerDay, row.RoomCount);
        }

        return table;
    }
}
