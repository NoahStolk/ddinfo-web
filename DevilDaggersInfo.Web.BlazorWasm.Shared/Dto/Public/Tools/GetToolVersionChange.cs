namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Tools;

public class GetToolVersionChange
{
	public string Description { get; init; } = null!;

	public IReadOnlyList<GetToolVersionChange>? SubChanges { get; init; }
}
