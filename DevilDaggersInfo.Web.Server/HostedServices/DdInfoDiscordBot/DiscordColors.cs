using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;

public static class DiscordColors
{
	public static DiscordColor Default { get; } = ToDiscordColor(DaggersV3_2.Default.Color);
	public static DiscordColor Bronze { get; } = ToDiscordColor(DaggersV3_2.Bronze.Color);
	public static DiscordColor Silver { get; } = ToDiscordColor(DaggersV3_2.Silver.Color);
	public static DiscordColor Golden { get; } = ToDiscordColor(DaggersV3_2.Golden.Color);
	public static DiscordColor Devil { get; } = ToDiscordColor(DaggersV3_2.Devil.Color);
	public static DiscordColor Leviathan { get; } = ToDiscordColor(DaggersV3_2.Leviathan.Color);

	private static DiscordColor ToDiscordColor(Color color) => new(color.R, color.G, color.B);
}
