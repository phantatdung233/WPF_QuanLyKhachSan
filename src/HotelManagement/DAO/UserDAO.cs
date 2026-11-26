using System.Data;
using HotelManagement.Models;

namespace HotelManagement.DAO;

public sealed class UserDAO
{
    private const string LoginQuery = """
        SELECT TOP (1) UserId, Username, Password, FullName, Role
        FROM dbo.Users
        WHERE Username = @Username AND Password = @Password;
        """;

    private static readonly Lazy<UserDAO> LazyInstance = new(static () => new UserDAO());
    private readonly Func<string, object?[]?, DataTable> _executeQuery;

    public static UserDAO Instance => LazyInstance.Value;

    public UserDAO()
        : this(DataProvider.Instance.ExecuteQuery)
    {
    }

    public UserDAO(Func<string, object?[]?, DataTable> executeQuery)
    {
        ArgumentNullException.ThrowIfNull(executeQuery);
        _executeQuery = executeQuery;
    }

    public User? Login(string? username, string? password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
        {
            return null;
        }

        var normalizedUsername = username.Trim();
        var data = _executeQuery(LoginQuery, [normalizedUsername, password]);
        if (data.Rows.Count == 0)
        {
            return null;
        }

        var user = new User(data.Rows[0]);
        return string.Equals(user.Password, password, StringComparison.Ordinal)
            ? user
            : null;
    }
}
