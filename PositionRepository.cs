using System.Text.Json;
using System.Text.Json.Serialization;
using DeliverySystem.Interfaces;
using DeliverySystem.Models;

namespace DeliverySystem.Storage; 

public sealed class PositionRepositorySingleton : IPositionRepository
{
    private static PositionRepositorySingleton? _instance;
    private static readonly object Lock = new object();
    
    private readonly List<Position> _positions;
    private readonly string _jsonFilePath;
    
    private PositionRepositorySingleton()
    { 
        _positions = new List<Position>();
        _jsonFilePath = "menu.json"; 
        
        LoadFromJson();
    }
    
    public static PositionRepositorySingleton Instance
    {
        get
        {
            lock (Lock)
            {
                return _instance ??= new PositionRepositorySingleton();
            }
        }
    }
    
    public void AddPosition(Position position)
    {
        if (position == null)
            throw new ArgumentNullException(nameof(position));
        
        if (_positions.Any(p => p.Name == position.Name))
        {
            throw new InvalidOperationException($"Позиция с названием '{position.Name}' уже есть в меню");
        }
        
        _positions.Add(position);
        SaveToJson(); 
    }
    
    public void SaveToJson()
    {
        try
        {
            var options = new JsonSerializerOptions 
            { 
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        
            string json = JsonSerializer.Serialize(_positions, options);
            File.WriteAllText(_jsonFilePath, json);
            Console.WriteLine($"Меню сохранено в файл: {_jsonFilePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сохранения: {ex.Message}");
        }
    }
    
    public void LoadFromJson()
    {
        try
        {
            if (File.Exists(_jsonFilePath))
            {
                string json = File.ReadAllText(_jsonFilePath);
                
                var loadedPositions = JsonSerializer.Deserialize<List<Position>>(json);
                
                _positions.Clear();
                if (loadedPositions != null) _positions.AddRange(loadedPositions);

                Console.WriteLine($"Меню загружено из файла: {_jsonFilePath}");
            }
            else
            {
                Console.WriteLine("Файл с меню не найден. Будет создан новый.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка : {ex.Message}");
        }
    }
    
    public Position? GetByName(string name)
    {
        return _positions.FirstOrDefault(p => p.Name == name);
    }
    
    public List<Position> GetAll()
    {
        return new List<Position>(_positions);
    }
    
    public void UpdateStock(string name, bool inStock)
    {
        var position = GetByName(name);
        
        if (position != null)
        {
            position.IsInStock = inStock;
            SaveToJson(); 
        }
    }
}