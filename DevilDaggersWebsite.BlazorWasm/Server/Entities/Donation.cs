using DevilDaggersWebsite.Enumerators;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersWebsite.BlazorWasm.Server.Entities
{
	public class Donation
	{
		[Key]
		public int Id { get; init; }

		public int PlayerId { get; set; }

		[ForeignKey(nameof(PlayerId))]
		public Player Player { get; set; } = null!;

		public int Amount { get; set; }

		public Currency Currency { get; set; }

		public int ConvertedEuroCentsReceived { get; set; }

		public DateTime DateReceived { get; set; }

		[StringLength(64)]
		public string? Note { get; set; }

		public bool IsRefunded { get; set; }
	}
}
