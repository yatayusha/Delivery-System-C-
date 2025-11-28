using DeliverySystem.Interfaces;
namespace DeliverySystem.Models.AllForOrders.Patterns.Observer;

// observe pattern
public class CustomerNotifier : IOrderObserver
{
    public List<string> Notifications { get; } = new List<string>();
    
    public void Update(IOrder order, OrderStatus oldStatus, OrderStatus newStatus)
    {
        var message = $"Заказ {order.OrderId} сменил статус с {oldStatus} на {newStatus}";
        Notifications.Add(message);
    }
    
    public void ClearNotifications() => Notifications.Clear();
}