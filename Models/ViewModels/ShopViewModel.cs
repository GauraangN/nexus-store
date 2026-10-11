namespace NexusStore.Models.ViewModels;

public class ShopViewModel
{
    public List<Product> Products { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public int? SelectedCategoryId { get; set; }
    public string? SelectedCategorySlug { get; set; }
    public string? SearchQuery { get; set; }
    public string SortBy { get; set; } = "featured";
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool OnlyInStock { get; set; }
    public int TotalCount { get; set; }
}
