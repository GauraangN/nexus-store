using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusStore.Data;
using NexusStore.Models;
using NexusStore.Models.ViewModels;

namespace NexusStore.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        var featured = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsFeatured)
            .Take(8)
            .ToListAsync();

        var newArrivals = await _context.Products
            .Include(p => p.Category)
            .OrderByDescending(p => p.CreatedAt)
            .Take(4)
            .ToListAsync();

        var trendingDeals = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.CompareAtPrice.HasValue && p.CompareAtPrice > p.Price)
            .Take(4)
            .ToListAsync();

        var vm = new HomeViewModel
        {
            Categories = categories,
            FeaturedProducts = featured,
            NewArrivals = newArrivals,
            TrendingDeals = trendingDeals
        };

        return View(vm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
