using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Domain.Utils;

public static class CustomLeaderboardUtils
{
	// TODO: Add unit tests.
	public static CustomLeaderboardDagger GetDaggerFromStat(CustomLeaderboardRankSorting rankSorting, int stat, int leviathan, int devil, int golden, int silver, int bronze)
	{
		if (rankSorting.IsAscending())
		{
			if (stat <= leviathan)
				return CustomLeaderboardDagger.Leviathan;
			if (stat <= devil)
				return CustomLeaderboardDagger.Devil;
			if (stat <= golden)
				return CustomLeaderboardDagger.Golden;
			if (stat <= silver)
				return CustomLeaderboardDagger.Silver;
			if (stat <= bronze)
				return CustomLeaderboardDagger.Bronze;

			return CustomLeaderboardDagger.Default;
		}

		if (stat >= leviathan)
			return CustomLeaderboardDagger.Leviathan;
		if (stat >= devil)
			return CustomLeaderboardDagger.Devil;
		if (stat >= golden)
			return CustomLeaderboardDagger.Golden;
		if (stat >= silver)
			return CustomLeaderboardDagger.Silver;
		if (stat >= bronze)
			return CustomLeaderboardDagger.Bronze;

		return CustomLeaderboardDagger.Default;
	}
}
