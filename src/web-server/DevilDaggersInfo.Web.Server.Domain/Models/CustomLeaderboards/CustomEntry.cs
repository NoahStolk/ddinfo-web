using DevilDaggersInfo.Core.Versioning;
using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Constants;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public class CustomEntry
{
	public int Id { get; init; }

	public int Rank { get; init; }

	public int PlayerId { get; set; }

	public required string PlayerName { get; set; }

	public string? CountryCode { get; set; }

	public int Time { get; set; }

	public CustomLeaderboardDagger? CustomLeaderboardDagger { get; init; }

	public int GemsCollected { get; set; }

	public int EnemiesKilled { get; set; }

	public int DaggersFired { get; set; }

	public int DaggersHit { get; set; }

	public int EnemiesAlive { get; set; }

	public int HomingStored { get; set; }

	public int? HomingEaten { get; set; }

	public int? GemsDespawned { get; set; }

	public int? GemsEaten { get; set; }

	public int? GemsTotal { get; set; }

	public byte DeathType { get; set; }

	public int LevelUpTime2 { get; set; }

	public int LevelUpTime3 { get; set; }

	public int LevelUpTime4 { get; set; }

	public DateTime SubmitDate { get; set; }

	public required string ClientVersion { get; set; }

	public CustomLeaderboardsClient Client { get; set; }

	public bool HasReplay { get; init; }

	public bool HasGraphs => Client != CustomLeaderboardsClient.DevilDaggersCustomLeaderboards || ClientVersionParsed >= FeatureConstants.OldDdclGraphs;

	private AppVersion ClientVersionParsed => AppVersion.TryParse(ClientVersion, out AppVersion? version) ? version : new(0, 0, 0);
}
