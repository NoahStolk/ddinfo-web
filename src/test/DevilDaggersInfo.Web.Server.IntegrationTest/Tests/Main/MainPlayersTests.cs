using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Main;

[NotInParallel(nameof(MainPlayersTests))]
internal sealed class MainPlayersTests : ApplicationTest
{
	[Test]
	public async Task GetPlayersForLeaderboard_IncludesBannedPlayers()
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "Clean"));

			PlayerEntity cheater = EntityFixtures.Player(2, "Cheater");
			cheater.BanType = BanType.Cheater;
			cheater.BanDescription = "Caught";
			dbContext.Players.Add(cheater);
		});

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/players/leaderboard");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

		// Banned players are deliberately returned so the client can strike them through.
		await Assert.That(document.RootElement.GetArrayLength()).IsEqualTo(2);

		JsonElement cheaterElement = document.RootElement.EnumerateArray().Single(e => e.GetProperty("id").GetInt32() == 2);
		await Assert.That(cheaterElement.GetProperty("banType").GetInt32()).IsEqualTo((int)BanType.Cheater);
		await Assert.That(cheaterElement.GetProperty("banDescription").GetString()).IsEqualTo("Caught");
	}

	[Test]
	public async Task GetPlayersForSettings_OnlyListsUnbannedPlayersWhoShareSettings()
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.PlayerWithSettings(1, "Sharing"));

			// Has settings but chose to hide them.
			PlayerEntity hidden = EntityFixtures.PlayerWithSettings(2, "Hiding");
			hidden.HideSettings = true;
			dbContext.Players.Add(hidden);

			// Has settings but is banned.
			PlayerEntity banned = EntityFixtures.PlayerWithSettings(3, "Banned");
			banned.BanType = BanType.Cheater;
			dbContext.Players.Add(banned);

			// Unbanned and not hiding, but has no settings to show.
			dbContext.Players.Add(EntityFixtures.Player(4, "NoSettings"));
		});

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/players/settings");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

		await Assert.That(document.RootElement.GetArrayLength()).IsEqualTo(1);
		await Assert.That(document.RootElement[0].GetProperty("id").GetInt32()).IsEqualTo(1);

		// Edpi is a computed property: Dpi * InGameSens.
		await Assert.That(document.RootElement[0].GetProperty("settings").GetProperty("edpi").GetDouble()).IsEqualTo(1200d);
	}

	[Test]
	public async Task GetPlayerById_ReturnsThePlayer()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.PlayerWithSettings(1, "Someone")));

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/players/1");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		JsonElement root = document.RootElement;

		await Assert.That(root.GetProperty("id").GetInt32()).IsEqualTo(1);
		await Assert.That(root.GetProperty("isBanned").GetBoolean()).IsFalse();
		await Assert.That(root.GetProperty("isPublicDonor").GetBoolean()).IsFalse();
		await Assert.That(root.GetProperty("settings").ValueKind).IsNotEqualTo(JsonValueKind.Null);
	}

	[Test]
	public async Task GetPlayerById_IsNotFoundForAnUnknownPlayer()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/players/999");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}

	[Test]
	public async Task GetPlayerById_ReportsPublicDonorStatus()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(1, "Donor")));
		await App.SeedAsync(dbContext => dbContext.Donations.Add(EntityFixtures.Donation(playerId: 1, amountInEurCents: 500)));

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/players/1");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("isPublicDonor").GetBoolean()).IsTrue();
	}

	[Test]
	public async Task GetPlayerById_DoesNotReportADonorWhoHidesDonations()
	{
		await App.SeedAsync(dbContext =>
		{
			PlayerEntity player = EntityFixtures.Player(1, "Shy");
			player.HideDonations = true;
			dbContext.Players.Add(player);
		});
		await App.SeedAsync(dbContext => dbContext.Donations.Add(EntityFixtures.Donation(playerId: 1, amountInEurCents: 500)));

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/players/1");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("isPublicDonor").GetBoolean()).IsFalse();
	}

	[Test]
	public async Task GetPlayerById_MarksABannedPlayerAsBanned()
	{
		await App.SeedAsync(dbContext =>
		{
			PlayerEntity banned = EntityFixtures.Player(1, "Banned");
			banned.BanType = BanType.Cheater;
			dbContext.Players.Add(banned);
		});

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/players/1");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("isBanned").GetBoolean()).IsTrue();
	}

	[Test]
	public async Task GetPlayerHistory_IsEmptyWithoutHistoryFiles()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(1, "Someone")));

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/players/1/history");

		// The repository uses TryGetFiles, so an empty history directory degrades to empty lists rather than failing.
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("scoreHistory").GetArrayLength()).IsEqualTo(0);
		await Assert.That(document.RootElement.GetProperty("rankHistory").GetArrayLength()).IsEqualTo(0);
	}

	[Test]
	[Arguments(0)]
	[Arguments(-1)]
	public async Task GetPlayerHistory_RejectsANonPositiveId(int id)
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync($"api/players/{id}/history");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
	}

	[Test]
	public async Task GetProfile_RequiresAuthentication()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/players/1/profile");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.Unauthorized);
	}

	[Test]
	public async Task GetProfile_ReturnsTheOwnProfileOfTheLinkedPlayer()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.PlayerWithSettings(1, "Owner")));

		string jwt = await CreateJwtForLinkedUserAsync("owner", playerId: 1);

		using HttpClient client = App.CreateApiClient(jwt);
		using HttpResponseMessage response = await client.GetAsync("api/players/1/profile");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("dpi").GetInt32()).IsEqualTo(800);
		await Assert.That(document.RootElement.GetProperty("fov").GetInt32()).IsEqualTo(105);
	}

	[Test]
	public async Task GetProfile_ForbidsReadingAnotherPlayersProfile()
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.PlayerWithSettings(1, "Owner"));
			dbContext.Players.Add(EntityFixtures.PlayerWithSettings(2, "Other"));
		});

		string jwt = await CreateJwtForLinkedUserAsync("owner", playerId: 1);

		using HttpClient client = App.CreateApiClient(jwt);
		using HttpResponseMessage response = await client.GetAsync("api/players/2/profile");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.Forbidden);
	}

	[Test]
	public async Task GetProfile_RejectsAUserNotLinkedToAPlayer()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.PlayerWithSettings(1, "Owner")));

		// A user account with no PlayerId cannot have a profile.
		string jwt = await App.CreateJwtAsync("unlinked");

		using HttpClient client = App.CreateApiClient(jwt);
		using HttpResponseMessage response = await client.GetAsync("api/players/1/profile");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("title").GetString()).IsEqualTo("User is not linked to a player.");
	}

	[Test]
	public async Task GetProfile_RejectsABannedPlayer()
	{
		await App.SeedAsync(dbContext =>
		{
			PlayerEntity banned = EntityFixtures.PlayerWithSettings(1, "Banned");
			banned.BanType = BanType.Cheater;
			dbContext.Players.Add(banned);
		});

		string jwt = await CreateJwtForLinkedUserAsync("owner", playerId: 1);

		using HttpClient client = App.CreateApiClient(jwt);
		using HttpResponseMessage response = await client.GetAsync("api/players/1/profile");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("title").GetString()).IsEqualTo("Player is banned.");
	}

	[Test]
	public async Task UpdateProfile_PersistsTheNewSettings()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.PlayerWithSettings(1, "Owner")));

		string jwt = await CreateJwtForLinkedUserAsync("owner", playerId: 1);

		using HttpClient client = App.CreateApiClient(jwt);
		using HttpResponseMessage response = await client.PutAsJsonAsync("api/players/1/profile", new
		{
			countryCode = "nl",
			dpi = 1600,
			inGameSens = 1.2f,
			fov = 110,
			isRightHanded = false,
			hasFlashHandEnabled = true,
			gamma = 2f,
			usesLegacyAudio = false,
			usesHrtf = true,
			usesInvertY = false,
			verticalSync = 1,
			hideSettings = false,
			hideDonations = false,
			hidePastUsernames = false,
		});

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using HttpResponseMessage readBack = await client.GetAsync("api/players/1/profile");
		using JsonDocument document = JsonDocument.Parse(await readBack.Content.ReadAsStringAsync());

		await Assert.That(document.RootElement.GetProperty("dpi").GetInt32()).IsEqualTo(1600);
		await Assert.That(document.RootElement.GetProperty("fov").GetInt32()).IsEqualTo(110);
		await Assert.That(document.RootElement.GetProperty("countryCode").GetString()).IsEqualTo("nl");
	}

	[Test]
	[Arguments(5, "DPI must be between 10 and 20,000.")]
	[Arguments(30000, "DPI must be between 10 and 20,000.")]
	public async Task UpdateProfile_RejectsAnOutOfRangeDpi(int dpi, string expectedMessage)
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.PlayerWithSettings(1, "Owner")));

		string jwt = await CreateJwtForLinkedUserAsync("owner", playerId: 1);

		using HttpClient client = App.CreateApiClient(jwt);
		using HttpResponseMessage response = await client.PutAsJsonAsync("api/players/1/profile", new
		{
			countryCode = (string?)null,
			dpi,
			inGameSens = (float?)null,
			fov = (int?)null,
			isRightHanded = (bool?)null,
			hasFlashHandEnabled = (bool?)null,
			gamma = (float?)null,
			usesLegacyAudio = (bool?)null,
			usesHrtf = (bool?)null,
			usesInvertY = (bool?)null,
			verticalSync = 0,
			hideSettings = false,
			hideDonations = false,
			hidePastUsernames = false,
		});

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

		string body = await response.Content.ReadAsStringAsync();
		await Assert.That(body).Contains(expectedMessage);
	}

	/// <summary>
	/// Creates a user linked to <paramref name="playerId"/> and returns a JWT for it. The profile endpoints resolve the
	/// caller by the user's name claim and then check that the user owns the requested player.
	/// </summary>
	private async Task<string> CreateJwtForLinkedUserAsync(string userName, int playerId)
	{
		string jwt = await App.CreateJwtAsync(userName);

		await App.SeedAsync(dbContext =>
		{
			UserEntity user = dbContext.Users.Single(u => u.Name == userName);
			user.PlayerId = playerId;
		});

		return jwt;
	}
}
