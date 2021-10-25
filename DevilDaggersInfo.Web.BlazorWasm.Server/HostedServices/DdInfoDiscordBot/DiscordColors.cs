using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;

public static class DiscordColors
{
	public static DiscordColor Default { get; } = ToDiscordColor(DaggersV3_1.Default.Color);
	public static DiscordColor Bronze { get; } = ToDiscordColor(DaggersV3_1.Bronze.Color);
	public static DiscordColor Silver { get; } = ToDiscordColor(DaggersV3_1.Silver.Color);
	public static DiscordColor Golden { get; } = ToDiscordColor(DaggersV3_1.Golden.Color);
	public static DiscordColor Devil { get; } = ToDiscordColor(DaggersV3_1.Devil.Color);
	public static DiscordColor Leviathan { get; } = ToDiscordColor(DaggersV3_1.Leviathan.Color);

	private static DiscordColor ToDiscordColor(Color color) => new(color.R, color.B, color.G);
}
