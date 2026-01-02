using System.ComponentModel.DataAnnotations;

namespace Boolk.Models;

public abstract class RestaurantBase
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(67, ErrorMessage = "Name cannot be longer than 67 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required")]
    [StringLength(67, ErrorMessage = "City cannot be longer than 67 characters")]
    public string City { get; set; } = string.Empty;
}

