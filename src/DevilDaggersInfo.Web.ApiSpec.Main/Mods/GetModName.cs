namespace DevilDaggersInfo.Web.ApiSpec.Main.Mods;

public sealed record GetModName
{
	public required int Id { get; init; }

	public required string Name { get; init; }
}
