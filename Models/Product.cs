using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusStore.Models;

public class Product
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public string ShortDescription { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, 100000)]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? CompareAtPrice { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    [Required]
    public string ImageUrl { get; set; } = string.Empty;

    public string? SecondaryImageUrl { get; set; }

    [Range(0, 5000)]
    public int StockQuantity { get; set; } = 10;

    public string Sku { get; set; } = string.Empty;

    public double Rating { get; set; } = 4.8;

    public int ReviewCount { get; set; } = 42;

    public bool IsFeatured { get; set; } = false;

    public string? Badge { get; set; }

    public string? Features { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [NotMapped]
    public bool InStock => StockQuantity > 0;

    [NotMapped]
    public int DiscountPercent => CompareAtPrice.HasValue && CompareAtPrice > Price
        ? (int)Math.Round(((CompareAtPrice.Value - Price) / CompareAtPrice.Value) * 100)
        : 0;
}
