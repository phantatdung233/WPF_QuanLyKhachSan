using AuraHotel.Models;

namespace AuraHotel.Services;

public static class UserSession
{
    public static User? CurrentUser { get; private set; }

    public static void Start(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (!RolePermissions.IsSupportedRole(user.Role))
        {
            throw new ArgumentException("Vai trò người dùng không hợp lệ.", nameof(user));
        }

        CurrentUser = user;
    }

    public static void Clear()
    {
        CurrentUser = null;
    }
}
