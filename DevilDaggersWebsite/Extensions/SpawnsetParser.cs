using DevilDaggersWebsite.Models.Game;
using DevilDaggersWebsite.Models.Spawnset;
using System;
using System.Collections.Generic;
using System.IO;

namespace DevilDaggersWebsite.Extensions
{
	public static class SpawnsetParser
	{
		private const int HEADER_BUFFER_SIZE = 36;
		private const int ARENA_BUFFER_SIZE = 10404;

		private const int ARENA_WIDTH = 51;
		private const int ARENA_HEIGHT = 51;

		/// <summary>
		/// Parses the contents of a spawnset file into a Spawnset instance.
		/// This only works for V3 spawnsets.
		/// </summary>
		/// <param name="path">The path to the spawnset file.</param>
		/// <returns>The <see cref="Spawnset">Spawnset</see>.</returns>
		public static Spawnset ParseFile(string path)
		{
			// Open the spawnset file
			FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

			// Set the file values for reading V3 spawnsets
			int spawnBufferSize = (int)fs.Length - (HEADER_BUFFER_SIZE + ARENA_BUFFER_SIZE);
			byte[] headerBuffer = new byte[HEADER_BUFFER_SIZE];
			byte[] arenaBuffer = new byte[ARENA_BUFFER_SIZE];
			byte[] spawnBuffer = new byte[spawnBufferSize];

			// Read the file and write the data into the buffers, then close the file since we do not need it anymore
			fs.Read(headerBuffer, 0, HEADER_BUFFER_SIZE);
			fs.Read(arenaBuffer, 0, ARENA_BUFFER_SIZE);
			fs.Read(spawnBuffer, 0, spawnBufferSize);

			fs.Close();

			// Set the header values
			float shrinkEnd = BitConverter.ToSingle(headerBuffer, 8);
			float shrinkStart = BitConverter.ToSingle(headerBuffer, 12);
			float shrinkRate = BitConverter.ToSingle(headerBuffer, 16);
			float brightness = BitConverter.ToSingle(headerBuffer, 20);

			// Set the arena values
			float[,] arenaTiles = new float[ARENA_WIDTH, ARENA_HEIGHT];
			for (int j = 0; j < arenaBuffer.Length; j += 4)
			{
				int x = j / (ARENA_WIDTH * 4);
				int y = (j / 4) % ARENA_HEIGHT;
				arenaTiles[x, y] = BitConverter.ToSingle(arenaBuffer, j);
			}

			// Set the spawn values
			List<Spawn> spawns = new List<Spawn>();
			int i = 40;
			while (i < spawnBufferSize)
			{
				int enemyType = BitConverter.ToInt32(spawnBuffer, i);
				i += 4;
				float delay = BitConverter.ToSingle(spawnBuffer, i);
				i += 24;

				Enemy enemy = null;
				switch (enemyType)
				{
					case 0: enemy = GameHelper.Squid1; break;
					case 1: enemy = GameHelper.Squid2; break;
					case 2: enemy = GameHelper.Centipede; break;
					case 3: enemy = GameHelper.Spider1; break;
					case 4: enemy = GameHelper.Leviathan; break;
					case 5: enemy = GameHelper.Gigapede; break;
					case 6: enemy = GameHelper.Squid3; break;
					case 7: enemy = GameHelper.Thorn; break;
					case 8: enemy = GameHelper.Spider2; break;
					case 9: enemy = GameHelper.Ghostpede; break;
				}

				spawns.Add(new Spawn(enemy, delay));
			}

			return new Spawnset(spawns, arenaTiles, shrinkStart, shrinkEnd, shrinkRate, brightness);
		}
	}
}