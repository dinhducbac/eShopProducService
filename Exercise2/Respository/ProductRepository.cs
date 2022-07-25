using EmployeeManagerment.Respository;
using eShopProductService.Entity;
using Exercise2.EF;
using Exercise2.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopProductService.Respository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly eShopDBContext _dbContext;
        public ProductRepository(eShopDBContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<List<Product>> GetListProduct(int pageIndex, int pageSize)
        {
            return  await _dbContext.Products.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public async Task<APIResult<string>> ChangeAmount(int Id, int amount)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == Id);
            if (product == null)
                return new APIResult<string>() { Success = false };
            product.Amount += amount;
            await _dbContext.SaveChangesAsync();
            return new APIResult<string>() { Success = true };
        }
    }
}
