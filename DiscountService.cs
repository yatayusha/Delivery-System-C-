using DeliverySystem.Interfaces;
using DeliverySystem.Models.AllForOrders;
using DeliverySystem.Models.AllForOrders.Patterns.Strategy;

namespace DeliverySystem.Services;

public class DiscountService
{
    private IDiscountStrategy _discountStrategy;

    public DiscountService()
    {
        _discountStrategy = new NoDiscStrategy();
    }

    public void SetDiscountStrategy(IDiscountStrategy strategy)
    {
        _discountStrategy = strategy;
    }

    public decimal ApplyDiscount(Order order)
    {
        var discountedPrice = _discountStrategy.ApplyDiscount(order);
        return discountedPrice;
    }
}