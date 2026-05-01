using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class EFBaicRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        internal DbSet<TEntity> _dbSet;

        public EFBaicRepository(IDataContext context)
        {
            _context = (DbContext)context;
            string TentityName = typeof(TEntity).Name;
            var foundSetInfo = _context.GetType().GetProperties().Where(x => x.Name.Contains(TentityName)).FirstOrDefault();

            if (foundSetInfo != null)
            {
                _dbSet = (DbSet<TEntity>)foundSetInfo.GetValue(_context);
            }

        }

        public TEntity Add(TEntity entity)
        {
            return _dbSet.Add(entity);
        }

        public void Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public IQueryable<TEntity> FindAll()
        {
            return _dbSet;
        }

        public TEntity FindById(object id)
        {
            return _dbSet.Find(id);
        }

        public TEntity FindSingleBy<T>(Expression<Func<TEntity, bool>> predicate) where T : class
        {
            return _dbSet.Where(predicate).SingleOrDefault();
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
