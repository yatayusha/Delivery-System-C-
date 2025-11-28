namespace DeliverySystem.Models.AllForOrders;


public enum OrderStatus
{
    Pending,
    Confirmed,
    Preparing,
    ReadyForDelivery,
    Shipped,
    Delivered,
    Cancelled,
}