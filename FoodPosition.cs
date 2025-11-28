namespace DeliverySystem.Models;

public class FoodPosition : Position
{
    private string MenuCategory { get; set; }

    public FoodPosition(string name, decimal price, string description,
        TimeSpan cookingTime, bool isInStock, string menuCategory) : base(name, price, description, cookingTime, isInStock)
    {
        MenuCategory = menuCategory ?? throw new ArgumentNullException(nameof(menuCategory));
    }

    public string GetCategory()
    {
        return MenuCategory;
    }
    public override string GetPositionType() => $"Позиция из меню кухни (категория {MenuCategory})";
}