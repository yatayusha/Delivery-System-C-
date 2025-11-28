using DeliverySystem.Models.AllForOrders;

namespace DeliverySystem.Services;

public class DeliveryCalculator
{
    private const decimal StandartDeliveryPrice = 100m;
    private const decimal ExpressDeliveryPrice = 180m;
    private readonly TimeSpan _standartDeliveryTime = TimeSpan.FromMinutes(50);
    private readonly TimeSpan _expressDeliveryTime = TimeSpan.FromMinutes(25);

    public DeliveryInfo CalculateDeliveryInfo(Order order, bool isExpress = false)
    {
        var orderTotal = order.CalculateBaseTotal();
        var deliveryPrice = isExpress ? ExpressDeliveryPrice : StandartDeliveryPrice;
        var deliveryTime = isExpress ? _expressDeliveryTime : _standartDeliveryTime;
    
        return new DeliveryInfo
        {
            OrderTotal = orderTotal, 
            DeliveryPrice = deliveryPrice,
            TotalPriceWithDelivery = OrderCalculations.AddDeliveryPrice(orderTotal, deliveryPrice),
            DeliveryTime = deliveryTime,
            IsExpress = isExpress 
        };
    }

}