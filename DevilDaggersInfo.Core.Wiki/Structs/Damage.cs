namespace DevilDaggersInfo.Core.Wiki.Structs;

public struct Damage
{
	public Damage(int? daggersPerShot, float? daggersPerSpraySecond)
	{
		DaggersPerShot = daggersPerShot;
		DaggersPerSpraySecond = daggersPerSpraySecond;
	}

	public int? DaggersPerShot { get; }
	public float? DaggersPerSpraySecond { get; }
}
