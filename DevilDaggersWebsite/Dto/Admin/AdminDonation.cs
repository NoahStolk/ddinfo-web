using DevilDaggersWebsite.Enumerators;
using System;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminDonation
	{
		public int PlayerId { get; init; }
		public int Amount { get; init; }
		public Currency Currency { get; init; }
		public int ConvertedEuroCentsReceived { get; init; }
		public DateTime DateReceived { get; init; }
		public string? Note { get; init; }
		public bool IsRefunded { get; init; }
	}
}
