using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NexusStore.Data;
using NexusStore.Models;
using NexusStore.Models.ViewModels;

namespace NexusStore.Controllers;

public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var totalOrders = await _context.Orders.CountAsync();
        var totalRevenue = await _context.Orders.SumAsync(o => o.TotalAmount);
        var totalProducts = await _context.Products.CountAsync();
        var lowStockProducts = await _context.Products
            .Where(p => p.StockQuantity <= 5)
            .OrderBy(p => p.StockQuantity)
            .ToListAsync();

        var recentOrders = await _context.Orders
            .Include(o => o.Items)
            .OrderByDescending(o => o.CreatedAt)
            .Take(6)
            .ToListAsync();

        var topProducts = await _context.Products
            .OrderByDescending(p => p.ReviewCount)
            .Take(5)
            .ToListAsync();

        var statusGroups = await _context.Orders
            .GroupBy(o => o.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToDictionaryAsync(g => g.Status, g => g.Count);

        var vm = new AdminDashboardViewModel
        {
            TotalRevenue = totalRevenue,
            TotalOrders = totalOrders,
            TotalProducts = totalProducts,
            LowStockCount = lowStockProducts.Count,
            AverageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0,
            RecentOrders = recentOrders,
            LowStockProducts = lowStockProducts,
            TopProducts = topProducts,
            OrdersCountByStatus = statusGroups
        };

        return View(vm);
    }

    public async Task<IActionResult> Products(string? search, int? categoryId)
    {
        var categories = await _context.Categories.ToListAsync();
        var query = _context.Products.Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Name.ToLower().Contains(search.ToLower()) || p.Sku.ToLower().Contains(search.ToLower()));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        var products = await query.OrderByDescending(p => p.Id).ToListAsync();

        ViewBag.Categories = categories;
        ViewBag.Search = search;
        ViewBag.CategoryId = categoryId;

        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> CreateProduct()
    {
        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
        return View("ProductForm", new Product { StockQuantity = 15, Rating = 4.8 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Slug))
        {
            product.Slug = product.Name.ToLower().Replace(" ", "-").Replace("'", "").Replace("\"", "");
        }

        if (ModelState.IsValid)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Product '{product.Name}' created successfully!";
            return RedirectToAction(nameof(Products));
        }

        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", product.CategoryId);
        return View("ProductForm", product);
    }

    [HttpGet]
    public async Task<IActionResult> EditProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", product.CategoryId);
        return View("ProductForm", product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProduct(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(product.Slug))
        {
            product.Slug = product.Name.ToLower().Replace(" ", "-");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Product '{product.Name}' updated successfully!";
                return RedirectToAction(nameof(Products));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
        }

        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", product.CategoryId);
        return View("ProductForm", product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Product '{product.Name}' has been deleted.";
        }

        return RedirectToAction(nameof(Products));
    }

    public async Task<IActionResult> Orders(OrderStatus? status)
    {
        var query = _context.Orders.Include(o => o.Items).AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(o => o.Status == status.Value);
        }

        var orders = await query.OrderByDescending(o => o.CreatedAt).ToListAsync();
        ViewBag.SelectedStatus = status;

        return View(orders);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, OrderStatus status)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order != null)
        {
            order.Status = status;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Order {order.OrderNumber} status updated to {status}.";
        }

        return RedirectToAction(nameof(Orders));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ResetDemoData()
    {
        DbInitializer.ResetAndSeed(_context);
        TempData["SuccessMessage"] = "Store demo data has been reset to defaults!";
        return RedirectToAction(nameof(Index));
    }
}
