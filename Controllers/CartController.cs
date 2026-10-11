using Microsoft.AspNetCore.Mvc;
using NexusStore.Services;

namespace NexusStore.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    public IActionResult Index()
    {
        var cart = _cartService.GetCart();
        return View(cart);
    }

    [HttpPost]
    public IActionResult Add(int productId, int quantity = 1, bool isAjax = false)
    {
        _cartService.AddItem(productId, quantity);
        var cart = _cartService.GetCart();

        if (isAjax || Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Json(new
            {
                success = true,
                message = "Product added to cart!",
                totalItems = cart.TotalItems,
                subtotal = cart.Subtotal.ToString("C"),
                grandTotal = cart.GrandTotal.ToString("C"),
                items = cart.Items
            });
        }

        TempData["SuccessMessage"] = "Product added to cart!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Update(int productId, int quantity, bool isAjax = false)
    {
        _cartService.UpdateQuantity(productId, quantity);
        var cart = _cartService.GetCart();

        if (isAjax || Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Json(new
            {
                success = true,
                totalItems = cart.TotalItems,
                subtotal = cart.Subtotal.ToString("C"),
                discount = cart.DiscountAmount.ToString("C"),
                shipping = cart.ShippingFee == 0 ? "FREE" : cart.ShippingFee.ToString("C"),
                tax = cart.EstimatedTax.ToString("C"),
                grandTotal = cart.GrandTotal.ToString("C"),
                items = cart.Items
            });
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Remove(int productId, bool isAjax = false)
    {
        _cartService.RemoveItem(productId);
        var cart = _cartService.GetCart();

        if (isAjax || Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Json(new
            {
                success = true,
                totalItems = cart.TotalItems,
                subtotal = cart.Subtotal.ToString("C"),
                grandTotal = cart.GrandTotal.ToString("C"),
                items = cart.Items
            });
        }

        TempData["SuccessMessage"] = "Item removed from cart.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult ApplyCoupon(string couponCode)
    {
        if (string.IsNullOrWhiteSpace(couponCode))
        {
            TempData["ErrorMessage"] = "Please enter a valid coupon code.";
            return RedirectToAction(nameof(Index));
        }

        bool applied = _cartService.ApplyCoupon(couponCode);
        if (applied)
        {
            TempData["SuccessMessage"] = $"Promo code '{couponCode.ToUpper()}' successfully applied!";
        }
        else
        {
            TempData["ErrorMessage"] = "Invalid promo code. Try 'DEMO20' for 20% off or 'SAVE10' for 10% off.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult RemoveCoupon()
    {
        _cartService.RemoveCoupon();
        TempData["SuccessMessage"] = "Coupon code removed.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult GetCartData()
    {
        var cart = _cartService.GetCart();
        return Json(new
        {
            totalItems = cart.TotalItems,
            subtotal = cart.Subtotal.ToString("C"),
            discount = cart.DiscountAmount.ToString("C"),
            couponCode = cart.CouponCode,
            shipping = cart.ShippingFee == 0 ? "FREE" : cart.ShippingFee.ToString("C"),
            tax = cart.EstimatedTax.ToString("C"),
            grandTotal = cart.GrandTotal.ToString("C"),
            items = cart.Items
        });
    }
}
