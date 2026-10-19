using DevilDaggersInfo.Web.ApiSpec.Admin.Mods;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Compression;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Ddae;

/// <summary>
/// The API used by Devil Daggers Asset Editor, a deprecated Windows tool that can no longer be updated.
/// </summary>
[NotInParallel(nameof(DdaeContractTests))]
internal sealed class DdaeContractTests : ApplicationTest
{
	private const string _api = "Ddae";

	/// <summary>
	/// Uploads three mods through the Admin API, so the archives and screenshots are written exactly as in production:
	/// "Hosted" with a binary and a screenshot, "NotHosted" without binaries, and "Hidden", which DDAE never lists.
	/// </summary>
	private async Task SeedAsync()
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "Alice"));
			dbContext.Players.Add(EntityFixtures.Player(2, "Bob"));
		});

		using HttpClient admin = App.CreateApiClient(await App.CreateJwtAsync("mod-admin", Roles.Mods));
		await AddModAsync(admin, "Hosted", isHidden: false, [1, 2], [ModFixtures.Binary("main")], new Dictionary<string, byte[]>(StringComparer.Ordinal) { ["first"] = ModFixtures.Png });
		await AddModAsync(admin, "NotHosted", isHidden: false, [2], [], []);
		await AddModAsync(admin, "Hidden", isHidden: true, [1], [ModFixtures.Binary("main")], []);

		// Adding a mod stamps the current time, which would make the snapshots differ on every run.
		await using AsyncServiceScope scope = App.CreateScope();
		await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Mods
			.ExecuteUpdateAsync(setters => setters.SetProperty(m => m.LastUpdated, new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
	}

	private static async Task AddModAsync(HttpClient admin, string name, bool isHidden, List<int> playerIds, List<BinaryData> binaries, Dictionary<string, byte[]> screenshots)
	{
		using HttpResponseMessage response = await admin.PostAsJsonAsync("api/admin/mods", new AddMod
		{
			Name = name,
			IsHidden = isHidden,
			TrailerUrl = null,
			HtmlDescription = null,
			ModTypes = null,
			Url = null,
			PlayerIds = playerIds,
			Binaries = binaries,
			Screenshots = screenshots,
		});
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await response.Content.ReadAsStringAsync());
	}

	/// <summary>
	/// The entries are game data from DevilDaggersInfo.Core and change when that package does, so only the shape is the
	/// contract: the six keys, including the legacy "ddModels" and "ddModelBindings" names DDAE expects, and the members of
	/// an entry. The snapshot keeps the first entry of each list, and every other entry is checked to have the same members.
	/// </summary>
	[Test]
	public async Task GetAssetInfo()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/ddae/assets/info");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
		await Assert.That(response.Content.Headers.ContentType?.MediaType).IsEqualTo("application/json");

		JsonObject assetInfo = JsonNode.Parse(await response.Content.ReadAsStringAsync())?.AsObject() ?? throw new InvalidOperationException("The response is not a JSON object.");
		JsonObject firstEntries = [];
		foreach ((string key, JsonNode? entries) in assetInfo)
		{
			JsonArray list = entries?.AsArray() ?? throw new InvalidOperationException($"'{key}' is not a list.");
			await Assert.That(list.Count).IsGreaterThan(0).Because($"'{key}' is empty.");

			foreach (JsonNode? entry in list)
			{
				string[] members = [.. (entry?.AsObject() ?? []).Select(kvp => kvp.Key)];
				await Assert.That(members).IsEquivalentTo(["name", "description", "tags"], CollectionOrdering.Matching).Because($"An entry in '{key}' has different members.");
			}

			firstEntries[key] = list[0]?.DeepClone();
		}

		await ContractSnapshot.AssertJsonMatchesAsync(firstEntries.ToJsonString(), _api, nameof(GetAssetInfo));
	}

	[Test]
	public async Task GetMods()
	{
		await SeedAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/ddae/mods");

		await ContractSnapshot.AssertMatchesAsync(response, _api, nameof(GetMods));
	}

	/// <summary>
	/// The filter applies whenever it is passed, so "false" also lists only hosted mods. DDAE cannot be updated, so this
	/// is pinned as it is rather than changed.
	/// </summary>
	[Test]
	[Arguments(true)]
	[Arguments(false)]
	public async Task GetMods_WithIsHostedFilter_ListsOnlyHostedMods(bool isHostedFilter)
	{
		await SeedAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync($"api/ddae/mods?isHostedFilter={isHostedFilter}");

		await ContractSnapshot.AssertMatchesAsync(response, _api, nameof(GetMods_WithIsHostedFilter_ListsOnlyHostedMods));
	}

	[Test]
	public async Task GetMods_FilteredByAuthorAndName()
	{
		await SeedAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/ddae/mods?authorFilter=BOB&nameFilter=not");

		await ContractSnapshot.AssertMatchesAsync(response, _api, nameof(GetMods_FilteredByAuthorAndName));
	}

	[Test]
	public async Task GetModFile_ServesTheArchiveAsAZip()
	{
		await SeedAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/ddae/mods/Hosted/file");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
		await Assert.That(response.Content.Headers.ContentType?.MediaType).IsEqualTo("application/zip");
		await Assert.That(response.Content.Headers.ContentDisposition?.FileName).IsEqualTo("Hosted.zip");

		await using Stream zipStream = await response.Content.ReadAsStreamAsync();
		await using ZipArchive archive = new(zipStream, ZipArchiveMode.Read);
		await Assert.That(archive.Entries.Select(e => e.Name)).IsEquivalentTo(["dd-Hosted-main"], CollectionOrdering.Matching);
	}

	[Test]
	public async Task GetModFile_IsNotFoundForAnUnknownMod()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/ddae/mods/Unknown/file");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}
}
