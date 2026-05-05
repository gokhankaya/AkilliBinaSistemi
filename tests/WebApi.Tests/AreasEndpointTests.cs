using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using WebApi.Controllers;
using WebApi.Data;

namespace WebApi.Tests;

[Collection(nameof(PostgresCollection))]
public class AreasEndpointTests
{
    private readonly PostgresWebApiFixture _fx;

    public AreasEndpointTests(PostgresWebApiFixture fx) => _fx = fx;

    [Fact]
    public async Task GetAll_returns_empty_when_no_rows()
    {
        await _fx.ResetAsync();

        var client = _fx.CreateClient();
        var response = await client.GetAsync("/api/areas");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<List<AreasController.AreaDto>>();
        body.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAll_returns_seeded_areas_excluding_soft_deleted()
    {
        await _fx.ResetAsync();
        await _fx.SeedAsync(db =>
        {
            db.Areas.Add(new Area { Name = "Living Room", Width = 30, Height = 20 });
            db.Areas.Add(new Area { Name = "Kitchen", Width = 18, Height = 14 });
            db.Areas.Add(new Area { Name = "Deleted", Width = 1, Height = 1, DeletedDate = DateTime.UtcNow });
        });

        var client = _fx.CreateClient();
        var areas = await client.GetFromJsonAsync<List<AreasController.AreaDto>>("/api/areas");

        areas.Should().NotBeNull();
        areas!.Should().HaveCount(2);
        areas.Select(a => a.Name).Should().BeEquivalentTo(new[] { "Living Room", "Kitchen" });
    }

    [Fact]
    public async Task GetById_returns_404_when_missing()
    {
        await _fx.ResetAsync();

        var client = _fx.CreateClient();
        var response = await client.GetAsync("/api/areas/9999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
