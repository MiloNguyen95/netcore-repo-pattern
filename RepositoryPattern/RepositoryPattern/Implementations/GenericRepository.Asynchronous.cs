using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RepositoryPattern.Implementations
{
    public partial class GenericRepository<T>
    {
        public virtual async Task<IList<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task<IList<T>> GetByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await DbSet.Where(expression).ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await DbSet.SingleOrDefaultAsync(expression);
        }

        public async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }

        public virtual async Task<object> InsertAsync(T entity, bool saveChanges = false)
        {
            var result = await DbSet.AddAsync(entity);
            if (saveChanges)
            {
                await _context.SaveChangesAsync();
            }
            return result;
        }

        public virtual async Task DeleteAsync(object id, bool saveChanges = false)
        {
            DbSet.Remove(GetById(id));
            if (saveChanges)
            {
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task DeleteAsync(T entity, bool saveChanges = false)
        {
            DbSet.Attach(entity);
            DbSet.Remove(entity);
            if (saveChanges)
            {
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task UpdateAsync(T entity, bool saveChanges = false)
        {
            var entry = _context.Entry(entity);
            DbSet.Attach(entity);
            entry.State = EntityState.Modified;
            if (saveChanges)
            {
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<T> UpdateAsync(T entity, object key, bool saveChanges = false)
        {
            if (entity == null)
                return null;
            var exist = await DbSet.FindAsync(key);
            if (exist == null) return null;
            _context.Entry(exist).CurrentValues.SetValues(entity);
            if (saveChanges)
            {
                await _context.SaveChangesAsync();
            }
            return exist;
        }

        public virtual async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}