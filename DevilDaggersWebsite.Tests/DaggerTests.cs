using DevilDaggersCore.Game;
using DevilDaggersWebsite.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Tests
{
	[TestClass]
	public class DaggerTests
	{
		private readonly DaggersController _daggersController = new();

		[TestMethod]
		public void GetDaggers()
		{
			List<Dagger> daggers = _daggersController.GetDaggers().Value;

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
				Dagger dagger = _daggersController.GetDaggerAtSeconds(seconds).Value;
				Assert.AreEqual(expectedDagger, dagger);
			}
		}
	}
}