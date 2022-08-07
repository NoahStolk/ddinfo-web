using DevilDaggersInfo.Api.Main.GameVersions;
using DevilDaggersInfo.Common.Exceptions;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class GameVersionExtensions
{
	public static string GetGameVersionString(this GameVersion? gameVersion)
	{
		if (!gameVersion.HasValue)
			return "Pre-release";

		return gameVersion.Value.ToDisplayString();
	}

	public static string ToDisplayString(this GameVersion gameVersion) => gameVersion switch
	{
		GameVersion.V1_0 => "V1",
		GameVersion.V2_0 => "V2",
		GameVersion.V3_0 => "V3",
		GameVersion.V3_1 => "V3.1",
		GameVersion.V3_2 => "V3.2",
		_ => throw new InvalidEnumConversionException(gameVersion),
	};
}
