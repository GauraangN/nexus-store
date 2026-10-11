using System.ComponentModel.DataAnnotations;

namespace NexusStore.Models.ViewModels;

public class CheckoutViewModel
{
    public Cart Cart { get; set; } = new();

    [Required(ErrorMessage = "Full Name is required")]
    [Display(Name = "Full Name")]
    public string CustomerName { get; set; } = "Alex Mercer";

    [Required(ErrorMessage = "Email Address is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [Display(Name = "Email Address")]
    public string CustomerEmail { get; set; } = "alex.mercer@example.com";

    [Required(ErrorMessage = "Phone Number is required")]
    [Phone]
    [Display(Name = "Phone Number")]
    public string CustomerPhone { get; set; } = "+1 (555) 382-9901";

    [Required(ErrorMessage = "Shipping Address is required")]
    [Display(Name = "Street Address")]
    public string ShippingAddress { get; set; } = "742 Evergreen Terrace";

    [Required(ErrorMessage = "City is required")]
    public string City { get; set; } = "Springfield";

    [Required(ErrorMessage = "State / Province is required")]
    public string State { get; set; } = "OR";

    [Required(ErrorMessage = "Postal Code is required")]
    [Display(Name = "Postal / Zip Code")]
    public string PostalCode { get; set; } = "97477";

    [Required]
    public string Country { get; set; } = "United States";

    [Required]
    public string PaymentMethod { get; set; } = "Credit Card";

    public string? OrderNotes { get; set; }

    // Demo Credit Card fields for rich UI feel
    public string CardNumber { get; set; } = "•••• •••• •••• 4242";
    public string CardExpiry { get; set; } = "12/28";
    public string CardCvc { get; set; } = "345";
}
