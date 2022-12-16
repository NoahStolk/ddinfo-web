using DevilDaggersInfo.Core.Versioning;
using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Constants;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomEntry
{
	public required int Id { get; init; }

	public required int Rank { get; init; }

	public required int PlayerId { get; init; }

	public required string PlayerName { get; init; }

	public required string? CountryCode { get; init; }

	public required int Time { get; init; }

	public required CustomLeaderboardDagger? CustomLeaderboardDagger { get; init; }

	public required int GemsCollected { get; init; }

	public required int EnemiesKilled { get; init; }

	public required int DaggersFired { get; init; }

	public required int DaggersHit { get; init; }

	public required int EnemiesAlive { get; init; }

	public required int HomingStored { get; init; }

	public required int? HomingEaten { get; init; }

	public required int? GemsDespawned { get; init; }

	public required int? GemsEaten { get; init; }

	public required int? GemsTotal { get; init; }

	public required byte DeathType { get; init; }

	public required int LevelUpTime2 { get; init; }

	public required int LevelUpTime3 { get; init; }

	public required int LevelUpTime4 { get; init; }

	public required DateTime SubmitDate { get; init; }

	public required string ClientVersion { get; init; }

	public required CustomLeaderboardsClient Client { get; init; }

	public required bool HasReplay { get; init; }

	public bool HasGraphs => Client != CustomLeaderboardsClient.DevilDaggersCustomLeaderboards || ClientVersionParsed >= FeatureConstants.OldDdclGraphs;

	private AppVersion ClientVersionParsed => AppVersion.TryParse(ClientVersion, out AppVersion? version) ? version : new(0, 0, 0);
}
