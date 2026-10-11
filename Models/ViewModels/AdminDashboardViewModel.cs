namespace NexusStore.Models.ViewModels;

public class AdminDashboardViewModel
{
    public decimal TotalRevenue { get; set; }
    public int TotalOrders { get; set; }
    public int TotalProducts { get; set; }
    public int LowStockCount { get; set; }
    public decimal AverageOrderValue { get; set; }

    public List<Order> RecentOrders { get; set; } = new();
    public List<Product> LowStockProducts { get; set; } = new();
    public List<Product> TopProducts { get; set; } = new();
    public Dictionary<OrderStatus, int> OrdersCountByStatus { get; set; } = new();
}
