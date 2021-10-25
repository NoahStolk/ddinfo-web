namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Titles;

public class AddTitle
{
	[StringLength(16)]
	public string Name { get; init; } = null!;

	public List<int>? PlayerIds { get; init; }
}
