namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;

public record Page<T>
{
	public List<T> Results { get; init; } = null!;
	public int TotalResults { get; init; }
}
