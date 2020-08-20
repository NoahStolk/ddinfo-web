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
			TestDaggerAtSeconds(1100, GameInfo.V3Devil);
			TestDaggerAtSeconds(600, GameInfo.V3Devil);
			TestDaggerAtSeconds(500, GameInfo.V3Devil);
			TestDaggerAtSeconds(400, GameInfo.V3Golden);
			TestDaggerAtSeconds(250, GameInfo.V3Golden);
			TestDaggerAtSeconds(200, GameInfo.V3Silver);
			TestDaggerAtSeconds(120, GameInfo.V3Silver);
			TestDaggerAtSeconds(100, GameInfo.V3Bronze);
			TestDaggerAtSeconds(60, GameInfo.V3Bronze);
			TestDaggerAtSeconds(30, GameInfo.V3Default);
			TestDaggerAtSeconds(0, GameInfo.V3Default);

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
			Assert.AreEqual(11, v1Deaths.Count);
			Assert.IsTrue(v1Deaths.All(d => d.GameVersion == GameVersion.V1));
			Assert.AreEqual(1, v1Deaths.Count(d => d.Name == GameInfo.V1Stricken.Name));
			Assert.AreEqual(1, v1Deaths.Count(d => d.Name == GameInfo.V1Devastated.Name));
			Assert.AreEqual(1, v1Deaths.Count(d => d.Name == GameInfo.V1Dismembered.Name));

			List<Death> v2Deaths = deathsController.GetDeaths(GameVersion.V2).Value;
			Assert.AreEqual(14, v2Deaths.Count);
			Assert.IsTrue(v2Deaths.All(d => d.GameVersion == GameVersion.V2));
			Assert.AreEqual(1, v2Deaths.Count(d => d.Name == GameInfo.V2Envenomated.Name));
			Assert.AreEqual(1, v2Deaths.Count(d => d.Name == GameInfo.V2Stricken.Name));
			Assert.AreEqual(1, v2Deaths.Count(d => d.Name == GameInfo.V2Devastated.Name));

			List<Death> v3Deaths = deathsController.GetDeaths(GameVersion.V3).Value;
			Assert.AreEqual(16, v3Deaths.Count);
			Assert.IsTrue(v3Deaths.All(d => d.GameVersion == GameVersion.V3));
			Assert.AreEqual(1, v3Deaths.Count(d => d.Name == GameInfo.V3Envenomated.Name));
			Assert.AreEqual(1, v3Deaths.Count(d => d.Name == GameInfo.V3Incarnated.Name));
			Assert.AreEqual(1, v3Deaths.Count(d => d.Name == GameInfo.V3Discarnated.Name));
			Assert.AreEqual(1, v3Deaths.Count(d => d.Name == GameInfo.V3Barbed.Name));

			List<Death> allDeaths = deathsController.GetDeaths().Value;
			Assert.AreEqual(41, allDeaths.Count);

			List<Death> swarmedDeaths = deathsController.GetDeaths(null, "swarmed").Value;
			Assert.AreEqual(3, swarmedDeaths.Count);
			Assert.AreEqual(1, swarmedDeaths.Count(d => d.GameVersion == GameVersion.V1));
			Assert.AreEqual(1, swarmedDeaths.Count(d => d.GameVersion == GameVersion.V2));
			Assert.AreEqual(1, swarmedDeaths.Count(d => d.GameVersion == GameVersion.V3));

			List<Death> annihilatedDeaths = deathsController.GetDeaths(null, "annihilated").Value;
			Assert.AreEqual(3, annihilatedDeaths.Count);
			Assert.AreEqual(1, annihilatedDeaths.Count(d => d.GameVersion == GameVersion.V1));
			Assert.AreEqual(1, annihilatedDeaths.Count(d => d.GameVersion == GameVersion.V2));
			Assert.AreEqual(1, annihilatedDeaths.Count(d => d.GameVersion == GameVersion.V3));

			List<Death> envenomatedDeaths = deathsController.GetDeaths(null, "envenomated").Value;
			Assert.AreEqual(2, envenomatedDeaths.Count);
			Assert.AreEqual(1, envenomatedDeaths.Count(d => d.GameVersion == GameVersion.V2));
			Assert.AreEqual(1, envenomatedDeaths.Count(d => d.GameVersion == GameVersion.V3));

			List<Death> impaledV1Deaths = deathsController.GetDeaths(GameVersion.V1, "impaled").Value;
			Assert.AreEqual(1, impaledV1Deaths.Count);
			Assert.IsTrue(impaledV1Deaths.All(d => d.GameVersion == GameVersion.V1));

			List<Death> deathsType1 = deathsController.GetDeaths(null, null, 1).Value;
			Assert.AreEqual(3, deathsType1.Count);
			Assert.AreEqual(GameInfo.V1Swarmed, deathsType1.Single(d => d.GameVersion == GameVersion.V1));
			Assert.AreEqual(GameInfo.V2Swarmed, deathsType1.Single(d => d.GameVersion == GameVersion.V2));
			Assert.AreEqual(GameInfo.V3Swarmed, deathsType1.Single(d => d.GameVersion == GameVersion.V3));
		}
	}
}