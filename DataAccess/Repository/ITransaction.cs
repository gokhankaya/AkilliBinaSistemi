using System;

namespace DataAccess
{
    public interface ITransaction : IDisposable
    {
        void Commit();
        void Flush();
        void Rollback();
    }
}