using cache_example.domain;
using cache_example.webapi.Services;
using Microsoft.AspNetCore.Mvc;

namespace cache_example.webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {
            var products = await _productService.GetProductsAsync();

            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] Product product)
        {
            var addedProduct = await _productService.AddProductAsync(product);

            return Ok(addedProduct);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductAsync(id);

            return Ok(product);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);

            return NoContent();
        }
    }
}
