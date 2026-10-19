using DevilDaggersInfo.Web.ApiSpec.Tools.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Data;

internal static class GameDataFixtures
{
	/// <summary>
	/// Per-frame stat arrays, all empty. Every property is required, so a test that cares about one of them sets it with
	/// a <c>with</c> expression rather than restating the rest.
	/// </summary>
	internal static AddGameData Empty()
	{
		return new AddGameData
		{
			GemsCollected = [],
			EnemiesKilled = [],
			DaggersFired = [],
			DaggersHit = [],
			EnemiesAlive = [],
			HomingStored = [],
			HomingEaten = [],
			GemsDespawned = [],
			GemsEaten = [],
			GemsTotal = [],
			Skull1sAlive = [],
			Skull2sAlive = [],
			Skull3sAlive = [],
			SpiderlingsAlive = [],
			Skull4sAlive = [],
			Squid1sAlive = [],
			Squid2sAlive = [],
			Squid3sAlive = [],
			CentipedesAlive = [],
			GigapedesAlive = [],
			Spider1sAlive = [],
			Spider2sAlive = [],
			LeviathansAlive = [],
			OrbsAlive = [],
			ThornsAlive = [],
			GhostpedesAlive = [],
			SpiderEggsAlive = [],
			Skull1sKilled = [],
			Skull2sKilled = [],
			Skull3sKilled = [],
			SpiderlingsKilled = [],
			Skull4sKilled = [],
			Squid1sKilled = [],
			Squid2sKilled = [],
			Squid3sKilled = [],
			CentipedesKilled = [],
			GigapedesKilled = [],
			Spider1sKilled = [],
			Spider2sKilled = [],
			LeviathansKilled = [],
			OrbsKilled = [],
			ThornsKilled = [],
			GhostpedesKilled = [],
			SpiderEggsKilled = [],
		};
	}
}
