using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(T id, params Expression<Func<T, object>>[] includes);

        Task<T> GetByIdAsync(T id);
        Task AddAsync(T Entity);
        Task UpdateAsync(T id, T Entity);
        Task DeleteAsync(T id);
    }
}
