namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Mods;

public record GetModName
{
	public int Id { get; init; }

	public string Name { get; init; } = null!;
}
