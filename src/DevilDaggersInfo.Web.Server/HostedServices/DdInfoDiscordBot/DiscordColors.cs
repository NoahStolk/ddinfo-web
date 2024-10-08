using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Structs;
using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.Server.HostedServices.DdInfoDiscordBot;

public static class DiscordColors
{
	public static DiscordColor Default { get; } = ToDiscordColor(Daggers.Default.Color);
	public static DiscordColor Bronze { get; } = ToDiscordColor(Daggers.Bronze.Color);
	public static DiscordColor Silver { get; } = ToDiscordColor(Daggers.Silver.Color);
	public static DiscordColor Golden { get; } = ToDiscordColor(Daggers.Golden.Color);
	public static DiscordColor Devil { get; } = ToDiscordColor(Daggers.Devil.Color);
	public static DiscordColor Leviathan { get; } = ToDiscordColor(Daggers.Leviathan.Color);

	public static DiscordColor None { get; } = new(127, 143, 127);

	private static DiscordColor ToDiscordColor(Color color)
	{
		return new DiscordColor(color.R, color.G, color.B);
	}
}
