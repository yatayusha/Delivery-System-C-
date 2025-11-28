using DeliverySystem.Services;
using DeliverySystem.Storage;
using DeliverySystem.Models;
using DeliverySystem.Models.AllForOrders;

namespace DeliverySystem;


public class Class1
{
    public static void Main()
    {
        var repository = PositionRepositorySingleton.Instance;
        var menuService = new MenuService(repository);
        var orderService = new OrderService(repository);
        
        var positions = new Position[]
        {
            new FoodPosition("Ризотто с грибами", 800, "Нежное ризотто", TimeSpan.FromMinutes(15), true, "Горячее"),
            new FoodPosition("Салат греческий", 450, "Свежий салат", TimeSpan.FromMinutes(10), true, "Салаты"),
            new BarPosition("Коктейль", 870, "классическая голубая лагуна", TimeSpan.FromMinutes(2), true, 0.5m)
        };
        
        foreach (var position in positions)
        {
            menuService.AddPosition(position);
        }
        
        // создали стандартный заказ
        var customer = new Customer("1", "Скоринцева Таисия", "taya@gmail.com", "1234567", "SPB");
        var standardOrder = orderService.CreateOrder("обычный заказ", "NEW001", customer);
        orderService.AddItem(standardOrder, "Ризотто с грибами", 1);
        orderService.AddItem(standardOrder, "Салат греческий", 2);
        Console.WriteLine(standardOrder.GetOrderSummary());
        
        // создали экспресс заказ
        var expressOrder = orderService.CreateOrder("экспресс заказ", "NEW002", customer);
        orderService.AddItem(expressOrder, "Ризотто с грибами", 1);
        orderService.AddItem(expressOrder, "Коктейль", 3);
        Console.WriteLine(expressOrder.GetOrderSummary());
        
        // смена статусов
        orderService.ChangeOrderStatus(standardOrder, OrderStatus.Confirmed);
        orderService.ChangeOrderStatus(standardOrder, OrderStatus.Preparing);
        orderService.ChangeOrderStatus(expressOrder, OrderStatus.Confirmed);
        orderService.ChangeOrderStatus(expressOrder, OrderStatus.Preparing);
        orderService.ChangeOrderStatus(expressOrder, OrderStatus.ReadyForDelivery);
        
        // подсчеты стоимости
        var total1 = orderService.CalculateTotal(standardOrder);
        var total2 = orderService.CalculateTotal(expressOrder);
    }
}
