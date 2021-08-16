namespace DevilDaggersInfo.Core.Wiki.Structs;

public struct HomingDamage
{
	public HomingDamage(float? level3HomingDaggers, float? level4HomingDaggers)
	{
		Level3HomingDaggers = level3HomingDaggers;
		Level4HomingDaggers = level4HomingDaggers;
	}

	public float? Level3HomingDaggers { get; }
	public float? Level4HomingDaggers { get; }
}
