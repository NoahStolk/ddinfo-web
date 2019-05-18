namespace DevilDaggersWebsite.Models.Users
{
	public class Donator
	{
		public int ID { get; set; }
		public string Username { get; set; }
		public int Amount { get; set; }
		public char CurrencySymbol { get; set; }

		public Donator(int id, string username, int amount, char currencySymbol)
		{
			ID = id;
			Username = username;
			Amount = amount;
			CurrencySymbol = currencySymbol;
		}
	}
}