using System;
using System.Transactions;

namespace DataAccess.Repository
{
    public class EFTransaction : ITransaction
    {
        protected UnitOfWorkFactory UnitOfWork { get; private set; }
        protected TransactionScope TransactionScope { get; private set; }

        public EFTransaction(UnitOfWorkFactory unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
            this.TransactionScope = new TransactionScope();
        }

        public void Commit()
        {
            this.UnitOfWork.TransactionFlush();
            this.TransactionScope.Complete();
        }

        public void Dispose()
        {
            if (this.TransactionScope != null)
            {
                (this.TransactionScope as IDisposable).Dispose();
                this.TransactionScope = null;
                this.UnitOfWork = null;
            }
        }

        public void Flush()
        {
            this.UnitOfWork.TransactionFlush();
        }

        public void Rollback()
        {
        }
    }
}
