namespace DevilDaggersInfo.Core.Wiki.Objects;

// TODO: Refactor.
public record Enemy(GameVersions GameVersions, string Name, Color Color, int Hp, int Gems, int NoFarmGems, Death? Death, float? Homing3, float? Homing4, int? FirstSpawnSecond, params Enemy[] SpawnedBy)
	: DevilDaggersObject(GameVersions, Name, Color)
{
	public int GemHp => Hp / Gems;

	public string GetGemHpString()
		=> $"({GemHp} x {Gems})";
}
