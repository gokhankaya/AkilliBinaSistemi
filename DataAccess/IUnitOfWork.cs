using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();

        Utilities.RepoTypes RepositoryType { get; }

        IRepository<TEntity> Repository<TEntity>() where TEntity : class;

        ITransaction TransactionBegin();

        void TransactionEnd(ITransaction transaction);
    }
}
