namespace DevilDaggersInfo.Web.ApiSpec.Admin;

public record Page<T>
{
	public required List<T> Results { get; init; }

	public required int TotalResults { get; init; }
}
