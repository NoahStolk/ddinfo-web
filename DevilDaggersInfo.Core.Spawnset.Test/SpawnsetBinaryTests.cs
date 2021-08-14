﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DevilDaggersInfo.Core.Spawnset.Test
{
	[TestClass]
	public class SpawnsetBinaryTests
	{
		[TestMethod]
		public void CompareBinaryOutput_V0()
			=> CompareBinaryOutput("V0");

		[TestMethod]
		public void CompareBinaryOutput_V1()
			=> CompareBinaryOutput("V1");

		[TestMethod]
		public void CompareBinaryOutput_V2()
			=> CompareBinaryOutput("V2");

		[TestMethod]
		public void CompareBinaryOutput_V3()
			=> CompareBinaryOutput("V3");

		[TestMethod]
		public void CompareBinaryOutput_V3_229()
			=> CompareBinaryOutput("V3_229");

		[TestMethod]
		public void CompareBinaryOutput_V3_451()
			=> CompareBinaryOutput("V3_451");

		[TestMethod]
		public void CompareBinaryOutput_Empty()
			=> CompareBinaryOutput("Empty");

		[TestMethod]
		public void CompareBinaryOutput_Scanner()
			=> CompareBinaryOutput("Scanner");

		private static void CompareBinaryOutput(string fileName)
		{
			using FileStream fs = new(Path.Combine("Data", fileName), FileMode.Open);
			using MemoryStream ms = new();
			fs.CopyTo(ms);

			Spawnset spawnset = Spawnset.Parse(ms);

			byte[] originalBytes = ms.ToArray();
			byte[] bytes = spawnset.ToBytes();

			Assert.AreEqual(originalBytes.Length, bytes.Length);
			for (int i = 0; i < originalBytes.Length; i++)
				Assert.AreEqual(originalBytes[i], bytes[i]);
		}
	}
}
