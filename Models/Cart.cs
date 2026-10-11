namespace NexusStore.Models;

public class CartItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductSlug { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; } = 1;

    public decimal TotalPrice => UnitPrice * Quantity;
}

public class Cart
{
    public List<CartItem> Items { get; set; } = new();
    public string? CouponCode { get; set; }
    public decimal DiscountPercent { get; set; } = 0;

    public int TotalItems => Items.Sum(i => i.Quantity);
    public decimal Subtotal => Items.Sum(i => i.TotalPrice);
    public decimal DiscountAmount => Math.Round(Subtotal * (DiscountPercent / 100m), 2);
    public decimal ShippingFee => Subtotal > 100 || Items.Count == 0 || DiscountPercent == 100 ? 0 : 9.99m;
    public decimal EstimatedTax => Math.Round((Subtotal - DiscountAmount) * 0.08m, 2);
    public decimal GrandTotal => Math.Max(0, Subtotal - DiscountAmount + ShippingFee + EstimatedTax);
}
