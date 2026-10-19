using DevilDaggersInfo.Web.ApiSpec.Admin;
using DevilDaggersInfo.Web.ApiSpec.Admin.Spawnsets;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Admin;

/// <summary>
/// Spawnsets written through the Admin API, read back through the APIs that serve them: Main (the website), Ddse
/// (Devil Daggers Survival Editor) and Tools (ddinfo-tools).
/// </summary>
[NotInParallel(nameof(AdminSpawnsetsTests))]
internal sealed class AdminSpawnsetsTests : ApplicationTest
{
	private static readonly byte[] _v3 = CustomLeaderboardFixtures.V3SpawnsetFile;

	// A second valid spawnset whose hash differs from V3's.
	private static readonly byte[] _default = SpawnsetBinary.CreateDefault().ToBytes();

	private async Task<HttpClient> CreateAdminClientAsync()
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "Author"));
			dbContext.Players.Add(EntityFixtures.Player(2, "Other author"));
		});

		return App.CreateApiClient(await App.CreateJwtAsync("spawnset-admin", Roles.Spawnsets));
	}

	private static async Task<HttpResponseMessage> AddAsync(HttpClient client, string name, byte[] file, int playerId = 1)
	{
		return await client.PostAsJsonAsync("api/admin/spawnsets", new AddSpawnset
		{
			PlayerId = playerId,
			Name = name,
			MaxDisplayWaves = 10,
			HtmlDescription = "<p>Description</p>",
			FileContents = file,
		});
	}

	private static async Task<HttpResponseMessage> EditAsync(HttpClient client, int id, string name, int playerId = 1)
	{
		return await client.PutAsJsonAsync($"api/admin/spawnsets/{id}", new EditSpawnset
		{
			PlayerId = playerId,
			Name = name,
			MaxDisplayWaves = 20,
			HtmlDescription = null,
		});
	}

	[Test]
	public async Task Add_IsServedByMainDdseAndTools()
	{
		using HttpClient client = await CreateAdminClientAsync();
		using HttpResponseMessage response = await AddAsync(client, "Admin V3", _v3);
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using HttpClient anonymous = App.CreateApiClient();

		using (HttpResponseMessage main = await anonymous.GetAsync($"api/spawnsets/by-hash?hash={Uri.EscapeDataString(Convert.ToBase64String(MD5.HashData(_v3)))}"))
		{
			await Assert.That(main.StatusCode).IsEqualTo(HttpStatusCode.OK);
			using JsonDocument document = JsonDocument.Parse(await main.Content.ReadAsStringAsync());
			await Assert.That(document.RootElement.GetProperty("name").GetString()).IsEqualTo("Admin V3");
			await Assert.That(document.RootElement.GetProperty("authorName").GetString()).IsEqualTo("Author");
		}

		using (HttpResponseMessage ddse = await anonymous.GetAsync("api/ddse/spawnsets"))
		{
			await Assert.That(ddse.StatusCode).IsEqualTo(HttpStatusCode.OK);
			await Assert.That(await ddse.Content.ReadAsStringAsync()).Contains("Admin V3");
		}

		using (HttpResponseMessage tools = await anonymous.GetAsync("api/app/spawnsets/1/buffer"))
		{
			await Assert.That(tools.StatusCode).IsEqualTo(HttpStatusCode.OK);
			using JsonDocument document = JsonDocument.Parse(await tools.Content.ReadAsStringAsync());
			await Assert.That(document.RootElement.GetProperty("data").GetBytesFromBase64()).IsEquivalentTo(_v3, CollectionOrdering.Matching);
		}
	}

	[Test]
	public async Task Add_DerivesTheStoredColumnsFromTheBinary()
	{
		using HttpClient client = await CreateAdminClientAsync();
		using HttpResponseMessage response = await AddAsync(client, "Admin V3", _v3);
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		if (!SpawnsetBinary.TryParse(_v3, out SpawnsetBinary? binary))
			throw new InvalidOperationException("V3 could not be parsed.");

		await using AsyncServiceScope scope = App.CreateScope();
		SpawnsetEntity spawnset = await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Spawnsets.SingleAsync();

		await Assert.That(spawnset.Md5Hash).IsEquivalentTo(MD5.HashData(_v3), CollectionOrdering.Matching);
		await Assert.That(spawnset.GameMode).IsEqualTo((SpawnsetGameMode)binary.GameMode);
		await Assert.That(spawnset.SpawnVersion).IsEqualTo(binary.SpawnVersion);
		await Assert.That(spawnset.WorldVersion).IsEqualTo(binary.WorldVersion);
		await Assert.That(spawnset.MaxDisplayWaves).IsEqualTo(10);
	}

	[Test]
	public async Task Add_WritesAnAuditLogEntryNamingTheAdmin()
	{
		using HttpClient client = await CreateAdminClientAsync();
		using HttpResponseMessage response = await AddAsync(client, "Admin V3", _v3);
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		List<string> auditLog = App.Services.GetRequiredService<ILogContainerService>().AuditLogEntries;
		await Assert.That(auditLog.Count).IsEqualTo(1);
		await Assert.That(auditLog[0]).Contains("SpawnsetEntity").And.Contains("spawnset-admin");
	}

	[Test]
	public async Task Add_RejectsAFileThatIsNotASpawnset()
	{
		using HttpClient client = await CreateAdminClientAsync();
		using HttpResponseMessage response = await AddAsync(client, "Garbage", [1, 2, 3, 4]);

		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "could not be parsed");
	}

	[Test]
	public async Task Add_RejectsAFileThatIsAlreadyUploadedUnderAnotherName()
	{
		using HttpClient client = await CreateAdminClientAsync();
		using (HttpResponseMessage first = await AddAsync(client, "Original", _v3))
			await Assert.That(first.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using HttpResponseMessage response = await AddAsync(client, "Copy", _v3);
		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "exactly the same as an already existing spawnset named 'Original'");
	}

	[Test]
	public async Task Add_RejectsANameThatDiffersOnlyInCase()
	{
		using HttpClient client = await CreateAdminClientAsync();
		using (HttpResponseMessage first = await AddAsync(client, "Taken", _v3))
			await Assert.That(first.StatusCode).IsEqualTo(HttpStatusCode.OK);

		// The uniqueness check is a query, so it inherits the column's case-insensitive collation.
		using HttpResponseMessage response = await AddAsync(client, "TAKEN", _default);
		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "already exists");
	}

	[Test]
	public async Task Add_RejectsAnUnknownAuthor()
	{
		using HttpClient client = await CreateAdminClientAsync();
		using HttpResponseMessage response = await AddAsync(client, "Orphan", _v3, playerId: 999);

		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "Player with ID '999' does not exist.");
	}

	[Test]
	public async Task Add_RejectsANameThatCannotBeAFileName()
	{
		using HttpClient client = await CreateAdminClientAsync();
		using HttpResponseMessage response = await AddAsync(client, "a/b", _v3);

		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "invalid file name characters");
	}

	[Test]
	public async Task Add_RejectsAnOversizedFileBeforeTheAction()
	{
		using HttpClient client = await CreateAdminClientAsync();
		using HttpResponseMessage response = await AddAsync(client, "Huge", new byte[SpawnsetConstants.MaxFileSize + 1]);

		await ProblemAssertions.AssertValidationProblemAsync(response);
	}

	[Test]
	public async Task Edit_RenamesAndReassigns()
	{
		using HttpClient client = await CreateAdminClientAsync();
		using (HttpResponseMessage added = await AddAsync(client, "Before", _v3))
			await Assert.That(added.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using (HttpResponseMessage edited = await EditAsync(client, 1, "After", playerId: 2))
			await Assert.That(edited.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using HttpClient anonymous = App.CreateApiClient();

		// The file is looked up by name, so the URL follows the rename.
		using (HttpResponseMessage renamed = await anonymous.GetAsync("api/spawnsets/After/file"))
		{
			await Assert.That(renamed.StatusCode).IsEqualTo(HttpStatusCode.OK);
			await Assert.That(await renamed.Content.ReadAsByteArrayAsync()).IsEquivalentTo(_v3, CollectionOrdering.Matching);
		}

		using (HttpResponseMessage old = await anonymous.GetAsync("api/spawnsets/Before/file"))
			await Assert.That(old.StatusCode).IsEqualTo(HttpStatusCode.NotFound);

		using HttpResponseMessage byAuthor = await anonymous.GetAsync("api/spawnsets/by-author?playerId=2");
		await Assert.That(await byAuthor.Content.ReadAsStringAsync()).Contains("After");
	}

	[Test]
	public async Task Edit_IsNotFoundForAnUnknownId()
	{
		using HttpClient client = await CreateAdminClientAsync();
		using HttpResponseMessage response = await EditAsync(client, 999, "Anything");

		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.NotFound, "Spawnset with ID '999' does not exist.");
	}

	[Test]
	public async Task Edit_RejectsRenamingOntoAnotherSpawnset()
	{
		using HttpClient client = await CreateAdminClientAsync();
		using (HttpResponseMessage first = await AddAsync(client, "First", _v3))
			await Assert.That(first.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using (HttpResponseMessage second = await AddAsync(client, "Second", _default))
			await Assert.That(second.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using HttpResponseMessage response = await EditAsync(client, 2, "First");
		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "already exists");
	}

	[Test]
	public async Task Delete_RemovesTheSpawnsetEverywhere()
	{
		using HttpClient client = await CreateAdminClientAsync();
		using (HttpResponseMessage added = await AddAsync(client, "Doomed", _v3))
			await Assert.That(added.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using (HttpResponseMessage deleted = await client.DeleteAsync("api/admin/spawnsets/1"))
			await Assert.That(deleted.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using HttpClient anonymous = App.CreateApiClient();
		using (HttpResponseMessage main = await anonymous.GetAsync("api/spawnsets/1"))
			await Assert.That(main.StatusCode).IsEqualTo(HttpStatusCode.NotFound);

		using HttpResponseMessage tools = await anonymous.GetAsync("api/app/spawnsets/1/buffer");
		await Assert.That(tools.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}

	[Test]
	public async Task Delete_IsRejectedWhileACustomLeaderboardUsesTheSpawnset()
	{
		using HttpClient client = await CreateAdminClientAsync();
		using (HttpResponseMessage added = await AddAsync(client, "In use", _v3))
			await Assert.That(added.StatusCode).IsEqualTo(HttpStatusCode.OK);

		await App.SeedAsync(dbContext => dbContext.CustomLeaderboards.Add(CustomLeaderboardFixtures.CustomLeaderboard(spawnsetId: 1)));

		using (HttpResponseMessage response = await client.DeleteAsync("api/admin/spawnsets/1"))
			await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "Spawnset with custom leaderboard cannot be deleted.");

		await using AsyncServiceScope scope = App.CreateScope();
		await Assert.That(await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Spawnsets.CountAsync()).IsEqualTo(1);
	}

	[Test]
	public async Task Delete_IsNotFoundForAnUnknownId()
	{
		using HttpClient client = await CreateAdminClientAsync();
		using HttpResponseMessage response = await client.DeleteAsync("api/admin/spawnsets/999");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}
}
