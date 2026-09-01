using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using System.Net;
using System.Text.Json;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Tools;

[NotInParallel(nameof(ToolsSpawnsetsTests))]
internal sealed class ToolsSpawnsetsTests : ApplicationTest
{
	private async Task SeedAsync()
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "Author"));
			dbContext.Spawnsets.Add(CustomLeaderboardFixtures.Spawnset(playerId: 1));
		});
	}

	[Test]
	public async Task GetSpawnsetById_ReturnsTheSpawnsetWithItsAuthor()
	{
		await SeedAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/app/spawnsets/1");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		JsonElement root = document.RootElement;

		await Assert.That(root.GetProperty("name").GetString()).IsEqualTo("V3");
		await Assert.That(root.GetProperty("authorName").GetString()).IsEqualTo("Author");

		// No leaderboard was seeded, so the tools client is told there is none.
		await Assert.That(root.GetProperty("customLeaderboardId").ValueKind).IsEqualTo(JsonValueKind.Null);

		// The spawnset bytes come from the database BLOB, not the file system.
		byte[] fileBytes = root.GetProperty("fileBytes").GetBytesFromBase64();
		await Assert.That(fileBytes).IsEquivalentTo(CustomLeaderboardFixtures.V3SpawnsetFile, CollectionOrdering.Matching);
	}

	[Test]
	public async Task GetSpawnsetById_ReportsTheLeaderboardWhenOneExists()
	{
		await SeedAsync();
		await App.SeedAsync(dbContext => dbContext.CustomLeaderboards.Add(CustomLeaderboardFixtures.CustomLeaderboard(spawnsetId: 1)));

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/app/spawnsets/1");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("customLeaderboardId").GetInt32()).IsEqualTo(1);
	}

	[Test]
	public async Task GetSpawnsetById_IsNotFoundForAnUnknownId()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/app/spawnsets/999");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}

	[Test]
	public async Task GetSpawnsetById_IsBadRequestForANonNumericId()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/app/spawnsets/not-a-number");

		// {id} is an int route parameter, so this fails model binding rather than falling through to a 404.
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
	}

	[Test]
	public async Task GetSpawnsetBuffer_ReturnsTheRawFile()
	{
		await SeedAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/app/spawnsets/1/buffer");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		byte[] data = document.RootElement.GetProperty("data").GetBytesFromBase64();

		await Assert.That(data).IsEquivalentTo(CustomLeaderboardFixtures.V3SpawnsetFile, CollectionOrdering.Matching);
	}

	[Test]
	public async Task GetSpawnsetBuffer_IsNotFoundForAnUnknownId()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/app/spawnsets/999/buffer");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}

	[Test]
	public async Task GetSpawnsetByHash_ReturnsTheSpawnsetAndItsEntries()
	{
		await SeedAsync();
		await App.SeedAsync(dbContext => dbContext.CustomLeaderboards.Add(CustomLeaderboardFixtures.CustomLeaderboard(spawnsetId: 1)));
		await App.SeedAsync(dbContext => dbContext.CustomEntries.Add(EntityFixtures.CustomEntry(customLeaderboardId: 1, playerId: 1, time: 123456)));

		byte[] hash = MD5.HashData(CustomLeaderboardFixtures.V3SpawnsetFile);

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync($"api/app/spawnsets/by-hash?hash={Uri.EscapeDataString(Convert.ToBase64String(hash))}");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		JsonElement root = document.RootElement;

		await Assert.That(root.GetProperty("spawnsetId").GetInt32()).IsEqualTo(1);
		await Assert.That(root.GetProperty("name").GetString()).IsEqualTo("V3");
		await Assert.That(root.GetProperty("authorName").GetString()).IsEqualTo("Author");

		JsonElement entries = root.GetProperty("customLeaderboard").GetProperty("customEntries");
		await Assert.That(entries.GetArrayLength()).IsEqualTo(1);

		// This endpoint reports the raw game-unit time, unlike GetCustomEntry which converts to seconds.
		await Assert.That(entries[0].GetProperty("time").GetInt32()).IsEqualTo(123456);

		// hasReplay is hard-coded false here; the endpoint never consults the file system.
		await Assert.That(entries[0].GetProperty("hasReplay").GetBoolean()).IsFalse();
	}

	[Test]
	public async Task GetSpawnsetByHash_ReportsNoLeaderboardWhenThereIsNone()
	{
		await SeedAsync();

		byte[] hash = MD5.HashData(CustomLeaderboardFixtures.V3SpawnsetFile);

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync($"api/app/spawnsets/by-hash?hash={Uri.EscapeDataString(Convert.ToBase64String(hash))}");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("customLeaderboard").ValueKind).IsEqualTo(JsonValueKind.Null);
	}

	[Test]
	public async Task GetSpawnsetByHash_IsNotFoundForAnUnknownHash()
	{
		await SeedAsync();

		byte[] unknown = MD5.HashData("nope"u8.ToArray());

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync($"api/app/spawnsets/by-hash?hash={Uri.EscapeDataString(Convert.ToBase64String(unknown))}");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}
}
