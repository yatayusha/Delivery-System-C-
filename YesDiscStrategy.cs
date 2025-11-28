using DeliverySystem.Interfaces;

namespace DeliverySystem.Models.AllForOrders.Patterns.Strategy;

// strategy pattern
public class YesDiscStrategy : IDiscountStrategy
{
    private readonly decimal _percentage;
    
    public YesDiscStrategy(decimal percentage)
    {
        _percentage = percentage;
    }
    
    public decimal ApplyDiscount(Order order)
    {
        return order.TotalPrice * (1 - _percentage / 100);
    }
    
    public string GetDiscountDescription() => $"Скидка {_percentage}%";
}