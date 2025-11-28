using DeliverySystem.Models.AllForOrders;
using DeliverySystem.Models;
using DeliverySystem.Interfaces;
using DeliverySystem.Patterns.Creators;

// только координация операций 
namespace DeliverySystem.Services;

public class OrderService : IManageOrder
{
    private readonly IPositionRepository _positionRepository;
    private readonly IOrderObservable? _orderNotifier;
    
    public OrderService(IPositionRepository positionRepository, IOrderObservable? orderNotifier = null)
    {
        _positionRepository = positionRepository;
        _orderNotifier = orderNotifier;
    }
    
    public IOrder CreateOrder(string orderType, string orderId, Customer customer)
    {
        return OrderFactory.CreateOrder(orderType, orderId, customer);
    }
    
    public IOrder CreateOrderWithItems(string orderType, string orderId, Customer customer, List<OrderItem> items)
    {
        return OrderFactory.CreateOrderWithItems(orderType, orderId, customer, items);
    }
    
    public void AddItem(IOrder order, string positionName, int quantity)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        
        var position = _positionRepository.GetByName(positionName)
                       ?? throw new ArgumentException($"Позиция '{positionName}' не найдена в меню");
        
        var orderItem = new OrderItem(position, quantity);
        order.AddOrderItem(orderItem);
    }

    public void RemoveItem(IOrder order, string positionName, int quantity = 0)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        order.RemoveOrderItem(positionName, quantity);
    }
    
    public decimal CalculateTotal(IOrder order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        return order.CalculateTotal();
    }
    
    public string GetOrderInfo(IOrder order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        return order.GetOrderSummary();
    }
    
    public void ChangeOrderStatus(IOrder order, OrderStatus newStatus)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        order.ChangeStatus(newStatus);
    }
}