namespace DeliverySystem.Services;

public class DeliveryInfo
{
    public decimal OrderTotal { get; set; }
    public decimal DeliveryPrice { get; set; }
    public decimal TotalPriceWithDelivery { get; set; }
    public TimeSpan DeliveryTime { get; set; }
    public bool IsExpress { get; set; }
}