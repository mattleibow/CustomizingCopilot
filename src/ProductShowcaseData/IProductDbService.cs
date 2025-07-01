namespace ProductShowcaseData;

public interface IProductDbService
{
    IEnumerable<Category> GetCategories();
    IEnumerable<Product> GetProducts();
}
