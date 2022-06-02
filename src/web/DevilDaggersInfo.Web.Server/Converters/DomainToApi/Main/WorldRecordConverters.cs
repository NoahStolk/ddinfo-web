using MainApi = DevilDaggersInfo.Api.Main.WorldRecords;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

public static class WorldRecordConverters
{
	public static MainApi.GameVersion ToMainApi(this GameVersion gameVersion) => gameVersion switch
	{
		GameVersion.V1_0 => MainApi.GameVersion.V1_0,
		GameVersion.V2_0 => MainApi.GameVersion.V2_0,
		GameVersion.V3_0 => MainApi.GameVersion.V3_0,
		GameVersion.V3_1 => MainApi.GameVersion.V3_1,
		GameVersion.V3_2 => MainApi.GameVersion.V3_2,
		_ => throw new ArgumentOutOfRangeException(nameof(gameVersion)),
	};
}
