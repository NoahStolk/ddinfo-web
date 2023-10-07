using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using System.Diagnostics;
using MainApi = DevilDaggersInfo.Web.ApiSpec.Main.Spawnsets;

namespace DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Main;

public static class SpawnsetConverters
{
	public static SpawnsetGameMode ToDomain(this MainApi.GameMode gameMode) => gameMode switch
	{
		MainApi.GameMode.Survival => SpawnsetGameMode.Survival,
		MainApi.GameMode.TimeAttack => SpawnsetGameMode.TimeAttack,
		MainApi.GameMode.Race => SpawnsetGameMode.Race,
		_ => throw new UnreachableException(),
	};
}
