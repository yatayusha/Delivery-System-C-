using DeliverySystem.Models.AllForOrders;
using DeliverySystem.Models;
using DeliverySystem.Interfaces;

namespace DeliverySystem.Patterns.Creators;

public class ExpressOrderCreator : OrderCreator
{
    public override IOrder CreateOrder(string orderId, Customer customer)
    {
        return new ExpressOrder(orderId, customer);
    }
}