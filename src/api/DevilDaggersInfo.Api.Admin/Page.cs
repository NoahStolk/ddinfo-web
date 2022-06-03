namespace DevilDaggersInfo.Api.Admin;

public class Page<T>
{
	public List<T> Results { get; init; } = null!;

	public int TotalResults { get; init; }
}
