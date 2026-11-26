using HotelManagement.Services;

namespace HotelManagement.Tests;

public class RolePermissionsTests
{
    [Fact]
    public void Administrator_ReceivesAllEightNavigationItemsInCanonicalOrder()
    {
        var items = RolePermissions.GetAllowedNavigationItems("Administrator");

        Assert.Equal(
            [
                "Sơ đồ phòng",
                "Đặt phòng",
                "Phòng",
                "Loại phòng",
                "Khách hàng",
                "Dịch vụ",
                "Hóa đơn",
                "Báo cáo"
            ],
            items.Select(item => item.Title));
    }

    [Fact]
    public void Staff_ReceivesSixReceptionItemsHidingRoomTypeAndReport()
    {
        var items = RolePermissions.GetAllowedNavigationItems("Staff");

        Assert.Equal(
            [
                "Sơ đồ phòng",
                "Đặt phòng",
                "Phòng",
                "Khách hàng",
                "Dịch vụ",
                "Hóa đơn"
            ],
            items.Select(item => item.Title));
    }

    [Theory]
    [InlineData("Administrator", "Quản trị", true, true, 8)]
    [InlineData("Staff", "Lễ tân", false, true, 6)]
    [InlineData(null, "", false, false, 0)]
    [InlineData("Guest", "", false, false, 0)]
    public void RoleDisplayAndAccessGuard_StrictlyPartitionAdministratorAndStaff(
        string? role,
        string expectedDisplay,
        bool canAccessRestrictedModules,
        bool canAccessReceptionModules,
        int expectedCount)
    {
        Assert.Equal(expectedDisplay, RolePermissions.GetRoleDisplay(role));
        Assert.Equal(expectedCount, RolePermissions.GetAllowedNavigationItems(role).Count);
        Assert.Equal(canAccessRestrictedModules, RolePermissions.CanAccess(role, NavigationKey.RoomType));
        Assert.Equal(canAccessRestrictedModules, RolePermissions.CanAccess(role, NavigationKey.Report));
        Assert.Equal(canAccessReceptionModules, RolePermissions.CanAccess(role, NavigationKey.RoomMap));
    }
}
