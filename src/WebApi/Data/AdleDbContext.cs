using Microsoft.EntityFrameworkCore;

namespace WebApi.Data;

public class AdleDbContext : DbContext
{
    public AdleDbContext(DbContextOptions<AdleDbContext> options) : base(options) { }

    public DbSet<Area> Areas => Set<Area>();
    public DbSet<AreaType> AreaTypes => Set<AreaType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(e =>
        {
            e.ToTable("Areas", "public");
            e.HasKey(x => x.ID);
        });

        modelBuilder.Entity<AreaType>(e =>
        {
            e.ToTable("AreaTypes", "public");
            e.HasKey(x => x.ID);
        });
    }
}
