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

			Assert.AreEqual(6, daggers.Count);
			Assert.IsTrue(daggers.All(d => d.GameVersion == GameVersion.V32));
			Assert.AreEqual(1, daggers.Count(d => d.Name == "Default"));
			Assert.AreEqual(1, daggers.Count(d => d.Name == "Bronze"));
			Assert.AreEqual(1, daggers.Count(d => d.Name == "Silver"));
			Assert.AreEqual(1, daggers.Count(d => d.Name == "Golden"));
			Assert.AreEqual(1, daggers.Count(d => d.Name == "Devil"));
			Assert.AreEqual(1, daggers.Count(d => d.Name == "Leviathan"));
		}

		[TestMethod]
		public void GetDaggerAtSeconds()
		{
			TestDaggerAtSeconds(1100, GameInfo.V31LeviathanDagger);
			TestDaggerAtSeconds(600, GameInfo.V31Devil);
			TestDaggerAtSeconds(500, GameInfo.V31Devil);
			TestDaggerAtSeconds(400, GameInfo.V31Golden);
			TestDaggerAtSeconds(250, GameInfo.V31Golden);
			TestDaggerAtSeconds(200, GameInfo.V31Silver);
			TestDaggerAtSeconds(120, GameInfo.V31Silver);
			TestDaggerAtSeconds(100, GameInfo.V31Bronze);
			TestDaggerAtSeconds(60, GameInfo.V31Bronze);
			TestDaggerAtSeconds(30, GameInfo.V31Default);
			TestDaggerAtSeconds(0, GameInfo.V31Default);

			void TestDaggerAtSeconds(uint seconds, Dagger expectedDagger)
			{
				Dagger dagger = _daggersController.GetDaggerAtSeconds(seconds).Value;
				Assert.AreEqual(expectedDagger, dagger);
			}
		}
	}
}
