namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Titles;

public class GetTitle : IGetDto
{
	public int Id { get; init; }

	public string Name { get; init; } = null!;
}
