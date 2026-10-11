using Microsoft.AspNetCore.Mvc;
using NexusStore.Data;
using NexusStore.Models;
using NexusStore.Models.ViewModels;
using NexusStore.Services;

namespace NexusStore.Controllers;

public class CheckoutController : Controller
{
    private readonly AppDbContext _context;
    private readonly ICartService _cartService;

    public CheckoutController(AppDbContext context, ICartService cartService)
    {
        _context = context;
        _cartService = cartService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var cart = _cartService.GetCart();
        if (cart.Items.Count == 0)
        {
            TempData["ErrorMessage"] = "Your cart is empty. Please add items before checking out.";
            return RedirectToAction("Index", "Shop");
        }

        var vm = new CheckoutViewModel
        {
            Cart = cart
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
    {
        var cart = _cartService.GetCart();
        if (cart.Items.Count == 0)
        {
            TempData["ErrorMessage"] = "Your shopping cart is empty.";
            return RedirectToAction("Index", "Shop");
        }

        model.Cart = cart;

        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }

        var orderNumber = $"NEX-{DateTime.UtcNow:yyMM}-{Random.Shared.Next(100000, 999999)}";

        var order = new Order
        {
            OrderNumber = orderNumber,
            CustomerName = model.CustomerName,
            CustomerEmail = model.CustomerEmail,
            CustomerPhone = model.CustomerPhone,
            ShippingAddress = model.ShippingAddress,
            City = model.City,
            State = model.State,
            PostalCode = model.PostalCode,
            Country = model.Country,
            PaymentMethod = model.PaymentMethod,
            Status = OrderStatus.Processing,
            Subtotal = cart.Subtotal,
            DiscountAmount = cart.DiscountAmount,
            ShippingFee = cart.ShippingFee,
            TaxAmount = cart.EstimatedTax,
            TotalAmount = cart.GrandTotal,
            PromoCode = cart.CouponCode,
            OrderNotes = model.OrderNotes,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var item in cart.Items)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product != null)
            {
                // Decrement inventory
                product.StockQuantity = Math.Max(0, product.StockQuantity - item.Quantity);
            }

            order.Items.Add(new OrderItem
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ProductImageUrl = item.ImageUrl,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity,
                TotalPrice = item.TotalPrice
            });
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Clear cart after successful checkout
        _cartService.ClearCart();

        return RedirectToAction("Confirmation", "Orders", new { orderNumber = order.OrderNumber });
    }
}
