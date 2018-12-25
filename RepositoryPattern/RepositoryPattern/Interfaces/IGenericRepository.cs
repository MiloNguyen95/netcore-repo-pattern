using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RepositoryPattern.Interfaces
{
    public partial interface IGenericRepository<T> where T : class
    {
        IList<T> GetAll();
        IList<T> GetByCondition(Expression<Func<T, bool>> expression);
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        T GetById(object id);
        T Find(Expression<Func<T, bool>> expression);
        IQueryable<T> GetIQueryable();
        IList<T> GetAllPaged(int pageIndex, int pageSize, out int totalCount);
        int Count();
        object Insert(T entity, bool saveChanges = false);
        void Delete(object id, bool saveChanges = false);
        void Delete(T entity, bool saveChanges = false);
        void Update(T entity, bool saveChanges = false);
        T Update(T entity, object key, bool saveChanges = false);
        void Commit();

        void Dispose();
    }
}
