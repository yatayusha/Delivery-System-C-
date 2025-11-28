using DeliverySystem.Models.AllForOrders;
using DeliverySystem.Models;
using DeliverySystem.Interfaces;
using DeliverySystem.Patterns.Observer;

namespace DeliverySystem.Patterns.Creators;

public static class OrderFactory
{
    public static IOrder CreateOrder(string orderType, string orderId, Customer customer, IOrderObservable? notifier = null)
    {
        var order = orderType.ToLower() switch
        {
            "обычный заказ" => new StandardOrderCreator().CreateOrder(orderId, customer),
            "экспресс заказ" => new ExpressOrderCreator().CreateOrder(orderId, customer),
            _ => throw new ArgumentException($"тип заказа неизвестен: {orderType}")
        };
        return new ObservableOrder(order, notifier);
    }

    public static IOrder CreateOrderWithItems(string orderType, string orderId, Customer customer, 
        List<OrderItem> items, IOrderObservable? notifier = null)
    {
        var order = orderType.ToLower() switch
        {
            "обычный заказ" => new StandardOrderCreator().CreateOrderWithItems(orderId, customer, items),
            "экспресс заказ" => new ExpressOrderCreator().CreateOrderWithItems(orderId, customer, items),
            _ => throw new ArgumentException($"тип заказа неизвестен: {orderType}")
        };
        return notifier != null ? new ObservableOrder(order, notifier) : order;
    }
}