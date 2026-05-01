using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using DataAccess;

namespace FakeDataProvider
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private Provider _provider;

        public Repository(Provider provider)
        {
            _provider = provider;
        }

        public TEntity Add(TEntity entity)
        {
            //((IEntityBase)entity).Id = Guid.NewGuid();
            ((IEntityBase)entity).CreatedBy = "";
            ((IEntityBase)entity).CreatedDate = DateTime.Now;
            _provider.Set<TEntity>().Add(entity);
            return entity;
        }

        public void Delete(object id)
        {
            var obje = (IEntityBase)FindById(id);
            obje.DeletedBy = "";
            obje.DeletedDate = DateTime.Now;
        }

        public void Delete(TEntity entity)
        {
            this.Delete(((IEntityBase)entity).ID);
        }

        public void Dispose()
        {

        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _provider.Set<TEntity>().Where(predicate.Compile()).Where(x => ((IEntityBase)x).DeletedDate == null).AsQueryable();
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> FindAll()
        {
            return _provider.Set<TEntity>().Where(x => ((IEntityBase)x).DeletedDate == null).AsQueryable();
        }

        public TEntity FindById(object id)
        {
            return _provider.Set<TEntity>().Find(x => ((IEntityBase)x).ID == (int)id && ((IEntityBase)x).DeletedDate == null);
        }

        public TEntity FindSingleBy<T>(Expression<Func<TEntity, bool>> predicate) where T : class
        {
            return _provider.Set<TEntity>().Where(predicate.Compile()).Where(x => ((IEntityBase)x).DeletedDate == null).FirstOrDefault();
        }

        public void Update(TEntity entity)
        {
            var old = FindById(((IEntityBase)entity).ID);

            ((IEntityBase)entity).ModifiedBy = "";
            ((IEntityBase)entity).ModifiedDate = DateTime.Now;

            if (old == null)
                return;

            var infos = typeof(TEntity).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            foreach (var info in infos)
            {
                info.SetValue(old, info.GetValue(entity));
            }
        }
    }
}
