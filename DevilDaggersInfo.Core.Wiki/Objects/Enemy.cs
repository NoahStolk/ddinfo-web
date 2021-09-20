namespace DevilDaggersInfo.Core.Wiki.Objects;

public readonly record struct Enemy(GameVersion GameVersion, string Name, Color Color, int Hp, int Gems, int NoFarmGems, Death Death, HomingDamage HomingDamage, int? FirstSpawnSecond, params Enemy[] SpawnedBy)
{
	public int GemHp => Hp / Gems;

	public string GetGemHpString()
		=> $"({GemHp} x {Gems})";
}
