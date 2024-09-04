using ShopProjectMVC.Core.Models;

namespace ShopProjectMVC.Core.Interfaces;

public interface IProductService
{
    Task<Product> AddProduct(Product product);
    Task<Product> UpdateProduct(Product product);
    Task DeleteProduct(int id);
    Task<Product> GetProductById(int id);
    IEnumerable<Product> GetAll();
    Task<Order> BuyProduct(int userId, int productId);
    IEnumerable<Category> GetAllCategories();
}
