using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using NSubstitute;
using System.Net;
using System.Text.Json;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Main;

/// <summary>
/// The official leaderboard proxy. The real implementation posts to dd.hasmodai.com, so the service is substituted;
/// what is under test is the controller's validation and the game-unit-to-seconds conversion.
/// </summary>
[NotInParallel(nameof(MainLeaderboardsTests))]
internal sealed class MainLeaderboardsTests : ApplicationTest
{
	// This endpoint never touches the database.
	protected override bool ResetBeforeEachTest => false;

	private static IDdLeaderboardService.EntryResponse Entry(int rank, int id, string username, int timeInGameUnits)
	{
		return new IDdLeaderboardService.EntryResponse
		{
			Rank = rank,
			Id = id,
			Username = username,
			Time = timeInGameUnits,
			Kills = 100,
			Gems = 200,
			DeathType = 3,
			DaggersHit = 40,
			DaggersFired = 50,
			TimeTotal = 1_000_000,
			KillsTotal = 2000,
			GemsTotal = 3000,
			DeathsTotal = 10,
			DaggersHitTotal = 4000,
			DaggersFiredTotal = 5000,
		};
	}

	private static IDdLeaderboardService.LeaderboardResponse Leaderboard(params IDdLeaderboardService.EntryResponse[] entries)
	{
		return new IDdLeaderboardService.LeaderboardResponse
		{
			DateTime = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			TotalPlayers = 12345,
			TimeGlobal = 9_000_000,
			KillsGlobal = 1,
			GemsGlobal = 2,
			DeathsGlobal = 3,
			DaggersHitGlobal = 4,
			DaggersFiredGlobal = 5,
			Entries = [.. entries],
		};
	}

	[Test]
	public async Task GetLeaderboard_ConvertsGameUnitsToSeconds()
	{
		App.DdLeaderboardService.GetLeaderboard(1, 100).Returns(Leaderboard(Entry(1, 1, "Champion", 12_345_678)));

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/leaderboards");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		JsonElement root = document.RootElement;

		await Assert.That(root.GetProperty("totalPlayers").GetInt32()).IsEqualTo(12345);
		await Assert.That(root.GetProperty("timeGlobal").GetDouble()).IsEqualTo(900d);
		await Assert.That(root.GetProperty("entries")[0].GetProperty("username").GetString()).IsEqualTo("Champion");
		await Assert.That(root.GetProperty("entries")[0].GetProperty("time").GetDouble()).IsEqualTo(1234.5678d);
	}

	[Test]
	[Arguments(0, 100)]
	[Arguments(-1, 100)]
	[Arguments(1, 0)]
	[Arguments(1, 1001)]
	public async Task GetLeaderboard_RejectsOutOfRangeParameters(int rankStart, int limit)
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync($"api/leaderboards?rankStart={rankStart}&limit={limit}");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
	}

	[Test]
	public async Task GetEntryById_ReturnsTheEntry()
	{
		App.DdLeaderboardService.GetEntryById(42).Returns(Entry(7, 42, "Someone", 500_000));

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/leaderboards/entry/by-id?id=42");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("id").GetInt32()).IsEqualTo(42);
		await Assert.That(document.RootElement.GetProperty("time").GetDouble()).IsEqualTo(50d);
		await Assert.That(document.RootElement.GetProperty("deathType").GetInt32()).IsEqualTo(3);
	}

	[Test]
	[Arguments(0)]
	[Arguments(-5)]
	public async Task GetEntryById_RejectsANonPositiveId(int id)
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync($"api/leaderboards/entry/by-id?id={id}");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
	}

	[Test]
	public async Task GetEntriesByIds_DropsNonNumericTokens()
	{
		// The substitute is shared by every test in this class, so received calls are cleared to keep the assertion
		// below counting only this test's request.
		App.DdLeaderboardService.ClearReceivedCalls();
		App.DdLeaderboardService.GetEntriesByIds(Arg.Any<IEnumerable<int>>()).Returns(_ => [Entry(1, 1, "One", 100_000)]);

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/leaderboards/entry/by-ids?commaSeparatedIds=1,nope,3");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		// The controller silently filters unparseable tokens rather than rejecting the request.
		await App.DdLeaderboardService.Received(1).GetEntriesByIds(Arg.Is<IEnumerable<int>>(ids => ids.SequenceEqual(new[] { 1, 3 })));
	}

	[Test]
	public async Task GetEntriesByIds_RequiresTheParameter()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/leaderboards/entry/by-ids");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
	}

	[Test]
	[Arguments("ab")]
	[Arguments("aVeryLongUsernameIndeed")]
	public async Task GetEntriesByName_RejectsOutOfRangeLengths(string name)
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync($"api/leaderboards/entry/by-username?name={name}");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
	}

	[Test]
	public async Task GetEntryByRank_IsNotFoundWhenTheRankIsEmpty()
	{
		App.DdLeaderboardService.GetLeaderboard(999, 1).Returns(Leaderboard());

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/leaderboards/entry/by-rank?rank=999");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}

	[Test]
	public async Task GetEntryByRank_ReturnsTheSingleEntry()
	{
		App.DdLeaderboardService.GetLeaderboard(3, 1).Returns(Leaderboard(Entry(3, 30, "Third", 300_000)));

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/leaderboards/entry/by-rank?rank=3");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("id").GetInt32()).IsEqualTo(30);
	}

	[Test]
	public async Task GetLeaderboard_SurfacesAnUpstreamFailureAsBadRequest()
	{
		App.DdLeaderboardService.GetLeaderboard(1, 100)
			.Returns<IDdLeaderboardService.LeaderboardResponse>(_ => throw new DdLeaderboardException("The leaderboard servers returned an unsuccessful response (HTTP 503 ServiceUnavailable)."));

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/leaderboards");

		// A DdLeaderboardException is a StatusCodeException mapping to 400, so the website sees a handled error rather
		// than a 500 when the official servers are down.
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("title").GetString()).Contains("unsuccessful response");
	}
}
