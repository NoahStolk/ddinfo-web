namespace DevilDaggersWebsite.Code.Users
{
	public class Flag
	{
		public int ID { get; set; }
		public string CountryCode { get; set; }

		public Flag(int id, string countryCode)
		{
			ID = id;
			CountryCode = countryCode;
		}
	}
}