using DeliverySystem.Models.AllForOrders;
using DeliverySystem.Models;
namespace DeliverySystem.Interfaces;

public interface IManageOrder
{
    IOrder CreateOrder(string orderType, string orderId, Customer customer);
    IOrder CreateOrderWithItems(string orderType, string orderId, Customer customer, List<OrderItem> items);
    void AddItem(IOrder order, string positionName, int quantity);
    void RemoveItem(IOrder order, string positionName, int quantity = 0);
    decimal CalculateTotal(IOrder order);
    string GetOrderInfo(IOrder order);
    void ChangeOrderStatus(IOrder order, OrderStatus newStatus);
}