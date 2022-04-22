using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;

namespace DevilDaggersInfo.Web.Server.Extensions;

public static class CustomEntryDataEntityExtensions
{
	public static void Populate(this CustomEntryDataEntity ced, AddUploadRequest uploadRequest)
	{
		ced.GemsCollectedData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.GemsCollected);
		ced.EnemiesKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.EnemiesKilled);
		ced.DaggersFiredData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.DaggersFired);
		ced.DaggersHitData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.DaggersHit);
		ced.EnemiesAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.EnemiesAlive);
		ced.HomingStoredData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.HomingStored);
		ced.HomingEatenData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.HomingEaten);
		ced.GemsDespawnedData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.GemsDespawned);
		ced.GemsEatenData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.GemsEaten);
		ced.GemsTotalData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.GemsTotal);

		ced.Skull1sAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Skull1sAlive);
		ced.Skull2sAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Skull2sAlive);
		ced.Skull3sAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Skull3sAlive);
		ced.SpiderlingsAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.SpiderlingsAlive);
		ced.Skull4sAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Skull4sAlive);
		ced.Squid1sAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Squid1sAlive);
		ced.Squid2sAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Squid2sAlive);
		ced.Squid3sAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Squid3sAlive);
		ced.CentipedesAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.CentipedesAlive);
		ced.GigapedesAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.GigapedesAlive);
		ced.Spider1sAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Spider1sAlive);
		ced.Spider2sAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Spider2sAlive);
		ced.LeviathansAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.LeviathansAlive);
		ced.OrbsAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.OrbsAlive);
		ced.ThornsAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.ThornsAlive);
		ced.GhostpedesAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.GhostpedesAlive);
		ced.SpiderEggsAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.SpiderEggsAlive);

		ced.Skull1sKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Skull1sKilled);
		ced.Skull2sKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Skull2sKilled);
		ced.Skull3sKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Skull3sKilled);
		ced.SpiderlingsKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.SpiderlingsKilled);
		ced.Skull4sKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Skull4sKilled);
		ced.Squid1sKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Squid1sKilled);
		ced.Squid2sKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Squid2sKilled);
		ced.Squid3sKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Squid3sKilled);
		ced.CentipedesKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.CentipedesKilled);
		ced.GigapedesKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.GigapedesKilled);
		ced.Spider1sKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Spider1sKilled);
		ced.Spider2sKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Spider2sKilled);
		ced.LeviathansKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.LeviathansKilled);
		ced.OrbsKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.OrbsKilled);
		ced.ThornsKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.ThornsKilled);
		ced.GhostpedesKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.GhostpedesKilled);
		ced.SpiderEggsKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.SpiderEggsKilled);
	}
}
