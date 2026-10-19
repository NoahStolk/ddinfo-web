using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.DdLive;

/// <summary>
/// The API used by DDLIVE.
/// </summary>
[NotInParallel(nameof(DdLiveContractTests))]
internal sealed class DdLiveContractTests : ApplicationTest
{
	private const string _api = "DdLive";

	private static readonly byte[] _replay = [.. "ddrpl. contents of the replay of entry 1"u8];

	/// <summary>
	/// The featured V3 leaderboard, so dagger thresholds are served, with an entry by player 1 (16.6666 seconds, with a replay on disk) and one by player 2 (18
	/// seconds, without a replay).
	/// </summary>
	private async Task SeedCustomLeaderboardAsync()
	{
		await App.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "Player 1"));
			dbContext.Players.Add(EntityFixtures.Player(2, "Player 2"));
		});

		await App.SeedAsync(dbContext => dbContext.Spawnsets.Add(CustomLeaderboardFixtures.Spawnset(playerId: 1)));
		await App.SeedAsync(dbContext =>
		{
			CustomLeaderboardEntity customLeaderboard = CustomLeaderboardFixtures.CustomLeaderboard(spawnsetId: 1);
			customLeaderboard.IsFeatured = true;
			dbContext.CustomLeaderboards.Add(customLeaderboard);
		});
		await App.SeedAsync(dbContext =>
		{
			dbContext.CustomEntries.Add(CustomLeaderboardFixtures.CustomEntry(customLeaderboardId: 1, playerId: 1));
			dbContext.CustomEntries.Add(CustomLeaderboardFixtures.CustomEntry(customLeaderboardId: 1, playerId: 2, id: 2, time: 180000));
		});

		await using AsyncServiceScope scope = App.CreateScope();
		await scope.ServiceProvider.GetRequiredService<IFileSystem>().WriteAllBytesAsync(DataSubDirectory.CustomEntryReplays, "1.ddreplay", _replay);
	}

	[Test]
	public async Task GetCustomLeaderboardsOverview()
	{
		await SeedCustomLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/ddlive/custom-leaderboards");

		await ContractSnapshot.AssertMatchesAsync(response, _api, nameof(GetCustomLeaderboardsOverview));
	}

	[Test]
	public async Task GetCustomLeaderboardById()
	{
		await SeedCustomLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/ddlive/custom-leaderboards/1");

		await ContractSnapshot.AssertMatchesAsync(response, _api, nameof(GetCustomLeaderboardById));
	}

	[Test]
	public async Task GetCustomLeaderboardById_IsNotFoundForAnUnknownId()
	{
		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/ddlive/custom-leaderboards/999");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}

	[Test]
	public async Task GetCustomEntryReplay_ServesTheReplayFile()
	{
		await SeedCustomLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/ddlive/custom-entries/1/replay");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
		await Assert.That(response.Content.Headers.ContentType?.MediaType).IsEqualTo("application/octet-stream");
		await Assert.That(response.Content.Headers.ContentDisposition?.FileName).IsNotNull();
		await Assert.That(await response.Content.ReadAsByteArrayAsync()).IsEquivalentTo(_replay, CollectionOrdering.Matching);
	}

	[Test]
	public async Task GetCustomEntryReplay_IsNotFoundWithoutAReplayFile()
	{
		await SeedCustomLeaderboardAsync();

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/ddlive/custom-entries/2/replay");

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
	}

	[Test]
	public async Task GetCommonNames()
	{
		await App.SeedAsync(dbContext =>
		{
			PlayerEntity withCommonName = EntityFixtures.Player(1, "xXPlayerXx");
			withCommonName.CommonName = "Player";
			dbContext.Players.Add(withCommonName);
			dbContext.Players.Add(EntityFixtures.Player(2, "No common name"));
		});

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync("api/ddlive/players/common-names");

		await ContractSnapshot.AssertMatchesAsync(response, _api, nameof(GetCommonNames));
	}

	/// <summary>
	/// The statistics come from the newest file in the leaderboard statistics directory, which the startup hosted service
	/// loads in production. Each entry is 15 bytes: time in game units, kills, gems, daggers hit, daggers fired, death type.
	/// There are twelve entries, so that the top 10 statistics differ from the global ones.
	/// </summary>
	[Test]
	[Arguments(null)]
	[Arguments("Top10")]
	public async Task GetLeaderboardStatistics(string? top)
	{
		using (MemoryStream ms = new())
		{
			await using (BinaryWriter writer = new(ms))
			{
				for (int i = 0; i < 12; i++)
				{
					writer.Write((uint)(12_000_000 - (i * 900_000)));
					writer.Write((ushort)(2000 - (i * 150)));
					writer.Write((ushort)(900 - (i * 70)));
					writer.Write((ushort)(4000 - (i * 300)));
					writer.Write((uint)(20000 - (i * 1500)));
					writer.Write((byte)((i % 3) + 1));
				}
			}

			await using AsyncServiceScope scope = App.CreateScope();
			await scope.ServiceProvider.GetRequiredService<IFileSystem>().WriteAllBytesAsync(DataSubDirectory.LeaderboardStatistics, "202401010000.bin", ms.ToArray());
			await scope.ServiceProvider.GetRequiredService<LeaderboardStatisticsCache>().InitiateAsync();
		}

		using HttpClient client = App.CreateApiClient();
		using HttpResponseMessage response = await client.GetAsync(top == null ? "api/ddlive/leaderboard-statistics" : $"api/ddlive/leaderboard-statistics?top={top}");

		await ContractSnapshot.AssertMatchesAsync(response, _api, $"{nameof(GetLeaderboardStatistics)}_{top ?? "Global"}");
	}
}
