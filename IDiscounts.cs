using DeliverySystem.Models.AllForOrders;
namespace DeliverySystem.Interfaces;

public interface IDiscountStrategy
{
    decimal ApplyDiscount(Order order);
    string GetDiscountDescription();
}