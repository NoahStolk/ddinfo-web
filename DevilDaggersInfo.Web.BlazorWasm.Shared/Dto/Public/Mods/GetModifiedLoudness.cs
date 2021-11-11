namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Mods;

public class GetModifiedLoudness
{
	public string AssetName { get; init; } = null!;

	public bool IsProhibited { get; init; }

	public float DefaultLoudness { get; init; }

	public float ModifiedLoudness { get; init; }
}
