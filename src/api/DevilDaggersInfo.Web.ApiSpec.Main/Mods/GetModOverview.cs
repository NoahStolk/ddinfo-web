namespace DevilDaggersInfo.Api.Main.Mods;

public record GetModOverview
{
	public required int Id { get; init; }

	public required string Name { get; init; }

	public required List<string> Authors { get; init; }

	public required DateTime LastUpdated { get; init; }

	public required ModTypes ModTypes { get; init; }

	public required bool IsHosted { get; init; }

	public required bool? ContainsProhibitedAssets { get; init; }
}
