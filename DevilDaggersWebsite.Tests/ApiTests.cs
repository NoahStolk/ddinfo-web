using DevilDaggersCore.Game;
using DevilDaggersWebsite.Code.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Tests
{
	[TestClass]
	public class ApiTests
	{
		private readonly DaggersController daggersController = new DaggersController();
		private readonly DeathsController deathsController = new DeathsController();

		[TestMethod]
		public void GetDaggers()
		{
			List<Dagger> daggers = daggersController.GetDaggers().Value;

			Assert.AreEqual(5, daggers.Count);
			Assert.IsTrue(daggers.All(d => d.GameVersion == GameVersion.V3));
			Assert.AreEqual(1, daggers.Count(d => d.Name == "Default"));
			Assert.AreEqual(1, daggers.Count(d => d.Name == "Bronze"));
			Assert.AreEqual(1, daggers.Count(d => d.Name == "Silver"));
			Assert.AreEqual(1, daggers.Count(d => d.Name == "Golden"));
			Assert.AreEqual(1, daggers.Count(d => d.Name == "Devil"));
		}

		[TestMethod]
		public void GetDaggerAtSeconds()
		{
			TestDaggerAtSeconds(1100, GameData.V3Devil);
			TestDaggerAtSeconds(600, GameData.V3Devil);
			TestDaggerAtSeconds(500, GameData.V3Devil);
			TestDaggerAtSeconds(400, GameData.V3Golden);
			TestDaggerAtSeconds(250, GameData.V3Golden);
			TestDaggerAtSeconds(200, GameData.V3Silver);
			TestDaggerAtSeconds(120, GameData.V3Silver);
			TestDaggerAtSeconds(100, GameData.V3Bronze);
			TestDaggerAtSeconds(60, GameData.V3Bronze);
			TestDaggerAtSeconds(30, GameData.V3Default);
			TestDaggerAtSeconds(0, GameData.V3Default);

			void TestDaggerAtSeconds(uint seconds, Dagger expectedDagger)
			{
				Dagger dagger = daggersController.GetDaggerAtSeconds(seconds).Value;
				Assert.AreEqual(expectedDagger, dagger);
			}
		}

		[TestMethod]
		public void GetDeaths()
		{
			List<Death> v1Deaths = deathsController.GetDeaths(GameVersion.V1).Value;
			Assert.AreEqual(12, v1Deaths.Count);
			Assert.IsTrue(v1Deaths.All(d => d.GameVersion == GameVersion.V1));
			Assert.AreEqual(1, v1Deaths.Count(d => d.Name == "STRICKEN"));
			Assert.AreEqual(1, v1Deaths.Count(d => d.Name == "DEVASTATED"));
			Assert.AreEqual(1, v1Deaths.Count(d => d.Name == "DISMEMBERED"));

			List<Death> v2Deaths = deathsController.GetDeaths(GameVersion.V2).Value;
			Assert.AreEqual(15, v2Deaths.Count);
			Assert.IsTrue(v2Deaths.All(d => d.GameVersion == GameVersion.V2));
			Assert.AreEqual(1, v2Deaths.Count(d => d.Name == "ENVENOMATED"));
			Assert.AreEqual(1, v2Deaths.Count(d => d.Name == "STRICKEN"));
			Assert.AreEqual(1, v2Deaths.Count(d => d.Name == "DEVASTATED"));

			List<Death> v3Deaths = deathsController.GetDeaths(GameVersion.V3).Value;
			Assert.AreEqual(17, v3Deaths.Count);
			Assert.IsTrue(v3Deaths.All(d => d.GameVersion == GameVersion.V3));
			Assert.AreEqual(1, v3Deaths.Count(d => d.Name == "ENVENOMATED"));
			Assert.AreEqual(1, v3Deaths.Count(d => d.Name == "INCARNATED"));
			Assert.AreEqual(1, v3Deaths.Count(d => d.Name == "DISCARNATED"));
			Assert.AreEqual(1, v3Deaths.Count(d => d.Name == "BARBED"));

			List<Death> allDeaths = deathsController.GetDeaths().Value;
			Assert.AreEqual(44, allDeaths.Count);
		}

		[TestMethod]
		public void GetDeathsByName()
		{
			List<Death> swarmedDeaths = deathsController.GetDeathsByName("swarmed").Value;
			Assert.AreEqual(3, swarmedDeaths.Count);
			Assert.AreEqual(1, swarmedDeaths.Count(d => d.GameVersion == GameVersion.V1));
			Assert.AreEqual(1, swarmedDeaths.Count(d => d.GameVersion == GameVersion.V2));
			Assert.AreEqual(1, swarmedDeaths.Count(d => d.GameVersion == GameVersion.V3));

			List<Death> annihilatedDeaths = deathsController.GetDeathsByName("annihilated").Value;
			Assert.AreEqual(3, annihilatedDeaths.Count);
			Assert.AreEqual(1, annihilatedDeaths.Count(d => d.GameVersion == GameVersion.V1));
			Assert.AreEqual(1, annihilatedDeaths.Count(d => d.GameVersion == GameVersion.V2));
			Assert.AreEqual(1, annihilatedDeaths.Count(d => d.GameVersion == GameVersion.V3));

			List<Death> envenomatedDeaths = deathsController.GetDeathsByName("envenomated").Value;
			Assert.AreEqual(2, envenomatedDeaths.Count);
			Assert.AreEqual(1, envenomatedDeaths.Count(d => d.GameVersion == GameVersion.V2));
			Assert.AreEqual(1, envenomatedDeaths.Count(d => d.GameVersion == GameVersion.V3));

			List<Death> impaledV1Deaths = deathsController.GetDeathsByName("impaled", GameVersion.V1).Value;
			Assert.AreEqual(1, impaledV1Deaths.Count);
			Assert.IsTrue(impaledV1Deaths.All(d => d.GameVersion == GameVersion.V1));
		}

		[TestMethod]
		public void GetDeathsByType()
		{
			List<Death> deathsType1 = deathsController.GetDeathsByType(1).Value;
			Assert.AreEqual(3, deathsType1.Count);
			Assert.AreEqual(GameData.V1Swarmed, deathsType1.Single(d => d.GameVersion == GameVersion.V1));
			Assert.AreEqual(GameData.V2Swarmed, deathsType1.Single(d => d.GameVersion == GameVersion.V2));
			Assert.AreEqual(GameData.V3Swarmed, deathsType1.Single(d => d.GameVersion == GameVersion.V3));
		}
	}
}