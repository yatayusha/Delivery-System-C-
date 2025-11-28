using DeliverySystem.Models;
using DeliverySystem.Models.AllForOrders;
using DeliverySystem.Services;
using DeliverySystem.Patterns.Observer;
using DeliverySystem.Patterns.Creators;
using DeliverySystem.Storage;
using DeliverySystem.Models.AllForOrders.Patterns.Observer;
using DeliverySystem.Models.AllForOrders.Patterns.Strategy;

namespace DeliverySystem.Tests;

public class UnitTest1
{
    [Fact]
    public void Order_Should_Be_Created_Correct_()
    {
        var customer = new Customer("1", "Taya", "tayaskor@mail.ru", "123456", "SPB");
        var order = new Order("NEW001", customer);
        Assert.Equal("NEW001", order.OrderId);
        Assert.Equal(OrderStatus.Pending, order.Status);
        Assert.Equal(0, order.TotalPrice);
        Assert.Empty(order.OrderItems);
    }
    
}

public class UnitTest2
{
    [Fact]
    public void Order_Add_Correct_Items()
    {
        var order = new Order("NEW001", new Customer("1", "Taya", "tayaskor@mail.ru", "123456", "SPB"));
        var position = new FoodPosition("Пицца", 500, "Свежая и хрустящая", TimeSpan.FromMinutes(15), true, "Кухня");
        var orderItem = new OrderItem(position, 2);
    
        order.AddOrderItem(orderItem);
    
        Assert.Single(order.OrderItems); 
        Assert.Equal(1000, order.TotalPrice);
    }
}

public class UnitTest3
{
    [Fact]
    public void For_StandardOrder_CalculateTotalWithDelivery()
    {
        var order = new StandardOrder("NEW001", new Customer("1", "Taya", "tayaskor@mail.ru", "123456", "SPB"));
        var position = new FoodPosition("Пицца", 500, "Свежая", TimeSpan.FromMinutes(15), true, "Кухня");
    
        order.AddOrderItem(new OrderItem(position, 1));
    
        Assert.True(order.TotalPrice > 500); 
    }
}

public class UnitTest4
{
    [Fact]
    public void For_ExpressOrder_FixedDeliveryTime()
    {
        var order = new ExpressOrder("NEW001", new Customer("1", "Taya", "tayaskor@mail.ru", "123456", "SPB"));
    
        Assert.True(order.FixDeliveryTime > DateTime.Now);
    }
}

public class UnitTest5
{
    [Fact]
    public void OrderFactory_Should_CreateStandardOrder()
    {
        var order = OrderFactory.CreateOrder("обычный заказ", "NEW001", new Customer("1", "Taya", "tayaskor@mail.ru", "123456", "SPB"));
    
        Assert.IsType<ObservableOrder>(order);
    }
}

public class UnitTest6
{
    [Fact]
    public void OrderFactory_Should_ThrowException_When_UnknownOrderType()
    {
        Assert.Throws<ArgumentException>(() => 
            OrderFactory.CreateOrder("неизвестный", "ORD001", new Customer("1", "Taya", "tayaskor@mail.ru", "123456", "SPB")));
    }
}

public class UnitTest7
{
    [Fact]
    public void StandardOrderCreator_Should_CreateStandardOrder()
    {
        var creator = new StandardOrderCreator();
        var order = creator.CreateOrder("NEW001", new Customer("1", "Taya", "tayaskor@mail.ru", "123456", "SPB"));

        Assert.IsType<StandardOrder>(order);
    }
}

public class UnitTest8
{
    [Fact]
    
    public void OrderService_Should_CreateOrderWithItems()
    {
        var repo = PositionRepositorySingleton.Instance;
        var service = new OrderService(repo);
        var items = new List<OrderItem> { };
    
        var order = service.CreateOrderWithItems("обычный заказ", "NEW001", 
            new Customer("1", "Taya", "tayaskor@mail.ru", "123456", "SPB"), items);
    
        Assert.Equal(items.Count, order.OrderItems.Count);
    }
}

public class UnitTest9
{
    [Fact]
    public void OrderService_Should_AddItemToOrder()
    {
        var repo = PositionRepositorySingleton.Instance;
        var service = new OrderService(repo);
        var order = service.CreateOrder("обычный заказ", "NEW001", new Customer("1", "Taya", "tayaskor@mail.ru", "123456", "SPB"));
        
        var position = new FoodPosition("Блюдо", 1000, "Тест", TimeSpan.Zero, true, "Тест");
        repo.AddPosition(position);
    
        service.AddItem(order, "Блюдо", 2);
    
        Assert.Single(order.OrderItems);
    }
}

public class UnitTest10
{
    [Fact]
    public void MenuService_Should_AddPositionToRepository()
    {
        var repo = PositionRepositorySingleton.Instance;
        var service = new MenuService(repo);
        var position = new FoodPosition("Блюдо", 1000, "Свежее", TimeSpan.Zero, true, "Кухня");
    
        service.AddPosition(position);
    
        var found = service.FindPosition("Блюдо");
        Assert.NotNull(found);
        Assert.Equal("Блюдо", found.Name);
    }
}

public class UnitTest11
{
    [Fact]
    public void ObservableOrder_Should_NotifyObservers_When_StatusChanges()
    {
        var notifier = new OrderNotifier();
        var customerObserver = new CustomerNotifier();
        notifier.AddObserver(customerObserver);
    
        var baseOrder = new Order("NEW001", new Customer("1", "Taya", "tayaskor@mail.ru", "123456", "SPB"));
        var observableOrder = new ObservableOrder(baseOrder, notifier);
    
        observableOrder.ChangeStatus(OrderStatus.Confirmed);
    
        Assert.Single(customerObserver.Notifications);
        Assert.Contains("NEW001", customerObserver.Notifications[0]);
    }
}

public class UnitTest12
{
    [Fact]
    public void KitchenNotifier_Should_OnlyNotify_When_PreparingStatus()
    {
        var notifier = new OrderNotifier();
        var kitchenObserver = new KitchenNotifier();
        notifier.AddObserver(kitchenObserver);
    
        var baseOrder = new Order("NEW001", new Customer("1", "Taya", "tayaskor@mail.ru", "123456", "SPB"));
        var observableOrder = new ObservableOrder(baseOrder, notifier);
        
        observableOrder.ChangeStatus(OrderStatus.Confirmed);
        Assert.Empty(kitchenObserver.KitchenOrders);
        
        observableOrder.ChangeStatus(OrderStatus.Preparing);
        Assert.Single(kitchenObserver.KitchenOrders);
    }
}

public class UnitTest13
{
    [Fact]
    public void DiscountService_Should_ApplyPercentageDiscount()
    {
        var service = new DiscountService();
        service.SetDiscountStrategy(new YesDiscStrategy(10));
    
        var order = new Order("NEW001", new Customer("1", "Taya", "tayaskor@mail.ru", "123456", "SPB"));
        order.AddOrderItem(new OrderItem(
            new FoodPosition("Блюдо", 1000, "Свежее", TimeSpan.Zero, true, "Кухня"), 1));
    
        var discountedPrice = service.ApplyDiscount(order);
        Assert.Equal(900, discountedPrice);
    }
}

public class UnitTest14
{
    [Fact]
    public void NoDiscountStrategy_Should_ReturnOriginalPrice()
    {
        var strategy = new NoDiscStrategy();
        var order = new Order("ORD001", new Customer("1", "Taya", "tayaskor@mail.ru", "123456", "SPB"));
        order.AddOrderItem(new OrderItem(
            new FoodPosition("Блюдо", 1000, "Свежее", TimeSpan.Zero, true, "Кухня"), 1));
    
        var result = strategy.ApplyDiscount(order);
        Assert.Equal(1000, result);
    }
}

public class UnitTest15
{
    [Fact]
    public void PositionRepository_Should_AddAndRetrievePosition()
    {
        var repo = PositionRepositorySingleton.Instance;
        var position = new FoodPosition("Уникальная позиция", 1000, "Свежая", TimeSpan.Zero, true, "Бар");
    
        repo.AddPosition(position);
        var found = repo.GetByName("Уникальная позиция");
    
        Assert.NotNull(found);
        Assert.Equal("Уникальная позиция", found.Name);
    }
}

public class UnitTest16
{
    [Fact]
    public void PositionRepository_Should_Throw_When_AddingDuplicate()
    {
        var repo = PositionRepositorySingleton.Instance;
        var position = new FoodPosition("Дубликат", 1000, "Холодный", TimeSpan.Zero, true, "Бар");
    
        repo.AddPosition(position);
    
        Assert.Throws<InvalidOperationException>(() => repo.AddPosition(position));
    }
}