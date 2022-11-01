using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Admin.Mods;

public record GetMod
{
	public int Id { get; init; }

	public required string Name { get; init; }

	public bool IsHidden { get; init; }

	public DateTime LastUpdated { get; init; }

	public string? TrailerUrl { get; init; }

	public string? HtmlDescription { get; init; }

	public ModTypes ModTypes { get; init; }

	public string? Url { get; init; }

	public List<int>? PlayerIds { get; init; }

	public List<string>? BinaryNames { get; init; }

	public List<string>? ScreenshotNames { get; init; }
}
