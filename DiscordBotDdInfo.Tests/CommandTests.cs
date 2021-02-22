using DevilDaggersCore.Game;
using DevilDaggersCore.Utils;
using DiscordBotDdInfo.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Text;

namespace DiscordBotDdInfo.Tests
{
	[TestClass]
	public class CommandTests
	{
		private static string[] MakeCommandInputs(string input)
		{
			StringBuilder randomizedCapitals = new();
			foreach (char c in input)
				randomizedCapitals.Append(RandomUtils.Choose(char.ToUpper(c, CultureInfo.InvariantCulture), char.ToLower(c, CultureInfo.InvariantCulture)));
			return input.Split(' ');
		}

		[TestMethod]
		public void DefineV1Gigapede()
		{
			bool result = EntityUtils.TryGetInfo(MakeCommandInputs("v1 gigapede"), out _, out Enemy? enemy);
			Assert.IsTrue(result);
			Assert.AreEqual(GameVersion.V1, enemy!.GameVersion);
			Assert.AreEqual(GameInfo.V1Gigapede, enemy);
		}

		[TestMethod]
		public void DefineV1Leviathan()
		{
			bool result = EntityUtils.TryGetInfo(MakeCommandInputs("v1 leviathan"), out _, out Enemy? enemy);
			Assert.IsTrue(result);
			Assert.AreEqual(GameVersion.V1, enemy!.GameVersion);
			Assert.AreEqual(GameInfo.V1Leviathan, enemy);
		}

		[TestMethod]
		public void DefineV3Gigapede()
		{
			bool result = EntityUtils.TryGetInfo(MakeCommandInputs("v3 gigapede"), out _, out Enemy? enemy);
			Assert.IsTrue(result);
			Assert.AreEqual(GameVersion.V3, enemy!.GameVersion);
			Assert.AreEqual(GameInfo.V3Gigapede, enemy);
		}

		[TestMethod]
		public void DefineV3Leviathan()
		{
			bool result = EntityUtils.TryGetInfo(MakeCommandInputs("v3 leviathan"), out _, out Enemy? enemy);
			Assert.IsTrue(result);
			Assert.AreEqual(GameVersion.V3, enemy!.GameVersion);
			Assert.AreEqual(GameInfo.V3Leviathan, enemy);
		}

		[TestMethod]
		public void DefineBronze()
		{
			bool result = EntityUtils.TryGetInfo(MakeCommandInputs("bronze"), out _, out Dagger? dagger);
			Assert.IsTrue(result);
			Assert.AreEqual(GameVersion.V3, dagger!.GameVersion);
			Assert.AreEqual(GameInfo.V3Bronze, dagger);
		}

		[TestMethod]
		public void DefineV1Bronze()
		{
			bool result = EntityUtils.TryGetInfo(MakeCommandInputs("v1 bronze"), out _, out Dagger? dagger);
			Assert.IsTrue(result);
			Assert.AreEqual(GameVersion.V1, dagger!.GameVersion);
			Assert.AreEqual(GameInfo.V1Bronze, dagger);
		}

		[TestMethod]
		public void DefineLevel4()
		{
			bool result = EntityUtils.TryGetInfo(MakeCommandInputs("level 4"), out _, out Upgrade? upgrade);
			Assert.IsTrue(result);
			Assert.AreEqual(GameVersion.V3, upgrade!.GameVersion);
			Assert.AreEqual(GameInfo.V3Level4, upgrade);
		}

		[TestMethod]
		public void DamageV2Spiderling()
		{
			bool result = EntityUtils.TryGetInfo(MakeCommandInputs("v2 spiderling"), out _, out Enemy? enemy);
			Assert.IsTrue(result);
			Assert.AreEqual(GameVersion.V2, enemy!.GameVersion);
			Assert.AreEqual(GameInfo.V2Spiderling, enemy);
		}

		[TestMethod]
		public void DamageV3Spiderling()
		{
			bool result = EntityUtils.TryGetInfo(MakeCommandInputs("v3 spiderling"), out _, out Enemy? enemy);
			Assert.IsTrue(result);
			Assert.AreEqual(GameVersion.V3, enemy!.GameVersion);
			Assert.AreEqual(GameInfo.V3Spiderling, enemy);
		}

		[TestMethod]
		public void DamageSpiderling()
		{
			bool result = EntityUtils.TryGetInfo(MakeCommandInputs("spiderling"), out _, out Enemy? enemy);
			Assert.IsTrue(result);
			Assert.AreEqual(GameVersion.V3, enemy!.GameVersion);
			Assert.AreEqual(GameInfo.V3Spiderling, enemy);
		}

		[TestMethod]
		public void DamageV1Andras()
		{
			bool result = EntityUtils.TryGetInfo<Enemy>(MakeCommandInputs("v1 andras"), out _, out _);
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void DamageAndras()
		{
			bool result = EntityUtils.TryGetInfo(MakeCommandInputs("andras"), out _, out Enemy? enemy);
			Assert.IsTrue(result);
			Assert.AreEqual(GameVersion.V2, enemy!.GameVersion);
			Assert.AreEqual(GameInfo.V2Andras, enemy);
		}

		[TestMethod]
		public void DefineLevelIV()
		{
			bool result = EntityUtils.TryGetInfo(MakeCommandInputs("level iv"), out _, out Upgrade? upgrade);
			Assert.IsTrue(result);
			Assert.AreEqual(GameVersion.V3, upgrade!.GameVersion);
			Assert.AreEqual(GameInfo.V3Level4, upgrade);
		}

		[TestMethod]
		public void DefineSquid3()
		{
			bool result = EntityUtils.TryGetInfo(MakeCommandInputs("squid 3"), out _, out Enemy? enemy);
			Assert.IsTrue(result);
			Assert.AreEqual(GameVersion.V3, enemy!.GameVersion);
			Assert.AreEqual(GameInfo.V3Squid3, enemy);
		}

		[TestMethod]
		public void DefineSquidIII()
		{
			bool result = EntityUtils.TryGetInfo(MakeCommandInputs("squid iii"), out _, out Enemy? enemy);
			Assert.IsTrue(result);
			Assert.AreEqual(GameVersion.V3, enemy!.GameVersion);
			Assert.AreEqual(GameInfo.V3Squid3, enemy);
		}

		[TestMethod]
		public void DefineV3SquidIII()
		{
			bool result = EntityUtils.TryGetInfo(MakeCommandInputs("v3 squid iii"), out _, out Enemy? enemy);
			Assert.IsTrue(result);
			Assert.AreEqual(GameVersion.V3, enemy!.GameVersion);
			Assert.AreEqual(GameInfo.V3Squid3, enemy);
		}

		[TestMethod]
		public void DamageLevel4()
		{
			bool result = EntityUtils.TryGetInfo<Enemy>(MakeCommandInputs("level 4"), out _, out _);
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void DefineV1Level4()
		{
			bool result = EntityUtils.TryGetInfo<DevilDaggersEntity>(MakeCommandInputs("v1 level 4"), out _, out _);
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void DefineDismembered()
		{
			bool result = EntityUtils.TryGetInfo(MakeCommandInputs("dismembered"), out _, out Death? death);
			Assert.IsTrue(result);
			Assert.AreEqual(GameVersion.V1, death!.GameVersion);
			Assert.AreEqual(GameInfo.V1Dismembered, death);
		}
	}
}
