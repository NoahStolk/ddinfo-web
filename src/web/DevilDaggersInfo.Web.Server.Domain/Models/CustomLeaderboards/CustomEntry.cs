using DevilDaggersInfo.Web.Server.Domain.Constants;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public class CustomEntry
{
	public int Id { get; init; }

	public int Rank { get; init; }

	public int PlayerId { get; set; }

	public string PlayerName { get; set; } = null!;

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

	public string ClientVersion { get; set; } = null!;

	public CustomLeaderboardsClient Client { get; set; }

	public double Accuracy => DaggersFired == 0 ? 0 : DaggersHit / (double)DaggersFired;

	public bool HasGraphs => Client != CustomLeaderboardsClient.DevilDaggersCustomLeaderboards || ClientVersionParsed >= FeatureConstants.DdclGraphs;

	private Version ClientVersionParsed => Version.TryParse(ClientVersion, out Version? version) ? version : new(0, 0, 0, 0);
}
