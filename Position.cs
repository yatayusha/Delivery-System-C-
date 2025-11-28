namespace DeliverySystem.Models;
public abstract class Position
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public TimeSpan CookingTime { get; set; }
    public bool IsInStock { get; set; }
    
    protected Position(string name, decimal price, string description, TimeSpan cookingTime, bool isInStock)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        
        if (price < 0)
            throw new ArgumentException("Установите корректную стоимость");
        
        Price = price;
        Description = description ?? throw new ArgumentNullException(nameof(description));
        CookingTime = cookingTime;
        IsInStock = isInStock;
    }
    
    public abstract string GetPositionType();
}