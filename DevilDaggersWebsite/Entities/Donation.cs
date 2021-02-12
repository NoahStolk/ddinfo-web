using DevilDaggersWebsite.Enumerators;
using System;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Entities
{
	public class Donation
	{
		[Key]
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
