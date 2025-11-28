using System.ComponentModel.Design;
using DeliverySystem.Interfaces;
using DeliverySystem.Models;

namespace DeliverySystem.Services;

public class MenuService
{
    private readonly IPositionRepository _positionRepository;

    public MenuService(IPositionRepository positionRepository)
    {
        _positionRepository = positionRepository;
    }

    public void AddPosition(Position position)
    {
        _positionRepository.AddPosition(position);
    }

    public void ShowMenu()
    {
        var positions = _positionRepository.GetAll();
        var barPositions = positions.OfType<BarPosition>();
        var foodPositions = positions.OfType<FoodPosition>();


    }

    public Position? FindPosition(string name)
    {
        return _positionRepository.GetByName(name);
    }

    public void UpdateStock(string positionName, bool inStock)
    {
        _positionRepository.UpdateStock(positionName, inStock);

    }
    
    public List<Position> GetPositionsByType<T>() where T : Position
    {
        return _positionRepository.GetAll().OfType<T>().Cast<Position>().ToList();
    }
    
    public bool IsPositionAvailable(string positionName)
    {
        var position = _positionRepository.GetByName(positionName);
        return position?.IsInStock ?? false;
    }
}