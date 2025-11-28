using DeliverySystem.Models.AllForOrders;
namespace DeliverySystem.Interfaces;

// издатель меняет заказ
public interface IOrderObserver
{
    void Update(IOrder order, OrderStatus oldStatus, OrderStatus newStatus);
}