namespace DevilDaggersInfo.Api.App.Spawnsets;

public record GetSpawnsetByHashCustomEntry
{
	public required int CustomEntryId { get; init; }

	public required int Time { get; init; }

	public required bool HasReplay { get; init; }
}
