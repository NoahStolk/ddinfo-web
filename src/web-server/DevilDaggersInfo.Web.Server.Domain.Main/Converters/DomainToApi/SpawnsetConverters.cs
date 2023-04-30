using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using System.Diagnostics;
using MainApi = DevilDaggersInfo.Api.Main.Spawnsets;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Converters.DomainToApi;

public static class SpawnsetConverters
{
	public static MainApi.GameMode ToMainApi(this SpawnsetGameMode gameMode) => gameMode switch
	{
		SpawnsetGameMode.Survival => MainApi.GameMode.Survival,
		SpawnsetGameMode.TimeAttack => MainApi.GameMode.TimeAttack,
		SpawnsetGameMode.Race => MainApi.GameMode.Race,
		_ => throw new UnreachableException(),
	};
}
