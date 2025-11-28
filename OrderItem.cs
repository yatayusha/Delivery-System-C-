namespace DeliverySystem.Models.AllForOrders;


public class OrderItem
{
    public Position Position { get; private set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => Position.Price * Quantity;

    public OrderItem(Position position, int quantity)
    {
        Position = position ?? throw new ArgumentNullException(nameof(position));
        Quantity = quantity;
    }
}