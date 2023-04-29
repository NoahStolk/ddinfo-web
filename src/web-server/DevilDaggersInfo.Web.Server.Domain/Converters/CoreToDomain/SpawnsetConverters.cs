using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Converters.CoreToDomain;

public static class SpawnsetConverters
{
	public static SpawnsetGameMode ToDomain(this DevilDaggersInfo.Core.Spawnset.GameMode gameMode)
	{
		return gameMode switch
		{
			DevilDaggersInfo.Core.Spawnset.GameMode.Survival => SpawnsetGameMode.Survival,
			DevilDaggersInfo.Core.Spawnset.GameMode.TimeAttack => SpawnsetGameMode.TimeAttack,
			DevilDaggersInfo.Core.Spawnset.GameMode.Race => SpawnsetGameMode.Race,
			_ => throw new($"Unknown game mode '{gameMode}'."),
		};
	}

	public static SpawnsetHandLevel ToDomain(this DevilDaggersInfo.Core.Spawnset.HandLevel handLevel)
	{
		return handLevel switch
		{
			DevilDaggersInfo.Core.Spawnset.HandLevel.Level1 => SpawnsetHandLevel.Level1,
			DevilDaggersInfo.Core.Spawnset.HandLevel.Level2 => SpawnsetHandLevel.Level2,
			DevilDaggersInfo.Core.Spawnset.HandLevel.Level3 => SpawnsetHandLevel.Level3,
			DevilDaggersInfo.Core.Spawnset.HandLevel.Level4 => SpawnsetHandLevel.Level4,
			_ => throw new($"Unknown hand level '{handLevel}'."),
		};
	}
}
