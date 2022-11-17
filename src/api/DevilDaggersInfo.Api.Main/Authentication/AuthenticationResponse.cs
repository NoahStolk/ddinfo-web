namespace DevilDaggersInfo.Api.Main.Authentication;

public record AuthenticationResponse
{
	public int Id { get; init; }

	public required string Name { get; init; }

	public required List<string> RoleNames { get; init; }

	public int? PlayerId { get; init; }
}
