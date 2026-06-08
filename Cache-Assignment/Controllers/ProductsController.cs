using Microsoft.AspNetCore.Mvc;
using Cache_Assignment.Models;
using Cache_Assignment.Services;

namespace Cache_Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class ProductsController(ProductsService productsService) : ControllerBase
    {
        private readonly ProductsService _productsService = productsService;

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(long id)
        {
            try
            {
                return await _productsService.GetProduct(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post(Product product)
        {
            await _productsService.AddProduct(product);
            return CreatedAtAction(nameof(Get), new { product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, Product product)
        {
            if (id != product.Id)
                return BadRequest();
            try
            {
                await _productsService.UpdateProduct(product);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
