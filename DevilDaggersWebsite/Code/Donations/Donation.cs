using System;

namespace DevilDaggersWebsite.Code.Donations
{
	public class Donation
	{
		public int DonatorId { get; set; }
		public int Amount { get; set; }
		public Currency Currency { get; set; }
		public int ConvertedEuroCentsReceived { get; set; }
		public DateTime DateReceived { get; set; }
		public bool IsRefunded { get; set; }

		public Donation(int donatorId, int amount, Currency currency, int convertedEuroCentsReceived, DateTime dateReceived, bool isRefunded = false)
		{
			DonatorId = donatorId;
			Amount = amount;
			Currency = currency;
			ConvertedEuroCentsReceived = convertedEuroCentsReceived;
			DateReceived = dateReceived;
			IsRefunded = isRefunded;
		}
	}
}