using NexusStore.Models;

namespace NexusStore.Services;

public interface ICartService
{
    Cart GetCart();
    void AddItem(int productId, int quantity = 1);
    void UpdateQuantity(int productId, int quantity);
    void RemoveItem(int productId);
    bool ApplyCoupon(string code);
    void RemoveCoupon();
    void ClearCart();
}
