using DevilDaggersInfo.Core.Spawnset.Enums;

namespace DevilDaggersInfo.Core.Spawnset
{
	public record SpawnsetSummary(int SpawnVersion, int WorldVersion, GameMode GameMode, int NonLoopSpawnCount, int LoopSpawnCount, float? NonLoopLength, float? LoopLength, HandLevel HandLevel, int AdditionalGems, float TimerStart);
}
