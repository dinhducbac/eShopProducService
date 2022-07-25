using eShopProductService.Entity;
using eShopProductService.Models;
using eShopProductService.Respository;
using Exercise2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopProductService.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<APIResult<string>> ChangeAmount(int Id, int amount)
        {
            var result = await _productRepository.ChangeAmount(Id, amount);
            return result;
        }

        public async Task<APIResult<Product>> Create(ProductCreateRequest productCreateRequest)
        {
            var product = new Product() {
                Name = productCreateRequest.Name,
                Amount = productCreateRequest.Amount
            };
            await _productRepository.CreateAsync(product);
            return new APIResult<Product>() { Success = true, Message = "Successful", ResultObject = product};
        }

        public async Task<APIResult<List<Product>>> GetListProduct(int pageIndex, int pageSize)
        {
            var result = await _productRepository.GetListProduct(pageIndex, pageSize);
            return new APIResult<List<Product>>() { Success = true, Message = "Successful", ResultObject = result };
        }
    }
}
