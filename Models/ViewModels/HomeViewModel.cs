namespace NexusStore.Models.ViewModels;

public class HomeViewModel
{
    public List<Category> Categories { get; set; } = new();
    public List<Product> FeaturedProducts { get; set; } = new();
    public List<Product> NewArrivals { get; set; } = new();
    public List<Product> TrendingDeals { get; set; } = new();
}
