namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Tools.Obsolete;

[Obsolete("This DTO is a copy of DevilDaggersInfo.Web.BlazorWasm.Server.Transients.ChangelogEntry which cannot be removed due to backwards compatibility with the current tools.")]
public class GetChangelogEntryObsolete
{
	public Version VersionNumber { get; init; } = null!;

	public DateTime Date { get; init; }

	public IReadOnlyList<GetChangeObsolete> Changes { get; init; } = new List<GetChangeObsolete>();
}
