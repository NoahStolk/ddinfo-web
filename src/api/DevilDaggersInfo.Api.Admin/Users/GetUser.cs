namespace DevilDaggersInfo.Api.Admin.Users;

public record GetUser
{
	public int Id { get; init; }

	public string? Name { get; init; }

	public bool IsAdmin { get; init; }

	public bool IsCustomLeaderboardsMaintainer { get; init; }

	public bool IsModsMaintainer { get; init; }

	public bool IsPlayersMaintainer { get; init; }

	public bool IsSpawnsetsMaintainer { get; init; }

	public int? PlayerId { get; init; }

	public string PlayerName { get; init; } = null!;
}
