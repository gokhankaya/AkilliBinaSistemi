using DataAccess.Providers;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class MongoDbRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Fields

        private MongoDBProvider _provider;
        internal IMongoCollection<TEntity> _collection;

        #endregion Fields


        public MongoDbRepository(string connectionString = "", string database = "")
        {
            _provider = new MongoDBProvider(connectionString, database);
            _collection = _provider.GetCollection<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            _collection.InsertOneAsync(entity);
            return entity;
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> FindAll()
        {
            return (IQueryable<TEntity>)_collection.Find(new BsonDocument());
        }

        public TEntity FindById(object id)
        {
            if (!(id is ObjectId))
                return null;

            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            var foundOne = _collection.Find(filter).FirstOrDefault();
            return foundOne;
        }

        public TEntity FindSingleBy<T>(Expression<Func<TEntity, bool>> predicate) where T : class
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
