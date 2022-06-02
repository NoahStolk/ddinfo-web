using MainApi = DevilDaggersInfo.Api.Main.WorldRecords;

namespace DevilDaggersInfo.Web.Client.Extensions;

// TODO: Remove global using for wiki project.
public static class GameVersionExtensions
{
	public static string GetGameVersionString(this MainApi.GameVersion? gameVersion)
	{
		if (!gameVersion.HasValue)
			return "Pre-release";

		return gameVersion.Value.ToDisplayString();
	}

	public static string ToDisplayString(this MainApi.GameVersion gameVersion) => gameVersion switch
	{
		MainApi.GameVersion.V1_0 => "V1",
		MainApi.GameVersion.V2_0 => "V2",
		MainApi.GameVersion.V3_0 => "V3",
		MainApi.GameVersion.V3_1 => "V3.1",
		MainApi.GameVersion.V3_2 => "V3.2",
		_ => throw new NotSupportedException($"{nameof(MainApi.GameVersion)} '{gameVersion}' is not supported."),
	};
}
