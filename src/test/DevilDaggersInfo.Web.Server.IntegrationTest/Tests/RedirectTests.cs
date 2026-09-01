using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using System.Net;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests;

/// <summary>
/// The legacy URL redirects configured in Program.cs. These are the site's contract with links published elsewhere on
/// the internet, and nothing else exercises them.
/// </summary>
[NotInParallel(nameof(RedirectTests))]
internal sealed class RedirectTests
{
	[ClassDataSource<MySqlFixture>(Shared = SharedType.PerTestSession)]
	public required MySqlFixture MySql { get; init; }

	[Test]
	[Arguments("Home/Index", "/")]
	[Arguments("Home", "/")]
	[Arguments("Home/Leaderboard", "/leaderboard")]
	[Arguments("Home/Spawnsets", "/custom/spawnsets")]
	[Arguments("Home/Donations", "/donations")]
	[Arguments("Home/Hands", "/wiki/upgrades")]
	[Arguments("Home/Enemies", "/wiki/enemies")]
	[Arguments("Daggers", "/wiki/daggers")]
	[Arguments("Enemies", "/wiki/enemies")]
	[Arguments("Spawns", "/wiki/spawns")]
	[Arguments("Upgrades", "/wiki/upgrades")]
	[Arguments("Wiki/SpawnsetGuide", "/guides/creating-spawnsets")]
	[Arguments("Wiki/AssetGuide", "/guides/creating-mods")]
	[Arguments("Leaderboard/PlayerSettings", "/leaderboard/player-settings")]
	[Arguments("Leaderboard/WorldRecordProgression", "/leaderboard/world-record-progression")]
	[Arguments("CustomLeaderboards", "/custom/leaderboards")]
	[Arguments("Mods", "/custom/mods")]
	[Arguments("Spawnsets", "/custom/spawnsets")]
	[Arguments("Tools/DevilDaggersAssetEditor", "/tools/asset-editor")]
	[Arguments("Tools/DevilDaggersCustomLeaderboards", "/tools/custom-leaderboards")]
	[Arguments("Tools/DevilDaggersSurvivalEditor", "/tools/survival-editor")]
	[Arguments("Wiki/Guides/SurvivalEditor", "/guides/creating-spawnsets")]
	[Arguments("Wiki/Guides/AssetEditor", "/guides/creating-mods")]
	[Arguments("guides/survival-editor", "/guides/creating-spawnsets")]
	[Arguments("guides/asset-editor", "/guides/creating-mods")]
	public async Task LegacyUrl_RedirectsToCurrentLocation(string requested, string expectedLocation)
	{
		TestApplication app = await MySql.GetApplicationAsync(nameof(RedirectTests));

		using HttpClient client = app.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync(requested);

		// AddRedirect emits a root-relative Location, so the expectations carry a leading slash even though the
		// replacement patterns in Program.cs do not.
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.Found);
		await Assert.That(response.Headers.Location?.OriginalString).IsEqualTo(expectedLocation);
	}

	[Test]
	public async Task UnknownPath_FallsThroughToTheBlazorHostPage()
	{
		TestApplication app = await MySql.GetApplicationAsync(nameof(RedirectTests));

		using HttpClient client = app.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("some/path/that/does/not/exist");

		// MapFallbackToPage("/_Host") serves the client shell for anything the server does not route itself.
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
		await Assert.That(response.Content.Headers.ContentType?.MediaType).IsEqualTo("text/html");
	}
}
