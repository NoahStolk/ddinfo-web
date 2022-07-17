namespace DevilDaggersInfo.Api.Ddiam;

public record GetTool
{
	public string Name { get; init; } = null!;

	public string Version { get; init; } = null!;
}
