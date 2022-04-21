namespace DevilDaggersInfo.Test.Core.Wiki;

[TestClass]
public class DeathTests
{
	[TestMethod]
	public void TestFallen()
	{
		Assert.AreEqual(DeathsV1_0.Fallen, Deaths.GetDeathByLeaderboardType(GameVersion.V1_0, 0));
		Assert.AreEqual(DeathsV2_0.Fallen, Deaths.GetDeathByLeaderboardType(GameVersion.V2_0, 0));
		Assert.AreEqual(DeathsV3_0.Fallen, Deaths.GetDeathByLeaderboardType(GameVersion.V3_0, 0));
		Assert.AreEqual(DeathsV3_1.Fallen, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 0));
		Assert.AreEqual(DeathsV3_2.Fallen, Deaths.GetDeathByLeaderboardType(GameVersion.V3_2, 0));

		Assert.AreEqual(DeathsV1_0.Fallen.Color, EnemyColors.Void);
		Assert.AreEqual(DeathsV2_0.Fallen.Color, EnemyColors.Void);
		Assert.AreEqual(DeathsV3_0.Fallen.Color, EnemyColors.Void);
		Assert.AreEqual(DeathsV3_1.Fallen.Color, EnemyColors.Void);
		Assert.AreEqual(DeathsV3_2.Fallen.Color, EnemyColors.Void);
	}

	[TestMethod]
	public void TestSwarmed()
	{
		Assert.AreEqual(DeathsV1_0.Swarmed, Deaths.GetDeathByLeaderboardType(GameVersion.V1_0, 1));
		Assert.AreEqual(DeathsV2_0.Swarmed, Deaths.GetDeathByLeaderboardType(GameVersion.V2_0, 1));
		Assert.AreEqual(DeathsV3_0.Swarmed, Deaths.GetDeathByLeaderboardType(GameVersion.V3_0, 1));
		Assert.AreEqual(DeathsV3_1.Swarmed, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 1));
		Assert.AreEqual(DeathsV3_2.Swarmed, Deaths.GetDeathByLeaderboardType(GameVersion.V3_2, 1));
		Assert.AreEqual(DeathsV1_0.Swarmed, EnemiesV1_0.Skull1.Death);
		Assert.AreEqual(DeathsV2_0.Swarmed, EnemiesV2_0.Skull1.Death);
		Assert.AreEqual(DeathsV3_0.Swarmed, EnemiesV3_0.Skull1.Death);
		Assert.AreEqual(DeathsV3_1.Swarmed, EnemiesV3_1.Skull1.Death);
		Assert.AreEqual(DeathsV3_2.Swarmed, EnemiesV3_2.Skull1.Death);
		Assert.AreEqual(DeathsV2_0.Swarmed, EnemiesV2_0.TransmutedSkull1.Death);
		Assert.AreEqual(DeathsV3_0.Swarmed, EnemiesV3_0.TransmutedSkull1.Death);
		Assert.AreEqual(DeathsV3_1.Swarmed, EnemiesV3_1.TransmutedSkull1.Death);
		Assert.AreEqual(DeathsV3_2.Swarmed, EnemiesV3_2.TransmutedSkull1.Death);

		Assert.AreEqual(DeathsV1_0.Swarmed.Color, EnemyColors.Skull1);
		Assert.AreEqual(DeathsV2_0.Swarmed.Color, EnemyColors.Skull1);
		Assert.AreEqual(DeathsV3_0.Swarmed.Color, EnemyColors.Skull1);
		Assert.AreEqual(DeathsV3_1.Swarmed.Color, EnemyColors.Skull1);
		Assert.AreEqual(DeathsV3_2.Swarmed.Color, EnemyColors.Skull1);
	}

	[TestMethod]
	public void TestImpaled()
	{
		Assert.AreEqual(DeathsV1_0.Impaled, Deaths.GetDeathByLeaderboardType(GameVersion.V1_0, 2));
		Assert.AreEqual(DeathsV2_0.Impaled, Deaths.GetDeathByLeaderboardType(GameVersion.V2_0, 2));
		Assert.AreEqual(DeathsV3_0.Impaled, Deaths.GetDeathByLeaderboardType(GameVersion.V3_0, 2));
		Assert.AreEqual(DeathsV3_1.Impaled, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 2));
		Assert.AreEqual(DeathsV3_2.Impaled, Deaths.GetDeathByLeaderboardType(GameVersion.V3_2, 2));
		Assert.AreEqual(DeathsV1_0.Impaled, EnemiesV1_0.Skull2.Death);
		Assert.AreEqual(DeathsV2_0.Impaled, EnemiesV2_0.Skull2.Death);
		Assert.AreEqual(DeathsV3_0.Impaled, EnemiesV3_0.Skull2.Death);
		Assert.AreEqual(DeathsV3_1.Impaled, EnemiesV3_1.Skull2.Death);
		Assert.AreEqual(DeathsV3_2.Impaled, EnemiesV3_2.Skull2.Death);
		Assert.AreEqual(DeathsV1_0.Impaled, EnemiesV1_0.TransmutedSkull2.Death);
		Assert.AreEqual(DeathsV2_0.Impaled, EnemiesV2_0.TransmutedSkull2.Death);
		Assert.AreEqual(DeathsV3_0.Impaled, EnemiesV3_0.TransmutedSkull2.Death);
		Assert.AreEqual(DeathsV3_1.Impaled, EnemiesV3_1.TransmutedSkull2.Death);
		Assert.AreEqual(DeathsV3_2.Impaled, EnemiesV3_2.TransmutedSkull2.Death);

		Assert.AreEqual(DeathsV1_0.Impaled.Color, EnemyColors.Skull2);
		Assert.AreEqual(DeathsV2_0.Impaled.Color, EnemyColors.Skull2);
		Assert.AreEqual(DeathsV3_0.Impaled.Color, EnemyColors.Skull2);
		Assert.AreEqual(DeathsV3_1.Impaled.Color, EnemyColors.Skull2);
		Assert.AreEqual(DeathsV3_2.Impaled.Color, EnemyColors.Skull2);
	}

	[TestMethod]
	public void TestGored()
	{
		Assert.AreEqual(DeathsV2_0.Gored, Deaths.GetDeathByLeaderboardType(GameVersion.V2_0, 3));
		Assert.AreEqual(DeathsV3_0.Gored, Deaths.GetDeathByLeaderboardType(GameVersion.V3_0, 3));
		Assert.AreEqual(DeathsV3_1.Gored, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 3));
		Assert.AreEqual(DeathsV3_2.Gored, Deaths.GetDeathByLeaderboardType(GameVersion.V3_2, 3));
		Assert.AreEqual(DeathsV2_0.Gored, EnemiesV2_0.Skull3.Death);
		Assert.AreEqual(DeathsV3_0.Gored, EnemiesV3_0.Skull3.Death);
		Assert.AreEqual(DeathsV3_1.Gored, EnemiesV3_1.Skull3.Death);
		Assert.AreEqual(DeathsV3_2.Gored, EnemiesV3_2.Skull3.Death);
		Assert.AreEqual(DeathsV2_0.Gored, EnemiesV2_0.TransmutedSkull3.Death);
		Assert.AreEqual(DeathsV3_0.Gored, EnemiesV3_0.TransmutedSkull3.Death);
		Assert.AreEqual(DeathsV3_1.Gored, EnemiesV3_1.TransmutedSkull3.Death);
		Assert.AreEqual(DeathsV3_2.Gored, EnemiesV3_2.TransmutedSkull3.Death);

		Assert.AreEqual(DeathsV2_0.Gored.Color, EnemyColors.Skull3);
		Assert.AreEqual(DeathsV3_0.Gored.Color, EnemyColors.Skull3);
		Assert.AreEqual(DeathsV3_1.Gored.Color, EnemyColors.Skull3);
		Assert.AreEqual(DeathsV3_2.Gored.Color, EnemyColors.Skull3);
	}

	[TestMethod]
	public void TestInfested()
	{
		Assert.AreEqual(DeathsV1_0.Infested, Deaths.GetDeathByLeaderboardType(GameVersion.V1_0, 4));
		Assert.AreEqual(DeathsV2_0.Infested, Deaths.GetDeathByLeaderboardType(GameVersion.V2_0, 4));
		Assert.AreEqual(DeathsV3_0.Infested, Deaths.GetDeathByLeaderboardType(GameVersion.V3_0, 4));
		Assert.AreEqual(DeathsV3_1.Infested, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 4));
		Assert.AreEqual(DeathsV3_2.Infested, Deaths.GetDeathByLeaderboardType(GameVersion.V3_2, 4));
		Assert.AreEqual(DeathsV1_0.Infested, EnemiesV1_0.SpiderEgg1.Death);
		Assert.AreEqual(DeathsV2_0.Infested, EnemiesV2_0.SpiderEgg1.Death);
		Assert.AreEqual(DeathsV3_0.Infested, EnemiesV3_0.Spiderling.Death);
		Assert.AreEqual(DeathsV3_1.Infested, EnemiesV3_1.Spiderling.Death);
		Assert.AreEqual(DeathsV3_2.Infested, EnemiesV3_2.Spiderling.Death);

		Assert.AreEqual(DeathsV1_0.Infested.Color, EnemyColors.SpiderEgg1);
		Assert.AreEqual(DeathsV2_0.Infested.Color, EnemyColors.SpiderEgg1);
		Assert.AreEqual(DeathsV3_0.Infested.Color, EnemyColors.Spiderling);
		Assert.AreEqual(DeathsV3_1.Infested.Color, EnemyColors.Spiderling);
		Assert.AreEqual(DeathsV3_2.Infested.Color, EnemyColors.Spiderling);
	}

	[TestMethod]
	public void TestOpened()
	{
		Assert.AreEqual(DeathsV2_0.Opened, Deaths.GetDeathByLeaderboardType(GameVersion.V2_0, 5));
		Assert.AreEqual(DeathsV3_0.Opened, Deaths.GetDeathByLeaderboardType(GameVersion.V3_0, 5));
		Assert.AreEqual(DeathsV3_1.Opened, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 5));
		Assert.AreEqual(DeathsV3_2.Opened, Deaths.GetDeathByLeaderboardType(GameVersion.V3_2, 5));
		Assert.AreEqual(DeathsV2_0.Opened, EnemiesV2_0.Skull4.Death);
		Assert.AreEqual(DeathsV3_0.Opened, EnemiesV3_0.Skull4.Death);
		Assert.AreEqual(DeathsV3_1.Opened, EnemiesV3_1.Skull4.Death);
		Assert.AreEqual(DeathsV3_2.Opened, EnemiesV3_2.Skull4.Death);
		Assert.AreEqual(DeathsV2_0.Opened, EnemiesV2_0.TransmutedSkull4.Death);
		Assert.AreEqual(DeathsV3_0.Opened, EnemiesV3_0.TransmutedSkull4.Death);
		Assert.AreEqual(DeathsV3_1.Opened, EnemiesV3_1.TransmutedSkull4.Death);
		Assert.AreEqual(DeathsV3_2.Opened, EnemiesV3_2.TransmutedSkull4.Death);

		Assert.AreEqual(DeathsV2_0.Opened.Color, EnemyColors.Skull4);
		Assert.AreEqual(DeathsV3_0.Opened.Color, EnemyColors.Skull4);
		Assert.AreEqual(DeathsV3_1.Opened.Color, EnemyColors.Skull4);
		Assert.AreEqual(DeathsV3_2.Opened.Color, EnemyColors.Skull4);
	}

	[TestMethod]
	public void TestPurged()
	{
		Assert.AreEqual(DeathsV1_0.Purged, Deaths.GetDeathByLeaderboardType(GameVersion.V1_0, 6));
		Assert.AreEqual(DeathsV2_0.Purged, Deaths.GetDeathByLeaderboardType(GameVersion.V2_0, 6));
		Assert.AreEqual(DeathsV3_0.Purged, Deaths.GetDeathByLeaderboardType(GameVersion.V3_0, 6));
		Assert.AreEqual(DeathsV3_1.Purged, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 6));
		Assert.AreEqual(DeathsV3_2.Purged, Deaths.GetDeathByLeaderboardType(GameVersion.V3_2, 6));
		Assert.AreEqual(DeathsV1_0.Purged, EnemiesV1_0.Squid1.Death);
		Assert.AreEqual(DeathsV2_0.Purged, EnemiesV2_0.Squid1.Death);
		Assert.AreEqual(DeathsV3_0.Purged, EnemiesV3_0.Squid1.Death);
		Assert.AreEqual(DeathsV3_1.Purged, EnemiesV3_1.Squid1.Death);
		Assert.AreEqual(DeathsV3_2.Purged, EnemiesV3_2.Squid1.Death);

		Assert.AreEqual(DeathsV1_0.Purged.Color, EnemyColors.Squid1);
		Assert.AreEqual(DeathsV2_0.Purged.Color, EnemyColors.Squid1);
		Assert.AreEqual(DeathsV3_0.Purged.Color, EnemyColors.Squid1);
		Assert.AreEqual(DeathsV3_1.Purged.Color, EnemyColors.Squid1);
		Assert.AreEqual(DeathsV3_2.Purged.Color, EnemyColors.Squid1);
	}

	[TestMethod]
	public void TestDesecrated()
	{
		Assert.AreEqual(DeathsV2_0.Desecrated, Deaths.GetDeathByLeaderboardType(GameVersion.V2_0, 7));
		Assert.AreEqual(DeathsV3_0.Desecrated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_0, 7));
		Assert.AreEqual(DeathsV3_1.Desecrated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 7));
		Assert.AreEqual(DeathsV3_2.Desecrated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_2, 7));
		Assert.AreEqual(DeathsV2_0.Desecrated, EnemiesV2_0.Squid2.Death);
		Assert.AreEqual(DeathsV3_0.Desecrated, EnemiesV3_0.Squid2.Death);
		Assert.AreEqual(DeathsV3_1.Desecrated, EnemiesV3_1.Squid2.Death);
		Assert.AreEqual(DeathsV3_2.Desecrated, EnemiesV3_2.Squid2.Death);

		Assert.AreEqual(DeathsV2_0.Desecrated.Color, EnemyColors.Squid2);
		Assert.AreEqual(DeathsV3_0.Desecrated.Color, EnemyColors.Squid2);
		Assert.AreEqual(DeathsV3_1.Desecrated.Color, EnemyColors.Squid2);
		Assert.AreEqual(DeathsV3_2.Desecrated.Color, EnemyColors.Squid2);
	}

	[TestMethod]
	public void TestSacrificed()
	{
		Assert.AreEqual(DeathsV1_0.Sacrificed, Deaths.GetDeathByLeaderboardType(GameVersion.V1_0, 8));
		Assert.AreEqual(DeathsV2_0.Sacrificed, Deaths.GetDeathByLeaderboardType(GameVersion.V2_0, 8));
		Assert.AreEqual(DeathsV3_0.Sacrificed, Deaths.GetDeathByLeaderboardType(GameVersion.V3_0, 8));
		Assert.AreEqual(DeathsV3_1.Sacrificed, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 8));
		Assert.AreEqual(DeathsV3_2.Sacrificed, Deaths.GetDeathByLeaderboardType(GameVersion.V3_2, 8));
		Assert.AreEqual(DeathsV1_0.Sacrificed, EnemiesV1_0.Squid2.Death);
		Assert.AreEqual(DeathsV2_0.Sacrificed, EnemiesV2_0.Squid3.Death);
		Assert.AreEqual(DeathsV3_0.Sacrificed, EnemiesV3_0.Squid3.Death);
		Assert.AreEqual(DeathsV3_1.Sacrificed, EnemiesV3_1.Squid3.Death);
		Assert.AreEqual(DeathsV3_2.Sacrificed, EnemiesV3_2.Squid3.Death);

		Assert.AreEqual(DeathsV1_0.Sacrificed.Color, EnemyColors.Squid2);
		Assert.AreEqual(DeathsV2_0.Sacrificed.Color, EnemyColors.Squid3);
		Assert.AreEqual(DeathsV3_0.Sacrificed.Color, EnemyColors.Squid3);
		Assert.AreEqual(DeathsV3_1.Sacrificed.Color, EnemyColors.Squid3);
		Assert.AreEqual(DeathsV3_2.Sacrificed.Color, EnemyColors.Squid3);
	}

	[TestMethod]
	public void TestEviscerated()
	{
		Assert.AreEqual(DeathsV1_0.Eviscerated, Deaths.GetDeathByLeaderboardType(GameVersion.V1_0, 9));
		Assert.AreEqual(DeathsV2_0.Eviscerated, Deaths.GetDeathByLeaderboardType(GameVersion.V2_0, 9));
		Assert.AreEqual(DeathsV3_0.Eviscerated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_0, 9));
		Assert.AreEqual(DeathsV3_1.Eviscerated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 9));
		Assert.AreEqual(DeathsV3_2.Eviscerated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_2, 9));
		Assert.AreEqual(DeathsV1_0.Eviscerated, EnemiesV1_0.Gigapede.Death);
		Assert.AreEqual(DeathsV1_0.Eviscerated, EnemiesV1_0.Centipede.Death);
		Assert.AreEqual(DeathsV2_0.Eviscerated, EnemiesV2_0.Centipede.Death);
		Assert.AreEqual(DeathsV3_0.Eviscerated, EnemiesV3_0.Centipede.Death);
		Assert.AreEqual(DeathsV3_1.Eviscerated, EnemiesV3_1.Centipede.Death);
		Assert.AreEqual(DeathsV3_2.Eviscerated, EnemiesV3_2.Centipede.Death);

		Assert.AreEqual(DeathsV1_0.Eviscerated.Color, EnemyColors.Centipede);
		Assert.AreEqual(DeathsV2_0.Eviscerated.Color, EnemyColors.Centipede);
		Assert.AreEqual(DeathsV3_0.Eviscerated.Color, EnemyColors.Centipede);
		Assert.AreEqual(DeathsV3_1.Eviscerated.Color, EnemyColors.Centipede);
		Assert.AreEqual(DeathsV3_2.Eviscerated.Color, EnemyColors.Centipede);
	}

	[TestMethod]
	public void TestAnnihilated()
	{
		Assert.AreEqual(DeathsV1_0.Annihilated, Deaths.GetDeathByLeaderboardType(GameVersion.V1_0, 10));
		Assert.AreEqual(DeathsV2_0.Annihilated, Deaths.GetDeathByLeaderboardType(GameVersion.V2_0, 10));
		Assert.AreEqual(DeathsV3_0.Annihilated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_0, 10));
		Assert.AreEqual(DeathsV3_1.Annihilated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 10));
		Assert.AreEqual(DeathsV3_2.Annihilated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_2, 10));
		Assert.AreEqual(DeathsV1_0.Annihilated, EnemiesV1_0.TransmutedSkull4.Death);
		Assert.AreEqual(DeathsV2_0.Annihilated, EnemiesV2_0.Gigapede.Death);
		Assert.AreEqual(DeathsV3_0.Annihilated, EnemiesV3_0.Gigapede.Death);
		Assert.AreEqual(DeathsV3_1.Annihilated, EnemiesV3_1.Gigapede.Death);
		Assert.AreEqual(DeathsV3_2.Annihilated, EnemiesV3_2.Gigapede.Death);

		Assert.AreEqual(DeathsV1_0.Annihilated.Color, EnemyColors.TransmutedSkull4);
		Assert.AreEqual(DeathsV2_0.Annihilated.Color, EnemyColors.GigapedeRed);
		Assert.AreEqual(DeathsV3_0.Annihilated.Color, EnemyColors.Gigapede);
		Assert.AreEqual(DeathsV3_1.Annihilated.Color, EnemyColors.Gigapede);
		Assert.AreEqual(DeathsV3_2.Annihilated.Color, EnemyColors.Gigapede);
	}

	[TestMethod]
	public void TestIntoxicated()
	{
		Assert.AreEqual(DeathsV3_0.Intoxicated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_0, 11));
		Assert.AreEqual(DeathsV3_1.Intoxicated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 11));
		Assert.AreEqual(DeathsV3_2.Intoxicated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_2, 11));
		Assert.AreEqual(DeathsV3_0.Intoxicated, EnemiesV3_0.SpiderEgg1.Death);
		Assert.AreEqual(DeathsV3_0.Intoxicated, EnemiesV3_0.Spider1.Death);
		Assert.AreEqual(DeathsV3_0.Intoxicated, EnemiesV3_0.Ghostpede.Death);
		Assert.AreEqual(DeathsV3_1.Intoxicated, EnemiesV3_1.SpiderEgg1.Death);
		Assert.AreEqual(DeathsV3_1.Intoxicated, EnemiesV3_1.Spider1.Death);
		Assert.AreEqual(DeathsV3_2.Intoxicated, EnemiesV3_2.SpiderEgg1.Death);
		Assert.AreEqual(DeathsV3_2.Intoxicated, EnemiesV3_2.Spider1.Death);

		Assert.AreEqual(DeathsV3_0.Intoxicated.Color, EnemyColors.SpiderEgg1);
		Assert.AreEqual(DeathsV3_1.Intoxicated.Color, EnemyColors.SpiderEgg1);
		Assert.AreEqual(DeathsV3_2.Intoxicated.Color, EnemyColors.SpiderEgg1);
	}

	[TestMethod]
	public void TestEnvenomated()
	{
		Assert.AreEqual(DeathsV2_0.Envenomated, Deaths.GetDeathByLeaderboardType(GameVersion.V2_0, 12));
		Assert.AreEqual(DeathsV3_0.Envenomated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_0, 12));
		Assert.AreEqual(DeathsV3_1.Envenomated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 12));
		Assert.AreEqual(DeathsV3_2.Envenomated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_2, 12));
		Assert.AreEqual(DeathsV2_0.Envenomated, EnemiesV2_0.SpiderEgg2.Death);
		Assert.AreEqual(DeathsV2_0.Envenomated, EnemiesV2_0.Spider2.Death);
		Assert.AreEqual(DeathsV3_0.Envenomated, EnemiesV3_0.SpiderEgg2.Death);
		Assert.AreEqual(DeathsV3_0.Envenomated, EnemiesV3_0.Spider2.Death);
		Assert.AreEqual(DeathsV3_1.Envenomated, EnemiesV3_1.SpiderEgg2.Death);
		Assert.AreEqual(DeathsV3_1.Envenomated, EnemiesV3_1.Spider2.Death);
		Assert.AreEqual(DeathsV3_2.Envenomated, EnemiesV3_2.SpiderEgg2.Death);
		Assert.AreEqual(DeathsV3_2.Envenomated, EnemiesV3_2.Spider2.Death);

		Assert.AreEqual(DeathsV2_0.Envenomated.Color, EnemyColors.SpiderEgg2);
		Assert.AreEqual(DeathsV3_0.Envenomated.Color, EnemyColors.SpiderEgg2);
		Assert.AreEqual(DeathsV3_1.Envenomated.Color, EnemyColors.SpiderEgg2);
		Assert.AreEqual(DeathsV3_2.Envenomated.Color, EnemyColors.SpiderEgg2);
	}

	[TestMethod]
	public void TestIncarnated()
	{
		Assert.AreEqual(DeathsV3_0.Incarnated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_0, 13));
		Assert.AreEqual(DeathsV3_1.Incarnated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 13));
		Assert.AreEqual(DeathsV3_2.Incarnated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_2, 13));
		Assert.AreEqual(DeathsV3_0.Incarnated, EnemiesV3_0.Leviathan.Death);
		Assert.AreEqual(DeathsV3_1.Incarnated, EnemiesV3_1.Leviathan.Death);
		Assert.AreEqual(DeathsV3_2.Incarnated, EnemiesV3_2.Leviathan.Death);

		Assert.AreEqual(DeathsV3_0.Incarnated.Color, EnemyColors.Leviathan);
		Assert.AreEqual(DeathsV3_1.Incarnated.Color, EnemyColors.Leviathan);
		Assert.AreEqual(DeathsV3_2.Incarnated.Color, EnemyColors.Leviathan);
	}

	[TestMethod]
	public void TestDiscarnated()
	{
		Assert.AreEqual(DeathsV3_0.Discarnated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_0, 14));
		Assert.AreEqual(DeathsV3_1.Discarnated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 14));
		Assert.AreEqual(DeathsV3_2.Discarnated, Deaths.GetDeathByLeaderboardType(GameVersion.V3_2, 14));
		Assert.AreEqual(DeathsV3_0.Discarnated, EnemiesV3_0.TheOrb.Death);
		Assert.AreEqual(DeathsV3_1.Discarnated, EnemiesV3_1.TheOrb.Death);
		Assert.AreEqual(DeathsV3_2.Discarnated, EnemiesV3_2.TheOrb.Death);

		Assert.AreEqual(DeathsV3_0.Discarnated.Color, EnemyColors.TheOrb);
		Assert.AreEqual(DeathsV3_1.Discarnated.Color, EnemyColors.TheOrb);
		Assert.AreEqual(DeathsV3_2.Discarnated.Color, EnemyColors.TheOrb);
	}

	[TestMethod]
	public void TestBarbed()
	{
		Assert.AreEqual(DeathsV3_0.Barbed, Deaths.GetDeathByLeaderboardType(GameVersion.V3_0, 15));
		Assert.AreEqual(DeathsV3_0.Barbed, EnemiesV3_0.Thorn.Death);

		Assert.AreEqual(DeathsV3_0.Barbed.Color, EnemyColors.Thorn);
	}

	[TestMethod]
	public void TestEntangled()
	{
		Assert.AreEqual(DeathsV3_1.Entangled, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 15));
		Assert.AreEqual(DeathsV3_2.Entangled, Deaths.GetDeathByLeaderboardType(GameVersion.V3_2, 15));
		Assert.AreEqual(DeathsV3_1.Entangled, EnemiesV3_1.Thorn.Death);
		Assert.AreEqual(DeathsV3_2.Entangled, EnemiesV3_2.Thorn.Death);

		Assert.AreEqual(DeathsV3_1.Entangled.Color, EnemyColors.Thorn);
		Assert.AreEqual(DeathsV3_2.Entangled.Color, EnemyColors.Thorn);
	}

	[TestMethod]
	public void TestHaunted()
	{
		Assert.AreEqual(DeathsV3_1.Haunted, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 16));
		Assert.AreEqual(DeathsV3_2.Haunted, Deaths.GetDeathByLeaderboardType(GameVersion.V3_2, 16));
		Assert.AreEqual(DeathsV3_1.Haunted, EnemiesV3_1.Ghostpede.Death);
		Assert.AreEqual(DeathsV3_2.Haunted, EnemiesV3_2.Ghostpede.Death);

		Assert.AreEqual(DeathsV3_1.Haunted.Color, EnemyColors.Ghostpede);
		Assert.AreEqual(DeathsV3_2.Haunted.Color, EnemyColors.Ghostpede);
	}

	[TestMethod]
	public void TestStricken()
	{
		Assert.AreEqual(DeathsV1_0.Stricken, Deaths.GetDeathByLeaderboardType(GameVersion.V1_0, 16));
		Assert.AreEqual(DeathsV2_0.Stricken, Deaths.GetDeathByLeaderboardType(GameVersion.V2_0, 16));
		Assert.AreEqual(DeathsV1_0.Stricken, EnemiesV1_0.Spiderling.Death);
		Assert.AreEqual(DeathsV2_0.Stricken, EnemiesV2_0.Spiderling.Death);

		Assert.AreEqual(DeathsV1_0.Stricken.Color, EnemyColors.Spiderling);
		Assert.AreEqual(DeathsV2_0.Stricken.Color, EnemyColors.Spiderling);
	}

	[TestMethod]
	public void TestDevastated()
	{
		Assert.AreEqual(DeathsV1_0.Devastated, Deaths.GetDeathByLeaderboardType(GameVersion.V1_0, 17));
		Assert.AreEqual(DeathsV2_0.Devastated, Deaths.GetDeathByLeaderboardType(GameVersion.V2_0, 17));
		Assert.AreEqual(DeathsV1_0.Devastated, EnemiesV1_0.Leviathan.Death);
		Assert.AreEqual(DeathsV2_0.Devastated, EnemiesV2_0.Leviathan.Death);

		Assert.AreEqual(DeathsV1_0.Devastated.Color, EnemyColors.Leviathan);
		Assert.AreEqual(DeathsV2_0.Devastated.Color, EnemyColors.Leviathan);
	}

	[TestMethod]
	public void TestDismembered()
	{
		Assert.AreEqual(DeathsV1_0.Dismembered, Deaths.GetDeathByLeaderboardType(GameVersion.V1_0, 18));
		Assert.AreEqual(DeathsV1_0.Dismembered, EnemiesV1_0.Skull3.Death);
		Assert.AreEqual(DeathsV1_0.Dismembered, EnemiesV1_0.TransmutedSkull3.Death);

		Assert.AreEqual(DeathsV1_0.Dismembered.Color, EnemyColors.Skull3);
	}

	[TestMethod]
	public void TestUnknown()
	{
		Assert.AreEqual(DeathsV1_0.Unknown, Deaths.GetDeathByLeaderboardType(GameVersion.V1_0, 255, false));
		Assert.AreEqual(DeathsV2_0.Unknown, Deaths.GetDeathByLeaderboardType(GameVersion.V2_0, 255, false));
		Assert.AreEqual(DeathsV3_0.Unknown, Deaths.GetDeathByLeaderboardType(GameVersion.V3_0, 255, false));
		Assert.AreEqual(DeathsV3_1.Unknown, Deaths.GetDeathByLeaderboardType(GameVersion.V3_1, 255, false));
		Assert.AreEqual(DeathsV3_2.Unknown, Deaths.GetDeathByLeaderboardType(GameVersion.V3_2, 255, false));

		Assert.AreEqual(DeathsV1_0.Unknown.Color, EnemyColors.Unknown);
		Assert.AreEqual(DeathsV2_0.Unknown.Color, EnemyColors.Unknown);
		Assert.AreEqual(DeathsV3_0.Unknown.Color, EnemyColors.Unknown);
		Assert.AreEqual(DeathsV3_1.Unknown.Color, EnemyColors.Unknown);
		Assert.AreEqual(DeathsV3_2.Unknown.Color, EnemyColors.Unknown);
	}

	[TestMethod]
	public void TestNone()
	{
		Assert.AreEqual(DeathsV2_0.None, Deaths.GetDeathByLeaderboardType(GameVersion.V2_0, 200));

		Assert.AreEqual(DeathsV2_0.None.Color, EnemyColors.Andras);
	}
}
