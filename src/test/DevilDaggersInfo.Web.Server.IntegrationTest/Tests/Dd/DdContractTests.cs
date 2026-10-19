using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using System.Net;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Dd;

/// <summary>
/// The API the game itself calls. Shipped game builds cannot be updated, so this route and response must never change.
/// </summary>
[NotInParallel(nameof(DdContractTests))]
internal sealed class DdContractTests : ApplicationTest
{
	private const string _api = "Dd";

	private static string V3HashQuery => Uri.EscapeDataString(Convert.ToBase64String(MD5.HashData(CustomLeaderboardFixtures.V3SpawnsetFile)));

	[Test]
	public async Task GetSpawnsetNameByHash()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(1, "Author")));
		await App.SeedAsync(dbContext => dbContext.Spawnsets.Add(CustomLeaderboardFixtures.Spawnset(playerId: 1)));

		using HttpClient client = App.CreateApiClient();

		// The route is absolute, outside the controller's api/dd prefix.
		using HttpResponseMessage response = await client.GetAsync($"api/spawnsets/name-by-hash?hash={V3HashQuery}");

		await ContractSnapshot.AssertMatchesAsync(response, _api, nameof(GetSpawnsetNameByHash));
	}

	[Test]
	public async Task GetSpawnsetNameByHash_IsNotFoundForAnUnknownHash()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync($"api/spawnsets/name-by-hash?hash={V3HashQuery}");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}
}
