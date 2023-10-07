namespace DevilDaggersInfo.Web.Server.Clients.Clubber;

public record DdUser
{
	public required ulong DiscordId { get; init; }

	public required int LeaderboardId { get; init; }
}
