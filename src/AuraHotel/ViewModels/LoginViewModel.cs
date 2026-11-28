using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AuraHotel.DAO;
using AuraHotel.Models;
using AuraHotel.Services;

namespace AuraHotel.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly Func<string, string, Task<User?>> _authenticateAsync;
    private bool _isCancelled;

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

    public bool HasUsernameError => !string.IsNullOrWhiteSpace(UsernameError);

    public bool HasPasswordError => !string.IsNullOrWhiteSpace(PasswordError);

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
        SuccessMessage = null;
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
        SuccessMessage = null;
        RefreshEmptyState();
    }

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

    public void Cancel()
    {
        _isCancelled = true;
        UserSession.Clear();
    }

    public void Reset()
    {
        _isCancelled = false;
        UserSession.Clear();
        Username = string.Empty;
        ClearPasswordInput();
        UsernameError = null;
        PasswordError = null;
        ErrorMessage = null;
        SuccessMessage = null;
        RefreshEmptyState();
    }

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync()
    {
        if (IsBusy)
        {
            return;
        }

        _isCancelled = false;
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
            RefreshEmptyState();
            return;
        }

        User? authenticatedUser;
        IsLoading = true;
        try
        {
            authenticatedUser = await _authenticateAsync(trimmedUsername, Password);
        }
        catch (Exception)
        {
            FailAuthentication("Không thể kết nối cơ sở dữ liệu. Vui lòng kiểm tra cấu hình SQL Server và thử lại.");
            return;
        }
        finally
        {
            IsLoading = false;
        }

        if (_isCancelled)
        {
            UserSession.Clear();
            return;
        }

        if (authenticatedUser is null)
        {
            FailAuthentication("Tên đăng nhập hoặc mật khẩu không chính xác.");
            return;
        }

        if (!RolePermissions.IsSupportedRole(authenticatedUser.Role))
        {
            FailAuthentication("Vai trò người dùng không hợp lệ trong hệ thống.");
            return;
        }

        Username = trimmedUsername;
        ClearPasswordInput();
        UserSession.Start(authenticatedUser);
        SuccessMessage = $"Xin chào {authenticatedUser.FullName}.";
        RefreshEmptyState();
        LoginSucceeded?.Invoke(authenticatedUser);
    }

    private void ClearPasswordInput()
    {
        var wasPasswordAlreadyEmpty = string.IsNullOrEmpty(Password);
        Password = string.Empty;
        if (wasPasswordAlreadyEmpty)
        {
            PasswordClearRequested?.Invoke();
        }
    }

    private void FailAuthentication(string message)
    {
        UserSession.Clear();
        ErrorMessage = message;
        RefreshEmptyState();
    }

    private void RefreshEmptyState() =>
        IsEmpty = UserSession.CurrentUser is null
            && string.IsNullOrWhiteSpace(Username)
            && string.IsNullOrEmpty(Password)
            && !HasUsernameError
            && !HasPasswordError
            && !HasError
            && !HasSuccessMessage;
}
