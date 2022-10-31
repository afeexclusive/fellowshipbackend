using Fellowship.Data;
using Fellowship.DataAccess.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fellowship.DataAccess.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        readonly FellowContext context;

        DbSet<T> table;
        public Repository(FellowContext eloContext)
        {
            context = eloContext;
            table = context.Set<T>();

        }


        public async Task Create(T entity)
        {
            await table.AddAsync(entity);
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetByID(id);
            table.Remove(entity);
        }



        public async Task<IEnumerable<T>> GetAll()
        {
            return await table.AsNoTracking().ToListAsync();
        }


        public async Task<T> GetByID(Guid id)
        {

            return await table.FindAsync(id);
        }

        public void Update(T entity)
        {
            table.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        //Exposing this method to the services, to chain queries for the specific queries
        //public IEnumerable<T> GetAllQuery()
        //{
        //    return table.AsEnumerable<T>();
        //}

        public IQueryable<T> GetAllQuery()
        {
            return table.AsQueryable();
        }

        public async Task CreateMany(List<T> entity)
        {
            await table.AddRangeAsync(entity);
        }

        public void UpdateEntityRange(List<T> entities)
        {
            table.UpdateRange(entities);
        }

        //public IQueryable<T> GetRange(List<Guid> Ids, Expression<Func<T, bool>> predicate)
        //{

        //    List <T> range = new List<T>();
        //    foreach (var id in Ids)
        //    {
        //        var ent = GetAllQuery().FirstOrDefault(predicate);

        //        if (ent != null) range.Add(ent);
        //    }
        //    return range.AsQueryable();
        //}


        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await table.Where(predicate).ToListAsync();
        }


    }
}
