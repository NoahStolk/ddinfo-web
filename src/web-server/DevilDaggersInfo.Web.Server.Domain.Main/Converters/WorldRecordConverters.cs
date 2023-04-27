using DevilDaggersInfo.Api.Main.WorldRecords;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Converters;

public static class WorldRecordConverters
{
	public static GameVersion ToMainApi(this Types.Core.Wiki.GameVersion gameVersion) => gameVersion switch
	{
		Types.Core.Wiki.GameVersion.V1_0 => GameVersion.V1_0,
		Types.Core.Wiki.GameVersion.V2_0 => GameVersion.V2_0,
		Types.Core.Wiki.GameVersion.V3_0 => GameVersion.V3_0,
		Types.Core.Wiki.GameVersion.V3_1 => GameVersion.V3_1,
		Types.Core.Wiki.GameVersion.V3_2 => GameVersion.V3_2,
		_ => throw new UnreachableException(),
	};
}
