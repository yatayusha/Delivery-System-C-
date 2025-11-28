using DeliverySystem.Interfaces;
namespace DeliverySystem.Models.AllForOrders.Patterns.Observer;

// observe pattern
public class KitchenNotifier : IOrderObserver
{
    public List<string> KitchenOrders { get; } = new List<string>();
    
    public void Update(IOrder order, OrderStatus oldStatus, OrderStatus newStatus)
    {
        if (newStatus == OrderStatus.Preparing)
        {
            var message = $"Заказ {order.OrderId} передан на приготовление";
            KitchenOrders.Add(message);
        }
    }
    
    public void ClearOrders() => KitchenOrders.Clear();
}