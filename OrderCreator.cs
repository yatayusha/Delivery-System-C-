using DeliverySystem.Models.AllForOrders;
using DeliverySystem.Models;
using DeliverySystem.Interfaces;

namespace DeliverySystem.Patterns.Creators;

public abstract class OrderCreator
{
    // fabric method pattern
    public abstract IOrder CreateOrder(string orderId, Customer customer);
    
    // создаем заказ с уже готовым набором позиций
    public IOrder CreateOrderWithItems(string orderId, Customer customer, List<OrderItem> items)
    {
        var order = CreateOrder(orderId, customer);
        
        foreach (var item in items)
        {
            order.AddOrderItem(item);
        }
        
        return order;
    }
}