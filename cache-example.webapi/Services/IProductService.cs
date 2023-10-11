using cache_example.domain;

namespace cache_example.webapi.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product?> GetProductAsync(int id);
        Task<Product> AddProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
