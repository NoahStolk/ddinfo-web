namespace DevilDaggersInfo.Web.ApiSpec.Main;

public record Page<T>
{
	public required List<T> Results { get; init; }

	public required int TotalResults { get; init; }
}
