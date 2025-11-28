using DeliverySystem.Models.AllForOrders;

namespace DeliverySystem.Interfaces;

// те кто получают уведомление
public interface IOrderObservable
{
    void AddObserver(IOrderObserver observer);
    void RemoveObserver(IOrderObserver observer);
    void NotifyObservers(IOrder order, OrderStatus oldStatus, OrderStatus newStatus);
}