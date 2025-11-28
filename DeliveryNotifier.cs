using DeliverySystem.Interfaces;
using DeliverySystem.Models.AllForOrders;
namespace DeliverySystem.Patterns.Observer;

// observe pattern
public class DeliveryNotifier : IOrderObserver
{
    public List<string> ReadyForDeliveryOrders { get; } = new List<string>();
    
    public void Update(IOrder order, OrderStatus oldStatus, OrderStatus newStatus)
    {
        if (newStatus == OrderStatus.ReadyForDelivery)
        {
            var message = $"Заказ {order.OrderId} готов к доставке";
            ReadyForDeliveryOrders.Add(message);
        }
    }
    
    public void ClearDeliveries() => ReadyForDeliveryOrders.Clear();
}