using DeliverySystem.Models.AllForOrders;

namespace DeliverySystem.Services;

// все доп рассчеты по заказу
public static class OrderCalculations
{
    public static decimal CalculateOrderTotal(IEnumerable<OrderItem> orderItems)
    {
        return orderItems.Sum(item => item.TotalPrice);
    }
    
    public static decimal ApplyDiscount(decimal total, decimal discount)
    {
        return total - (total * (discount/100));
    }

    public static decimal AddDeliveryPrice(decimal total, decimal deliveryPrice)
    {
        return total + Math.Max(0, deliveryPrice);
    }

    public static TimeSpan DeliveryTime(Order order, TimeSpan deliveryTime)
    {
        TimeSpan cookingTime = TimeSpan.Zero;
        foreach (var item in order.OrderItems)
        {
            cookingTime += item.Position.CookingTime;
        }
        return cookingTime + deliveryTime;
    }
}