namespace DevilDaggersInfo.Web.Shared.Dto.Public.Tools;

public record GetToolVersionChange
{
	public string Description { get; init; } = null!;

	public IReadOnlyList<GetToolVersionChange>? SubChanges { get; init; }
}
