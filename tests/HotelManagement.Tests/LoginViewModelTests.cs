using HotelManagement.Models;
using HotelManagement.Services;
using HotelManagement.ViewModels;

namespace HotelManagement.Tests;

[Collection("UserSession")]
public class LoginViewModelTests
{
    public LoginViewModelTests()
    {
        UserSession.Clear();
    }

    [Theory]
    [InlineData("", "", true, true)]
    [InlineData("   ", "123456", true, false)]
    [InlineData("admin", "", false, true)]
    public async Task LoginCommand_ShowsInlineErrorsForEmptyFieldsWithoutCallingAuthenticator(
        string username,
        string password,
        bool expectedUsernameError,
        bool expectedPasswordError)
    {
        var authCallCount = 0;
        var viewModel = new LoginViewModel((_, _) =>
        {
            authCallCount++;
            return (User?)null;
        })
        {
            Username = username,
            Password = password
        };

        await viewModel.LoginCommand.ExecuteAsync(null);

        Assert.Equal(0, authCallCount);
        Assert.Equal(expectedUsernameError, viewModel.HasUsernameError);
        Assert.Equal(expectedPasswordError, viewModel.HasPasswordError);
        Assert.False(viewModel.HasError);
        Assert.False(viewModel.IsEmpty);
        Assert.False(viewModel.IsEmptyStateVisible);
        Assert.Null(UserSession.CurrentUser);
    }

    [Fact]
    public async Task TypingIntoInvalidFields_ClearsCorrespondingInlineValidationErrors()
    {
        var viewModel = new LoginViewModel((_, _) => (User?)null);

        await viewModel.LoginCommand.ExecuteAsync(null);
        Assert.True(viewModel.HasUsernameError);
        Assert.True(viewModel.HasPasswordError);

        viewModel.Username = "admin";
        Assert.False(viewModel.HasUsernameError);
        Assert.True(viewModel.HasPasswordError);

        viewModel.Password = "123456";
        Assert.False(viewModel.HasPasswordError);
    }

    [Fact]
    public async Task LoginCommand_SetsInfoBarErrorWhenCredentialsAreInvalid()
    {
        var viewModel = new LoginViewModel((_, _) => (User?)null)
        {
            Username = "admin",
            Password = "wrong-password"
        };

        await viewModel.LoginCommand.ExecuteAsync(null);

        Assert.True(viewModel.HasError);
        Assert.False(string.IsNullOrWhiteSpace(viewModel.ErrorMessage));
        Assert.False(viewModel.HasUsernameError);
        Assert.False(viewModel.HasPasswordError);
        Assert.False(viewModel.IsBusy);
        Assert.Null(UserSession.CurrentUser);
    }

    [Fact]
    public async Task LoginCommand_CatchesDatabaseFailureAndSetsInfoBarErrorWithoutCrashing()
    {
        var viewModel = new LoginViewModel(new Func<string, string, User?>((_, _) =>
            throw new InvalidOperationException("SQL Server unavailable")))
        {
            Username = "admin",
            Password = "123456"
        };

        await viewModel.LoginCommand.ExecuteAsync(null);

        Assert.True(viewModel.HasError);
        Assert.False(string.IsNullOrWhiteSpace(viewModel.ErrorMessage));
        Assert.False(viewModel.HasUsernameError);
        Assert.False(viewModel.HasPasswordError);
        Assert.False(viewModel.IsBusy);
        Assert.Null(UserSession.CurrentUser);
    }

    [Fact]
    public async Task LoginCommand_TrimsUsernameOnlyStartsSessionAndLocksDoubleSubmitWhileBusy()
    {
        var completionSource = new TaskCompletionSource<User?>();
        var authCalls = new List<(string Username, string Password)>();
        User? succeededUser = null;

        var viewModel = new LoginViewModel((username, password) =>
        {
            authCalls.Add((username, password));
            return completionSource.Task;
        })
        {
            Username = "  admin  ",
            Password = " 123456 "
        };
        viewModel.LoginSucceeded += user => succeededUser = user;

        var firstSubmit = viewModel.LoginCommand.ExecuteAsync(null);

        Assert.True(viewModel.IsBusy);
        Assert.True(viewModel.IsLoading);
        Assert.False(viewModel.IsNotBusy);
        Assert.False(viewModel.LoginCommand.CanExecute(null));

        // Attempt a duplicate submission while IsBusy is true
        await viewModel.LoginCommand.ExecuteAsync(null);
        Assert.Single(authCalls);
        Assert.Equal(("admin", " 123456 "), authCalls[0]);

        var expectedUser = new User
        {
            UserId = 1,
            Username = "admin",
            Password = " 123456 ",
            FullName = "Trần Minh Quân",
            Role = RolePermissions.Administrator
        };
        completionSource.SetResult(expectedUser);
        await firstSubmit;

        Assert.False(viewModel.IsBusy);
        Assert.True(viewModel.LoginCommand.CanExecute(null));
        Assert.Equal("admin", viewModel.Username);
        Assert.Equal(string.Empty, viewModel.Password);
        Assert.Same(expectedUser, UserSession.CurrentUser);
        Assert.Same(expectedUser, succeededUser);
        Assert.False(viewModel.HasError);
    }

    [Theory]
    [InlineData("admin", "123456", "Trần Minh Quân", "Administrator", "Quản trị", 8, true)]
    [InlineData("staff", "123456", "Lê Thu Ngân", "Staff", "Lễ tân", 6, false)]
    public async Task LoginToShellAndLogout_ScopesSessionByRoleAndResetsStateOnLogout(
        string username,
        string password,
        string fullName,
        string role,
        string expectedRoleDisplay,
        int expectedNavCount,
        bool expectsRestrictedModules)
    {
        var passwordClearedEvents = 0;
        var loginViewModel = new LoginViewModel((u, p) =>
            u == username && p == password
                ? new User
                {
                    UserId = role == RolePermissions.Administrator ? 1 : 2,
                    Username = u,
                    Password = p,
                    FullName = fullName,
                    Role = role
                }
                : null);
        loginViewModel.PasswordClearRequested += () => passwordClearedEvents++;

        MainViewModel? launchedMainViewModel = null;
        loginViewModel.LoginSucceeded += user => launchedMainViewModel = new MainViewModel(user);

        loginViewModel.Username = username;
        loginViewModel.Password = password;
        await loginViewModel.LoginCommand.ExecuteAsync(null);

        Assert.NotNull(UserSession.CurrentUser);
        Assert.Equal(fullName, UserSession.CurrentUser.FullName);
        Assert.Equal(expectedRoleDisplay, RolePermissions.GetRoleDisplay(UserSession.CurrentUser.Role));
        Assert.Equal(expectsRestrictedModules, RolePermissions.IsAdministrator(UserSession.CurrentUser.Role));
        Assert.Equal(!expectsRestrictedModules, RolePermissions.IsStaff(UserSession.CurrentUser.Role));

        Assert.NotNull(launchedMainViewModel);
        Assert.IsType<RoomMapViewModel>(launchedMainViewModel.CurrentView);
        Assert.Equal("Sơ đồ phòng", launchedMainViewModel.PageTitle);
        Assert.Equal(fullName, launchedMainViewModel.FullName);
        Assert.Equal(expectedRoleDisplay, launchedMainViewModel.RoleDisplay);
        Assert.Equal(expectedNavCount, launchedMainViewModel.NavigationItems.Count);
        Assert.Equal(
            expectsRestrictedModules,
            launchedMainViewModel.NavigationItems.Any(item => item.Key == NavigationKey.RoomType));
        Assert.Equal(
            expectsRestrictedModules,
            launchedMainViewModel.NavigationItems.Any(item => item.Key == NavigationKey.Report));

        // Simulate typing into password again or reusing login view model, then logging out from MainViewModel
        loginViewModel.Password = "stale-secret";
        launchedMainViewModel.LogoutRequested += loginViewModel.Reset;
        launchedMainViewModel.LogoutCommand.Execute(null);

        Assert.Null(UserSession.CurrentUser);
        Assert.False(launchedMainViewModel.IsSessionActive);
        Assert.Equal(string.Empty, loginViewModel.Username);
        Assert.Equal(string.Empty, loginViewModel.Password);
        Assert.True(loginViewModel.IsEmpty);
        Assert.True(passwordClearedEvents >= 2);
    }

    [Fact]
    public async Task Cancel_WhileAuthenticationIsInFlight_PreventsSessionStartAndLoginSucceeded()
    {
        var completionSource = new TaskCompletionSource<User?>();
        var succeeded = false;
        var viewModel = new LoginViewModel((_, _) => completionSource.Task)
        {
            Username = "admin",
            Password = "123456"
        };
        viewModel.LoginSucceeded += _ => succeeded = true;

        var inFlightTask = viewModel.LoginCommand.ExecuteAsync(null);
        Assert.True(viewModel.IsBusy);

        viewModel.Cancel();
        completionSource.SetResult(new User
        {
            UserId = 1,
            Username = "admin",
            Password = "123456",
            FullName = "Trần Minh Quân",
            Role = RolePermissions.Administrator
        });
        await inFlightTask;

        Assert.False(succeeded);
        Assert.False(viewModel.IsBusy);
        Assert.Null(UserSession.CurrentUser);
    }

    [Fact]
    public async Task LoginCommand_RejectsUnsupportedUserRoleAndKeepsSessionCleared()
    {
        var succeeded = false;
        var viewModel = new LoginViewModel((_, _) => new User
        {
            UserId = 99,
            Username = "guest",
            Password = "123456",
            FullName = "Khách Vãng Lai",
            Role = "Guest"
        })
        {
            Username = "guest",
            Password = "123456"
        };
        viewModel.LoginSucceeded += _ => succeeded = true;

        await viewModel.LoginCommand.ExecuteAsync(null);

        Assert.False(succeeded);
        Assert.True(viewModel.HasError);
        Assert.Null(UserSession.CurrentUser);
    }
}
