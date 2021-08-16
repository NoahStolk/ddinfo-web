namespace DevilDaggersInfo.Core.Wiki.Objects;

public record Enemy(GameVersionFlags GameVersions, string Name, Color Color, int Hp, int Gems, int NoFarmGems, Death Death, HomingDamage HomingDamage, int? FirstSpawnSecond, params Enemy[] SpawnedBy)
	: DevilDaggersObject(GameVersions, Name, Color)
{
	public int GemHp => Hp / Gems;

	public string GetGemHpString()
		=> $"({GemHp} x {Gems})";
}
