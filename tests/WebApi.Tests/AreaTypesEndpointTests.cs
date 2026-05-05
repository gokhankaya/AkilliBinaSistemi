using System.Net.Http.Json;
using FluentAssertions;
using WebApi.Controllers;
using WebApi.Data;

namespace WebApi.Tests;

[Collection(nameof(PostgresCollection))]
public class AreaTypesEndpointTests
{
    private readonly PostgresWebApiFixture _fx;

    public AreaTypesEndpointTests(PostgresWebApiFixture fx) => _fx = fx;

    [Fact]
    public async Task GetAll_returns_seeded_types()
    {
        await _fx.ResetAsync();
        await _fx.SeedAsync(db =>
        {
            db.AreaTypes.Add(new AreaType { Name = "LivingRoom", Definition = "Salon" });
            db.AreaTypes.Add(new AreaType { Name = "Kitchen", Definition = "Mutfak" });
        });

        var client = _fx.CreateClient();
        var types = await client.GetFromJsonAsync<List<AreaTypesController.AreaTypeDto>>("/api/area-types");

        types.Should().NotBeNull();
        types!.Should().HaveCount(2);
        types.Select(t => t.Name).Should().Contain(new[] { "LivingRoom", "Kitchen" });
    }
}
