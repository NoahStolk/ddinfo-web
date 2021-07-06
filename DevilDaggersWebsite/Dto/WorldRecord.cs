using DevilDaggersCore.Game;
using DevilDaggersWebsite.Clients;
using System;

namespace DevilDaggersWebsite.Dto
{
	public class WorldRecord
	{
		public WorldRecord(DateTime dateTime, Entry entry, GameVersion? gameVersion)
		{
			DateTime = dateTime;
			Entry = entry;
			GameVersion = gameVersion;
		}

		public DateTime DateTime { get; }
		public Entry Entry { get; }
		public GameVersion? GameVersion { get; }
	}
}
