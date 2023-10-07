namespace DevilDaggersInfo.Web.ApiSpec.Admin.Players;

public record GetPlayerName
{
	public required int Id { get; init; }

	public required string PlayerName { get; init; }
}
