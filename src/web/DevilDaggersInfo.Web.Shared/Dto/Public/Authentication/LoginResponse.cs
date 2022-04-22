namespace DevilDaggersInfo.Web.Shared.Dto.Public.Authentication;

public record LoginResponse
{
	public int Id { get; init; }

	public string Name { get; init; } = null!;

	public string Token { get; init; } = null!;
}
