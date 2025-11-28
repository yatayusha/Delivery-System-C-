using DeliverySystem.Models.AllForOrders;

namespace DeliverySystem.Interfaces;

public interface IOrder
{
    string OrderId { get; }
    decimal TotalPrice { get; set; }
    OrderStatus Status { get; set; }
    List<OrderItem> OrderItems { get; }
    DateTime CreatedAt { get; }
    DateTime? DeliveredAt { get; set; }
    
    void ChangeStatus(OrderStatus newStatus);
    decimal CalculateTotal();
    void AddOrderItem(OrderItem orderItem);
    void RemoveOrderItem(OrderItem orderItem);
    void RemoveOrderItem(string positionName, int quantity = 0);
    void ClearOrderItems();
    TimeSpan GetOrderAge();
    string GetOrderSummary();
}