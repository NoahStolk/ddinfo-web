namespace DevilDaggersInfo.Web.Server.Clients.Clubber;

internal sealed record DdUser
{
	public required ulong DiscordId { get; init; }

	public required int LeaderboardId { get; init; }
}
