using DevilDaggersInfo.Core.Wiki;
using DSharpPlus.Entities;
using System.Numerics;

namespace DevilDaggersInfo.Web.Server.HostedServices.DdInfoDiscordBot;

internal static class DiscordColors
{
	public static DiscordColor Default { get; } = ToDiscordColor(Daggers.Default.Color);
	public static DiscordColor Bronze { get; } = ToDiscordColor(Daggers.Bronze.Color);
	public static DiscordColor Silver { get; } = ToDiscordColor(Daggers.Silver.Color);
	public static DiscordColor Golden { get; } = ToDiscordColor(Daggers.Golden.Color);
	public static DiscordColor Devil { get; } = ToDiscordColor(Daggers.Devil.Color);
	public static DiscordColor Leviathan { get; } = ToDiscordColor(Daggers.Leviathan.Color);

	public static DiscordColor None { get; } = new(127, 143, 127);

	private static DiscordColor ToDiscordColor(Vector3 color)
	{
		return new DiscordColor(color.X * 255, color.Y * 255, color.Z * 255);
	}
}
