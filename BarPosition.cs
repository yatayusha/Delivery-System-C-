namespace DeliverySystem.Models;

public class BarPosition : Position
{
    public decimal Size { get; private set; }

    public BarPosition(string name, decimal price, string description,
        TimeSpan cookingTime, bool isInStock, decimal size) : base(name, price, description, cookingTime, isInStock)
    {
        Size = size;
    }

    public override string GetPositionType() => "Позиция из барного меню";
}