using DeliverySystem.Interfaces;
using DeliverySystem.Services;

namespace DeliverySystem.Models.AllForOrders;

// только бизнес логика и состояние заказа 
// базовый класс для фабричного метода
public class Order : IOrder
{
    
    public string OrderId { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    private Customer Customer { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    
    
    public Order(string orderId, Customer customer)
    {
        OrderId  = orderId;
        TotalPrice = 0;
        Status = OrderStatus.Pending;
        OrderItems = new List<OrderItem>();
        Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        CreatedAt = DateTime.Now;
        DeliveredAt = null;
        
    }

    public void ChangeStatus(OrderStatus newStatus)
    {
        if (!SomeOrderChecks.CanChangeStatus(Status, newStatus))
            throw new InvalidOperationException($"Недопустимый переход из {Status} в {newStatus}");
            
        Status = newStatus;
        if (newStatus == OrderStatus.Delivered)
            DeliveredAt = DateTime.Now;
    }

    public virtual decimal CalculateTotal()
    {
        TotalPrice = OrderCalculations.CalculateOrderTotal(OrderItems);
        return TotalPrice;
    }
    
    public decimal CalculateBaseTotal()
    {
        return OrderCalculations.CalculateOrderTotal(OrderItems);
    }
    
    public void AddOrderItem(OrderItem orderItem)
    {
        if (orderItem == null)
            throw new ArgumentNullException(nameof(orderItem));
        
        SomeOrderChecks.OrderItemChecks(orderItem);
        
        if (!SomeOrderChecks.CanBeModified(Status))
            throw new InvalidOperationException("Заказ нельзя изменить в текущем статусе");
        
        var existingItem = OrderItems.FirstOrDefault(item => 
            item.Position.Name.Equals(orderItem.Position.Name, StringComparison.OrdinalIgnoreCase));

        if (existingItem != null)
        {
            existingItem.Quantity += orderItem.Quantity;
        }
        else
        {
            OrderItems.Add(orderItem);
        }

        CalculateTotal();
    }

    public void RemoveOrderItem(OrderItem orderItem)
    {
        if (orderItem == null)
            throw new ArgumentNullException(nameof(orderItem));
        
        if (!SomeOrderChecks.CanBeModified(Status))
            throw new InvalidOperationException("Заказ нельзя изменить в текущем статусе");

        OrderItems.Remove(orderItem);
        CalculateTotal();
    }
        
    
    public void RemoveOrderItem(string positionName, int quantity = 0)
    {
        if (!SomeOrderChecks.CanBeModified(Status))
            throw new InvalidOperationException("Заказ нельзя изменить в текущем статусе");

        var existingItem = OrderItems.FirstOrDefault(item =>
            item.Position.Name.Equals(positionName, StringComparison.OrdinalIgnoreCase));
        
        if (existingItem == null)
            throw new ArgumentException($"'{positionName}' нет в заказе");
        
        if (existingItem.Quantity <= quantity || quantity <=0)
        {
            OrderItems.Remove(existingItem);
        }
        else
        {
            existingItem.Quantity -= quantity;
        }
        
        CalculateTotal();
    }
    
    public void ClearOrderItems()
    {
        if (!SomeOrderChecks.CanBeModified(Status))
            throw new InvalidOperationException("Заказ нельзя изменить в текущем статусе");
        
        OrderItems.Clear();
        CalculateTotal();
    }
    
    public TimeSpan GetOrderAge()
    {
        return DateTime.Now - CreatedAt;
    }
    public virtual string GetOrderSummary()
    {
        string itemsInfo;
        if (OrderItems.Count > 0)
        {
            List<string> itemDescriptions = new List<string>();
            foreach (var item in OrderItems)
            {
                string description = $"{item.Position.Name} x{item.Quantity}";
                itemDescriptions.Add(description);
            }
            itemsInfo = string.Join(", ", itemDescriptions);
        }
        else
        {
            itemsInfo = "нет позиций в заказе";
        }
        
        string summary = $"Заказ: {OrderId}, " + $"Клиент: {Customer.Name}, " + $"Статус: {Status}, " + $"Сумма: {TotalPrice}, " + $"Позиции: {itemsInfo}";
        return summary;
    }
}