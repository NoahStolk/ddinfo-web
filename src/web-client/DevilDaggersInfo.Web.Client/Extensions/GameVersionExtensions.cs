using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Core.Wiki.Extensions;
using DevilDaggersInfo.Types.Core.Wiki;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class GameVersionExtensions
{
	public static string GetGameVersionString(this GameVersion? gameVersion)
	{
		if (!gameVersion.HasValue)
			return "Pre-release";

		return gameVersion.Value.ToDisplayString();
	}
}
