using System.Data;
using System.Globalization;
using HotelManagement.Models;
using HotelManagement.Services;

namespace HotelManagement.DAO;

public sealed class RoomTypeDAO
{
    private const string GetAllWithRoomCountQuery = """
        SELECT rt.RoomTypeId,
               rt.TypeName,
               rt.PricePerDay,
               COUNT(r.RoomId) AS RoomCount
        FROM dbo.RoomTypes AS rt
        LEFT JOIN dbo.Rooms AS r ON r.RoomTypeId = rt.RoomTypeId
        GROUP BY rt.RoomTypeId, rt.TypeName, rt.PricePerDay
        ORDER BY rt.RoomTypeId ASC;
        """;

    private const string InsertQuery = """
        INSERT INTO dbo.RoomTypes (TypeName, PricePerDay)
        OUTPUT INSERTED.RoomTypeId
        VALUES (@TypeName, @PricePerDay);
        """;

    private const string UpdateQuery = """
        UPDATE dbo.RoomTypes
        SET TypeName = @TypeName,
            PricePerDay = @PricePerDay
        WHERE RoomTypeId = @RoomTypeId;
        """;

    private const string SafeDeleteQuery = """
        DELETE FROM dbo.RoomTypes
        WHERE RoomTypeId = @RoomTypeId
          AND NOT EXISTS (
              SELECT 1
              FROM dbo.Rooms
              WHERE RoomTypeId = @RoomTypeId
          );
        """;

    private static readonly Lazy<RoomTypeDAO> LazyInstance = new(static () => new RoomTypeDAO());
    private readonly Func<string, object?[]?, DataTable> _executeQuery;
    private readonly Func<string, object?[]?, int> _executeNonQuery;
    private readonly Func<string, object?[]?, object?> _executeScalar;

    public static RoomTypeDAO Instance => LazyInstance.Value;

    public RoomTypeDAO()
        : this(
            DataProvider.Instance.ExecuteQuery,
            DataProvider.Instance.ExecuteNonQuery,
            DataProvider.Instance.ExecuteScalar)
    {
    }

    public RoomTypeDAO(
        Func<string, object?[]?, DataTable> executeQuery,
        Func<string, object?[]?, int> executeNonQuery,
        Func<string, object?[]?, object?> executeScalar)
    {
        ArgumentNullException.ThrowIfNull(executeQuery);
        ArgumentNullException.ThrowIfNull(executeNonQuery);
        ArgumentNullException.ThrowIfNull(executeScalar);

        _executeQuery = executeQuery;
        _executeNonQuery = executeNonQuery;
        _executeScalar = executeScalar;
    }

    public IReadOnlyList<RoomType> GetAll()
    {
        var table = _executeQuery(GetAllWithRoomCountQuery, null);
        var result = new List<RoomType>(table.Rows.Count);

        foreach (DataRow row in table.Rows)
        {
            result.Add(new RoomType(row));
        }

        return result;
    }

    public int Insert(RoomType roomType)
    {
        var normalizedName = ValidateRoomTypePayload(roomType);
        var insertedId = _executeScalar(InsertQuery, [normalizedName, roomType.PricePerDay]);
        return Convert.ToInt32(insertedId, CultureInfo.InvariantCulture);
    }

    public bool Update(RoomType roomType)
    {
        var normalizedName = ValidateRoomTypePayload(roomType);
        if (roomType.RoomTypeId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(roomType), "Mã loại phòng không hợp lệ.");
        }

        return _executeNonQuery(
            UpdateQuery,
            [normalizedName, roomType.PricePerDay, roomType.RoomTypeId]) > 0;
    }

    public bool Delete(int roomTypeId)
    {
        if (roomTypeId <= 0)
        {
            return false;
        }

        return _executeNonQuery(SafeDeleteQuery, [roomTypeId]) > 0;
    }

    private static string ValidateRoomTypePayload(RoomType roomType)
    {
        ArgumentNullException.ThrowIfNull(roomType);

        var normalizedName = roomType.TypeName?.Trim() ?? string.Empty;
        if (normalizedName.Length == 0 || normalizedName.Length > ValidationRules.MaxRoomTypeNameLength)
        {
            throw new ArgumentException("Tên loại phòng không hợp lệ.", nameof(roomType));
        }

        if (roomType.PricePerDay <= 0m || roomType.PricePerDay > ValidationRules.MaxRoomTypePricePerDay)
        {
            throw new ArgumentOutOfRangeException(nameof(roomType), "Đơn giá/đêm phải lớn hơn 0.");
        }

        return normalizedName;
    }
}
