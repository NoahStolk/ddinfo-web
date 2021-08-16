namespace DevilDaggersInfo.Core.Wiki;

public static class Deaths
{
	public static readonly Death Fallen = new(GameVersions.V1 | GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "FALLEN", new(0xDD, 0xDD, 0xDD));

	public static readonly Death Skull1Swarmed = new(GameVersions.V1 | GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "SWARMED", EnemyColors.Skull1);

	public static readonly Death Skull2Impaled = new(GameVersions.V1 | GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "IMPALED", EnemyColors.Skull2);

	public static readonly Death Skull3Dismembered = new(GameVersions.V1, "DISMEMBERED", EnemyColors.Skull3);
	public static readonly Death Skull3Gored = new(GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "GORED", EnemyColors.Skull3);

	public static readonly Death Skull4Opened = new(GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "OPENED", EnemyColors.Skull4);

	public static readonly Death TransmutedSkull4Annihilated = new(GameVersions.V1, "ANNIHILATED", EnemyColors.TransmutedSkull4);

	public static readonly Death SpiderlingStricken = new(GameVersions.V1 | GameVersions.V2, "STRICKEN", EnemyColors.Spiderling);
	public static readonly Death SpiderlingInfested = new(GameVersions.V3 | GameVersions.V31, "INFESTED", EnemyColors.Spiderling);

	public static readonly Death SpiderEgg1Infested = new(GameVersions.V1 | GameVersions.V2, "INFESTED", EnemyColors.SpiderEgg1);
	public static readonly Death SpiderEgg1Intoxicated = new(GameVersions.V3 | GameVersions.V31, "INTOXICATED", EnemyColors.SpiderEgg1);

	public static readonly Death SpiderEgg2Envenomated = new(GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "ENVENOMATED", EnemyColors.SpiderEgg2);

	public static readonly Death Spider1Infested = new(GameVersions.V1 | GameVersions.V2, "INFESTED", EnemyColors.Spider1);
	public static readonly Death Spider1Intoxicated = new(GameVersions.V3 | GameVersions.V31, "INTOXICATED", EnemyColors.Spider1);

	public static readonly Death Spider2Envenomated = new(GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "ENVENOMATED", EnemyColors.Spider2);

	public static readonly Death Squid1Purged = new(GameVersions.V1 | GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "PURGED", EnemyColors.Squid1);

	public static readonly Death Squid2Sacrificed = new(GameVersions.V1, "SACRIFICED", EnemyColors.Squid2);
	public static readonly Death Squid2Desecrated = new(GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "DESECRATED", EnemyColors.Squid2);

	public static readonly Death Squid3Sacrificed = new(GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "SACRIFICED", EnemyColors.Squid3);

	public static readonly Death CentipedeEviscerated = new(GameVersions.V1 | GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "EVISCERATED", EnemyColors.Centipede);

	public static readonly Death GigapedeV1V2Eviscerated = new(GameVersions.V1, "EVISCERATED", EnemyColors.GigapedeV1V2);
	public static readonly Death GigapedeV1V2Annihilated = new(GameVersions.V2, "ANNIHILATED", EnemyColors.GigapedeV1V2);
	public static readonly Death GigapedeAnnihilated = new(GameVersions.V3 | GameVersions.V31, "ANNIHILATED", EnemyColors.Gigapede);

	public static readonly Death LeviathanDevastated = new(GameVersions.V1 | GameVersions.V2, "DEVASTATED", EnemyColors.Leviathan);
	public static readonly Death LeviathanIncarnated = new(GameVersions.V3 | GameVersions.V31, "INCARNATED", EnemyColors.Leviathan);

	public static readonly Death TheOrbDiscarnated = new(GameVersions.V3 | GameVersions.V31, "DISCARNATED", EnemyColors.TheOrb);

	public static readonly Death ThornBarbed = new(GameVersions.V3, "BARBED", EnemyColors.Thorn);
	public static readonly Death ThornEntangled = new(GameVersions.V31, "ENTANGLED", EnemyColors.Thorn);

	public static readonly Death GhostpedeIntoxicated = new(GameVersions.V3, "INTOXICATED", EnemyColors.Ghostpede);
	public static readonly Death GhostpedeHaunted = new(GameVersions.V31, "HAUNTED", EnemyColors.Ghostpede);
}
