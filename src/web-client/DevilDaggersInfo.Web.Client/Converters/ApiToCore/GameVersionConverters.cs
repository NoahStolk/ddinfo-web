using DevilDaggersInfo.Types.Core.Wiki;
using MainApi = DevilDaggersInfo.Api.Main.GameVersions;

namespace DevilDaggersInfo.Web.Client.Converters.ApiToCore;

public static class GameVersionConverters
{
	public static GameVersion ToCore(this MainApi.GameVersion gameVersion) => gameVersion switch
	{
		MainApi.GameVersion.V1_0 => GameVersion.V1_0,
		MainApi.GameVersion.V2_0 => GameVersion.V2_0,
		MainApi.GameVersion.V3_0 => GameVersion.V3_0,
		MainApi.GameVersion.V3_1 => GameVersion.V3_1,
		MainApi.GameVersion.V3_2 => GameVersion.V3_2,
		_ => throw new ArgumentOutOfRangeException(nameof(gameVersion)),
	};
}
