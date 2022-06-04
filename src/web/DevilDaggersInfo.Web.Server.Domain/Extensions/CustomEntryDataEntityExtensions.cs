using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Utils;

namespace DevilDaggersInfo.Web.Server.Domain.Extensions;

public static class CustomEntryDataEntityExtensions
{
	public static void Populate(this CustomEntryDataEntity ced, UploadRequestData data)
	{
		ced.GemsCollectedData = IntegerArrayCompressor.CompressData(data.GemsCollected);
		ced.EnemiesKilledData = IntegerArrayCompressor.CompressData(data.EnemiesKilled);
		ced.DaggersFiredData = IntegerArrayCompressor.CompressData(data.DaggersFired);
		ced.DaggersHitData = IntegerArrayCompressor.CompressData(data.DaggersHit);
		ced.EnemiesAliveData = IntegerArrayCompressor.CompressData(data.EnemiesAlive);
		ced.HomingStoredData = IntegerArrayCompressor.CompressData(data.HomingStored);
		ced.HomingEatenData = IntegerArrayCompressor.CompressData(data.HomingEaten);
		ced.GemsDespawnedData = IntegerArrayCompressor.CompressData(data.GemsDespawned);
		ced.GemsEatenData = IntegerArrayCompressor.CompressData(data.GemsEaten);
		ced.GemsTotalData = IntegerArrayCompressor.CompressData(data.GemsTotal);

		ced.Skull1sAliveData = IntegerArrayCompressor.CompressData(data.Skull1sAlive);
		ced.Skull2sAliveData = IntegerArrayCompressor.CompressData(data.Skull2sAlive);
		ced.Skull3sAliveData = IntegerArrayCompressor.CompressData(data.Skull3sAlive);
		ced.SpiderlingsAliveData = IntegerArrayCompressor.CompressData(data.SpiderlingsAlive);
		ced.Skull4sAliveData = IntegerArrayCompressor.CompressData(data.Skull4sAlive);
		ced.Squid1sAliveData = IntegerArrayCompressor.CompressData(data.Squid1sAlive);
		ced.Squid2sAliveData = IntegerArrayCompressor.CompressData(data.Squid2sAlive);
		ced.Squid3sAliveData = IntegerArrayCompressor.CompressData(data.Squid3sAlive);
		ced.CentipedesAliveData = IntegerArrayCompressor.CompressData(data.CentipedesAlive);
		ced.GigapedesAliveData = IntegerArrayCompressor.CompressData(data.GigapedesAlive);
		ced.Spider1sAliveData = IntegerArrayCompressor.CompressData(data.Spider1sAlive);
		ced.Spider2sAliveData = IntegerArrayCompressor.CompressData(data.Spider2sAlive);
		ced.LeviathansAliveData = IntegerArrayCompressor.CompressData(data.LeviathansAlive);
		ced.OrbsAliveData = IntegerArrayCompressor.CompressData(data.OrbsAlive);
		ced.ThornsAliveData = IntegerArrayCompressor.CompressData(data.ThornsAlive);
		ced.GhostpedesAliveData = IntegerArrayCompressor.CompressData(data.GhostpedesAlive);
		ced.SpiderEggsAliveData = IntegerArrayCompressor.CompressData(data.SpiderEggsAlive);

		ced.Skull1sKilledData = IntegerArrayCompressor.CompressData(data.Skull1sKilled);
		ced.Skull2sKilledData = IntegerArrayCompressor.CompressData(data.Skull2sKilled);
		ced.Skull3sKilledData = IntegerArrayCompressor.CompressData(data.Skull3sKilled);
		ced.SpiderlingsKilledData = IntegerArrayCompressor.CompressData(data.SpiderlingsKilled);
		ced.Skull4sKilledData = IntegerArrayCompressor.CompressData(data.Skull4sKilled);
		ced.Squid1sKilledData = IntegerArrayCompressor.CompressData(data.Squid1sKilled);
		ced.Squid2sKilledData = IntegerArrayCompressor.CompressData(data.Squid2sKilled);
		ced.Squid3sKilledData = IntegerArrayCompressor.CompressData(data.Squid3sKilled);
		ced.CentipedesKilledData = IntegerArrayCompressor.CompressData(data.CentipedesKilled);
		ced.GigapedesKilledData = IntegerArrayCompressor.CompressData(data.GigapedesKilled);
		ced.Spider1sKilledData = IntegerArrayCompressor.CompressData(data.Spider1sKilled);
		ced.Spider2sKilledData = IntegerArrayCompressor.CompressData(data.Spider2sKilled);
		ced.LeviathansKilledData = IntegerArrayCompressor.CompressData(data.LeviathansKilled);
		ced.OrbsKilledData = IntegerArrayCompressor.CompressData(data.OrbsKilled);
		ced.ThornsKilledData = IntegerArrayCompressor.CompressData(data.ThornsKilled);
		ced.GhostpedesKilledData = IntegerArrayCompressor.CompressData(data.GhostpedesKilled);
		ced.SpiderEggsKilledData = IntegerArrayCompressor.CompressData(data.SpiderEggsKilled);
	}
}
