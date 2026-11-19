namespace HotelManagement.Services;

public enum NavigationPage { RoomMap, Booking, Room, RoomType, Customer, Service, Invoice, Report }

public sealed record NavigationItem(NavigationPage Page, string Title);

public static class RolePermissions
{
    private static readonly NavigationItem[] Pages =
    [
        new(NavigationPage.RoomMap, "Sơ đồ phòng"),
        new(NavigationPage.Booking, "Đặt phòng"),
        new(NavigationPage.Room, "Phòng"),
        new(NavigationPage.RoomType, "Loại phòng"),
        new(NavigationPage.Customer, "Khách hàng"),
        new(NavigationPage.Service, "Dịch vụ"),
        new(NavigationPage.Invoice, "Hóa đơn"),
        new(NavigationPage.Report, "Báo cáo")
    ];

    public static IReadOnlyList<NavigationItem> GetNavigationItems(string role) =>
        Array.AsReadOnly(Pages.Where(item => role == "Administrator" ||
            role == "Staff" && item.Page is not (NavigationPage.RoomType or NavigationPage.Report)).ToArray());
}
