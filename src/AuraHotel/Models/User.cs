using System.Data;

namespace AuraHotel.Models;

public class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public User()
    {
    }

    public User(DataRow row)
    {
        ArgumentNullException.ThrowIfNull(row);

        UserId = DataRowReader.GetInt32(row, nameof(UserId));
        Username = DataRowReader.GetString(row, nameof(Username));
        Password = DataRowReader.GetString(row, nameof(Password));
        FullName = DataRowReader.GetString(row, nameof(FullName));
        Role = DataRowReader.GetString(row, nameof(Role));
    }
}
