using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelManagement.DAO;
using HotelManagement.Models;
using HotelManagement.Services;

namespace HotelManagement.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly Func<string, string, Task<User?>> _authenticateAsync;

    public event Action<User>? LoginSucceeded;

    public event Action? PasswordClearRequested;

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasUsernameError))]
    private string? _usernameError;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasPasswordError))]
    private string? _passwordError;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAuthenticated))]
    private User? _authenticatedUser;

    public bool HasUsernameError => !string.IsNullOrWhiteSpace(UsernameError);

    public bool HasPasswordError => !string.IsNullOrWhiteSpace(PasswordError);

    public bool IsAuthenticated => AuthenticatedUser is not null;

    public bool IsBusy => IsLoading;

    public bool IsNotBusy => !IsBusy;

    public LoginViewModel()
        : this(static (username, password) => Task.Run(() => UserDAO.Instance.Login(username, password)))
    {
    }

    public LoginViewModel(Func<string, string, User?> authenticate)
        : this(WrapSyncAuthenticator(authenticate))
    {
    }

    public LoginViewModel(Func<string, string, Task<User?>> authenticateAsync)
    {
        ArgumentNullException.ThrowIfNull(authenticateAsync);
        _authenticateAsync = authenticateAsync;
        LoadingMessage = "Đang xác thực thông tin đăng nhập...";
        IsEmpty = true;
    }

    private static Func<string, string, Task<User?>> WrapSyncAuthenticator(Func<string, string, User?> authenticate)
    {
        ArgumentNullException.ThrowIfNull(authenticate);
        return (username, password) => Task.FromResult(authenticate(username, password));
    }

    partial void OnUsernameChanged(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            UsernameError = null;
        }

        ErrorMessage = null;
        RefreshEmptyState();
    }

    partial void OnPasswordChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            PasswordClearRequested?.Invoke();
        }
        else
        {
            PasswordError = null;
        }

        ErrorMessage = null;
        RefreshEmptyState();
    }

    private void RefreshEmptyState() =>
        IsEmpty = AuthenticatedUser is null
            && string.IsNullOrWhiteSpace(Username)
            && string.IsNullOrEmpty(Password);

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName == nameof(IsLoading))
        {
            OnPropertyChanged(nameof(IsBusy));
            OnPropertyChanged(nameof(IsNotBusy));
            LoginCommand.NotifyCanExecuteChanged();
        }
    }

    private bool CanLogin() => !IsBusy;

    public void Reset()
    {
        var wasPasswordAlreadyEmpty = string.IsNullOrEmpty(Password);
        UserSession.Clear();
        AuthenticatedUser = null;
        Username = string.Empty;
        Password = string.Empty;
        UsernameError = null;
        PasswordError = null;
        ErrorMessage = null;
        SuccessMessage = null;

        if (wasPasswordAlreadyEmpty)
        {
            PasswordClearRequested?.Invoke();
        }

        RefreshEmptyState();
    }

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync()
    {
        if (IsBusy)
        {
            return;
        }

        ErrorMessage = null;
        SuccessMessage = null;

        var trimmedUsername = Username?.Trim() ?? string.Empty;
        UsernameError = string.IsNullOrEmpty(trimmedUsername)
            ? "Vui lòng nhập tên đăng nhập."
            : null;
        PasswordError = string.IsNullOrEmpty(Password)
            ? "Vui lòng nhập mật khẩu."
            : null;

        if (HasUsernameError || HasPasswordError)
        {
            return;
        }

        IsLoading = true;
        try
        {
            var user = await _authenticateAsync(trimmedUsername, Password);
            if (user is null)
            {
                UserSession.Clear();
                AuthenticatedUser = null;
                ErrorMessage = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                return;
            }

            if (!RolePermissions.IsAdministrator(user.Role) && !RolePermissions.IsStaff(user.Role))
            {
                UserSession.Clear();
                AuthenticatedUser = null;
                ErrorMessage = "Vai trò người dùng không hợp lệ trong hệ thống.";
                return;
            }

            var wasPasswordAlreadyEmpty = string.IsNullOrEmpty(Password);
            Username = trimmedUsername;
            AuthenticatedUser = user;
            Password = string.Empty;
            if (wasPasswordAlreadyEmpty)
            {
                PasswordClearRequested?.Invoke();
            }

            UserSession.Start(user);
            SuccessMessage = $"Xin chào {user.FullName}.";
            RefreshEmptyState();
            LoginSucceeded?.Invoke(user);
        }
        catch (Exception)
        {
            UserSession.Clear();
            AuthenticatedUser = null;
            ErrorMessage = "Không thể kết nối cơ sở dữ liệu. Vui lòng kiểm tra cấu hình SQL Server và thử lại.";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
