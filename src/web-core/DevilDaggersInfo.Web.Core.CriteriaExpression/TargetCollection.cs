namespace DevilDaggersInfo.Web.Core.CriteriaExpression;

public record TargetCollection
{
	public int GemsCollected { get; init; }

	public int GemsDespawned { get; init; }

	public int GemsEaten { get; init; }

	public int EnemiesKilled { get; init; }

	public int DaggersFired { get; init; }

	public int DaggersHit { get; init; }

	public int HomingStored { get; init; }

	public int HomingEaten { get; init; }

	public int Skull1Kills { get; init; }

	public int Skull2Kills { get; init; }

	public int Skull3Kills { get; init; }

	public int Skull4Kills { get; init; }

	public int SpiderlingKills { get; init; }

	public int SpiderEggKills { get; init; }

	public int Squid1Kills { get; init; }

	public int Squid2Kills { get; init; }

	public int Squid3Kills { get; init; }

	public int CentipedeKills { get; init; }

	public int GigapedeKills { get; init; }

	public int GhostpedeKills { get; init; }

	public int Spider1Kills { get; init; }

	public int Spider2Kills { get; init; }

	public int LeviathanKills { get; init; }

	public int OrbKills { get; init; }

	public int ThornKills { get; init; }
}
