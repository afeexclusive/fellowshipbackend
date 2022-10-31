using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fellowship.DataAccess.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        Task Create(T entity);

        void Update(T entity);

        Task Delete(Guid id);

        Task<IEnumerable<T>> GetAll();

        Task<T> GetByID(Guid id);

        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);

        //IEnumerable<T> GetAllQuery();

        Task CreateMany(List<T> entity);
        IQueryable<T> GetAllQuery();
        void UpdateEntityRange(List<T> entities);

        //Task<IQueryable<T>> GetRange(List<Guid> Ids);
        //IQueryable<T> GetRange(List<Guid> Ids, Expression<Func<T, bool>> predicate);
    }
}

