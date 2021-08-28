namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Tools;

public class GetChange
{
	public string Description { get; init; } = null!;

	public IReadOnlyList<GetChange>? SubChanges { get; init; }
}
