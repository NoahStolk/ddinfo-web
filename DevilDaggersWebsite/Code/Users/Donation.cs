using System;

namespace DevilDaggersWebsite.Code.Users
{
	public class Donation : AbstractUserData
	{
		public override string FileName => "donations";

		public int DonatorId { get; set; }
		public int Amount { get; set; }
		public Currency Currency { get; set; }
		public int ConvertedEuroCentsReceived { get; set; }
		public DateTime DateReceived { get; set; }
		public string Note { get; set; }
		public bool IsRefunded { get; set; }
	}
}