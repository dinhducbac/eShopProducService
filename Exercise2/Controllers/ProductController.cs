using eShopProductService.Models;
using eShopProductService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShopProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("products")]
        public async Task<IActionResult> GetListProduct(int pageIndex, int pageSize)
        {
            var result = await _productService.GetListProduct(pageIndex, pageSize);
            return Ok(result);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ProductCreateRequest productCreateRequest)
        {
            var result = await _productService.Create(productCreateRequest);
            return Ok(result);
        }
    }
}
