using cache_example.domain;
using Microsoft.EntityFrameworkCore;

namespace cache_example.repository
{
    public class ProductsRepository : DbContext
    {
        public ProductsRepository(DbContextOptions<ProductsRepository> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

    }
}
