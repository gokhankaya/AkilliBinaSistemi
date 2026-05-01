using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity FindById(object id);

        IQueryable<TEntity> FindAll();

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        TEntity FindSingleBy<T>(Expression<Func<TEntity, bool>> predicate) where T : class;

        TEntity Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void Delete(object id);
    }
}
