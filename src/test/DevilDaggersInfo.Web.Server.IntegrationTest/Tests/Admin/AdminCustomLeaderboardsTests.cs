using DevilDaggersInfo.Web.ApiSpec.Admin.CustomLeaderboards;
using DevilDaggersInfo.Web.ApiSpec.Admin.Spawnsets;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using AdminRankSorting = DevilDaggersInfo.Web.ApiSpec.Admin.CustomLeaderboards.CustomLeaderboardRankSorting;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Admin;

/// <summary>
/// Custom leaderboards created through the Admin API on spawnsets that were themselves uploaded through it, and read
/// back through Main (the website) and Tools (ddinfo-tools).
/// </summary>
[NotInParallel(nameof(AdminCustomLeaderboardsTests))]
internal sealed class AdminCustomLeaderboardsTests : ApplicationTest
{
	private static readonly byte[] _v3 = CustomLeaderboardFixtures.V3SpawnsetFile;

	// The request types declare 48 required criteria members. They are found by reflection so that the default request
	// sets every one of them to "any value" without listing them all, and so a criteria member added later is included.
	private static readonly string[] _criteriaPropertyNames =
	[
		.. typeof(AddCustomLeaderboard).GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.Where(p => p.PropertyType == typeof(AddCustomLeaderboardCriteria))
			.Select(p => JsonNamingPolicy.CamelCase.ConvertName(p.Name)),
	];

	private static readonly AddCustomLeaderboardDaggers _orderedTimeDaggers = new()
	{
		Bronze = 60,
		Silver = 120,
		Golden = 250,
		Devil = 500,
		Leviathan = 1000,
	};

	// ! Assigned by the before-test hook, which TUnit runs before every test method.
	private HttpClient _client = null!;

	[Before(HookType.Test)]
	public async Task CreateAdminClientAsync()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(1, "Author")));

		_client = App.CreateApiClient(await App.CreateJwtAsync("leaderboard-admin", Roles.CustomLeaderboards, Roles.Spawnsets));
	}

	[After(HookType.Test)]
	public void DisposeAdminClient()
	{
		_client.Dispose();
	}

	/// <summary>
	/// Uploads a spawnset through the Admin API and returns the ID it was given.
	/// </summary>
	private async Task<int> AddSpawnsetAsync(string name, byte[] file)
	{
		using HttpResponseMessage response = await _client.PostAsJsonAsync("api/admin/spawnsets", new AddSpawnset
		{
			PlayerId = 1,
			Name = name,
			MaxDisplayWaves = null,
			HtmlDescription = null,
			FileContents = file,
		});
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await response.Content.ReadAsStringAsync());

		await using AsyncServiceScope scope = App.CreateScope();
		return await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Spawnsets.Where(s => s.Name == name).Select(s => s.Id).SingleAsync();
	}

	private static Dictionary<string, object?> CreateRequest(
		int? spawnsetId,
		AdminRankSorting rankSorting,
		AddCustomLeaderboardDaggers daggers,
		bool isFeatured,
		Dictionary<string, AddCustomLeaderboardCriteria>? criteria = null)
	{
		Dictionary<string, object?> request = new(StringComparer.Ordinal)
		{
			["rankSorting"] = (int)rankSorting,
			["daggers"] = daggers,
			["isFeatured"] = isFeatured,
		};

		if (spawnsetId.HasValue)
			request["spawnsetId"] = spawnsetId.Value;

		foreach (string name in _criteriaPropertyNames)
			request[name] = new AddCustomLeaderboardCriteria { Operator = CustomLeaderboardCriteriaOperator.Any, Expression = null };

		foreach ((string name, AddCustomLeaderboardCriteria value) in criteria ?? [])
			request[name] = value;

		return request;
	}

	private async Task<HttpResponseMessage> AddAsync(
		int spawnsetId,
		AdminRankSorting rankSorting = AdminRankSorting.TimeDesc,
		AddCustomLeaderboardDaggers? daggers = null,
		bool isFeatured = true,
		Dictionary<string, AddCustomLeaderboardCriteria>? criteria = null)
	{
		return await _client.PostAsJsonAsync("api/admin/custom-leaderboards", CreateRequest(spawnsetId, rankSorting, daggers ?? _orderedTimeDaggers, isFeatured, criteria));
	}

	private async Task<HttpResponseMessage> EditAsync(
		int id,
		AdminRankSorting rankSorting = AdminRankSorting.TimeDesc,
		AddCustomLeaderboardDaggers? daggers = null,
		bool isFeatured = true,
		Dictionary<string, AddCustomLeaderboardCriteria>? criteria = null)
	{
		return await _client.PutAsJsonAsync($"api/admin/custom-leaderboards/{id}", CreateRequest(null, rankSorting, daggers ?? _orderedTimeDaggers, isFeatured, criteria));
	}

	private async Task SeedEntryAsync(int customLeaderboardId)
	{
		await App.SeedAsync(dbContext => dbContext.CustomEntries.Add(CustomLeaderboardFixtures.CustomEntry(customLeaderboardId, playerId: 1)));
	}

	[Test]
	public async Task Add_IsServedByMainAndTools()
	{
		int spawnsetId = await AddSpawnsetAsync("Admin V3", _v3);
		using (HttpResponseMessage added = await AddAsync(spawnsetId))
			await Assert.That(added.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await added.Content.ReadAsStringAsync());

		using HttpClient anonymous = App.CreateApiClient();

		// Times are entered in seconds, stored in game units and served in seconds again.
		using (HttpResponseMessage main = await anonymous.GetAsync("api/custom-leaderboards/1"))
		{
			await Assert.That(main.StatusCode).IsEqualTo(HttpStatusCode.OK);
			using JsonDocument document = JsonDocument.Parse(await main.Content.ReadAsStringAsync());
			await Assert.That(document.RootElement.GetProperty("spawnsetName").GetString()).IsEqualTo("Admin V3");
			await Assert.That(document.RootElement.GetProperty("daggers").GetProperty("leviathan").GetDouble()).IsEqualTo(1000d);
		}

		using (HttpResponseMessage withLeaderboardOnly = await anonymous.GetAsync("api/spawnsets?withCustomLeaderboardOnly=true&pageSize=15"))
			await Assert.That(await withLeaderboardOnly.Content.ReadAsStringAsync()).Contains("Admin V3");

		string hash = Uri.EscapeDataString(Convert.ToBase64String(MD5.HashData(_v3)));
		using HttpRequestMessage exists = new(HttpMethod.Head, $"api/app/custom-leaderboards/exists?hash={hash}");
		using HttpResponseMessage tools = await anonymous.SendAsync(exists);
		await Assert.That(tools.StatusCode).IsEqualTo(HttpStatusCode.OK);

		await using AsyncServiceScope scope = App.CreateScope();
		CustomLeaderboardEntity stored = await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().CustomLeaderboards.SingleAsync();
		await Assert.That(stored.Leviathan).IsEqualTo(10_000_000);
		await Assert.That(stored.Bronze).IsEqualTo(600_000);
	}

	[Test]
	public async Task Add_StoresCriteriaThatAdminReadsBack()
	{
		int spawnsetId = await AddSpawnsetAsync("Admin V3", _v3);
		using (HttpResponseMessage added = await AddAsync(spawnsetId, criteria: new Dictionary<string, AddCustomLeaderboardCriteria>(StringComparer.Ordinal)
		{
			["gemsCollectedCriteria"] = new() { Operator = CustomLeaderboardCriteriaOperator.GreaterThan, Expression = "10" },
		}))
		{
			await Assert.That(added.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await added.Content.ReadAsStringAsync());
		}

		using HttpResponseMessage response = await _client.GetAsync("api/admin/custom-leaderboards/1");
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		JsonElement gemsCollected = document.RootElement.GetProperty("gemsCollectedCriteria");
		await Assert.That(gemsCollected.GetProperty("operator").GetInt32()).IsEqualTo((int)CustomLeaderboardCriteriaOperator.GreaterThan);
		await Assert.That(gemsCollected.GetProperty("expression").GetString()).IsEqualTo("10");
	}

	[Test]
	public async Task Add_RejectsASecondLeaderboardForTheSameSpawnset()
	{
		int spawnsetId = await AddSpawnsetAsync("Admin V3", _v3);
		using (HttpResponseMessage first = await AddAsync(spawnsetId))
			await Assert.That(first.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using HttpResponseMessage response = await AddAsync(spawnsetId, AdminRankSorting.TimeAsc, new AddCustomLeaderboardDaggers { Bronze = 1000, Silver = 500, Golden = 250, Devil = 120, Leviathan = 60 });
		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "A leaderboard for this spawnset already exists.");
	}

	[Test]
	[Arguments(AdminRankSorting.TimeDesc, "Leviathan time must be greater than Devil time")]
	[Arguments(AdminRankSorting.TimeAsc, "Devil time must be smaller than Golden time")]
	public async Task Add_RejectsFeaturedDaggersOutOfOrder(AdminRankSorting rankSorting, string expectedMessage)
	{
		int spawnsetId = await AddSpawnsetAsync("Admin V3", _v3);

		// Descending: Leviathan is not above Devil. Ascending: every dagger except Leviathan < Devil is out of order.
		using HttpResponseMessage response = await AddAsync(spawnsetId, rankSorting, new AddCustomLeaderboardDaggers { Bronze = 60, Silver = 120, Golden = 250, Devil = 500, Leviathan = 400 });
		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, expectedMessage);
	}

	[Test]
	public async Task Add_RejectsAFeaturedTimeDaggerAboveTwoHours()
	{
		int spawnsetId = await AddSpawnsetAsync("Admin V3", _v3);
		using HttpResponseMessage response = await AddAsync(spawnsetId, daggers: new AddCustomLeaderboardDaggers { Bronze = 60, Silver = 120, Golden = 250, Devil = 500, Leviathan = 7201 });

		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "between 0 and 7200");
	}

	[Test]
	public async Task Add_AcceptsUnorderedDaggersWhenNotFeatured()
	{
		int spawnsetId = await AddSpawnsetAsync("Admin V3", _v3);
		using HttpResponseMessage response = await AddAsync(spawnsetId, daggers: new AddCustomLeaderboardDaggers { Bronze = 0, Silver = 0, Golden = 0, Devil = 0, Leviathan = 0 }, isFeatured: false);

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await response.Content.ReadAsStringAsync());
	}

	[Test]
	public async Task Add_RejectsASortingTheGameModeDoesNotAllow()
	{
		int spawnsetId = await AddSpawnsetAsync("Race", (SpawnsetBinary.CreateDefault() with { GameMode = GameMode.Race }).ToBytes());

		// Race only allows time sortings.
		AddCustomLeaderboardDaggers gemDaggers = new() { Bronze = 10, Silver = 20, Golden = 30, Devil = 40, Leviathan = 50 };
		using HttpResponseMessage response = await AddAsync(spawnsetId, AdminRankSorting.GemsCollectedDesc, gemDaggers);
		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "Combining game mode");
	}

	[Test]
	public async Task Add_RejectsATimeAttackSpawnsetWithoutSpawns()
	{
		int spawnsetId = await AddSpawnsetAsync("Empty time attack", (SpawnsetBinary.CreateDefault() with { GameMode = GameMode.TimeAttack }).ToBytes());
		using HttpResponseMessage response = await AddAsync(spawnsetId, AdminRankSorting.TimeAsc, new AddCustomLeaderboardDaggers { Bronze = 1000, Silver = 500, Golden = 250, Devil = 120, Leviathan = 60 });

		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "Time Attack spawnset must have spawns.");
	}

	[Test]
	public async Task Add_RejectsAnExpressionThatCannotBeParsed()
	{
		int spawnsetId = await AddSpawnsetAsync("Admin V3", _v3);
		using HttpResponseMessage response = await AddAsync(spawnsetId, criteria: new Dictionary<string, AddCustomLeaderboardCriteria>(StringComparer.Ordinal)
		{
			["gemsCollectedCriteria"] = new() { Operator = CustomLeaderboardCriteriaOperator.GreaterThan, Expression = "10 +" },
		});

		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "cannot be parsed");
	}

	[Test]
	public async Task Add_RejectsDeathTypeCriteriaWithAnOrderingOperator()
	{
		int spawnsetId = await AddSpawnsetAsync("Admin V3", _v3);
		using HttpResponseMessage response = await AddAsync(spawnsetId, criteria: new Dictionary<string, AddCustomLeaderboardCriteria>(StringComparer.Ordinal)
		{
			["deathTypeCriteria"] = new() { Operator = CustomLeaderboardCriteriaOperator.GreaterThan, Expression = "1" },
		});

		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "death type criteria");
	}

	[Test]
	public async Task Add_RejectsAnUndefinedRankSorting()
	{
		int spawnsetId = await AddSpawnsetAsync("Admin V3", _v3);
		using HttpResponseMessage response = await AddAsync(spawnsetId, (AdminRankSorting)99);

		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "is not a valid rank sorting");
	}

	[Test]
	public async Task Add_RejectsAnUnknownSpawnset()
	{
		using HttpResponseMessage response = await AddAsync(999);

		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "Spawnset with ID '999' does not exist.");
	}

	[Test]
	public async Task Edit_UpdatesWhatMainServes()
	{
		int spawnsetId = await AddSpawnsetAsync("Admin V3", _v3);
		using (HttpResponseMessage added = await AddAsync(spawnsetId))
			await Assert.That(added.StatusCode).IsEqualTo(HttpStatusCode.OK);

		// Not featured: Main stops serving dagger thresholds, and the leaderboard leaves the global leaderboard.
		using (HttpResponseMessage edited = await EditAsync(1, isFeatured: false))
			await Assert.That(edited.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await edited.Content.ReadAsStringAsync());

		using HttpClient anonymous = App.CreateApiClient();
		using HttpResponseMessage main = await anonymous.GetAsync("api/custom-leaderboards/1");
		using JsonDocument document = JsonDocument.Parse(await main.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("daggers").ValueKind).IsEqualTo(JsonValueKind.Null);
	}

	[Test]
	public async Task Edit_AllowsChangingDaggersOnceScoresExist()
	{
		int spawnsetId = await AddSpawnsetAsync("Admin V3", _v3);
		using (HttpResponseMessage added = await AddAsync(spawnsetId))
			await Assert.That(added.StatusCode).IsEqualTo(HttpStatusCode.OK);

		await SeedEntryAsync(1);

		using HttpResponseMessage response = await EditAsync(1, daggers: new AddCustomLeaderboardDaggers { Bronze = 30, Silver = 60, Golden = 120, Devil = 240, Leviathan = 480 });
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await response.Content.ReadAsStringAsync());
	}

	[Test]
	public async Task Edit_RejectsChangingTheRankSortingOnceScoresExist()
	{
		int spawnsetId = await AddSpawnsetAsync("Admin V3", _v3);
		using (HttpResponseMessage added = await AddAsync(spawnsetId))
			await Assert.That(added.StatusCode).IsEqualTo(HttpStatusCode.OK);

		await SeedEntryAsync(1);

		using HttpResponseMessage response = await EditAsync(1, AdminRankSorting.TimeAsc, new AddCustomLeaderboardDaggers { Bronze = 1000, Silver = 500, Golden = 250, Devil = 120, Leviathan = 60 });
		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "Cannot change rank sorting for custom leaderboard with scores.");
	}

	[Test]
	public async Task Edit_RejectsChangingCriteriaOnceScoresExist()
	{
		int spawnsetId = await AddSpawnsetAsync("Admin V3", _v3);
		using (HttpResponseMessage added = await AddAsync(spawnsetId))
			await Assert.That(added.StatusCode).IsEqualTo(HttpStatusCode.OK);

		await SeedEntryAsync(1);

		using HttpResponseMessage response = await EditAsync(1, criteria: new Dictionary<string, AddCustomLeaderboardCriteria>(StringComparer.Ordinal)
		{
			["enemiesKilledCriteria"] = new() { Operator = CustomLeaderboardCriteriaOperator.LessThan, Expression = "5" },
		});
		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "Cannot change criteria for custom leaderboard with scores.");
	}

	[Test]
	public async Task Edit_IsNotFoundForAnUnknownId()
	{
		using HttpResponseMessage response = await EditAsync(999);

		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.NotFound, "Custom leaderboard with ID '999' does not exist.");
	}

	[Test]
	public async Task Delete_RemovesALeaderboardWithoutScores_AndFreesTheSpawnset()
	{
		int spawnsetId = await AddSpawnsetAsync("Admin V3", _v3);
		using (HttpResponseMessage added = await AddAsync(spawnsetId))
			await Assert.That(added.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using (HttpResponseMessage deleted = await _client.DeleteAsync("api/admin/custom-leaderboards/1"))
			await Assert.That(deleted.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using HttpClient anonymous = App.CreateApiClient();
		using (HttpResponseMessage main = await anonymous.GetAsync("api/custom-leaderboards/1"))
			await Assert.That(main.StatusCode).IsEqualTo(HttpStatusCode.NotFound);

		// The spawnset could not be deleted while the leaderboard existed.
		using HttpResponseMessage spawnsetDeleted = await _client.DeleteAsync($"api/admin/spawnsets/{spawnsetId}");
		await Assert.That(spawnsetDeleted.StatusCode).IsEqualTo(HttpStatusCode.OK);
	}

	[Test]
	public async Task Delete_RejectsALeaderboardWithScores()
	{
		int spawnsetId = await AddSpawnsetAsync("Admin V3", _v3);
		using (HttpResponseMessage added = await AddAsync(spawnsetId))
			await Assert.That(added.StatusCode).IsEqualTo(HttpStatusCode.OK);

		await SeedEntryAsync(1);

		using HttpResponseMessage response = await _client.DeleteAsync("api/admin/custom-leaderboards/1");
		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "Custom leaderboard with scores cannot be deleted.");
	}
}
