using System;

namespace DevilDaggersWebsite.Code.Users
{
	[Obsolete("Moved to database.")]
	public class User : AbstractUserData
	{
		public override string FileName => "users";

		public int Id { get; set; }
		public string Username { get; set; }
		public bool IsAnonymous { get; set; }
		public string[] Titles { get; set; }

		public override string ToString()
			=> $"{Username} ({Id})";
	}
}