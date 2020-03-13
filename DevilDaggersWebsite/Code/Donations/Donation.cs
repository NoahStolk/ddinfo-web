using System;

namespace DevilDaggersWebsite.Code.Donations
{
	public class Donation
	{
		public int DonatorId { get; set; }
		public int Amount { get; set; }
		public string Currency { get; set; }
		public int ConvertedEuroCentsReceived { get; set; }
		public DateTime DateReceived { get; set; }

		public Donation(int donatorId, int amount, string currency, int convertedEuroCentsReceived, DateTime dateReceived)
		{
			DonatorId = donatorId;
			Amount = amount;
			Currency = currency;
			ConvertedEuroCentsReceived = convertedEuroCentsReceived;
			DateReceived = dateReceived;
		}
	}
}