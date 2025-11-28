using DeliverySystem.Services;

namespace DeliverySystem.Models.AllForOrders;

public class StandardOrder : Order
{
    private readonly DeliveryCalculator _deliveryCalculator = new DeliveryCalculator();
    private readonly Customer _customer;
    
    public StandardOrder(string orderId, Customer customer) 
        : base(orderId, customer)
    {
        _customer = customer;
    }
    
    public override decimal CalculateTotal()
    {
        var deliveryInfo = _deliveryCalculator.CalculateDeliveryInfo(this, isExpress: false);
        TotalPrice = deliveryInfo.TotalPriceWithDelivery;
        return TotalPrice;
    }

    public override string GetOrderSummary()
    {
        var deliveryInfo = _deliveryCalculator.CalculateDeliveryInfo(this, isExpress: false);
        
        string itemsInfo;
        if (OrderItems.Count == 0)
        {
            itemsInfo = "нет позиций в заказе";
        }
        else
        {
            List<string> itemDescriptions = new List<string>();
            foreach (var item in OrderItems)
            {
                string description = $"{item.Position.Name} x{item.Quantity}";
                itemDescriptions.Add(description);
            }
            itemsInfo = string.Join(", ", itemDescriptions);
        }
        
        string result = "Стандартный заказ: " + OrderId + ", " +
                        "Клиент: " + _customer.Name + ", " +
                        "Статус: " + Status + ", " +
                        "Сумма: " + TotalPrice + " (стоимость доставки" + deliveryInfo.DeliveryPrice + "уже включена в стоимость), " +
                        "Время доставки: " + deliveryInfo.DeliveryTime.TotalMinutes + " мин, " +
                        "Позиции: " + itemsInfo;

        return result;
    }
}