namespace DevilDaggersWebsite.Code.Users
{
	public class Flag
	{
		public int Id { get; set; }
		public string CountryCode { get; set; }

		public Flag(int id, string countryCode)
		{
			Id = id;
			CountryCode = countryCode;
		}
	}
}