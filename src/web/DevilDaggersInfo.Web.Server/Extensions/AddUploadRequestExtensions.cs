using DevilDaggersInfo.Web.Shared.Dto.Public.CustomEntries;

namespace DevilDaggersInfo.Web.Server.Extensions;

// TODO: Move to base library and re-use in DDCL.
public static class AddUploadRequestExtensions
{
	public static string CreateValidationV2(this AddUploadRequest uploadRequest)
	{
		return string.Join(
			";",
			uploadRequest.PlayerId,
			uploadRequest.TimeAsBytes.ByteArrayToHexString(),
			uploadRequest.GemsCollected,
			uploadRequest.GemsDespawned,
			uploadRequest.GemsEaten,
			uploadRequest.GemsTotal,
			uploadRequest.EnemiesAlive,
			uploadRequest.EnemiesKilled,
			uploadRequest.DeathType,
			uploadRequest.DaggersHit,
			uploadRequest.DaggersFired,
			uploadRequest.HomingStored,
			uploadRequest.HomingEaten,
			uploadRequest.IsReplay,
			uploadRequest.Status,
			uploadRequest.SurvivalHashMd5.ByteArrayToHexString(),
			uploadRequest.LevelUpTime2AsBytes.ByteArrayToHexString(),
			uploadRequest.LevelUpTime3AsBytes.ByteArrayToHexString(),
			uploadRequest.LevelUpTime4AsBytes.ByteArrayToHexString(),
			uploadRequest.GameMode,
			uploadRequest.TimeAttackOrRaceFinished,
			uploadRequest.ProhibitedMods);
	}
}
