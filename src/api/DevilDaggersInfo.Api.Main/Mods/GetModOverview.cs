using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Main.Mods;

public record GetModOverview
{
	public int Id { get; init; }

	public string Name { get; init; } = null!;

	public List<string> Authors { get; init; } = null!;

	public DateTime LastUpdated { get; init; }

	public ModTypes ModTypes { get; init; }

	public bool IsHosted { get; init; }

	public bool? ContainsProhibitedAssets { get; init; }
}
