using DevilDaggersWebsite.Enumerators;
using System;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Dto.Donations
{
	public class EditDonationDto
	{
		public int PlayerId { get; init; }

		public int Amount { get; init; }

		public Currency Currency { get; init; }

		public int ConvertedEuroCentsReceived { get; init; }

		public DateTime DateReceived { get; init; }

		[StringLength(64)]
		public string? Note { get; init; }

		public bool IsRefunded { get; init; }
	}
}
