using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using System.Net;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Ddse;

/// <summary>
/// The API used by Devil Daggers Survival Editor, a deprecated Windows tool that can no longer be updated.
/// </summary>
[NotInParallel(nameof(DdseContractTests))]
internal sealed class DdseContractTests : ApplicationTest
{
	private const string _api = "Ddse";

	/// <summary>
	/// Spawnset 1 is V3 by Alice, with a custom leaderboard. Spawnset 2 is a placeholder by Bob, without one.
	/// </summary>
	private async Task SeedAsync()
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "Alice"));
			dbContext.Players.Add(EntityFixtures.Player(2, "Bob"));
		});

		// Separate saves: V3 takes its explicit ID first, and the placeholder then gets the next auto-increment value.
		await App.SeedAsync(dbContext => dbContext.Spawnsets.Add(CustomLeaderboardFixtures.Spawnset(playerId: 1)));
		await App.SeedAsync(dbContext => dbContext.Spawnsets.Add(EntityFixtures.Spawnset("Placeholder", playerId: 2)));

		await App.SeedAsync(dbContext => dbContext.CustomLeaderboards.Add(CustomLeaderboardFixtures.CustomLeaderboard(spawnsetId: 1)));
	}

	[Test]
	public async Task GetSpawnsets()
	{
		await SeedAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/ddse/spawnsets");

		await ContractSnapshot.AssertMatchesAsync(response, _api, nameof(GetSpawnsets));
	}

	[Test]
	public async Task GetSpawnsets_FilteredByAuthorAndName()
	{
		await SeedAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/ddse/spawnsets?authorFilter=bob&nameFilter=place");

		await ContractSnapshot.AssertMatchesAsync(response, _api, nameof(GetSpawnsets_FilteredByAuthorAndName));
	}

	[Test]
	public async Task GetSpawnsetFile_ServesTheRawFileNamedAfterTheSpawnset()
	{
		await SeedAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/ddse/spawnsets/1/file");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
		await Assert.That(response.Content.Headers.ContentType?.MediaType).IsEqualTo("application/octet-stream");
		await Assert.That(response.Content.Headers.ContentDisposition?.FileName).IsEqualTo("V3");
		await Assert.That(await response.Content.ReadAsByteArrayAsync()).IsEquivalentTo(CustomLeaderboardFixtures.V3SpawnsetFile, CollectionOrdering.Matching);
	}

	[Test]
	public async Task GetSpawnsetFile_IsNotFoundForAnUnknownId()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/ddse/spawnsets/999/file");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}
}
