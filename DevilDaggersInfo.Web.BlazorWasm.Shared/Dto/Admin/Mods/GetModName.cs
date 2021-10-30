namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Mods;

public class GetModName : IGetDto
{
	public int Id { get; init; }

	public string Name { get; init; } = null!;
}
