using Boolk.Client.ApiClients;
using Boolk.Client.Auth;

namespace Boolk.Client.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly AuthApiClient _authClient;
    private readonly JwtAuthenticationStateProvider _authStateProvider;

    public LoginViewModel(AuthApiClient authClient, JwtAuthenticationStateProvider authStateProvider)
    {
        _authClient = authClient;
        _authStateProvider = authStateProvider;
    }

    private string _email = "";
    public string Email { get => _email; set => SetProperty(ref _email, value); }

    private string _password = "";
    public string Password { get => _password; set => SetProperty(ref _password, value); }

    private string _errorMessage = "";
    public string ErrorMessage { get => _errorMessage; private set => SetProperty(ref _errorMessage, value); }

    private bool _isSubmitting;
    public bool IsSubmitting { get => _isSubmitting; private set => SetProperty(ref _isSubmitting, value); }

    public async Task<bool> LoginAsync()
    {
        IsSubmitting = true;
        ErrorMessage = "";
        try
        {
            var response = await _authClient.LoginAsync(Email, Password);
            if (response?.Success == true)
            {
                _authStateProvider.NotifyAuthenticationStateChanged();
                return true;
            }
            ErrorMessage = response?.Error ?? "Login failed";
            return false;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return false;
        }
        finally
        {
            IsSubmitting = false;
        }
    }
}
