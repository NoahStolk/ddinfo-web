namespace DevilDaggersInfo.Api.Admin.Players;

public record GetPlayerName
{
	public int Id { get; init; }

	public required string PlayerName { get; init; }
}
