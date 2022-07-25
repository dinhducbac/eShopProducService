using Exercise2.EF;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagerment.Respository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private DbContext _dbContext;
        public GenericRepository(DbContext context)
        {
            _dbContext = context;
        }

        public  async Task<T> CreateAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await this.SavechangeAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await this.SavechangeAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task SavechangeAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
