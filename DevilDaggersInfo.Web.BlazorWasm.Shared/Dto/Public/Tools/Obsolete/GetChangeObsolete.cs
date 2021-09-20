namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Tools.Obsolete;

[Obsolete("This DTO is a copy of DevilDaggersInfo.Web.BlazorWasm.Server.Transients.Change which cannot be removed due to backwards compatibility with the current tools.")]
public class GetChangeObsolete
{
	public string Description { get; init; } = null!;

	public IReadOnlyList<GetChangeObsolete>? SubChanges { get; init; }
}
