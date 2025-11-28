namespace DeliverySystem.Models.AllForOrders;


public static class SomeOrderChecks
{
    public static bool CanBeModified(OrderStatus status)
    {
        return status == OrderStatus.Pending || status == OrderStatus.Confirmed;
    }

    public static bool CanChangeStatus(OrderStatus currentStatus, OrderStatus newStatus)
    {
        return currentStatus switch
        {
            OrderStatus.Pending => newStatus is OrderStatus.Confirmed or OrderStatus.Cancelled,
            OrderStatus.Confirmed => newStatus is OrderStatus.Preparing or OrderStatus.Cancelled,
            OrderStatus.Preparing => newStatus is OrderStatus.ReadyForDelivery or OrderStatus.Cancelled,
            OrderStatus.ReadyForDelivery => newStatus is OrderStatus.Shipped or OrderStatus.Cancelled,
            OrderStatus.Shipped => newStatus == OrderStatus.Delivered,
            _ => false
        };
    }
    
    public static void OrderItemChecks(OrderItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        if (item.Quantity <= 0) throw new ArgumentException("Количество должно быть положительным");
        if (!item.Position.IsInStock) throw new InvalidOperationException("Позиция отсутствует в наличии");
    }
}