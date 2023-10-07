namespace DevilDaggersInfo.Web.ApiSpec.Ddae.Assets;

public record GetAssetInfo
{
	public required string Name { get; init; }

	public required string Description { get; init; }

	public required List<string> Tags { get; init; }
}
