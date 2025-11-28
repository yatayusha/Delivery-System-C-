using DeliverySystem.Models;

namespace DeliverySystem.Interfaces;

public interface IPositionRepository
{
    Position? GetByName(string name);
    List<Position> GetAll();
    void UpdateStock(string name, bool inStock);
    void AddPosition(Position position); 
    void SaveToJson();  
    void LoadFromJson(); 
}