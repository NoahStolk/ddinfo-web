namespace DevilDaggersInfo.Web.Server.Domain.Configuration;

public sealed record DiscordOptions
{
	public required string BotToken { get; init; }
}
