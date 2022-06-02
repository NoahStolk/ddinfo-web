namespace DevilDaggersInfo.Api.Main.Mods;

public record GetModName
{
	public int Id { get; init; }

	public string Name { get; init; } = null!;
}
