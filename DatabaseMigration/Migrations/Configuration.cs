namespace DatabaseMigration.Migrations
{
    using DomainObjects;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DatabaseMigration.DB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DatabaseMigration.DB context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            if (context.AreaTypes.ToList().Count == 0)
                context.AreaTypes.AddOrUpdate<AreaType>(new AreaType()
                {
                    Name = "HouseArea",
                    Definition = "That is a room of the house",
                    Areas = null,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "USER",
                    DeletedBy = null,
                    DeletedDate = null,
                    ModifiedBy = null,
                    ModifiedDate = null
                });
        }
    }
}
