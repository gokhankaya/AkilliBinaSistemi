using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Providers
{
    public class MongoDBProvider: IDataContext
    {
        private IMongoClient _client;
        private IMongoDatabase _database;

        public MongoDBProvider(string connectionString = "", string database = "")
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(database);
        }

        public void Dispose()
        {
            
        }

        public IMongoCollection<T> GetCollection<T>() where T : class
        {
            return _database.GetCollection<T>(typeof(T).Name);
        }

        public int SaveAllChanges()
        {
            return 1;
        }
    }
}
