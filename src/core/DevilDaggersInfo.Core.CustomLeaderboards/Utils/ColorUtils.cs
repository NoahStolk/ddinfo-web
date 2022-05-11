using DevilDaggersCustomLeaderboards.Clients;
using DevilDaggersInfo.Core.CustomLeaderboards.Enums;

namespace DevilDaggersInfo.Core.CustomLeaderboards.Utils;

public static class ColorUtils
{
	public const CustomColor ForegroundDefault = CustomColor.Gray;
	public const CustomColor BackgroundDefault = CustomColor.Black;

	public const CustomColor Error = CustomColor.Red;
	public const CustomColor Warning = CustomColor.Yellow;
	public const CustomColor Success = CustomColor.Green;

	public const CustomColor Better = CustomColor.Green;
	public const CustomColor Neutral = CustomColor.Gray;
	public const CustomColor Worse = CustomColor.Red;

	public const CustomColor Fallen = CustomColor.Gray;
	public const CustomColor Swarmed = CustomColor.Skull;
	public const CustomColor Impaled = CustomColor.Skull;
	public const CustomColor Gored = CustomColor.Skull;
	public const CustomColor Infested = CustomColor.Yellow;
	public const CustomColor Opened = CustomColor.Skull;
	public const CustomColor Purged = CustomColor.Squid;
	public const CustomColor Desecrated = CustomColor.Squid;
	public const CustomColor Sacrificed = CustomColor.Squid;
	public const CustomColor Eviscerated = CustomColor.Gray;
	public const CustomColor Annihilated = CustomColor.Gigapede;
	public const CustomColor Intoxicated = CustomColor.Green;
	public const CustomColor Envenomated = CustomColor.Green;
	public const CustomColor Incarnated = CustomColor.Red;
	public const CustomColor Discarnated = CustomColor.Magenta;
	public const CustomColor Entangled = CustomColor.Thorn;
	public const CustomColor Haunted = CustomColor.Ghostpede;

	public const CustomColor Leviathan = CustomColor.LeviathanDagger;
	public const CustomColor Devil = CustomColor.Red;
	public const CustomColor Golden = CustomColor.Yellow;
	public const CustomColor Silver = CustomColor.Gray;
	public const CustomColor Bronze = CustomColor.Bronze;
	public const CustomColor Default = CustomColor.DarkGray;

	public static CustomColor GetDaggerHighlightColor(CustomColor daggerColor) => daggerColor switch
	{
		Silver or Golden => BackgroundDefault,
		_ => ForegroundDefault,
	};

	public static CustomColor GetDeathColor(int deathType) => deathType switch
	{
		1 => Swarmed,
		2 => Impaled,
		3 => Gored,
		4 => Infested,
		5 => Opened,
		6 => Purged,
		7 => Desecrated,
		8 => Sacrificed,
		9 => Eviscerated,
		10 => Annihilated,
		11 => Intoxicated,
		12 => Envenomated,
		13 => Incarnated,
		14 => Discarnated,
		15 => Entangled,
		16 => Haunted,
		_ => Fallen,
	};

	public static CustomColor GetDaggerColor(double time, GetCustomLeaderboardDdcl leaderboard)
	{
		if (leaderboard.Daggers == null)
			return Silver;

		if (Compare(time, leaderboard.Daggers.Leviathan))
			return Leviathan;
		if (Compare(time, leaderboard.Daggers.Devil))
			return Devil;
		if (Compare(time, leaderboard.Daggers.Golden))
			return Golden;
		if (Compare(time, leaderboard.Daggers.Silver))
			return Silver;
		if (Compare(time, leaderboard.Daggers.Bronze))
			return Bronze;
		return Default;

		bool Compare(double time, double daggerTime)
		{
			if (leaderboard.IsAscending)
				return time <= daggerTime;
			return time >= daggerTime;
		}
	}

	public static CustomColor GetImprovementColor<T>(T n)
		where T : IComparable<T>
	{
		int comparison = n.CompareTo(default);
		return comparison == 0 ? Neutral : comparison == 1 ? Better : Worse;
	}
}
