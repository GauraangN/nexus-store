using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NexusStore.Data;
using NexusStore.Models;

namespace NexusStore.Services;

public class CartService : ICartService
{
    private const string CartSessionKey = "_NexusCart_";
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _context;

    public CartService(IHttpContextAccessor httpContextAccessor, AppDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    private ISession Session => _httpContextAccessor.HttpContext?.Session 
        ?? throw new InvalidOperationException("Session is not available.");

    public Cart GetCart()
    {
        var json = Session.GetString(CartSessionKey);
        if (string.IsNullOrEmpty(json))
        {
            var newCart = new Cart();
            SaveCart(newCart);
            return newCart;
        }

        try
        {
            var cart = JsonSerializer.Deserialize<Cart>(json);
            return cart ?? new Cart();
        }
        catch
        {
            var fallback = new Cart();
            SaveCart(fallback);
            return fallback;
        }
    }

    private void SaveCart(Cart cart)
    {
        var json = JsonSerializer.Serialize(cart);
        Session.SetString(CartSessionKey, json);
    }

    public void AddItem(int productId, int quantity = 1)
    {
        if (quantity <= 0) quantity = 1;

        var product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == productId);
        if (product == null || product.StockQuantity <= 0) return;

        var cart = GetCart();
        var existing = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (existing != null)
        {
            var newQty = existing.Quantity + quantity;
            existing.Quantity = Math.Min(newQty, product.StockQuantity);
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductSlug = product.Slug,
                ImageUrl = product.ImageUrl,
                CategoryName = product.Category?.Name ?? "General",
                UnitPrice = product.Price,
                Quantity = Math.Min(quantity, product.StockQuantity)
            });
        }

        SaveCart(cart);
    }

    public void UpdateQuantity(int productId, int quantity)
    {
        var cart = GetCart();
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null) return;

        if (quantity <= 0)
        {
            cart.Items.Remove(item);
        }
        else
        {
            var product = _context.Products.Find(productId);
            var maxStock = product?.StockQuantity ?? 99;
            item.Quantity = Math.Min(quantity, maxStock);
        }

        SaveCart(cart);
    }

    public void RemoveItem(int productId)
    {
        var cart = GetCart();
        cart.Items.RemoveAll(i => i.ProductId == productId);
        SaveCart(cart);
    }

    public bool ApplyCoupon(string code)
    {
        var cart = GetCart();
        var normalized = code?.Trim().ToUpperInvariant() ?? "";

        if (normalized == "DEMO20")
        {
            cart.CouponCode = "DEMO20 (20% OFF)";
            cart.DiscountPercent = 20;
            SaveCart(cart);
            return true;
        }
        else if (normalized == "SAVE10")
        {
            cart.CouponCode = "SAVE10 (10% OFF)";
            cart.DiscountPercent = 10;
            SaveCart(cart);
            return true;
        }
        else if (normalized == "VIP50")
        {
            cart.CouponCode = "VIP50 (50% OFF)";
            cart.DiscountPercent = 50;
            SaveCart(cart);
            return true;
        }

        return false;
    }

    public void RemoveCoupon()
    {
        var cart = GetCart();
        cart.CouponCode = null;
        cart.DiscountPercent = 0;
        SaveCart(cart);
    }

    public void ClearCart()
    {
        var cart = new Cart();
        SaveCart(cart);
    }
}
