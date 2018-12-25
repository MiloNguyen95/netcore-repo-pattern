using Microsoft.EntityFrameworkCore;
using RepositoryPattern.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RepositoryPattern.Implementations
{
    public partial class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;
        private DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        protected DbSet<T> DbSet => _dbSet ?? (_dbSet = _context.Set<T>());
        
        public IList<T> GetAll()
        {
            return DbSet.ToList();
        }

        public IList<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return DbSet.Where(expression).ToList();
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = DbSet;
            foreach (var includeProperty in includeProperties)
            {

                queryable = queryable.Include(includeProperty);
            }
            return queryable;
        }

        public T GetById(object id)
        {
            return DbSet.Find(id);
        }

        public T Find(Expression<Func<T, bool>> expression)
        {
            return DbSet.SingleOrDefault(expression);
        }

        public IQueryable<T> GetIQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IList<T> GetAllPaged(int pageIndex, int pageSize, out int totalCount)
        {
            totalCount = DbSet.Count();
            return DbSet.Skip(pageSize * pageIndex).Take(pageSize).ToList();
        }

        public int Count()
        {
            return DbSet.Count();
        }

        public object Insert(T entity, bool saveChanges = false)
        {
            var result = DbSet.Add(entity);
            if (saveChanges)
            {
                _context.SaveChanges();
            }
            return result;
        }

        public void Delete(object id, bool saveChanges = false)
        {
            var item = GetById(id);
            DbSet.Remove(item);
            if (saveChanges)
            {
                _context.SaveChanges();
            }
        }

        public void Delete(T entity, bool saveChanges = false)
        {
            DbSet.Attach(entity);
            DbSet.Remove(entity);
            if (saveChanges)
            {
                _context.SaveChanges();
            }
        }

        public void Update(T entity, bool saveChanges = false)
        {
            var entry = _context.Entry(entity);
            DbSet.Attach(entity);
            entry.State = EntityState.Modified;
            if (saveChanges)
            {
                _context.SaveChanges();
            }
        }

        public T Update(T entity, object key, bool saveChanges = false)
        {
            if (entity == null)
                return null;
            var exist = DbSet.Find(key);
            if (exist == null) return null;
            _context.Entry(exist).CurrentValues.SetValues(entity);
            if (saveChanges)
            {
                _context.SaveChanges();
            }
            return exist;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
