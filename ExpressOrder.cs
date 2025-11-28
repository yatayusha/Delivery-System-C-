using DeliverySystem.Services;

namespace DeliverySystem.Models.AllForOrders;

public class ExpressOrder : Order
{
    private readonly DeliveryCalculator _deliveryCalculator = new DeliveryCalculator();
    private readonly Customer _customer;
    public DateTime FixDeliveryTime { get; set; }

    public ExpressOrder(string orderId, Customer customer)
        : base(orderId, customer)
    {
        _customer = customer; 
        var deliveryInfo = _deliveryCalculator.CalculateDeliveryInfo(this, isExpress: true);
        FixDeliveryTime = CreatedAt.Add(deliveryInfo.DeliveryTime);
    }

    public override decimal CalculateTotal()
    {
        var deliveryInfo = _deliveryCalculator.CalculateDeliveryInfo(this, isExpress: true);
        TotalPrice = deliveryInfo.TotalPriceWithDelivery;
        return TotalPrice; 
    }

    public override string GetOrderSummary()
    {
        var deliveryInfo = _deliveryCalculator.CalculateDeliveryInfo(this, isExpress: true);
        
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
                string description = item.Position.Name + item.Quantity;
                itemDescriptions.Add(description);
            }
            itemsInfo = string.Join(", ", itemDescriptions);
        }
        
        string result = "Экспресс заказ: " + OrderId + ", " +
                        "Клиент: " + _customer.Name + ", " +
                        "Статус: " + Status + ", " +
                        "Сумма: " + TotalPrice + " (включая доставку " + deliveryInfo.DeliveryPrice + "), " +
                        "Время доставки: " + deliveryInfo.DeliveryTime.TotalMinutes + " мин, " +
                        "Фиксированное время доставки: " + FixDeliveryTime.ToString("dd.MM.yyyy HH:mm") + ", " +
                        "Позиции: " + itemsInfo;

        return result;
    }
}