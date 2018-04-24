namespace DevilDaggersWebsite.Models
{
	public class Donator
	{
		public int Rank { get; set; }
		public string Username { get; set; }
		public int Amount { get; set; }
		public char CurrencySymbol { get; set; }

		public Donator(int rank, string username, int amount, char currencySymbol)
		{
			Rank = rank;
			Username = username;
			Amount = amount;
			CurrencySymbol = currencySymbol;
		}
	}
}