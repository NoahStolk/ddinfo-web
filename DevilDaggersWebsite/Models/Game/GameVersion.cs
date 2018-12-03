using System;

namespace DevilDaggersWebsite.Models.Game
{
	public class GameVersion
	{
		public string Name { get; set; }
		public DateTime ReleaseDate { get; set; }

		public GameVersion(string name, DateTime releaseDate)
		{
			Name = name;
			ReleaseDate = releaseDate;
		}
	}
}