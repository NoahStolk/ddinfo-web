using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using System.Net;
using System.Text.Json;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Main;

[NotInParallel(nameof(MainModsTests))]
internal sealed class MainModsTests : ApplicationTest
{
	private async Task SeedAsync()
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "Alice"));
			dbContext.Players.Add(EntityFixtures.Player(2, "Bob"));
		});

		await App.SeedAsync(dbContext =>
		{
			dbContext.Mods.Add(EntityFixtures.Mod("Visible"));

			ModEntity hidden = EntityFixtures.Mod("Hidden");
			hidden.IsHidden = true;
			dbContext.Mods.Add(hidden);
		});

		await App.SeedAsync(dbContext => dbContext.PlayerMods.Add(new PlayerModEntity { PlayerId = 1, ModId = 1 }));
	}

	[Test]
	public async Task GetMods_ExcludesHiddenMods()
	{
		await SeedAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/mods?onlyHosted=false&pageSize=15");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

		await Assert.That(document.RootElement.GetProperty("totalResults").GetInt32()).IsEqualTo(1);
		await Assert.That(document.RootElement.GetProperty("results")[0].GetProperty("name").GetString()).IsEqualTo("Visible");
	}

	[Test]
	public async Task GetMods_ReportsNotHostedWithoutAnArchiveOnDisk()
	{
		await SeedAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/mods?onlyHosted=false&pageSize=15");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		JsonElement mod = document.RootElement.GetProperty("results")[0];

		// No zip was written to the scratch data directory, so the mod is known but not hosted.
		await Assert.That(mod.GetProperty("isHosted").GetBoolean()).IsFalse();
		await Assert.That(mod.GetProperty("containsProhibitedAssets").ValueKind).IsEqualTo(JsonValueKind.Null);
	}

	[Test]
	public async Task GetMods_OnlyHosted_IsEmptyWithoutArchives()
	{
		await SeedAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/mods?onlyHosted=true&pageSize=15");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("totalResults").GetInt32()).IsEqualTo(0);
	}

	[Test]
	public async Task GetMods_ListsTheAuthorFromPlayerMods()
	{
		await SeedAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/mods?onlyHosted=false&pageSize=15");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		JsonElement authors = document.RootElement.GetProperty("results")[0].GetProperty("authors");

		await Assert.That(authors.GetArrayLength()).IsEqualTo(1);
		await Assert.That(authors[0].GetString()).IsEqualTo("Alice");
	}

	[Test]
	public async Task GetMods_FiltersByAuthorCaseInsensitively()
	{
		await SeedAsync();

		using HttpClient client = App.CreateApiClient();

		using (HttpResponseMessage match = await client.GetAsync("api/mods?onlyHosted=false&authorFilter=ALICE&pageSize=15"))
		{
			using JsonDocument document = JsonDocument.Parse(await match.Content.ReadAsStringAsync());
			await Assert.That(document.RootElement.GetProperty("totalResults").GetInt32()).IsEqualTo(1);
		}

		using HttpResponseMessage miss = await client.GetAsync("api/mods?onlyHosted=false&authorFilter=bob&pageSize=15");
		using JsonDocument missDocument = JsonDocument.Parse(await miss.Content.ReadAsStringAsync());
		await Assert.That(missDocument.RootElement.GetProperty("totalResults").GetInt32()).IsEqualTo(0);
	}

	[Test]
	public async Task GetModById_ReturnsAHiddenModToo()
	{
		await SeedAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/mods/2");

		// The list endpoint filters hidden mods, but a direct lookup does not.
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("name").GetString()).IsEqualTo("Hidden");
	}

	[Test]
	public async Task GetModById_IsNotFoundForAnUnknownId()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/mods/999");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}

	[Test]
	public async Task GetTotalModData_IncludesHiddenMods()
	{
		await SeedAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/mods/total-data");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

		// Deliberately counts both mods, unlike the paged list.
		await Assert.That(document.RootElement.GetProperty("count").GetInt32()).IsEqualTo(2);
	}

	[Test]
	public async Task GetModFile_IsNotFoundForAnUnknownMod()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/mods/Nonexistent/file");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}

	[Test]
	public async Task GetModFile_IsBadRequestWhenTheModExistsButTheArchiveDoesNot()
	{
		await SeedAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/mods/Visible/file");

		// A known mod with no zip on disk is reported differently from an unknown mod.
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
	}

	[Test]
	public async Task GetModsByAuthor_ListsTheirMods()
	{
		await SeedAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/mods/by-author?playerId=1");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetArrayLength()).IsEqualTo(1);
		await Assert.That(document.RootElement[0].GetProperty("name").GetString()).IsEqualTo("Visible");
	}

	[Test]
	public async Task GetModScreenshot_IsNotFoundWhenTheFileIsMissing()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/mod-screenshots?modName=Visible&fileName=shot.png");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}
}
