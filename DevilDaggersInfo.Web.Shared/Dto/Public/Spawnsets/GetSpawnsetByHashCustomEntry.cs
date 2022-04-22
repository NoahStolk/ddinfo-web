namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Spawnsets;

public record GetSpawnsetByHashCustomEntry
{
	public int CustomEntryId { get; init; }

	public int Time { get; init; }

	public bool HasReplay { get; init; }
}
