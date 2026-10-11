using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusStore.Models;

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}

public class Order
{
    public int Id { get; set; }

    [Required, StringLength(30)]
    public string OrderNumber { get; set; } = string.Empty;

    [Required, StringLength(100)]
    [Display(Name = "Full Name")]
    public string CustomerName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(120)]
    [Display(Name = "Email Address")]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required, Phone, StringLength(25)]
    [Display(Name = "Phone Number")]
    public string CustomerPhone { get; set; } = string.Empty;

    [Required, StringLength(200)]
    [Display(Name = "Address")]
    public string ShippingAddress { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string State { get; set; } = string.Empty;

    [Required, StringLength(20)]
    [Display(Name = "Postal / Zip Code")]
    public string PostalCode { get; set; } = string.Empty;

    [Required, StringLength(60)]
    public string Country { get; set; } = "United States";

    [Required, StringLength(50)]
    public string PaymentMethod { get; set; } = "Credit Card (Demo)";

    public OrderStatus Status { get; set; } = OrderStatus.Processing;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Subtotal { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ShippingFee { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    public string? PromoCode { get; set; }

    public string? OrderNotes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<OrderItem> Items { get; set; } = new();
}

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    [Required, StringLength(120)]
    public string ProductName { get; set; } = string.Empty;

    public string ProductImageUrl { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }
}
