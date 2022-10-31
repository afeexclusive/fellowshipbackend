using Fellowship.Data;
using Fellowship.DataAccess.Repository.Implementation;
using Fellowship.DataAccess.Repository.Interface;
using Fellowship.DataAccess.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fellowship.DataAccess.UnitOfWork.Implementation
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {
        private readonly FellowContext context;
        private IRepository<T> _repository;

        public UnitOfWork(FellowContext context)
        {
            this.context = context;
        }

        public IRepository<T> Repository
        {
            get
            {
                return _repository = _repository ?? new Repository<T>(context);
            }
        }

        public async Task<int> Save()
        {
            return await context.SaveChangesAsync();
        }
    }
}
