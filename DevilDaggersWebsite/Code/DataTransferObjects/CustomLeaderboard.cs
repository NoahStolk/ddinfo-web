using System;

namespace DevilDaggersWebsite.Code.DataTransferObjects
{
	public class CustomLeaderboard
	{
		public string SpawnsetName { get; set; }
		public string SpawnsetAuthorName { get; set; }
		public int Bronze { get; set; }
		public int Silver { get; set; }
		public int Golden { get; set; }
		public int Devil { get; set; }
		public int Homing { get; set; }
		public DateTime? DateLastPlayed { get; set; }
		public DateTime? DateCreated { get; set; }
	}
}