using HotelManagement.Services;
using HotelManagement.ViewModels;

namespace HotelManagement.Tests;

public class MainViewModelTests
{
    [Fact]
    public void Constructor_DefaultsCurrentViewToRoomMapAndPopulatesSessionMetadata()
    {
        var viewModel = new MainViewModel("Nguyễn Văn An", RolePermissions.Administrator);

        Assert.IsType<RoomMapViewModel>(viewModel.CurrentView);
        Assert.Equal("Sơ đồ phòng", viewModel.PageTitle);
        Assert.Equal(NavigationKey.RoomMap, viewModel.SelectedNavigationItem?.Key);
        Assert.Equal("Nguyễn Văn An", viewModel.FullName);
        Assert.Equal("Quản trị", viewModel.RoleDisplay);
        Assert.Equal(8, viewModel.NavigationItems.Count);
    }

    [Fact]
    public void NavigateCommand_AsAdministrator_SwitchesAcrossAllEightRegisteredViewModels()
    {
        var viewModel = new MainViewModel("Nguyễn Văn An", RolePermissions.Administrator);

        (NavigationKey Key, Type ExpectedType, string ExpectedTitle)[] expectedRoutes =
        [
            (NavigationKey.Booking, typeof(BookingViewModel), "Đặt phòng"),
            (NavigationKey.Room, typeof(RoomViewModel), "Phòng"),
            (NavigationKey.RoomType, typeof(RoomTypeViewModel), "Loại phòng"),
            (NavigationKey.Customer, typeof(CustomerViewModel), "Khách hàng"),
            (NavigationKey.Service, typeof(ServiceViewModel), "Dịch vụ"),
            (NavigationKey.Invoice, typeof(InvoiceViewModel), "Hóa đơn"),
            (NavigationKey.Report, typeof(ReportViewModel), "Báo cáo"),
            (NavigationKey.RoomMap, typeof(RoomMapViewModel), "Sơ đồ phòng")
        ];

        foreach (var (key, expectedType, expectedTitle) in expectedRoutes)
        {
            viewModel.NavigateCommand.Execute(key);

            Assert.IsType(expectedType, viewModel.CurrentView);
            Assert.Equal(expectedTitle, viewModel.PageTitle);
            Assert.Equal(key, viewModel.SelectedNavigationItem?.Key);
            Assert.False(viewModel.HasError);
        }
    }

    [Fact]
    public void SelectedNavigationItem_WhenChanged_TransitionsCurrentViewAndTitle()
    {
        var viewModel = new MainViewModel("Trần Thị Bình", RolePermissions.Staff);
        var customerItem = viewModel.NavigationItems.Single(item => item.Key == NavigationKey.Customer);

        viewModel.SelectedNavigationItem = customerItem;

        Assert.IsType<CustomerViewModel>(viewModel.CurrentView);
        Assert.Equal("Khách hàng", viewModel.PageTitle);
    }

    [Fact]
    public void NavigateCommand_AsStaff_BlocksAdministratorOnlyModulesAndSetsError()
    {
        var viewModel = new MainViewModel("Trần Thị Bình", RolePermissions.Staff);

        viewModel.NavigateCommand.Execute(NavigationKey.RoomType);
        Assert.IsType<RoomMapViewModel>(viewModel.CurrentView);
        Assert.Equal("Sơ đồ phòng", viewModel.PageTitle);
        Assert.True(viewModel.HasError);

        viewModel.NavigateCommand.Execute(NavigationKey.Booking);
        Assert.False(viewModel.HasError);
        Assert.IsType<BookingViewModel>(viewModel.CurrentView);

        viewModel.NavigateCommand.Execute(NavigationKey.Report);
        Assert.IsType<BookingViewModel>(viewModel.CurrentView);
        Assert.Equal("Đặt phòng", viewModel.PageTitle);
        Assert.True(viewModel.HasError);
    }

    [Fact]
    public void LogoutCommand_MarksSessionEndedAndRaisesLogoutRequested()
    {
        var eventCount = 0;
        var viewModel = new MainViewModel("Trần Thị Bình", RolePermissions.Staff);
        viewModel.LogoutRequested += () => eventCount++;

        viewModel.LogoutCommand.Execute(null);

        Assert.False(viewModel.IsSessionActive);
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void IsEmptyStateVisible_HidesEmptyPlaceholderWhileLoading()
    {
        var roomMap = new RoomMapViewModel();

        Assert.True(roomMap.IsEmpty);
        Assert.True(roomMap.IsEmptyStateVisible);

        roomMap.IsLoading = true;
        Assert.False(roomMap.IsEmptyStateVisible);

        roomMap.IsLoading = false;
        Assert.True(roomMap.IsEmptyStateVisible);
    }
}
