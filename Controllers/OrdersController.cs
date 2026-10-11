using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusStore.Data;

namespace NexusStore.Controllers;

public class OrdersController : Controller
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Confirmation(string orderNumber)
    {
        if (string.IsNullOrEmpty(orderNumber))
        {
            return RedirectToAction("Index", "Home");
        }

        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    public async Task<IActionResult> Track(string? orderNumber)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
        {
            return View();
        }

        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.OrderNumber.ToUpper() == orderNumber.Trim().ToUpper());

        ViewBag.Searched = true;
        ViewBag.SearchTerm = orderNumber;

        return View(order);
    }
}
