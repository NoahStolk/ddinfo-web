namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Users;

public class GetUser : IGetDto
{
	public int Id { get; init; }

	public string? Name { get; init; }

	public bool IsAdmin { get; init; }

	public bool IsCustomLeaderboardsMaintainer { get; init; }

	public bool IsModsMaintainer { get; init; }

	public bool IsPlayersMaintainer { get; init; }

	public bool IsSpawnsetsMaintainer { get; init; }
}
