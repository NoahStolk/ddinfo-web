using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using MainApi = DevilDaggersInfo.Web.ApiSpec.Main.CustomLeaderboards;
using System.Net;
using System.Text.Json;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Main;

[NotInParallel(nameof(MainCustomLeaderboardsTests))]
internal sealed class MainCustomLeaderboardsTests : ApplicationTest
{
	/// <summary>
	/// Seeds a featured leaderboard with three entries. Featured matters: a leaderboard that is not featured reports
	/// null daggers everywhere, which is the most common seeding trap in this area.
	/// </summary>
	private async Task SeedFeaturedLeaderboardAsync()
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "Author"));
			dbContext.Players.Add(EntityFixtures.Player(2, "Runner"));
			dbContext.Spawnsets.Add(CustomLeaderboardFixtures.Spawnset(playerId: 1));
		});

		await App.SeedAsync(dbContext =>
		{
			CustomLeaderboardEntity leaderboard = CustomLeaderboardFixtures.CustomLeaderboard(spawnsetId: 1);
			leaderboard.IsFeatured = true;
			dbContext.CustomLeaderboards.Add(leaderboard);
		});

		await App.SeedAsync(dbContext =>
		{
			// 1000 s clears the leviathan threshold; 60 s is exactly bronze.
			dbContext.CustomEntries.Add(EntityFixtures.CustomEntry(customLeaderboardId: 1, playerId: 1, time: 10000000));
			dbContext.CustomEntries.Add(EntityFixtures.CustomEntry(customLeaderboardId: 1, playerId: 2, time: 600000));
		});
	}

	[Test]
	public async Task GetCustomLeaderboards_PagesAndReportsTheTotal()
	{
		await SeedFeaturedLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/custom-leaderboards?gameMode=0&rankSorting=0&pageSize=15");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		JsonElement overview = document.RootElement.GetProperty("results")[0];

		await Assert.That(document.RootElement.GetProperty("totalResults").GetInt32()).IsEqualTo(1);
		await Assert.That(overview.GetProperty("spawnsetName").GetString()).IsEqualTo("V3");
		await Assert.That(overview.GetProperty("spawnsetAuthorName").GetString()).IsEqualTo("Author");
		await Assert.That(overview.GetProperty("playerCount").GetInt32()).IsEqualTo(2);
		await Assert.That(overview.GetProperty("isFeatured").GetBoolean()).IsTrue();

		// TimeDesc, so the world record is the longest run and its holder tops the board.
		await Assert.That(overview.GetProperty("worldRecord").GetDouble()).IsEqualTo(1000d);
		await Assert.That(overview.GetProperty("topPlayer").GetString()).IsEqualTo("Author");
	}

	[Test]
	public async Task GetCustomLeaderboards_ReportsDaggerThresholdsInSecondsForATimeLeaderboard()
	{
		await SeedFeaturedLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/custom-leaderboards?gameMode=0&rankSorting=0&pageSize=15");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		JsonElement daggers = document.RootElement.GetProperty("results")[0].GetProperty("daggers");

		// Game units divided by 10,000.
		await Assert.That(daggers.GetProperty("bronze").GetDouble()).IsEqualTo(60d);
		await Assert.That(daggers.GetProperty("silver").GetDouble()).IsEqualTo(120d);
		await Assert.That(daggers.GetProperty("golden").GetDouble()).IsEqualTo(250d);
		await Assert.That(daggers.GetProperty("devil").GetDouble()).IsEqualTo(500d);
		await Assert.That(daggers.GetProperty("leviathan").GetDouble()).IsEqualTo(1000d);
	}

	[Test]
	public async Task GetCustomLeaderboards_LeavesDaggerThresholdsRawForANonTimeLeaderboard()
	{
		// The game-unit-to-seconds conversion is applied only for time rank sortings. A gems leaderboard reports its
		// thresholds as the plain gem counts, which is the counterpart to the seconds conversion asserted above.
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "Author"));
			dbContext.Spawnsets.Add(CustomLeaderboardFixtures.Spawnset(playerId: 1));
		});

		await App.SeedAsync(dbContext =>
		{
			CustomLeaderboardEntity leaderboard = EntityFixtures.CustomLeaderboard(spawnsetId: 1, rankSorting: CustomLeaderboardRankSorting.GemsCollectedDesc);
			leaderboard.IsFeatured = true;
			leaderboard.Bronze = 100;
			leaderboard.Silver = 200;
			leaderboard.Golden = 300;
			leaderboard.Devil = 400;
			leaderboard.Leviathan = 500;
			dbContext.CustomLeaderboards.Add(leaderboard);
		});

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync($"api/custom-leaderboards?gameMode=0&rankSorting={(int)CustomLeaderboardRankSorting.GemsCollectedDesc}&pageSize=15");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		JsonElement daggers = document.RootElement.GetProperty("results")[0].GetProperty("daggers");

		await Assert.That(daggers.GetProperty("bronze").GetDouble()).IsEqualTo(100d);
		await Assert.That(daggers.GetProperty("leviathan").GetDouble()).IsEqualTo(500d);
	}

	[Test]
	public async Task GetCustomLeaderboards_ReportsNullDaggersWhenNotFeatured()
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "Author"));
			dbContext.Spawnsets.Add(CustomLeaderboardFixtures.Spawnset(playerId: 1));
		});
		await App.SeedAsync(dbContext => dbContext.CustomLeaderboards.Add(EntityFixtures.CustomLeaderboard(spawnsetId: 1)));

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/custom-leaderboards?gameMode=0&rankSorting=0&pageSize=15");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		JsonElement overview = document.RootElement.GetProperty("results")[0];

		await Assert.That(overview.GetProperty("isFeatured").GetBoolean()).IsFalse();
		await Assert.That(overview.GetProperty("daggers").ValueKind).IsEqualTo(JsonValueKind.Null);
	}

	[Test]
	public async Task GetCustomLeaderboards_FiltersByRankSorting()
	{
		await SeedFeaturedLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();

		// The seeded board is TimeDesc, so asking for a different sorting must exclude it.
		using HttpResponseMessage response = await client.GetAsync($"api/custom-leaderboards?gameMode=0&rankSorting={(int)CustomLeaderboardRankSorting.GemsCollectedAsc}&pageSize=15");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("totalResults").GetInt32()).IsEqualTo(0);
	}

	[Test]
	public async Task GetCustomLeaderboardById_RanksEntriesAndAssignsDaggers()
	{
		await SeedFeaturedLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/custom-leaderboards/1");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		JsonElement entries = document.RootElement.GetProperty("customEntries");

		await Assert.That(entries.GetArrayLength()).IsEqualTo(2);
		await Assert.That(entries[0].GetProperty("rank").GetInt32()).IsEqualTo(1);
		await Assert.That(entries[0].GetProperty("time").GetDouble()).IsEqualTo(1000d);
		await Assert.That(entries[0].GetProperty("customLeaderboardDagger").GetInt32()).IsEqualTo((int)MainApi.CustomLeaderboardDagger.Leviathan);

		// Exactly on the bronze threshold, and the boundary is inclusive.
		await Assert.That(entries[1].GetProperty("rank").GetInt32()).IsEqualTo(2);
		await Assert.That(entries[1].GetProperty("customLeaderboardDagger").GetInt32()).IsEqualTo((int)MainApi.CustomLeaderboardDagger.Bronze);
	}

	[Test]
	public async Task GetCustomLeaderboardById_IsNotFoundForAnUnknownId()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/custom-leaderboards/999");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("title").GetString()).IsEqualTo("Custom leaderboard '999' could not be found.");
	}

	[Test]
	public async Task GetTotalCustomLeaderboardData_CountsPerGameMode()
	{
		await SeedFeaturedLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/custom-leaderboards/total-data");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		JsonElement root = document.RootElement;

		// Enum dictionary keys serialize as the enum NAME, unlike enum values which serialize as integers.
		await Assert.That(root.GetProperty("leaderboardsPerGameMode").GetProperty("Survival").GetInt32()).IsEqualTo(1);
		await Assert.That(root.GetProperty("leaderboardsPerGameMode").GetProperty("Race").GetInt32()).IsEqualTo(0);
		await Assert.That(root.GetProperty("scoresPerGameMode").GetProperty("Survival").GetInt32()).IsEqualTo(2);
		await Assert.That(root.GetProperty("totalPlayers").GetInt32()).IsEqualTo(2);
	}

	[Test]
	public async Task GetGlobalLeaderboard_AwardsPointsByRankAndDagger()
	{
		await SeedFeaturedLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/custom-leaderboards/global-leaderboard?gameMode=0&rankSorting=0");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		JsonElement entries = document.RootElement.GetProperty("entries");

		await Assert.That(entries.GetArrayLength()).IsEqualTo(2);

		// Rank 1 of 2 with a leviathan: (2 - 0) * 2 + 10 = 14.
		await Assert.That(entries[0].GetProperty("playerName").GetString()).IsEqualTo("Author");
		await Assert.That(entries[0].GetProperty("points").GetInt32()).IsEqualTo(14);
		await Assert.That(entries[0].GetProperty("leviathanDaggerCount").GetInt32()).IsEqualTo(1);

		// Rank 2 of 2 with a bronze: (2 - 1) * 2 + 1 = 3.
		await Assert.That(entries[1].GetProperty("playerName").GetString()).IsEqualTo("Runner");
		await Assert.That(entries[1].GetProperty("points").GetInt32()).IsEqualTo(3);
		await Assert.That(entries[1].GetProperty("bronzeDaggerCount").GetInt32()).IsEqualTo(1);
	}

	[Test]
	public async Task GetAllowedCategories_AlwaysReturnsTheFullSet()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/custom-leaderboards/allowed-categories");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

		// The shape is fixed regardless of what is in the database; only the counts vary.
		await Assert.That(document.RootElement.GetArrayLength()).IsEqualTo(19);
		await Assert.That(document.RootElement.EnumerateArray().All(e => e.GetProperty("leaderboardCount").GetInt32() == 0)).IsTrue();
	}

	[Test]
	public async Task GetPlayerCustomLeaderboardStatistics_OnlyCountsFeaturedLeaderboards()
	{
		await SeedFeaturedLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/players/1/custom-leaderboard-statistics");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

		await Assert.That(document.RootElement.GetArrayLength()).IsEqualTo(1);
		await Assert.That(document.RootElement[0].GetProperty("leviathanDaggerCount").GetInt32()).IsEqualTo(1);
		await Assert.That(document.RootElement[0].GetProperty("leaderboardsPlayedCount").GetInt32()).IsEqualTo(1);
		await Assert.That(document.RootElement[0].GetProperty("totalCount").GetInt32()).IsEqualTo(1);
	}

	[Test]
	public async Task GetPlayerCustomLeaderboardStatistics_IsEmptyWhenNoFeaturedEntriesExist()
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "Author"));
			dbContext.Spawnsets.Add(CustomLeaderboardFixtures.Spawnset(playerId: 1));
		});
		await App.SeedAsync(dbContext => dbContext.CustomLeaderboards.Add(EntityFixtures.CustomLeaderboard(spawnsetId: 1)));
		await App.SeedAsync(dbContext => dbContext.CustomEntries.Add(EntityFixtures.CustomEntry(customLeaderboardId: 1, playerId: 1, time: 500000)));

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/players/1/custom-leaderboard-statistics");

		// The entry exists but its leaderboard is not featured, so it is invisible here.
		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetArrayLength()).IsEqualTo(0);
	}
}
