using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

public record AddUploadRequest
{
	[MaxLength(16)]
	[MinLength(16)]
	public byte[] SurvivalHashMd5 { get; init; } = null!;

	public int PlayerId { get; init; }

	[StringLength(32)]
	public string PlayerName { get; init; } = null!;

	public int ReplayPlayerId { get; init; }

	public double TimeInSeconds { get; init; }

	[MaxLength(4)]
	[MinLength(4)]
	public byte[] TimeAsBytes { get; init; } = null!;

	public int GemsCollected { get; init; }

	public int EnemiesKilled { get; init; }

	public int DaggersFired { get; init; }

	public int DaggersHit { get; init; }

	public int EnemiesAlive { get; init; }

	/// <summary>
	/// This value is not reliable in game memory and therefore no longer used. It is now only used for the request signature.
	/// </summary>
	public int HomingStored { get; init; }

	public int HomingEaten { get; init; }

	public int GemsDespawned { get; init; }

	public int GemsEaten { get; init; }

	public int GemsTotal { get; init; }

	public byte DeathType { get; init; }

	public double LevelUpTime2InSeconds { get; init; }

	public double LevelUpTime3InSeconds { get; init; }

	public double LevelUpTime4InSeconds { get; init; }

	[MaxLength(4)]
	[MinLength(4)]
	public byte[] LevelUpTime2AsBytes { get; init; } = null!;

	[MaxLength(4)]
	[MinLength(4)]
	public byte[] LevelUpTime3AsBytes { get; init; } = null!;

	[MaxLength(4)]
	[MinLength(4)]
	public byte[] LevelUpTime4AsBytes { get; init; } = null!;

	[StringLength(16)]
	public string ClientVersion { get; init; } = null!;

	public string Client { get; init; } = null!;

	public string OperatingSystem { get; init; } = null!;

	public string BuildMode { get; init; } = null!;

	public string Validation { get; init; } = null!;

	public int ValidationVersion { get; init; }

	public bool IsReplay { get; init; }

	public bool ProhibitedMods { get; init; }

	public byte GameMode { get; init; }

	public bool TimeAttackOrRaceFinished { get; init; }

	public AddGameData GameData { get; init; } = null!;

	[Required]
	[MaxLength(ReplayConstants.MaxFileSize, ErrorMessage = ReplayConstants.MaxFileSizeErrorMessage)]
	public byte[] ReplayData { get; init; } = null!;

	public int Status { get; set; }
}
