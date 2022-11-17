using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Admin.Mods;

public record GetModForOverview : IAdminOverviewGetDto
{
	public int Id { get; init; }

	public required string Name { get; init; }

	public bool IsHidden { get; init; }

	public DateTime LastUpdated { get; init; }

	public string? TrailerUrl { get; init; }

	public string? HtmlDescription { get; init; }

	public ModTypes ModTypes { get; init; }

	public string? Url { get; init; }
}
