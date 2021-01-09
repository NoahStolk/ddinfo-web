using DevilDaggersWebsite.Core.Enumerators;
using System;

namespace DevilDaggersWebsite.Core.Entities
{
	public class Donation
	{
		public int Id { get; set; }

		public int? PlayerId { get; set; }
		public int Amount { get; set; }
		public Currency Currency { get; set; }
		public int ConvertedEuroCentsReceived { get; set; }
		public DateTime DateReceived { get; set; }
		public string? Note { get; set; }
		public bool IsRefunded { get; set; }
	}
}