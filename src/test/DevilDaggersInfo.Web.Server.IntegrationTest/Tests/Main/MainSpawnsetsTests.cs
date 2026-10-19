using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using System.Net;
using System.Text.Json;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Main;

[NotInParallel(nameof(MainSpawnsetsTests))]
internal sealed class MainSpawnsetsTests : ApplicationTest
{
	private async Task SeedThreeAsync()
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "Alice"));
			dbContext.Players.Add(EntityFixtures.Player(2, "Bob"));
		});

		await App.SeedAsync(dbContext =>
		{
			dbContext.Spawnsets.Add(EntityFixtures.Spawnset("Charlie", playerId: 1));
			dbContext.Spawnsets.Add(EntityFixtures.Spawnset("alpha", playerId: 2));
			dbContext.Spawnsets.Add(EntityFixtures.Spawnset("Bravo", playerId: 1));
		});
	}

	[Test]
	public async Task GetSpawnsets_PagesAndReportsTheTotal()
	{
		await SeedThreeAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/spawnsets?withCustomLeaderboardOnly=false&pageIndex=0&pageSize=15");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("totalResults").GetInt32()).IsEqualTo(3);
		await Assert.That(document.RootElement.GetProperty("results").GetArrayLength()).IsEqualTo(3);
	}

	[Test]
	public async Task GetSpawnsets_FiltersByNameCaseInsensitively()
	{
		await SeedThreeAsync();

		using HttpClient client = App.CreateApiClient();

		// The column collation is case insensitive on MySQL, which the production code comments rely on. This is
		// exactly the behaviour an in-memory provider would get wrong.
		using HttpResponseMessage response = await client.GetAsync("api/spawnsets?withCustomLeaderboardOnly=false&spawnsetFilter=ALPHA&pageSize=15");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("totalResults").GetInt32()).IsEqualTo(1);
		await Assert.That(document.RootElement.GetProperty("results")[0].GetProperty("name").GetString()).IsEqualTo("alpha");
	}

	[Test]
	public async Task GetSpawnsets_FiltersByAuthor()
	{
		await SeedThreeAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/spawnsets?withCustomLeaderboardOnly=false&authorFilter=bob&pageSize=15");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("totalResults").GetInt32()).IsEqualTo(1);
		await Assert.That(document.RootElement.GetProperty("results")[0].GetProperty("authorName").GetString()).IsEqualTo("Bob");
	}

	[Test]
	public async Task GetSpawnsets_WithCustomLeaderboardOnly_ExcludesSpawnsetsWithoutOne()
	{
		await SeedThreeAsync();
		await App.SeedAsync(dbContext => dbContext.CustomLeaderboards.Add(EntityFixtures.CustomLeaderboard(spawnsetId: 1)));

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/spawnsets?withCustomLeaderboardOnly=true&pageSize=15");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("totalResults").GetInt32()).IsEqualTo(1);
	}

	[Test]
	public async Task GetSpawnsets_SortsByNameAscending()
	{
		await SeedThreeAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/spawnsets?withCustomLeaderboardOnly=false&sortBy=0&ascending=true&pageSize=15");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		JsonElement results = document.RootElement.GetProperty("results");

		// MySQL's case-insensitive collation orders these as alpha, Bravo, Charlie.
		await Assert.That(results[0].GetProperty("name").GetString()).IsEqualTo("alpha");
		await Assert.That(results[1].GetProperty("name").GetString()).IsEqualTo("Bravo");
		await Assert.That(results[2].GetProperty("name").GetString()).IsEqualTo("Charlie");
	}

	[Test]
	public async Task GetSpawnsetById_ReturnsTheSpawnset()
	{
		await SeedThreeAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/spawnsets/1");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("name").GetString()).IsEqualTo("Charlie");
		await Assert.That(document.RootElement.GetProperty("authorName").GetString()).IsEqualTo("Alice");
	}

	[Test]
	public async Task GetSpawnsetById_IsNotFoundForAnUnknownId()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/spawnsets/999");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}

	[Test]
	public async Task GetSpawnsetByHash_ResolvesTheSpawnset()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(1, "Alice")));
		await App.SeedAsync(dbContext => dbContext.Spawnsets.Add(CustomLeaderboardFixtures.Spawnset(playerId: 1)));

		byte[] hash = MD5.HashData(CustomLeaderboardFixtures.V3SpawnsetFile);

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync($"api/spawnsets/by-hash?hash={Uri.EscapeDataString(Convert.ToBase64String(hash))}");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("name").GetString()).IsEqualTo("V3");
	}

	[Test]
	public async Task GetSpawnsetHash_ReturnsTheStoredHash()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(1, "Alice")));
		await App.SeedAsync(dbContext => dbContext.Spawnsets.Add(CustomLeaderboardFixtures.Spawnset(playerId: 1)));

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/spawnsets/hash?fileName=V3");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		byte[] hash = document.RootElement.GetBytesFromBase64();

		await Assert.That(hash).IsEquivalentTo(MD5.HashData(CustomLeaderboardFixtures.V3SpawnsetFile), CollectionOrdering.Matching);
	}

	[Test]
	public async Task GetSpawnsetFile_ServesTheBytesFromTheDatabase()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(1, "Alice")));
		await App.SeedAsync(dbContext => dbContext.Spawnsets.Add(CustomLeaderboardFixtures.Spawnset(playerId: 1)));

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/spawnsets/V3/file");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		byte[] bytes = await response.Content.ReadAsByteArrayAsync();
		await Assert.That(bytes).IsEquivalentTo(CustomLeaderboardFixtures.V3SpawnsetFile, CollectionOrdering.Matching);
	}

	[Test]
	public async Task GetTotalSpawnsetData_CountsAll()
	{
		await SeedThreeAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/spawnsets/total-data");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("count").GetInt32()).IsEqualTo(3);
	}

	[Test]
	public async Task GetSpawnsetsByAuthor_ListsOnlyTheirs()
	{
		await SeedThreeAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/spawnsets/by-author?playerId=1");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetArrayLength()).IsEqualTo(2);
	}

	[Test]
	public async Task GetSpawnsetsByAuthor_IsEmptyForAnUnknownPlayer()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/spawnsets/by-author?playerId=999");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetArrayLength()).IsEqualTo(0);
	}
}
