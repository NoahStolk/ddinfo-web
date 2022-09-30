using DevilDaggersInfo.Types.Core.Spawnsets;

namespace DevilDaggersInfo.Core.Spawnset;

public readonly struct Spawn
{
	public Spawn(EnemyType enemyType, float delay)
	{
		EnemyType = enemyType;
		Delay = delay;
	}

	public EnemyType EnemyType { get; }

	public float Delay { get; }

	public override string ToString()
		=> $"{Delay:0.0000}: {EnemyType}";
}
