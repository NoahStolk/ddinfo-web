namespace DevilDaggersWebsite.Code.Users
{
	public class Flag : AbstractUserData
	{
		public override string FileName => "flags";

		public int Id { get; set; }
		public string CountryCode { get; set; }
	}
}