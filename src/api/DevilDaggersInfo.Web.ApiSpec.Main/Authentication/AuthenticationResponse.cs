namespace DevilDaggersInfo.Api.Main.Authentication;

public record AuthenticationResponse
{
	public required int Id { get; init; }

	public required string Name { get; init; }

	public required List<string> RoleNames { get; init; }

	public required int? PlayerId { get; init; }
}
