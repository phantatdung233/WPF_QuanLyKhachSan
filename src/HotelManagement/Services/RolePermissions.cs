namespace HotelManagement.Services;

public enum NavigationKey
{
    RoomMap,
    Booking,
    Room,
    RoomType,
    Customer,
    Service,
    Invoice,
    Report
}

public sealed record NavigationItemDescriptor(
    NavigationKey Key,
    string Title,
    bool AdministratorOnly = false);

public static class RolePermissions
{
    public const string Administrator = "Administrator";
    public const string Staff = "Staff";

    private static readonly NavigationItemDescriptor[] AllItems =
    [
        new(NavigationKey.RoomMap, "Sơ đồ phòng"),
        new(NavigationKey.Booking, "Đặt phòng"),
        new(NavigationKey.Room, "Phòng"),
        new(NavigationKey.RoomType, "Loại phòng", AdministratorOnly: true),
        new(NavigationKey.Customer, "Khách hàng"),
        new(NavigationKey.Service, "Dịch vụ"),
        new(NavigationKey.Invoice, "Hóa đơn"),
        new(NavigationKey.Report, "Báo cáo", AdministratorOnly: true)
    ];

    private static readonly NavigationItemDescriptor[] StaffItems =
        AllItems.Where(item => !item.AdministratorOnly).ToArray();

    public static bool IsAdministrator(string? role) =>
        string.Equals(role?.Trim(), Administrator, StringComparison.OrdinalIgnoreCase);

    public static bool IsStaff(string? role) =>
        string.Equals(role?.Trim(), Staff, StringComparison.OrdinalIgnoreCase);

    public static string GetRoleDisplay(string? role)
    {
        if (IsAdministrator(role))
        {
            return "Quản trị";
        }

        if (IsStaff(role))
        {
            return "Lễ tân";
        }

        return string.Empty;
    }

    public static IReadOnlyList<NavigationItemDescriptor> GetAllowedNavigationItems(string? role)
    {
        if (IsAdministrator(role))
        {
            return AllItems;
        }

        if (IsStaff(role))
        {
            return StaffItems;
        }

        return [];
    }

    public static bool CanAccess(string? role, NavigationKey key) =>
        GetAllowedNavigationItems(role).Any(item => item.Key == key);
}
