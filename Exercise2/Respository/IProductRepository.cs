using EmployeeManagerment.Respository;
using eShopProductService.Entity;
using Exercise2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopProductService.Respository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public Task<List<Product>> GetListProduct(int pageIndex, int pageSize);
        public Task<APIResult<string>> ChangeAmount(int Id, int amount);
    }
}
