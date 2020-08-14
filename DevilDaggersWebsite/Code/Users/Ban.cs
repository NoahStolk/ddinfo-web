using System;

namespace DevilDaggersWebsite.Code.Users
{
	[Obsolete("Moved to database.")]
	public class Ban : AbstractUserData
	{
		public override string FileName => "bans";

		public int Id { get; set; }
		public string Description { get; set; }
		public int? IdResponsible { get; set; }
	}
}