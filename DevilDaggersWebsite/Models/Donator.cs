namespace DevilDaggersWebsite.Models
{
	public class Donator
	{
		public string Username { get; set; }
		public int Amount { get; set; }
		public char CurrencySymbol { get; set; }

		public Donator(string username, int amount, char currencySymbol)
		{
			Username = username;
			Amount = amount;
			CurrencySymbol = currencySymbol;
		}
	}
}