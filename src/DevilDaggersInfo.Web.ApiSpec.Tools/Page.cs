namespace DevilDaggersInfo.Web.ApiSpec.Tools;

public record Page<T>
{
	public required List<T> Results { get; init; }

	public required int TotalResults { get; init; }
}
