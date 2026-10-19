using DevilDaggersInfo.Web.ApiSpec.Admin.Players;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AdminBanType = DevilDaggersInfo.Web.ApiSpec.Admin.Players.BanType;
using AdminVerticalSync = DevilDaggersInfo.Web.ApiSpec.Admin.Players.VerticalSync;
using EntityBanType = DevilDaggersInfo.Web.Server.Domain.Entities.Enums.BanType;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Admin;

/// <summary>
/// Players created, edited and banned through the Admin API. Banning is checked against the history-based endpoints
/// it is meant to affect: world records and other players' ranks.
/// </summary>
[NotInParallel(nameof(AdminPlayersTests))]
internal sealed class AdminPlayersTests : ApplicationTest
{
	// ! Assigned by the before-test hook, which TUnit runs before every test method.
	private HttpClient _client = null!;

	[Before(HookType.Test)]
	public async Task CreateAdminClientAsync()
	{
		_client = App.CreateApiClient(await App.CreateJwtAsync("player-admin", Roles.Players));
	}

	[After(HookType.Test)]
	public void DisposeAdminClient()
	{
		_client.Dispose();
	}

	private static AddPlayer CreateAddPlayer(int id, AdminBanType banType = AdminBanType.NotBanned)
	{
		return new AddPlayer
		{
			Id = id,
			CommonName = null,
			DiscordUserId = null,
			CountryCode = null,
			Dpi = null,
			InGameSens = null,
			Fov = null,
			IsRightHanded = null,
			HasFlashHandEnabled = null,
			Gamma = null,
			UsesLegacyAudio = null,
			UsesHrtf = null,
			UsesInvertY = null,
			VerticalSync = AdminVerticalSync.Unknown,
			BanType = banType,
			BanDescription = null,
			BanResponsibleId = null,
			IsBannedFromDdcl = false,
			HideSettings = false,
			HideDonations = false,
			HidePastUsernames = false,
			ModIds = null,
		};
	}

	private static EditPlayer CreateEditPlayer(AdminBanType banType = AdminBanType.NotBanned, List<int>? modIds = null)
	{
		return new EditPlayer
		{
			CommonName = null,
			DiscordUserId = null,
			CountryCode = null,
			Dpi = null,
			InGameSens = null,
			Fov = null,
			IsRightHanded = null,
			HasFlashHandEnabled = null,
			Gamma = null,
			UsesLegacyAudio = null,
			UsesHrtf = null,
			UsesInvertY = null,
			VerticalSync = AdminVerticalSync.Unknown,
			BanType = banType,
			BanDescription = null,
			BanResponsibleId = null,
			IsBannedFromDdcl = false,
			HideSettings = false,
			HideDonations = false,
			HidePastUsernames = false,
			ModIds = modIds,
		};
	}

	private async Task<string> GetStoredNameAsync(int id)
	{
		await using AsyncServiceScope scope = App.CreateScope();
		return await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Players.Where(p => p.Id == id).Select(p => p.PlayerName).SingleAsync();
	}

	private async Task<int[]> GetWorldRecordHolderIdsAsync()
	{
		using HttpClient anonymous = App.CreateApiClient();
		using HttpResponseMessage response = await anonymous.GetAsync("api/world-records");
		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		return [.. document.RootElement.GetProperty("worldRecordHolders").EnumerateArray().Select(h => h.GetProperty("id").GetInt32()).Order()];
	}

	[Test]
	public async Task Add_TakesTheNameFromTheOfficialLeaderboard_AndIsServedByMain()
	{
		App.DdLeaderboardService.GetEntryById(5).Returns(new IDdLeaderboardService.EntryResponse
		{
			Rank = 1,
			Id = 5,
			Username = "Official name",
			Time = 0,
			Kills = 0,
			Gems = 0,
			DeathType = 0,
			DaggersHit = 0,
			DaggersFired = 0,
			TimeTotal = 0,
			KillsTotal = 0,
			GemsTotal = 0,
			DeathsTotal = 0,
			DaggersHitTotal = 0,
			DaggersFiredTotal = 0,
		});

		using (HttpResponseMessage added = await _client.PostAsJsonAsync("api/admin/players", CreateAddPlayer(5) with { CountryCode = "nl", Dpi = 800, InGameSens = 1.5f }))
			await Assert.That(added.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await added.Content.ReadAsStringAsync());

		await Assert.That(await GetStoredNameAsync(5)).IsEqualTo("Official name");

		using HttpClient anonymous = App.CreateApiClient();
		using HttpResponseMessage main = await anonymous.GetAsync("api/players/5");
		await Assert.That(main.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await main.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("countryCode").GetString()).IsEqualTo("nl");
		await Assert.That(document.RootElement.GetProperty("settings").GetProperty("dpi").GetInt32()).IsEqualTo(800);
	}

	[Test]
	public async Task Add_StoresAnEmptyNameWhenTheOfficialLeaderboardIsUnavailable()
	{
		App.DdLeaderboardService.GetEntryById(5).Returns<IDdLeaderboardService.EntryResponse>(_ => throw new DdLeaderboardException("Unavailable."));

		using (HttpResponseMessage added = await _client.PostAsJsonAsync("api/admin/players", CreateAddPlayer(5)))
			await Assert.That(added.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await added.Content.ReadAsStringAsync());

		await Assert.That(await GetStoredNameAsync(5)).IsEqualTo(string.Empty);
	}

	[Test]
	public async Task Add_RejectsAnExistingId()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(5, "Existing")));

		using HttpResponseMessage response = await _client.PostAsJsonAsync("api/admin/players", CreateAddPlayer(5));
		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "Player with ID '5' already exists.");
	}

	[Test]
	public async Task Add_RejectsSettingsOnABannedPlayer()
	{
		using HttpResponseMessage response = await _client.PostAsJsonAsync("api/admin/players", CreateAddPlayer(5, AdminBanType.Cheater) with { Dpi = 800 });

		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "Banned players must not have settings.");
	}

	[Test]
	public async Task Add_RejectsACountryCodeOnABannedPlayer()
	{
		using HttpResponseMessage response = await _client.PostAsJsonAsync("api/admin/players", CreateAddPlayer(5, AdminBanType.Cheater) with { CountryCode = "nl" });

		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "Banned players must not have a country code.");
	}

	[Test]
	public async Task Add_RejectsABanDescriptionOnAnUnbannedPlayer()
	{
		using HttpResponseMessage response = await _client.PostAsJsonAsync("api/admin/players", CreateAddPlayer(5) with { BanDescription = "Not banned" });

		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "BanDescription must only be used for banned players.");
	}

	[Test]
	public async Task Add_RejectsAnUndefinedBanType()
	{
		using HttpResponseMessage response = await _client.PostAsJsonAsync("api/admin/players", CreateAddPlayer(5, (AdminBanType)99));

		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "is not a valid ban type");
	}

	[Test]
	public async Task Edit_BanningACheater_RemovesThemFromWorldRecordsAndOtherPlayersRanks()
	{
		await LeaderboardHistoryFixtures.SeedAsync(App, cheaterBanType: EntityBanType.NotBanned);

		// Unbanned, the cheater holds the world record on two of the four days.
		await Assert.That(await GetWorldRecordHolderIdsAsync()).IsEquivalentTo([1, LeaderboardHistoryFixtures.CheaterId], CollectionOrdering.Matching);

		using (HttpResponseMessage banned = await _client.PutAsJsonAsync($"api/admin/players/{LeaderboardHistoryFixtures.CheaterId}", CreateEditPlayer(AdminBanType.Cheater)))
			await Assert.That(banned.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await banned.Content.ReadAsStringAsync());

		await Assert.That(await GetWorldRecordHolderIdsAsync()).IsEquivalentTo([1], CollectionOrdering.Matching);

		// Player 3 was fourth behind the cheater on the only day they appear; with the cheater banned they are third.
		using HttpClient anonymous = App.CreateApiClient();
		using HttpResponseMessage history = await anonymous.GetAsync("api/players/3/history");
		using JsonDocument document = JsonDocument.Parse(await history.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("bestRank").GetInt32()).IsEqualTo(3);
	}

	/// <summary>
	/// An alt account's world records belong to the player's main account, so WorldRecordRepository deliberately keeps
	/// them. Only the player history excludes alts.
	/// </summary>
	[Test]
	public async Task Edit_BanningAnAlt_KeepsTheirWorldRecords()
	{
		await LeaderboardHistoryFixtures.SeedAsync(App, cheaterBanType: EntityBanType.NotBanned);

		using (HttpResponseMessage banned = await _client.PutAsJsonAsync($"api/admin/players/{LeaderboardHistoryFixtures.CheaterId}", CreateEditPlayer(AdminBanType.Alt)))
			await Assert.That(banned.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await banned.Content.ReadAsStringAsync());

		await Assert.That(await GetWorldRecordHolderIdsAsync()).IsEquivalentTo([1, LeaderboardHistoryFixtures.CheaterId], CollectionOrdering.Matching);
	}

	[Test]
	public async Task Edit_LinksAndUnlinksMods()
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "Modder"));
			dbContext.Mods.Add(EntityFixtures.Mod("Linked mod"));
		});

		using HttpClient anonymous = App.CreateApiClient();

		using (HttpResponseMessage linked = await _client.PutAsJsonAsync("api/admin/players/1", CreateEditPlayer(modIds: [1])))
			await Assert.That(linked.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await linked.Content.ReadAsStringAsync());

		using (HttpResponseMessage byAuthor = await anonymous.GetAsync("api/mods/by-author?playerId=1"))
			await Assert.That(await byAuthor.Content.ReadAsStringAsync()).Contains("Linked mod");

		using (HttpResponseMessage unlinked = await _client.PutAsJsonAsync("api/admin/players/1", CreateEditPlayer(modIds: [])))
			await Assert.That(unlinked.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using HttpResponseMessage afterUnlink = await anonymous.GetAsync("api/mods/by-author?playerId=1");
		await Assert.That(await afterUnlink.Content.ReadAsStringAsync()).DoesNotContain("Linked mod");
	}

	[Test]
	public async Task Edit_IsNotFoundForAnUnknownId()
	{
		using HttpResponseMessage response = await _client.PutAsJsonAsync("api/admin/players/999", CreateEditPlayer());

		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.NotFound, "Player with ID '999' does not exist.");
	}

	[Test]
	public async Task Delete_RemovesAPlayerWithoutData()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(1, "Doomed")));

		using (HttpResponseMessage deleted = await _client.DeleteAsync("api/admin/players/1"))
			await Assert.That(deleted.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using HttpClient anonymous = App.CreateApiClient();
		using HttpResponseMessage main = await anonymous.GetAsync("api/players/1");
		await Assert.That(main.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}

	[Test]
	public async Task Delete_RejectsAPlayerWithDonations()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(1, "Donor")));
		await App.SeedAsync(dbContext => dbContext.Donations.Add(EntityFixtures.Donation(playerId: 1, amountInEurCents: 500)));

		using HttpResponseMessage response = await _client.DeleteAsync("api/admin/players/1");
		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "Player with donations cannot be deleted.");
	}

	[Test]
	public async Task Delete_RejectsAPlayerWithSpawnsets()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(1, "Author")));
		await App.SeedAsync(dbContext => dbContext.Spawnsets.Add(EntityFixtures.Spawnset("Theirs", playerId: 1)));

		using HttpResponseMessage response = await _client.DeleteAsync("api/admin/players/1");
		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "Player with spawnsets cannot be deleted.");
	}

	/// <summary>
	/// Users.PlayerId is an optional foreign key without a cascade, so the database refuses to delete a player that a
	/// user account is linked to. The service checks scores, donations, mods and spawnsets, but not this.
	/// </summary>
	[Test]
	public async Task Delete_RejectsAPlayerLinkedToAUser()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(1, "Linked")));
		await App.SeedAsync(dbContext => dbContext.Users.Add(EntityFixtures.User("linked-user", playerId: 1)));

		using HttpResponseMessage response = await _client.DeleteAsync("api/admin/players/1");
		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "linked");
	}
}
