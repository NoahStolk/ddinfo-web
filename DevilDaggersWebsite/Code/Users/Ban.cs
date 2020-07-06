namespace DevilDaggersWebsite.Code.Users
{
	public class Ban : AbstractUserData
	{
		public override string FileName => "bans";

		public int Id { get; set; }
		public string Description { get; set; }
		public int? IdResponsible { get; set; }
	}
}