namespace DevilDaggersInfo.Core.Wiki.Objects;

public record Enemy(GameVersion GameVersion, string Name, Color Color, int Hp, int Gems, int NoFarmGems, Death Death, HomingDamage HomingDamage, int? FirstSpawnSecond, params Enemy[] SpawnedBy)
	: DevilDaggersObject(GameVersion, Name, Color)
{
	public int GemHp => Hp / Gems;

	public string GetGemHpString()
		=> $"({GemHp} x {Gems})";
}
