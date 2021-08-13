using DevilDaggersCore.Game;
using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot
{
	public static class DiscordColors
	{
		public static DiscordColor Default { get; } = new(GameInfo.V31Default.ColorCode);
		public static DiscordColor Bronze { get; } = new(GameInfo.V31Bronze.ColorCode);
		public static DiscordColor Silver { get; } = new(GameInfo.V31Silver.ColorCode);
		public static DiscordColor Golden { get; } = new(GameInfo.V31Golden.ColorCode);
		public static DiscordColor Devil { get; } = new(GameInfo.V31Devil.ColorCode);
		public static DiscordColor Leviathan { get; } = new(GameInfo.V31LeviathanDagger.ColorCode);
	}
}
