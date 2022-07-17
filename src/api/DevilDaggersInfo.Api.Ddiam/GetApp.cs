namespace DevilDaggersInfo.Api.Ddiam;

public record GetApp
{
	public string Name { get; init; } = null!;

	public string Version { get; init; } = null!;

	public BuildType BuildType { get; init; }
}
