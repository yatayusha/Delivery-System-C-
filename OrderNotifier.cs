using DeliverySystem.Interfaces;
using DeliverySystem.Models.AllForOrders;

namespace DeliverySystem.Patterns.Observer;

// observe pattern
public class OrderNotifier : IOrderObservable
{
    public readonly List<IOrderObserver> _observers = new();

    public void AddObserver(IOrderObserver observer)
    {
        throw new NotImplementedException();
    }

    public void RemoveObserver(IOrderObserver observer)
    { 
        _observers.Remove(observer);
    }

    public void NotifyObservers(IOrder order, OrderStatus oldStatus, OrderStatus newStatus)
    {
        foreach (var observer in _observers)
        {
            observer.Update(order, oldStatus, newStatus);
        }
    }
}