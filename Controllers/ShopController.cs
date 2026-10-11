using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusStore.Data;
using NexusStore.Models.ViewModels;

namespace NexusStore.Controllers;

public class ShopController : Controller
{
    private readonly AppDbContext _context;

    public ShopController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? categoryId, string? category, string? search, string? sort, decimal? minPrice, decimal? maxPrice, bool inStock = false)
    {
        var categories = await _context.Categories
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        var query = _context.Products.Include(p => p.Category).AsQueryable();

        // Filter by category slug if provided
        if (!string.IsNullOrEmpty(category))
        {
            var matchedCat = categories.FirstOrDefault(c => c.Slug.Equals(category, StringComparison.OrdinalIgnoreCase));
            if (matchedCat != null)
            {
                categoryId = matchedCat.Id;
            }
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(term) || 
                                     p.Description.ToLower().Contains(term) ||
                                     (p.Category != null && p.Category.Name.ToLower().Contains(term)));
        }

        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        if (inStock)
        {
            query = query.Where(p => p.StockQuantity > 0);
        }

        // Sorting
        query = sort switch
        {
            "price-asc" => query.OrderBy(p => p.Price),
            "price-desc" => query.OrderByDescending(p => p.Price),
            "rating" => query.OrderByDescending(p => p.Rating),
            "newest" => query.OrderByDescending(p => p.CreatedAt),
            _ => query.OrderByDescending(p => p.IsFeatured).ThenByDescending(p => p.Rating)
        };

        var products = await query.ToListAsync();

        var vm = new ShopViewModel
        {
            Products = products,
            Categories = categories,
            SelectedCategoryId = categoryId,
            SelectedCategorySlug = category,
            SearchQuery = search,
            SortBy = sort ?? "featured",
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            OnlyInStock = inStock,
            TotalCount = products.Count
        };

        return View(vm);
    }

    public async Task<IActionResult> Details(int? id, string? slug)
    {
        if (id == null && string.IsNullOrEmpty(slug))
        {
            return NotFound();
        }

        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => (id.HasValue && p.Id == id.Value) || (!string.IsNullOrEmpty(slug) && p.Slug == slug));

        if (product == null)
        {
            return NotFound();
        }

        // Related products in the same category
        var related = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
            .Take(4)
            .ToListAsync();

        ViewBag.RelatedProducts = related;

        return View(product);
    }
}
