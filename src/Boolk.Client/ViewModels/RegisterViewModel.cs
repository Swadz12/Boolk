using System.ComponentModel.DataAnnotations;
using Boolk.Client.ApiClients;
using Boolk.Client.Auth;

namespace Boolk.Client.ViewModels;

public class RegisterViewModel : ViewModelBase
{
    private readonly AuthApiClient _authClient;
    private readonly JwtAuthenticationStateProvider _authStateProvider;

    public RegisterViewModel(AuthApiClient authClient, JwtAuthenticationStateProvider authStateProvider)
    {
        _authClient = authClient;
        _authStateProvider = authStateProvider;
    }

    public RegisterFormModel FormModel { get; set; } = new();

    private string? _errorMessage;
    public string? ErrorMessage { get => _errorMessage; private set => SetProperty(ref _errorMessage, value); }

    private bool _isSubmitting;
    public bool IsSubmitting { get => _isSubmitting; private set => SetProperty(ref _isSubmitting, value); }

    public async Task<bool> RegisterAsync()
    {
        IsSubmitting = true;
        ErrorMessage = null;
        try
        {
            var response = await _authClient.RegisterAsync(
                FormModel.Email, FormModel.Name, FormModel.Password, FormModel.BirthDate);

            if (response?.Success == true)
            {
                _authStateProvider.NotifyAuthenticationStateChanged();
                return true;
            }
            ErrorMessage = response?.Error ?? "Registration failed";
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

    public class RegisterFormModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Birth Date is required")]
        [MinimumAge(13, ErrorMessage = "You must be at least 13 years old to register")]
        public DateTime BirthDate { get; set; } = DateTime.Today;
    }

    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minAge;

        public MinimumAgeAttribute(int minAge)
        {
            _minAge = minAge;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime birthDate)
            {
                if (birthDate.AddYears(_minAge) > DateTime.Today)
                {
                    return new ValidationResult(ErrorMessage ?? $"You must be at least {_minAge} years old.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
