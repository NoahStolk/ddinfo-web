using DevilDaggersCore.Game;
using DevilDaggersWebsite.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Tests
{
	[TestClass]
	public class DeathTests
	{
		private readonly DeathsController _deathsController = new();

		[TestMethod]
		public void GetDeaths()
		{
			List<Death> v1Deaths = _deathsController.GetDeaths(GameVersion.V1).Value;
			Assert.AreEqual(11, v1Deaths.Count);
			Assert.IsTrue(v1Deaths.All(d => d.GameVersion == GameVersion.V1));
			Assert.AreEqual(1, v1Deaths.Count(d => d.Name == GameInfo.V1Stricken.Name));
			Assert.AreEqual(1, v1Deaths.Count(d => d.Name == GameInfo.V1Devastated.Name));
			Assert.AreEqual(1, v1Deaths.Count(d => d.Name == GameInfo.V1Dismembered.Name));

			List<Death> v2Deaths = _deathsController.GetDeaths(GameVersion.V2).Value;
			Assert.AreEqual(14, v2Deaths.Count);
			Assert.IsTrue(v2Deaths.All(d => d.GameVersion == GameVersion.V2));
			Assert.AreEqual(1, v2Deaths.Count(d => d.Name == GameInfo.V2Envenomated.Name));
			Assert.AreEqual(1, v2Deaths.Count(d => d.Name == GameInfo.V2Stricken.Name));
			Assert.AreEqual(1, v2Deaths.Count(d => d.Name == GameInfo.V2Devastated.Name));

			List<Death> v3Deaths = _deathsController.GetDeaths(GameVersion.V3).Value;
			Assert.AreEqual(16, v3Deaths.Count);
			Assert.IsTrue(v3Deaths.All(d => d.GameVersion == GameVersion.V3));
			Assert.AreEqual(1, v3Deaths.Count(d => d.Name == GameInfo.V3Envenomated.Name));
			Assert.AreEqual(1, v3Deaths.Count(d => d.Name == GameInfo.V3Incarnated.Name));
			Assert.AreEqual(1, v3Deaths.Count(d => d.Name == GameInfo.V3Discarnated.Name));
			Assert.AreEqual(1, v3Deaths.Count(d => d.Name == GameInfo.V3Barbed.Name));

			List<Death> allDeaths = _deathsController.GetDeaths().Value;
			Assert.AreEqual(41, allDeaths.Count);

			List<Death> swarmedDeaths = _deathsController.GetDeaths(null, "swarmed").Value;
			Assert.AreEqual(3, swarmedDeaths.Count);
			Assert.AreEqual(1, swarmedDeaths.Count(d => d.GameVersion == GameVersion.V1));
			Assert.AreEqual(1, swarmedDeaths.Count(d => d.GameVersion == GameVersion.V2));
			Assert.AreEqual(1, swarmedDeaths.Count(d => d.GameVersion == GameVersion.V3));

			List<Death> annihilatedDeaths = _deathsController.GetDeaths(null, "annihilated").Value;
			Assert.AreEqual(3, annihilatedDeaths.Count);
			Assert.AreEqual(1, annihilatedDeaths.Count(d => d.GameVersion == GameVersion.V1));
			Assert.AreEqual(1, annihilatedDeaths.Count(d => d.GameVersion == GameVersion.V2));
			Assert.AreEqual(1, annihilatedDeaths.Count(d => d.GameVersion == GameVersion.V3));

			List<Death> envenomatedDeaths = _deathsController.GetDeaths(null, "envenomated").Value;
			Assert.AreEqual(2, envenomatedDeaths.Count);
			Assert.AreEqual(1, envenomatedDeaths.Count(d => d.GameVersion == GameVersion.V2));
			Assert.AreEqual(1, envenomatedDeaths.Count(d => d.GameVersion == GameVersion.V3));

			List<Death> impaledV1Deaths = _deathsController.GetDeaths(GameVersion.V1, "impaled").Value;
			Assert.AreEqual(1, impaledV1Deaths.Count);
			Assert.IsTrue(impaledV1Deaths.All(d => d.GameVersion == GameVersion.V1));

			List<Death> deathsType1 = _deathsController.GetDeaths(null, null, 1).Value;
			Assert.AreEqual(3, deathsType1.Count);
			Assert.AreEqual(GameInfo.V1Swarmed, deathsType1.Single(d => d.GameVersion == GameVersion.V1));
			Assert.AreEqual(GameInfo.V2Swarmed, deathsType1.Single(d => d.GameVersion == GameVersion.V2));
			Assert.AreEqual(GameInfo.V3Swarmed, deathsType1.Single(d => d.GameVersion == GameVersion.V3));
		}
	}
}
