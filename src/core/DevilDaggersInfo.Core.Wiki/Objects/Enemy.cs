namespace DevilDaggersInfo.Core.Wiki.Objects;

public record Enemy(GameVersion GameVersion, string Name, Color Color, int Hp, int Gems, int NoFarmGems, Death Death, HomingDamage HomingDamage, int? FirstSpawnSecond, params Enemy[] SpawnedBy)
{
	public int GemHp => Hp / Gems;

	// TODO: Move to client.
	public string GetGemHpString()
		=> $"({GemHp} x {Gems})";

	// TODO: Move to client.
	public string GetImageName()
	{
		return Name
			.Replace(" IV", "-4")
			.Replace(" III", "-3")
			.Replace(" II", "-2")
			.Replace(" I", "-1")
			.Replace(' ', '-')
			.ToLower();
	}
}
