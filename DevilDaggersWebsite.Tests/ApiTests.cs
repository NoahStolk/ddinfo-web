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

		[TestMethod]
		public void GetDaggers()
		{
			List<Dagger> daggers = daggersController.GetDaggers().Value;

			Assert.AreEqual(5, daggers.Count);
			Assert.IsTrue(daggers.Single(d => d.Name == "Default") != null);
			Assert.IsTrue(daggers.Single(d => d.Name == "Bronze") != null);
			Assert.IsTrue(daggers.Single(d => d.Name == "Silver") != null);
			Assert.IsTrue(daggers.Single(d => d.Name == "Golden") != null);
			Assert.IsTrue(daggers.Single(d => d.Name == "Devil") != null);
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
	}
}