using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using FakeDataProvider;

namespace FakeDataProvider
{
    public class UnitOfWork : IUnitOfWork
    {
        Provider _privider;
        private static IUnitOfWork _uow;

        public Utilities.RepoTypes RepositoryType
        {
            get
            {
                return Utilities.RepoTypes.Basic;
            }
        }

        public Utilities.ServerTypes ServerType
        {
            get
            {
                return Utilities.ServerTypes.MsSqlServer;
            }
        }

        private UnitOfWork()
        {
            _privider = new Provider();
        }

        public static IUnitOfWork CreateContext()
        {
            if (_uow == null)
            {
                _uow = new UnitOfWork();
            }

            return _uow;
        }

        public static IUnitOfWork GetContext()
        {
            return _uow;
        }

        public void Dispose()
        {

        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return new Repository<TEntity>(_privider);
        }

        public int SaveChanges()
        {
            return 1;
        }

        public ITransaction TransactionBegin()
        {
            throw new NotImplementedException();
        }

        public void TransactionEnd(ITransaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
