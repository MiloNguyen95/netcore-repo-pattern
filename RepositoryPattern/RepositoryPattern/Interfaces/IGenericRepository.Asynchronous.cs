using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RepositoryPattern.Interfaces
{
    public partial interface IGenericRepository<T>
    {
        Task<IList<T>> GetAllAsync();
        Task<IList<T>> GetByConditionAsync(Expression<Func<T, bool>> expression);
        Task<T> GetByIdAsync(object id);
        Task<T> FindAsync(Expression<Func<T, bool>> expression);
        Task<int> CountAsync();
        Task<object> InsertAsync(T entity, bool saveChanges = false);
        Task DeleteAsync(object id, bool saveChanges = false);
        Task DeleteAsync(T entity, bool saveChanges = false);
        Task UpdateAsync(T entity, bool saveChanges = false);
        Task<T> UpdateAsync(T entity, object key, bool saveChanges = false);
        Task CommitAsync();
    }
}