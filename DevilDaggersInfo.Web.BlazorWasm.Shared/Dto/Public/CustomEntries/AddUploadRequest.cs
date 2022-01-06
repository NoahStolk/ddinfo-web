namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;

public class AddUploadRequest
{
	// TODO: Add MaxLength (and min length) 16?
	public byte[] SurvivalHashMd5 { get; init; } = null!;

	public int PlayerId { get; init; }

	[StringLength(32)]
	public string PlayerName { get; init; } = null!;

	public int ReplayPlayerId { get; init; }

	public int Time { get; set; } // Use set to fix replay times (bug in DD).

	public int GemsCollected { get; init; }

	public int EnemiesKilled { get; init; }

	public int DaggersFired { get; init; }

	public int DaggersHit { get; init; }

	public int EnemiesAlive { get; init; }

	public int HomingDaggers { get; set; } // Use set to fix negative values (bug in DD).

	public int HomingDaggersEaten { get; init; }

	public int GemsDespawned { get; init; }

	public int GemsEaten { get; init; }

	public int GemsTotal { get; init; }

	public byte DeathType { get; init; }

	public int LevelUpTime2 { get; init; }

	public int LevelUpTime3 { get; init; }

	public int LevelUpTime4 { get; init; }

	[StringLength(16)]
	public string ClientVersion { get; init; } = null!;

	public string Client { get; init; } = null!;

	public string OperatingSystem { get; init; } = null!;

	public string BuildMode { get; init; } = null!;

	public string Validation { get; set; } = null!; // Use set for unit tests.

	public bool IsReplay { get; init; }

	public bool ProhibitedMods { get; init; }

	public List<AddGameState> GameStates { get; init; } = new();

	[MaxLength(ReplayConstants.MaxFileSize, ErrorMessage = ReplayConstants.MaxFileSizeErrorMessage)]
	public byte[]? ReplayData { get; init; }

	public int Status { get; set; }
}
