using DeliverySystem.Models.AllForOrders;
using DeliverySystem.Models;
using DeliverySystem.Interfaces;

namespace DeliverySystem.Patterns.Creators;
// у нас каждый создатель отвечает только за свой тип заказа 
public class StandardOrderCreator : OrderCreator
{
    public override IOrder CreateOrder(string orderId, Customer customer)
    {
        return new StandardOrder(orderId, customer);
    }
}