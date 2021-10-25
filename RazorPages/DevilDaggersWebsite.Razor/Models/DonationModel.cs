using DevilDaggersWebsite.Enumerators;

namespace DevilDaggersWebsite.Razor.Models
{
	public class DonationModel
	{
		public int Amount { get; init; }
		public int ConvertedEuroCentsReceived { get; init; }
		public Currency Currency { get; init; }
		public bool IsRefunded { get; init; }
	}
}
