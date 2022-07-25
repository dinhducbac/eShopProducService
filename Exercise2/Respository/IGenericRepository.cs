using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagerment.Respository
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<T> GetByIdAsync(int id);
        public Task<T> CreateAsync(T entity);

        public Task SavechangeAsync();

        public Task DeleteAsync(T entity);
    }
}
