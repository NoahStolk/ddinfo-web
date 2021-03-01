using DevilDaggersWebsite.Enumerators;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersWebsite.Entities
{
	public class Donation : IEntity
	{
		[Key]
		public int Id { get; set; }

		public int PlayerId { get; set; }

		[ForeignKey(nameof(PlayerId))]
		public Player Player { get; set; } = null!;

		public int Amount { get; set; }
		public Currency Currency { get; set; }
		public int ConvertedEuroCentsReceived { get; set; }
		public DateTime DateReceived { get; set; }
		public string? Note { get; set; }
		public bool IsRefunded { get; set; }
	}
}
