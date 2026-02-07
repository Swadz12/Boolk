namespace Boolk.Domain.Entities;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
}
