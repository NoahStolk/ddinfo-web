using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Admin.Mods;

public record GetModForOverview : IAdminOverviewGetDto
{
	public required int Id { get; init; }

	public required string Name { get; init; }

	public required bool IsHidden { get; init; }

	public required DateTime LastUpdated { get; init; }

	public required string? TrailerUrl { get; init; }

	public required string? HtmlDescription { get; init; }

	public required ModTypes ModTypes { get; init; }

	public required string? Url { get; init; }
}
