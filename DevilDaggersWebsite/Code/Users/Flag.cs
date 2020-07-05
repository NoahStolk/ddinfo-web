namespace DevilDaggersWebsite.Code.Users
{
	public class Flag : AbstractUserData
	{
		public override string FileName => "flags";

		public int Id { get; set; }
		public string CountryCode { get; set; }

		public Flag()
		{
		}

		public Flag(int id, string countryCode)
		{
			Id = id;
			CountryCode = countryCode;
		}
	}
}