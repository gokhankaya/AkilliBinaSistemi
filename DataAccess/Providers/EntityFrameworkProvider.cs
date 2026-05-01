using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Providers
{
    public class EntityFrameworkProvider : DbContext, IDataContext
    {
        public EntityFrameworkProvider(string contectionString) : base(contectionString)
        {
            
        }


        public int SaveAllChanges()
        {
            //TODO:Log
            return base.SaveChanges();
        }

        public DbSet<T> CreateDbSet<T>() where T : class
        {
            return Set<T>();
        }
    }
}
