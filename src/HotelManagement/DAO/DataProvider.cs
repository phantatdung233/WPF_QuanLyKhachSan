using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace HotelManagement.DAO;

public sealed partial class DataProvider
{
    public const string ConnectionEnvironmentVariable = "HOTEL_DB_CONNECTION";
    public const string DefaultConnectionString =
        @"Server=.\SQLEXPRESS;Database=HotelManagementDb;Trusted_Connection=True;TrustServerCertificate=True;";

    private static readonly Lazy<DataProvider> LazyInstance = new(static () => new DataProvider());
    private readonly string? _explicitConnectionString;

    public static DataProvider Instance => LazyInstance.Value;

    public string ConnectionString =>
        ResolveConnectionString(
            _explicitConnectionString ?? Environment.GetEnvironmentVariable(ConnectionEnvironmentVariable));

    public DataProvider(string? connectionString = null)
    {
        _explicitConnectionString = string.IsNullOrWhiteSpace(connectionString)
            ? null
            : connectionString.Trim();
    }

    public static string ResolveConnectionString(string? configuredValue) =>
        string.IsNullOrWhiteSpace(configuredValue)
            ? DefaultConnectionString
            : configuredValue.Trim();

    public static IReadOnlyList<string> ExtractParameterNames(string query)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(query);

        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var result = new List<string>();

        foreach (Match match in SqlTokenRegex().Matches(query))
        {
            var paramGroup = match.Groups["param"];
            if (paramGroup.Success && seen.Add(paramGroup.Value))
            {
                result.Add(paramGroup.Value);
            }
        }

        return result;
    }

    public static SqlCommand CreateCommand(
        string query,
        object?[]? parameters = null,
        SqlConnection? connection = null)
    {
        var parameterNames = ExtractParameterNames(query);
        var suppliedCount = parameters?.Length ?? 0;

        if (parameterNames.Count != suppliedCount)
        {
            throw new ArgumentException(
                $"Câu truy vấn yêu cầu {parameterNames.Count} tham số ({string.Join(", ", parameterNames)}) nhưng nhận được {suppliedCount}.",
                nameof(parameters));
        }

        var command = connection is null
            ? new SqlCommand(query)
            : new SqlCommand(query, connection);

        for (var i = 0; i < parameterNames.Count; i++)
        {
            command.Parameters.AddWithValue(parameterNames[i], parameters![i] ?? DBNull.Value);
        }

        return command;
    }

    public DataTable ExecuteQuery(string query, object?[]? parameters = null) =>
        ExecuteWithConnection(query, parameters, static command =>
        {
            using var adapter = new SqlDataAdapter(command);
            var data = new DataTable();
            adapter.Fill(data);
            return data;
        });

    public int ExecuteNonQuery(string query, object?[]? parameters = null) =>
        ExecuteWithConnection(query, parameters, static command => command.ExecuteNonQuery());

    public object? ExecuteScalar(string query, object?[]? parameters = null) =>
        ExecuteWithConnection(query, parameters, static command =>
        {
            var result = command.ExecuteScalar();
            return result is DBNull ? null : result;
        });

    private T ExecuteWithConnection<T>(
        string query,
        object?[]? parameters,
        Func<SqlCommand, T> executor)
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();
        using var command = CreateCommand(query, parameters, connection);
        return executor(command);
    }

    [GeneratedRegex(
        @"--[^\r\n]*|/\*[\s\S]*?\*/|N?'(?:''|[^'])*'|@@[A-Za-z_][A-Za-z0-9_]*|(?<![@\w])(?<param>@[A-Za-z_][A-Za-z0-9_]*)")]
    private static partial Regex SqlTokenRegex();
}
