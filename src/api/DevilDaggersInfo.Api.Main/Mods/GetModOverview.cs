using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Main.Mods;

public record GetModOverview
{
	public int Id { get; init; }

	public required string Name { get; init; }

	public required List<string> Authors { get; init; }

	public DateTime LastUpdated { get; init; }

	public ModTypes ModTypes { get; init; }

	public bool IsHosted { get; init; }

	public bool? ContainsProhibitedAssets { get; init; }
}
