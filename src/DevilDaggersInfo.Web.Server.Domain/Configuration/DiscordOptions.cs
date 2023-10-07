namespace DevilDaggersInfo.Web.Server.Domain.Configuration;

public record DiscordOptions
{
	public required string BotToken { get; init; }
}
