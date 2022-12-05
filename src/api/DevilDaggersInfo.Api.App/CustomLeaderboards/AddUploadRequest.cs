using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record AddUploadRequest
{
	[MaxLength(16)]
	[MinLength(16)]
	public required byte[] SurvivalHashMd5 { get; init; }

	public int PlayerId { get; init; }

	[StringLength(32)]
	public required string PlayerName { get; init; }

	public int ReplayPlayerId { get; init; }

	public double TimeInSeconds { get; init; }

	[MaxLength(4)]
	[MinLength(4)]
	public required byte[] TimeAsBytes { get; init; }

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
	public required byte[] LevelUpTime2AsBytes { get; init; }

	[MaxLength(4)]
	[MinLength(4)]
	public required byte[] LevelUpTime3AsBytes { get; init; }

	[MaxLength(4)]
	[MinLength(4)]
	public required byte[] LevelUpTime4AsBytes { get; init; }

	[StringLength(16)]
	public required string ClientVersion { get; init; }

	public required string Client { get; init; }

	public required string OperatingSystem { get; init; }

	public required string BuildMode { get; init; }

	public required string Validation { get; init; }

	public int ValidationVersion { get; init; }

	public bool IsReplay { get; init; }

	public bool ProhibitedMods { get; init; }

	public byte GameMode { get; init; }

	public bool TimeAttackOrRaceFinished { get; init; }

	public required AddGameData GameData { get; init; }

	[Required]
	[MaxLength(ReplayConstants.MaxFileSize, ErrorMessage = ReplayConstants.MaxFileSizeErrorMessage)]
	public required byte[] ReplayData { get; init; }

	public int Status { get; set; }
}
