using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record AddUploadRequest
{
	[MaxLength(16)]
	[MinLength(16)]
	public required byte[] SurvivalHashMd5 { get; init; }

	public required int PlayerId { get; init; }

	[StringLength(32)]
	public required string PlayerName { get; init; }

	public required int ReplayPlayerId { get; init; }

	public required double TimeInSeconds { get; init; }

	[MaxLength(4)]
	[MinLength(4)]
	public required byte[] TimeAsBytes { get; init; }

	public required int GemsCollected { get; init; }

	public required int EnemiesKilled { get; init; }

	public required int DaggersFired { get; init; }

	public required int DaggersHit { get; init; }

	public required int EnemiesAlive { get; init; }

	/// <summary>
	/// This value is not reliable in game memory and therefore no longer used. It is now only used for the request signature.
	/// </summary>
	public required int HomingStored { get; init; }

	public required int HomingEaten { get; init; }

	public required int GemsDespawned { get; init; }

	public required int GemsEaten { get; init; }

	public required int GemsTotal { get; init; }

	public required byte DeathType { get; init; }

	public required double LevelUpTime2InSeconds { get; init; }

	public required double LevelUpTime3InSeconds { get; init; }

	public required double LevelUpTime4InSeconds { get; init; }

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

	public required int ValidationVersion { get; init; }

	public required bool IsReplay { get; init; }

	public required bool ProhibitedMods { get; init; }

	public required byte GameMode { get; init; }

	public required bool TimeAttackOrRaceFinished { get; init; }

	public required AddGameData GameData { get; init; }

	[Required]
	[MaxLength(ReplayConstants.MaxFileSize, ErrorMessage = ReplayConstants.MaxFileSizeErrorMessage)]
	public required byte[] ReplayData { get; init; }

	public required int Status { get; init; }
}
