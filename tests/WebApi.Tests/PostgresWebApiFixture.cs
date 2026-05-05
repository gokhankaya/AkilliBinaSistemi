using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using WebApi.Data;

namespace WebApi.Tests;

public sealed class PostgresWebApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:15")
        .WithDatabase("adle_sim")
        .WithUsername("adle_user")
        .WithPassword("Password1")
        .Build();

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AdleDbContext>();
        await db.Database.EnsureCreatedAsync();
    }

    public new async Task DisposeAsync()
    {
        await _postgres.DisposeAsync();
        await base.DisposeAsync();
    }

    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:Adle", _postgres.GetConnectionString());

        builder.ConfigureServices(services =>
        {
            var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<AdleDbContext>));
            services.Remove(descriptor);
            services.AddDbContext<AdleDbContext>(opt => opt.UseNpgsql(_postgres.GetConnectionString()));
        });
    }

    public async Task SeedAsync(Action<AdleDbContext> seed)
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AdleDbContext>();
        seed(db);
        await db.SaveChangesAsync();
    }

    public async Task ResetAsync()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AdleDbContext>();
        db.Areas.RemoveRange(db.Areas);
        db.AreaTypes.RemoveRange(db.AreaTypes);
        await db.SaveChangesAsync();
    }
}
