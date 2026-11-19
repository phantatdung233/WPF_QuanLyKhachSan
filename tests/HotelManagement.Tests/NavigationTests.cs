using HotelManagement.ViewModels;
using HotelManagement.Services;

namespace HotelManagement.Tests;

public class NavigationTests
{
    [Theory]
    [InlineData("Administrator", 8)]
    [InlineData("Staff", 6)]
    [InlineData("unknown", 0)]
    public void RoleDeterminesVisiblePages(string role, int count)
    {
        Assert.Equal(count, RolePermissions.GetNavigationItems(role).Count);
    }
    [Fact]
    public void ShellStartsOnRoomMap()
    {
        var vm = new MainViewModel("Trần Minh Quản", "Administrator");
        Assert.IsType<RoomMapViewModel>(vm.CurrentView);
        Assert.Equal("Sơ đồ phòng", vm.PageTitle);
    }

    [Fact]
    public void StaffCannotNavigateToAdministration()
    {
        var vm = new MainViewModel("Lê Thu Ngân", "Staff");
        Assert.False(vm.Navigate(NavigationPage.Report));
        Assert.False(vm.Navigate(NavigationPage.RoomType));
        Assert.IsType<RoomMapViewModel>(vm.CurrentView);
    }

    [Theory]
    [InlineData(NavigationPage.RoomMap, typeof(RoomMapViewModel), "Sơ đồ phòng")]
    [InlineData(NavigationPage.Booking, typeof(BookingViewModel), "Đặt phòng")]
    [InlineData(NavigationPage.Room, typeof(RoomViewModel), "Phòng")]
    [InlineData(NavigationPage.RoomType, typeof(RoomTypeViewModel), "Loại phòng")]
    [InlineData(NavigationPage.Customer, typeof(CustomerViewModel), "Khách hàng")]
    [InlineData(NavigationPage.Service, typeof(ServiceViewModel), "Dịch vụ")]
    [InlineData(NavigationPage.Invoice, typeof(InvoiceViewModel), "Hóa đơn")]
    [InlineData(NavigationPage.Report, typeof(ReportViewModel), "Báo cáo")]
    public void AdministratorCanSwitchAllPages(NavigationPage page, Type viewModelType, string title)
    {
        var vm = new MainViewModel("Trần Minh Quản", "Administrator");
        Assert.True(vm.Navigate(page));
        Assert.IsType(viewModelType, vm.CurrentView);
        Assert.Equal(title, vm.PageTitle);
        Assert.Equal(page, vm.SelectedItem?.Page);
    }

    [Fact]
    public void LogoutClearsSessionAndPreventsFurtherNavigation()
    {
        var vm = new MainViewModel("Trần Minh Quản", "Administrator");
        vm.LogoutCommand.Execute(null);
        Assert.Empty(vm.SessionDisplay);
        Assert.Empty(vm.FullName);
        Assert.Empty(vm.RoleDisplay);
        Assert.Empty(vm.NavigationItems);
        Assert.Null(vm.CurrentView);
        Assert.False(vm.Navigate(NavigationPage.RoomMap));
    }
}
