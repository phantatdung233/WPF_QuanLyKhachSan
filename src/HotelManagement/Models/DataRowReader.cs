using System.Data;
using System.Globalization;

namespace HotelManagement.Models;

internal static class DataRowReader
{
    public static int GetInt32(DataRow row, string columnName, int defaultValue = 0) =>
        TryGetValue(row, columnName, required: true, out var value)
            ? Convert.ToInt32(value, CultureInfo.InvariantCulture)
            : defaultValue;

    public static int GetOptionalInt32(DataRow row, string columnName, int defaultValue = 0) =>
        TryGetValue(row, columnName, required: false, out var value)
            ? Convert.ToInt32(value, CultureInfo.InvariantCulture)
            : defaultValue;

    public static decimal GetDecimal(DataRow row, string columnName, decimal defaultValue = 0m) =>
        TryGetValue(row, columnName, required: true, out var value)
            ? Convert.ToDecimal(value, CultureInfo.InvariantCulture)
            : defaultValue;

    public static decimal GetOptionalDecimal(DataRow row, string columnName, decimal defaultValue = 0m) =>
        TryGetValue(row, columnName, required: false, out var value)
            ? Convert.ToDecimal(value, CultureInfo.InvariantCulture)
            : defaultValue;

    public static string GetString(DataRow row, string columnName, string defaultValue = "") =>
        GetNullableString(row, columnName) ?? defaultValue;

    public static string GetOptionalString(DataRow row, string columnName, string defaultValue = "") =>
        TryGetValue(row, columnName, required: false, out var value)
            ? Convert.ToString(value, CultureInfo.InvariantCulture) ?? defaultValue
            : defaultValue;

    public static string? GetNullableString(DataRow row, string columnName) =>
        TryGetValue(row, columnName, required: true, out var value)
            ? Convert.ToString(value, CultureInfo.InvariantCulture)
            : null;

    public static DateTime GetDateTime(DataRow row, string columnName) =>
        GetNullableDateTime(row, columnName) ?? default;

    public static DateTime? GetNullableDateTime(DataRow row, string columnName) =>
        TryGetValue(row, columnName, required: true, out var value)
            ? Convert.ToDateTime(value, CultureInfo.InvariantCulture)
            : null;

    private static bool TryGetValue(DataRow row, string columnName, bool required, out object value)
    {
        ArgumentNullException.ThrowIfNull(row);

        var hasColumn = row.Table?.Columns.Contains(columnName) == true;
        if (!hasColumn)
        {
            if (required)
            {
                throw new ArgumentException(
                    $"DataRow thiếu cột bắt buộc '{columnName}'.",
                    nameof(row));
            }

            value = DBNull.Value;
            return false;
        }

        if (row.IsNull(columnName))
        {
            value = DBNull.Value;
            return false;
        }

        value = row[columnName];
        return true;
    }
}
