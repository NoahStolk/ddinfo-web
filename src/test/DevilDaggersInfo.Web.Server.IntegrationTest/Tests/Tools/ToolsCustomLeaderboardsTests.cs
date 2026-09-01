using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using System.Net;
using System.Text.Json;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Tools;

/// <summary>
/// The custom leaderboard endpoints ddinfo-tools reads. These are the game-facing contract, so the assertions are on
/// exact values rather than just status codes.
/// </summary>
[NotInParallel(nameof(ToolsCustomLeaderboardsTests))]
internal sealed class ToolsCustomLeaderboardsTests : ApplicationTest
{
	private static readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);

	/// <summary>
	/// Seeds one leaderboard on the V3 spawnset with three entries, deliberately inserted out of order so that any
	/// ordering the endpoint reports has to come from its own sorting rather than from insertion order.
	/// </summary>
	private async Task<byte[]> SeedLeaderboardAsync()
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "Author"));
			dbContext.Players.Add(EntityFixtures.Player(2, "Runner Two"));
			dbContext.Players.Add(EntityFixtures.Player(3, "Runner Three"));
			dbContext.Spawnsets.Add(CustomLeaderboardFixtures.Spawnset(playerId: 1));
		});

		await App.SeedAsync(dbContext => dbContext.CustomLeaderboards.Add(CustomLeaderboardFixtures.CustomLeaderboard(spawnsetId: 1)));

		await App.SeedAsync(dbContext =>
		{
			dbContext.CustomEntries.Add(EntityFixtures.CustomEntry(customLeaderboardId: 1, playerId: 2, time: 500000));
			dbContext.CustomEntries.Add(EntityFixtures.CustomEntry(customLeaderboardId: 1, playerId: 1, time: 900000));
			dbContext.CustomEntries.Add(EntityFixtures.CustomEntry(customLeaderboardId: 1, playerId: 3, time: 100000));
		});

		return MD5.HashData(CustomLeaderboardFixtures.V3SpawnsetFile);
	}

	[Test]
	public async Task GetCustomLeaderboards_ReturnsTheSeededLeaderboard()
	{
		await SeedLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/app/custom-leaderboards?selectedPlayerId=1");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		JsonElement root = document.RootElement;

		await Assert.That(root.GetArrayLength()).IsEqualTo(1);
		await Assert.That(root[0].GetProperty("spawnsetName").GetString()).IsEqualTo("V3");
		await Assert.That(root[0].GetProperty("id").GetInt32()).IsEqualTo(1);
	}

	[Test]
	public async Task GetCustomLeaderboards_IsEmptyWhenNoneExist()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/app/custom-leaderboards?selectedPlayerId=1");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetArrayLength()).IsEqualTo(0);
	}

	[Test]
	public async Task GetCustomLeaderboardById_SortsEntriesBestFirst()
	{
		await SeedLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/app/custom-leaderboards/1");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		JsonElement entries = document.RootElement.GetProperty("sortedEntries");

		// The leaderboard is TimeDesc, so the longest run ranks first regardless of insertion order. The API reports
		// seconds while the database stores ten-thousandths.
		await Assert.That(entries.GetArrayLength()).IsEqualTo(3);
		await Assert.That(entries[0].GetProperty("timeInSeconds").GetDouble()).IsEqualTo(90d);
		await Assert.That(entries[1].GetProperty("timeInSeconds").GetDouble()).IsEqualTo(50d);
		await Assert.That(entries[2].GetProperty("timeInSeconds").GetDouble()).IsEqualTo(10d);

		// Ranks are assigned by the sorted position, not by insertion order or player ID.
		await Assert.That(entries[0].GetProperty("rank").GetInt32()).IsEqualTo(1);
		await Assert.That(entries[1].GetProperty("rank").GetInt32()).IsEqualTo(2);
		await Assert.That(entries[2].GetProperty("rank").GetInt32()).IsEqualTo(3);
		await Assert.That(entries[0].GetProperty("playerName").GetString()).IsEqualTo("Author");
	}

	[Test]
	public async Task GetCustomLeaderboardById_IsNotFoundForAnUnknownId()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/app/custom-leaderboards/12345");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}

	[Test]
	public async Task GetCustomLeaderboardBySpawnsetHash_FindsTheLeaderboard()
	{
		byte[] hash = await SeedLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync($"api/app/custom-leaderboards/by-hash?hash={Uri.EscapeDataString(Convert.ToBase64String(hash))}");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("spawnsetName").GetString()).IsEqualTo("V3");
		await Assert.That(document.RootElement.GetProperty("sortedEntries").GetArrayLength()).IsEqualTo(3);
	}

	[Test]
	public async Task GetCustomLeaderboardBySpawnsetHash_IsNotFoundForAnUnknownHash()
	{
		await SeedLeaderboardAsync();

		byte[] unknown = MD5.HashData("not a spawnset"u8.ToArray());

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync($"api/app/custom-leaderboards/by-hash?hash={Uri.EscapeDataString(Convert.ToBase64String(unknown))}");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}

	[Test]
	public async Task CustomLeaderboardExists_AnswersWithStatusOnly()
	{
		byte[] hash = await SeedLeaderboardAsync();
		byte[] unknown = MD5.HashData("not a spawnset"u8.ToArray());

		using HttpClient client = App.CreateApiClient();

		using (HttpRequestMessage request = new(HttpMethod.Head, $"api/app/custom-leaderboards/exists?hash={Uri.EscapeDataString(Convert.ToBase64String(hash))}"))
		using (HttpResponseMessage response = await client.SendAsync(request))
		{
			await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
		}

		using (HttpRequestMessage request = new(HttpMethod.Head, $"api/app/custom-leaderboards/exists?hash={Uri.EscapeDataString(Convert.ToBase64String(unknown))}"))
		using (HttpResponseMessage response = await client.SendAsync(request))
		{
			await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
		}
	}

	[Test]
	public async Task GetAllowedCategories_ListsEveryGameModeAndSortingCombination()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/app/custom-leaderboards/allowed-categories");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

		// 16 rank sortings for Survival, 2 for Race, 1 for TimeAttack.
		await Assert.That(document.RootElement.GetArrayLength()).IsEqualTo(19);
	}
}
