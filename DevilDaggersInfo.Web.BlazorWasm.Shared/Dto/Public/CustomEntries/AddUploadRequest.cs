using DevilDaggersInfo.Core.Extensions;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;

public record AddUploadRequest
{
	[MaxLength(16)]
	[MinLength(16)]
	public byte[] SurvivalHashMd5 { get; init; } = null!;

	public int PlayerId { get; init; }

	[StringLength(32)]
	public string PlayerName { get; init; } = null!;

	public int ReplayPlayerId { get; init; }

	[Obsolete("Use TimeInSeconds instead.")]
	public int Time { get; init; }

	public double TimeInSeconds { get; init; }

	public int GemsCollected { get; init; }

	public int EnemiesKilled { get; init; }

	public int DaggersFired { get; init; }

	public int DaggersHit { get; init; }

	public int EnemiesAlive { get; init; }

	public int HomingDaggers { get; set; } // Use set to get rid of negative values.

	public int HomingDaggersEaten { get; init; }

	public int GemsDespawned { get; init; }

	public int GemsEaten { get; init; }

	public int GemsTotal { get; init; }

	public byte DeathType { get; init; }

	[Obsolete("Use LevelUpTime2InSeconds instead.")]
	public int LevelUpTime2 { get; init; }

	[Obsolete("Use LevelUpTime3InSeconds instead.")]
	public int LevelUpTime3 { get; init; }

	[Obsolete("Use LevelUpTime4InSeconds instead.")]
	public int LevelUpTime4 { get; init; }

	public double LevelUpTime2InSeconds { get; init; }

	public double LevelUpTime3InSeconds { get; init; }

	public double LevelUpTime4InSeconds { get; init; }

	[StringLength(16)]
	public string ClientVersion { get; init; } = null!;

	public string Client { get; init; } = null!;

	public string OperatingSystem { get; init; } = null!;

	public string BuildMode { get; init; } = null!;

	public string Validation { get; init; } = null!;

	public bool IsReplay { get; init; }

	public bool ProhibitedMods { get; init; }

	public byte GameMode { get; init; }

	public bool TimeAttackOrRaceFinished { get; init; }

	public AddGameData GameData { get; init; } = null!;

	[Required]
	[MaxLength(ReplayConstants.MaxFileSize, ErrorMessage = ReplayConstants.MaxFileSizeErrorMessage)]
	public byte[] ReplayData { get; init; } = null!;

	public int Status { get; set; }

	public int GetTime() => TimeInSeconds == 0 ? Time : TimeInSeconds.To10thMilliTime();

	public int GetLevelUpTime2() => LevelUpTime2InSeconds == 0 ? LevelUpTime2 : LevelUpTime2InSeconds.To10thMilliTime();

	public int GetLevelUpTime3() => LevelUpTime3InSeconds == 0 ? LevelUpTime3 : LevelUpTime3InSeconds.To10thMilliTime();

	public int GetLevelUpTime4() => LevelUpTime4InSeconds == 0 ? LevelUpTime4 : LevelUpTime4InSeconds.To10thMilliTime();
}
