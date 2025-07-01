using System.Text.Json;

namespace ProductShowcaseData;

public class ProductDbService : IProductDbService
{
    private static readonly JsonSerializerOptions CamelCaseOptions =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

    private readonly string _dbFilePath;
    private ProductDbJson? _db;

    public ProductDbService(string dbFilePath)
    {
        _dbFilePath = dbFilePath;
        Load();
    }

    public void Load()
    {
        var json = File.ReadAllText(_dbFilePath);
        var db = JsonSerializer.Deserialize<ProductDbJson>(json, CamelCaseOptions);
        _db = db ?? new ProductDbJson();
    }

    public void Save()
    {
        var json = JsonSerializer.Serialize(_db, CamelCaseOptions);
        File.WriteAllText(_dbFilePath, json);
    }

    public IEnumerable<Product> GetProducts() => GetDb().Products.Items;

    public IEnumerable<Category> GetCategories() => GetDb().Categories.Items;

    private ProductDbJson GetDb()
    {
        if (_db == null)
            Load();

        return _db!;
    }

    private class ProductDbJson
    {
        public string? Version { get; set; } = "1.0";
        public ProductList Products { get; set; } = new();
        public CategoryList Categories { get; set; } = new();
    }

    private class ProductList
    {
        public int Count { get; set; }
        public List<Product> Items { get; set; } = new();
    }

    private class CategoryList
    {
        public int Count { get; set; }
        public List<Category> Items { get; set; } = new();
    }
}
