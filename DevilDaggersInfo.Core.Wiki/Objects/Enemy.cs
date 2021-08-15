namespace DevilDaggersInfo.Core.Wiki.Objects;

// TODO: Refactor.
public record Enemy<TDeathType>(GameVersions GameVersions, string Name, Color Color, int Hp, int Gems, int NoFarmGems, TDeathType DeathType, float Homing3, float Homing4, int? FirstSpawnSecond, params Enemy<TDeathType>[] SpawnedBy)
	: DevilDaggersObject(GameVersions, Name, Color)
	where TDeathType : Enum
{
	public int GemHp => Hp / Gems;

	public string GetGemHpString()
		=> $"({GemHp} x {Gems})";
}
