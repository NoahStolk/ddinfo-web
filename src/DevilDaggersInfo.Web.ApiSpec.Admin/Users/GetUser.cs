namespace DevilDaggersInfo.Web.ApiSpec.Admin.Users;

public record GetUser : IAdminOverviewGetDto
{
	public required int Id { get; init; }

	public required string? Name { get; init; }

	public required bool IsAdmin { get; init; }

	public required bool IsCustomLeaderboardsMaintainer { get; init; }

	public required bool IsModsMaintainer { get; init; }

	public required bool IsPlayersMaintainer { get; init; }

	public required bool IsSpawnsetsMaintainer { get; init; }

	public required int? PlayerId { get; init; }

	public required string PlayerName { get; init; }

	public required DateTime RegistrationDate { get; init; }
}
