using eShopProductService.Entity;
using eShopProductService.Models;
using Exercise2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopProductService.Services
{
    public interface IProductService
    {
        public Task<APIResult<List<Product>>> GetListProduct(int pageIndex, int pageSize);
        public Task<APIResult<Product>> Create(ProductCreateRequest productCreateRequest);
        public Task<APIResult<string>> ChangeAmount(int Id, int amount);
    }
}
