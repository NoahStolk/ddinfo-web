using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests;

// Every test in this class shares one database and one data directory, and the reset between tests would otherwise
// race. Other classes get their own database, so they still run in parallel with this one.
[NotInParallel(nameof(HarnessTests))]
internal sealed class HarnessTests
{
	[ClassDataSource<MySqlFixture>(Shared = SharedType.PerTestSession)]
	public required MySqlFixture MySql { get; init; }

	[Test]
	public async Task Schema_IsCreatedFromTheEntityModel()
	{
		TestApplication app = await MySql.GetApplicationAsync(nameof(HarnessTests));
		await app.ResetAsync();

		await using AsyncServiceScope scope = app.CreateScope();
		ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		// Reaching every DbSet proves each entity was mapped to a table the server actually accepts.
		await Assert.That(await dbContext.Players.CountAsync()).IsEqualTo(0);
		await Assert.That(await dbContext.Spawnsets.CountAsync()).IsEqualTo(0);
		await Assert.That(await dbContext.CustomLeaderboards.CountAsync()).IsEqualTo(0);
		await Assert.That(await dbContext.CustomEntries.CountAsync()).IsEqualTo(0);
		await Assert.That(await dbContext.CustomEntryData.CountAsync()).IsEqualTo(0);
		await Assert.That(await dbContext.Mods.CountAsync()).IsEqualTo(0);
		await Assert.That(await dbContext.PlayerMods.CountAsync()).IsEqualTo(0);
		await Assert.That(await dbContext.Donations.CountAsync()).IsEqualTo(0);
		await Assert.That(await dbContext.Markers.CountAsync()).IsEqualTo(0);
		await Assert.That(await dbContext.Users.CountAsync()).IsEqualTo(0);
		await Assert.That(await dbContext.UserRoles.CountAsync()).IsEqualTo(0);

		// ResetAsync seeds these, so this also proves the reset ran.
		await Assert.That(await dbContext.Roles.CountAsync()).IsEqualTo(5);
	}

	[Test]
	public async Task Reset_TruncatesTables_AndResetsAutoIncrement()
	{
		TestApplication app = await MySql.GetApplicationAsync(nameof(HarnessTests));
		await app.ResetAsync();

		await app.SeedAsync(dbContext => dbContext.Mods.Add(EntityFixtures.Mod("first")));

		await using (AsyncServiceScope scope = app.CreateScope())
		{
			ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			await Assert.That(await dbContext.Mods.Select(m => m.Id).SingleAsync()).IsEqualTo(1);
		}

		await app.ResetAsync();
		await app.SeedAsync(dbContext => dbContext.Mods.Add(EntityFixtures.Mod("second")));

		await using (AsyncServiceScope scope = app.CreateScope())
		{
			ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

			// A truncate rather than a delete, so the identity column starts over. Replay files are named after this
			// value, which is why it has to be deterministic per test.
			await Assert.That(await dbContext.Mods.Select(m => m.Id).SingleAsync()).IsEqualTo(1);
		}
	}

	[Test]
	public async Task FileSystemService_IsRootedOutsideTheWorkingDirectory()
	{
		TestApplication app = await MySql.GetApplicationAsync(nameof(HarnessTests));
		await app.ResetAsync();

		await using AsyncServiceScope scope = app.CreateScope();
		IFileSystemService fileSystemService = scope.ServiceProvider.GetRequiredService<IFileSystemService>();

		string path = fileSystemService.GetPath(DataSubDirectory.Mods);

		await Assert.That(Path.IsPathRooted(path)).IsTrue();
		await Assert.That(path).Contains("ddinfo-integration-tests");
		await Assert.That(Directory.Exists(path)).IsTrue();
	}

	[Test]
	public async Task AdminEndpoint_RejectsAnonymous_AndWrongRole_AndAcceptsCorrectRole()
	{
		TestApplication app = await MySql.GetApplicationAsync(nameof(HarnessTests));
		await app.ResetAsync();

		// api/admin/players requires Roles.Players.
		using (HttpClient anonymous = app.CreateApiClient())
		{
			using HttpResponseMessage response = await anonymous.GetAsync("api/admin/players?pageIndex=0&pageSize=25");
			await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.Unauthorized);
		}

		string wrongRoleJwt = await app.CreateJwtAsync("mods-only", Roles.Mods);
		using (HttpClient wrongRole = app.CreateApiClient(wrongRoleJwt))
		{
			using HttpResponseMessage response = await wrongRole.GetAsync("api/admin/players?pageIndex=0&pageSize=25");
			await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.Forbidden);
		}

		string correctRoleJwt = await app.CreateJwtAsync("players-role", Roles.Players);
		using (HttpClient correctRole = app.CreateApiClient(correctRoleJwt))
		{
			using HttpResponseMessage response = await correctRole.GetAsync("api/admin/players?pageIndex=0&pageSize=25");
			await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
		}
	}

	[Test]
	[Arguments(14, HttpStatusCode.BadRequest)]
	[Arguments(15, HttpStatusCode.OK)]
	[Arguments(35, HttpStatusCode.OK)]
	[Arguments(36, HttpStatusCode.BadRequest)]
	public async Task PagedEndpoint_EnforcesPageSizeRange(int pageSize, HttpStatusCode expected)
	{
		TestApplication app = await MySql.GetApplicationAsync(nameof(HarnessTests));
		await app.ResetAsync();

		using HttpClient client = app.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync($"api/spawnsets?withCustomLeaderboardOnly=false&pageIndex=0&pageSize={pageSize}");

		await Assert.That(response.StatusCode).IsEqualTo(expected);
	}

	[Test]
	public async Task MainEndpoint_ReturnsSeededData()
	{
		TestApplication app = await MySql.GetApplicationAsync(nameof(HarnessTests));
		await app.ResetAsync();

		await app.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "Player 1"));
			dbContext.Spawnsets.Add(EntityFixtures.Spawnset("V3", playerId: 1));
		});

		using HttpClient client = app.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/spawnsets?withCustomLeaderboardOnly=false&pageIndex=0&pageSize=25");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		string json = await response.Content.ReadAsStringAsync();
		await Assert.That(json).Contains("V3");
	}
}
