namespace DevilDaggersInfo.Api.Admin;

public record Page<T>
{
	public required List<T> Results { get; init; }

	public int TotalResults { get; init; }
}
