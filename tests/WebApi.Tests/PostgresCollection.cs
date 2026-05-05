namespace WebApi.Tests;

[CollectionDefinition(nameof(PostgresCollection))]
public class PostgresCollection : ICollectionFixture<PostgresWebApiFixture>
{
}
