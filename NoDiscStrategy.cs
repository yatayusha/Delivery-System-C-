using DeliverySystem.Interfaces;
namespace DeliverySystem.Models.AllForOrders.Patterns.Strategy;

// strategy pattern
public class NoDiscStrategy : IDiscountStrategy
{
    public decimal ApplyDiscount(Order order) => order.TotalPrice;
    public string GetDiscountDescription() => "Без скидки";
}