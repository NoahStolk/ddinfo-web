namespace DevilDaggersInfo.Api.Ddcl;

public record Page<T>
{
	public List<T> Results { get; init; } = null!;

	public int TotalResults { get; init; }
}
