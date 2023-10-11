using cache_example.domain;
using cache_example.repository;
using cache_example.webapi.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace cache_example.webapi.Services
{
    public class ProductService: IProductService
    {
        private readonly ProductsRepository _productsRepository;
        private readonly ICacheHandler _cacheHandler;
        private readonly IDistributedCache distributedCache;

        public ProductService(ProductsRepository productsRepository, ICacheHandler cacheHandler, IDistributedCache distributedCache)
        {
            _productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(productsRepository));
            _cacheHandler = cacheHandler ?? throw new ArgumentNullException("Cache handler instance is null");
            this.distributedCache = distributedCache;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            await _productsRepository.AddAsync(product);
            await _productsRepository.SaveChangesAsync();

            // update cache
            await _cacheHandler.StringSetAsync(product.Id.ToString(), JsonSerializer.Serialize(product));

            await distributedCache.SetStringAsync(product.Id.ToString(), JsonSerializer.Serialize(product));

            return product;
        }

        public async Task DeleteProductAsync(int id)
        {
            if (id == default)
                throw new ArgumentException("Id cannot be default value", nameof(id));

            var product = await _productsRepository.Products
                .FirstOrDefaultAsync(p => p.Id == id)
                .ConfigureAwait(false);
            if (product != null)
            {
                _productsRepository.Remove(product);
                _productsRepository.SaveChanges();
            }

            // After deleting from database, delete from cache
            await _cacheHandler.StringDeleteAsync(id.ToString());
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            if (id == default || id < 1)
                throw new ArgumentException("Id cannot be default value", nameof(id));  

            // Before getting from database, check if it is in cache
            var productFromCache = await _cacheHandler.StringGetAsync(id.ToString());
            if (productFromCache != null)
            {
                return JsonSerializer.Deserialize<Product>(productFromCache);
            }

            // else, get from database and update cache
            var product = await _productsRepository.Products
                .FirstOrDefaultAsync(p => p.Id == id)
                .ConfigureAwait(false);

            if (product != null)
            {
                await _cacheHandler.StringSetAsync(product.Id.ToString(), JsonSerializer.Serialize(product));
            }

            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _productsRepository.Products.ToListAsync().ConfigureAwait(false);
        }       
    }
}
