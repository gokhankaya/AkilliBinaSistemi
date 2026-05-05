using DataAccess;
using Npgsql;
using SimulationObjects;
using System.Data.Entity;

namespace SimulationDB_Migrations
{
    [DbConfigurationType(typeof(DatabaseMigration.NpgsqlDbConfiguration))]
    public class DB : DbContext, IDataContext
    {
        private const string ConnStr = "Host=localhost;Port=5432;Database=adle_sim;Username=adle_user;Password=Password1;";

        public DB() : base(new NpgsqlConnection(ConnStr), contextOwnsConnection: true)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>().ToTable("Actor", "public");
            modelBuilder.Entity<Habit>().ToTable("Habits", "public");
            modelBuilder.Entity<Operation>().ToTable("Operations", "public");
            modelBuilder.Entity<OperationDevice>().ToTable("OperationDevices", "public");
            modelBuilder.Entity<OperationHabitMapping>().ToTable("OperationHabitMappings", "public");
            modelBuilder.Entity<AreaBase>().ToTable("AreaBases", "public");
            modelBuilder.Entity<DeviceBase>().ToTable("DeviceBases", "public");
            modelBuilder.Entity<GraphObject>().ToTable("GraphObjects", "public");
            modelBuilder.Entity<GraphNodeDeviceMapping>().ToTable("GraphNodeDeviceMappings", "public");
            modelBuilder.Entity<Scenario>().ToTable("Scenarios", "public");

            modelBuilder.Entity<Habit>()
                .HasMany(h => h.Actors)
                .WithMany(a => a.HabitsOfActor)
                .Map(m => m.ToTable("HabitActors", "public"));

            modelBuilder.Entity<Scenario>()
                .HasMany(s => s.ActorsInScenario)
                .WithMany(a => a.Scenarios)
                .Map(m => m.ToTable("ScenarioActors", "public"));

            modelBuilder.Entity<Scenario>()
                .HasMany(s => s.AreasInScenario)
                .WithMany(a => a.Scenarios)
                .Map(m => m.ToTable("ScenarioAreaBases", "public"));
        }

        public int SaveAllChanges() => base.SaveChanges();

        public DbSet<DeviceBase> DeviceBases { get; set; }
        public DbSet<AreaBase> AreaBases { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Habit> Habits { get; set; }
        public DbSet<OperationDevice> OperationDevices { get; set; }
        public DbSet<OperationHabitMapping> OperationHabitMappings { get; set; }
        public DbSet<Scenario> Scenarios { get; set; }
        public DbSet<GraphObject> GraphObjects { get; set; }
        public DbSet<GraphNodeDeviceMapping> GraphNodeDeviceMappings { get; set; }
    }
}
