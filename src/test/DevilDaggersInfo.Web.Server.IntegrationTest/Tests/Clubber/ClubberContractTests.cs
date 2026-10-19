using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Clubber;

/// <summary>
/// The API used by Clubber, the Discord bot of the Devil Daggers server.
/// </summary>
[NotInParallel(nameof(ClubberContractTests))]
internal sealed class ClubberContractTests : ApplicationTest
{
	private const string _api = "Clubber";

	[Test]
	public async Task GetPlayerCountryCode()
	{
		await App.SeedAsync(dbContext =>
		{
			PlayerEntity player = EntityFixtures.Player(1, "Dutch player");
			player.CountryCode = "nl";
			dbContext.Players.Add(player);
		});

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/clubber/players/1/country-code");

		await ContractSnapshot.AssertMatchesAsync(response, _api, nameof(GetPlayerCountryCode));
	}

	[Test]
	public async Task GetPlayerCountryCode_ForAPlayerWithoutOne()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(1, "Anywhere")));

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/clubber/players/1/country-code");

		await ContractSnapshot.AssertMatchesAsync(response, _api, nameof(GetPlayerCountryCode_ForAPlayerWithoutOne));
	}

	[Test]
	public async Task GetPlayerHistory()
	{
		await LeaderboardHistoryFixtures.SeedAsync(App);

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/clubber/players/1/history");

		await ContractSnapshot.AssertMatchesAsync(response, _api, nameof(GetPlayerHistory));
	}
}
