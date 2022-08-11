using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Domain.Utils;

public static class CustomLeaderboardUtils
{
	// TODO: Add unit tests.
	public static CustomLeaderboardDagger GetDaggerFromTime(CustomLeaderboardCategory category, int time, int leviathan, int devil, int golden, int silver, int bronze)
	{
		if (category.IsAscending())
		{
			if (time <= leviathan)
				return CustomLeaderboardDagger.Leviathan;
			if (time <= devil)
				return CustomLeaderboardDagger.Devil;
			if (time <= golden)
				return CustomLeaderboardDagger.Golden;
			if (time <= silver)
				return CustomLeaderboardDagger.Silver;
			if (time <= bronze)
				return CustomLeaderboardDagger.Bronze;

			return CustomLeaderboardDagger.Default;
		}

		if (time >= leviathan)
			return CustomLeaderboardDagger.Leviathan;
		if (time >= devil)
			return CustomLeaderboardDagger.Devil;
		if (time >= golden)
			return CustomLeaderboardDagger.Golden;
		if (time >= silver)
			return CustomLeaderboardDagger.Silver;
		if (time >= bronze)
			return CustomLeaderboardDagger.Bronze;

		return CustomLeaderboardDagger.Default;
	}
}
