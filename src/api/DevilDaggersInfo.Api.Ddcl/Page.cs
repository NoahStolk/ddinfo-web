namespace DevilDaggersInfo.Api.Ddcl;

[Obsolete("DDCL 1.8.3 will be removed.")]
public record Page<T>
{
	public required List<T> Results { get; init; }

	public int TotalResults { get; init; }
}
