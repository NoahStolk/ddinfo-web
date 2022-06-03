namespace DevilDaggersInfo.Api.Main.Spawnsets;

public record GetSpawnsetByHashCustomEntry
{
	public int CustomEntryId { get; init; }

	public int Time { get; init; }

	public bool HasReplay { get; init; }
}
