using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Extensions;

// TODO: Move to base library and re-use in DDCL.
public static class AddUploadRequestExtensions
{
	public static string CreateValidationV1(this AddUploadRequest uploadRequest)
	{
		return string.Join(
			";",
			uploadRequest.PlayerId,
			uploadRequest.GetTime(),
			uploadRequest.GemsCollected,
			uploadRequest.GemsDespawned,
			uploadRequest.GemsEaten,
			uploadRequest.GemsTotal,
			uploadRequest.EnemiesKilled,
			uploadRequest.DeathType,
			uploadRequest.DaggersHit,
			uploadRequest.DaggersFired,
			uploadRequest.EnemiesAlive,
			uploadRequest.GetHomingStored(),
			uploadRequest.GetHomingEaten(),
			uploadRequest.IsReplay ? 1 : 0,
			uploadRequest.SurvivalHashMd5.ByteArrayToHexString(),
			string.Join(",", uploadRequest.GetLevelUpTime2(), uploadRequest.GetLevelUpTime3(), uploadRequest.GetLevelUpTime4()));
	}

	public static string CreateValidationV2(this AddUploadRequest uploadRequest)
	{
		return string.Join(
			";",
			uploadRequest.PlayerId,
			uploadRequest.TimeAsBytes!.ByteArrayToHexString(),
			uploadRequest.GemsCollected,
			uploadRequest.GemsDespawned,
			uploadRequest.GemsEaten,
			uploadRequest.GemsTotal,
			uploadRequest.EnemiesAlive,
			uploadRequest.EnemiesKilled,
			uploadRequest.DeathType,
			uploadRequest.DaggersHit,
			uploadRequest.DaggersFired,
			uploadRequest.GetHomingStored(),
			uploadRequest.GetHomingEaten(),
			uploadRequest.IsReplay ? 1 : 0,
			uploadRequest.Status,
			uploadRequest.SurvivalHashMd5.ByteArrayToHexString(),
			uploadRequest.LevelUpTime2AsBytes!.ByteArrayToHexString(),
			uploadRequest.LevelUpTime3AsBytes!.ByteArrayToHexString(),
			uploadRequest.LevelUpTime4AsBytes!.ByteArrayToHexString(),
			uploadRequest.GameMode,
			uploadRequest.TimeAttackOrRaceFinished,
			uploadRequest.ProhibitedMods);
	}
}
