using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Server;

internal sealed class CustomLeaderboardHighscoreLoggerTests
{
	[Test]
	[Arguments(7, 3, "3/50 (+7)")]
	[Arguments(0, 3, "3/50 (+0)")]
	[Arguments(-2, 5, "5/50 (-2)")]
	public async Task LogHighscore_RankValue(int rankDiff, int rank, string expected)
	{
		CustomLeaderboardHighscoreLogger highscoreLogger = new(NullLogger<CustomLeaderboardHighscoreLogger>.Instance);
		CustomLeaderboardEntity customLeaderboard = new() { RankSorting = CustomLeaderboardRankSorting.TimeDesc };
		CustomEntryEntity customEntry = new() { ClientVersion = "1.0.0.0" };

		highscoreLogger.LogHighscore(
			customLeaderboard: customLeaderboard,
			customEntry: customEntry,
			rank: rank,
			rankDifference: rankDiff,
			totalPlayers: 50,
			playerName: "player",
			spawnsetName: "spawnset",
			valueDifference: 0);

		await Assert.That(highscoreLogger.GetHighscoreLogs()[0].RankValue).IsEqualTo(expected);
	}
}
