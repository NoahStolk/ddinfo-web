using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Extensions;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class GameVersionExtensions
{
	public static string GetGameVersionString(this GameVersion? gameVersion)
	{
		if (!gameVersion.HasValue)
			return "Pre-release";

		return gameVersion.Value.ToDisplayString();
	}

	public static ApiSpec.Main.WorldRecords.GameVersion ToMainApi(this GameVersion gameVersion) => gameVersion switch
	{
		GameVersion.V1_0 => ApiSpec.Main.WorldRecords.GameVersion.V1_0,
		GameVersion.V2_0 => ApiSpec.Main.WorldRecords.GameVersion.V2_0,
		GameVersion.V3_0 => ApiSpec.Main.WorldRecords.GameVersion.V3_0,
		GameVersion.V3_1 => ApiSpec.Main.WorldRecords.GameVersion.V3_1,
		GameVersion.V3_2 => ApiSpec.Main.WorldRecords.GameVersion.V3_2,
		_ => throw new UnreachableException($"Unknown game version {gameVersion}."),
	};

	public static GameVersion ToCore(this ApiSpec.Main.WorldRecords.GameVersion gameVersion) => gameVersion switch
	{
		ApiSpec.Main.WorldRecords.GameVersion.V1_0 => GameVersion.V1_0,
		ApiSpec.Main.WorldRecords.GameVersion.V2_0 => GameVersion.V2_0,
		ApiSpec.Main.WorldRecords.GameVersion.V3_0 => GameVersion.V3_0,
		ApiSpec.Main.WorldRecords.GameVersion.V3_1 => GameVersion.V3_1,
		ApiSpec.Main.WorldRecords.GameVersion.V3_2 => GameVersion.V3_2,
		_ => throw new UnreachableException($"Unknown game version {gameVersion}."),
	};
}
