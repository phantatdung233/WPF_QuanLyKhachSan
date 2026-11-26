using HotelManagement.Models;

namespace HotelManagement.Services;

public static class UserSession
{
    public static User? CurrentUser { get; private set; }

    public static bool IsAuthenticated => CurrentUser is not null;

    public static bool IsAdministrator =>
        CurrentUser is not null && RolePermissions.IsAdministrator(CurrentUser.Role);

    public static bool IsStaff =>
        CurrentUser is not null && RolePermissions.IsStaff(CurrentUser.Role);

    public static string RoleDisplay =>
        CurrentUser is null ? string.Empty : RolePermissions.GetRoleDisplay(CurrentUser.Role);

    public static void Start(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (!RolePermissions.IsAdministrator(user.Role) && !RolePermissions.IsStaff(user.Role))
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
