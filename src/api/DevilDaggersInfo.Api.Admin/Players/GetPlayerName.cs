namespace DevilDaggersInfo.Api.Admin.Players;

public record GetPlayerName
{
	public int Id { get; init; }

	public string PlayerName { get; init; } = null!;
}
