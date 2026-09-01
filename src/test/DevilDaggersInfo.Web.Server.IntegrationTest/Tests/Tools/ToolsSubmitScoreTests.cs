using DevilDaggersInfo.Web.ApiSpec.Tools.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Tools;

/// <summary>
/// The score submission endpoint ddinfo-tools posts to. This is the only write path the game-facing API exposes, and it
/// spans the anti-cheat signature, the database and the replay file on disk, so it is exercised over real HTTP.
/// </summary>
[NotInParallel(nameof(ToolsSubmitScoreTests))]
internal sealed class ToolsSubmitScoreTests : ApplicationTest
{
	private static byte[] SpawnsetFile => CustomLeaderboardFixtures.V3SpawnsetFile;

	private static byte[] SpawnsetHash()
	{
		if (!SpawnsetBinary.TryParse(SpawnsetFile, out SpawnsetBinary? spawnsetBinary))
			throw new InvalidOperationException("Spawnset could not be parsed.");

		return MD5.HashData(spawnsetBinary.ToBytes());
	}

	private async Task SeedLeaderboardAsync()
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "TestPlayer1"));
			dbContext.Spawnsets.Add(CustomLeaderboardFixtures.Spawnset(playerId: 1));
		});

		await App.SeedAsync(dbContext => dbContext.CustomLeaderboards.Add(CustomLeaderboardFixtures.CustomLeaderboard(spawnsetId: 1)));
	}

	private static AddUploadRequest Request(float timeInSeconds, int playerId, int status = 4, string? clientVersion = null, string? validation = null)
	{
		return UploadRequestFixtures.Create(
			survivalHashMd5: SpawnsetHash(),
			replayData: UploadRequestFixtures.MockReplay(SpawnsetFile),
			timeInSeconds: timeInSeconds,
			playerId: playerId,
			status: status,
			clientVersion: clientVersion,
			validation: validation);
	}

	[Test]
	public async Task SubmitScore_FirstScore_IsAcceptedAndPersisted()
	{
		await SeedLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.PostAsJsonAsync("api/app/custom-entries/submit", Request(timeInSeconds: 20, playerId: 1));

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		JsonElement root = document.RootElement;

		await Assert.That(root.GetProperty("spawnsetName").GetString()).IsEqualTo("V3");
		await Assert.That(root.GetProperty("customLeaderboardId").GetInt32()).IsEqualTo(1);

		// A first submission reports through the firstScore branch, not highscore or noHighscore.
		await Assert.That(root.GetProperty("firstScore").ValueKind).IsNotEqualTo(JsonValueKind.Null);
		await Assert.That(root.GetProperty("highscore").ValueKind).IsEqualTo(JsonValueKind.Null);

		await using AsyncServiceScope scope = App.CreateScope();
		ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		await Assert.That(await dbContext.CustomEntries.Select(ce => ce.Time).SingleAsync()).IsEqualTo(200000);
	}

	[Test]
	public async Task SubmitScore_WritesTheReplayFileNamedAfterTheEntryId()
	{
		await SeedLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.PostAsJsonAsync("api/app/custom-entries/submit", Request(timeInSeconds: 20, playerId: 1));

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		await using AsyncServiceScope scope = App.CreateScope();
		ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		int entryId = await dbContext.CustomEntries.Select(ce => ce.Id).SingleAsync();

		IFileSystemService fileSystemService = scope.ServiceProvider.GetRequiredService<IFileSystemService>();
		string replayPath = Path.Combine(fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{entryId}.ddreplay");

		await Assert.That(File.Exists(replayPath)).IsTrue();

		// The endpoint that serves it back must now find it.
		using HttpResponseMessage replayResponse = await client.GetAsync($"api/app/custom-entries/{entryId}/replay-buffer");
		await Assert.That(replayResponse.StatusCode).IsEqualTo(HttpStatusCode.OK);
	}

	[Test]
	public async Task SubmitScore_NewHighscore_ReplacesThePreviousTime()
	{
		await SeedLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();

		using (HttpResponseMessage first = await client.PostAsJsonAsync("api/app/custom-entries/submit", Request(timeInSeconds: 20, playerId: 1)))
		{
			await Assert.That(first.StatusCode).IsEqualTo(HttpStatusCode.OK);
		}

		using (HttpResponseMessage second = await client.PostAsJsonAsync("api/app/custom-entries/submit", Request(timeInSeconds: 40, playerId: 1)))
		{
			await Assert.That(second.StatusCode).IsEqualTo(HttpStatusCode.OK);

			using JsonDocument document = JsonDocument.Parse(await second.Content.ReadAsStringAsync());
			await Assert.That(document.RootElement.GetProperty("highscore").ValueKind).IsNotEqualTo(JsonValueKind.Null);
		}

		await using AsyncServiceScope scope = App.CreateScope();
		ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		// One entry per player, updated in place rather than appended.
		await Assert.That(await dbContext.CustomEntries.CountAsync()).IsEqualTo(1);
		await Assert.That(await dbContext.CustomEntries.Select(ce => ce.Time).SingleAsync()).IsEqualTo(400000);
	}

	[Test]
	public async Task SubmitScore_SlowerRun_KeepsTheExistingHighscore()
	{
		await SeedLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();

		using (HttpResponseMessage first = await client.PostAsJsonAsync("api/app/custom-entries/submit", Request(timeInSeconds: 40, playerId: 1)))
		{
			await Assert.That(first.StatusCode).IsEqualTo(HttpStatusCode.OK);
		}

		using (HttpResponseMessage second = await client.PostAsJsonAsync("api/app/custom-entries/submit", Request(timeInSeconds: 20, playerId: 1)))
		{
			await Assert.That(second.StatusCode).IsEqualTo(HttpStatusCode.OK);

			using JsonDocument document = JsonDocument.Parse(await second.Content.ReadAsStringAsync());
			await Assert.That(document.RootElement.GetProperty("noHighscore").ValueKind).IsNotEqualTo(JsonValueKind.Null);
		}

		await using AsyncServiceScope scope = App.CreateScope();
		ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		await Assert.That(await dbContext.CustomEntries.Select(ce => ce.Time).SingleAsync()).IsEqualTo(400000);
	}

	[Test]
	public async Task SubmitScore_CreatesThePlayerWhenUnknown()
	{
		await SeedLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.PostAsJsonAsync("api/app/custom-entries/submit", Request(timeInSeconds: 20, playerId: 77));

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		await using AsyncServiceScope scope = App.CreateScope();
		ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		await Assert.That(await dbContext.Players.Where(p => p.Id == 77).Select(p => p.PlayerName).SingleAsync()).IsEqualTo("TestPlayer77");
	}

	[Test]
	public async Task SubmitScore_RejectsATamperedValidation()
	{
		await SeedLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.PostAsJsonAsync(
			"api/app/custom-entries/submit",
			Request(timeInSeconds: 20, playerId: 1, validation: "Malformed validation"));

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

		// Errors from the domain layer are RFC 7807 problem responses.
		await Assert.That(response.Content.Headers.ContentType?.MediaType).IsEqualTo("application/problem+json");
		await Assert.That(response.Content.Headers.ContentType?.CharSet).IsEqualTo("utf-8");

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("title").GetString()).StartsWith("Could not decrypt");
		await Assert.That(document.RootElement.GetProperty("status").GetInt32()).IsEqualTo(400);

		await AssertNothingWasStoredAsync();
	}

	[Test]
	public async Task SubmitScore_RejectsAnOutdatedClient()
	{
		await SeedLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.PostAsJsonAsync(
			"api/app/custom-entries/submit",
			Request(timeInSeconds: 20, playerId: 1, clientVersion: "0.0.0.0"));

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("title").GetString()).Contains("unsupported and outdated");

		await AssertNothingWasStoredAsync();
	}

	[Test]
	public async Task SubmitScore_RejectsAnUnknownSpawnset()
	{
		// No spawnset seeded at all, so the hash cannot resolve.
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.PostAsJsonAsync("api/app/custom-entries/submit", Request(timeInSeconds: 20, playerId: 1));

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("title").GetString()).Contains("doesn't exist");
	}

	[Test]
	[Arguments(0)]
	[Arguments(1)]
	[Arguments(2)]
	[Arguments(6)]
	[Arguments(8)]
	public async Task SubmitScore_RejectsANonPlayingGameStatus(int status)
	{
		await SeedLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.PostAsJsonAsync("api/app/custom-entries/submit", Request(timeInSeconds: 20, playerId: 1, status: status));

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
		await AssertNothingWasStoredAsync();
	}

	[Test]
	public async Task SubmitScore_RejectsABannedPlayer()
	{
		await SeedLeaderboardAsync();
		await App.SeedAsync(dbContext =>
		{
			PlayerEntity banned = EntityFixtures.Player(5, "Banned");
			banned.IsBannedFromDdcl = true;
			dbContext.Players.Add(banned);
		});

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.PostAsJsonAsync("api/app/custom-entries/submit", Request(timeInSeconds: 20, playerId: 5));

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		await Assert.That(document.RootElement.GetProperty("title").GetString()).Contains("Banned");

		await AssertNothingWasStoredAsync();
	}

	private async Task AssertNothingWasStoredAsync()
	{
		await using AsyncServiceScope scope = App.CreateScope();
		ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		await Assert.That(await dbContext.CustomEntries.CountAsync()).IsEqualTo(0);
	}
}
