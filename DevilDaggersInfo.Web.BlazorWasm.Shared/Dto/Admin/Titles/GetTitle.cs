namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Titles;

public class GetTitle : IGetDto<int>
{
	public int Id { get; init; }

	public string Name { get; init; } = null!;
}
