namespace DevilDaggersInfo.Api.App;

public record Page<T>
{
	public required List<T> Results { get; init; }

	public int TotalResults { get; init; }
}
