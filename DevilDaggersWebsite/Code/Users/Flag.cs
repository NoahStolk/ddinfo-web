using System;

namespace DevilDaggersWebsite.Code.Users
{
	[Obsolete("Moved to database.")]
	public class Flag : AbstractUserData
	{
		public override string FileName => "flags";

		public int Id { get; set; }
		public string CountryCode { get; set; }
	}
}