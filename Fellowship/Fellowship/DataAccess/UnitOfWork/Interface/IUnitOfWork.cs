using Fellowship.DataAccess.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fellowship.DataAccess.UnitOfWork.Interface
{
    public interface IUnitOfWork<T> where T : class
    {
        IRepository<T> Repository { get; }

        Task<int> Save();
    }
}

