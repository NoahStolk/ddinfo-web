namespace DevilDaggersInfo.Api.Main.Authentication;

public record LoginResponse
{
	public int Id { get; init; }

	public string Name { get; init; } = null!;

	public string Token { get; init; } = null!;
}
