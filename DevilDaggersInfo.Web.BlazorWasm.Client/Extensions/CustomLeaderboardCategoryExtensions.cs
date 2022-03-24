using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;

public static class CustomLeaderboardCategoryExtensions
{
	public static string GetDescription(this CustomLeaderboardCategory category) => category switch
	{
		CustomLeaderboardCategory.Survival => "Survive as long as you can.",
		CustomLeaderboardCategory.TimeAttack => "Kill all enemies as quickly as possible.",
		CustomLeaderboardCategory.Speedrun => "Jump into the void as quickly as possible. Note that this category has been superseded by the Race category; new Speedrun leaderboards will not be added.",
		CustomLeaderboardCategory.Pacifist => "Survive as long as you can, but without damaging or killing any enemies.",
		CustomLeaderboardCategory.Race => "Grab the dagger as fast as you can.",
		_ => throw new NotSupportedException($"{nameof(CustomLeaderboardCategory)} '{category}' is not supported."),
	};
}
