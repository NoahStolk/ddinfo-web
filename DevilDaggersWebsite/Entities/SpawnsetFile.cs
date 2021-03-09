using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersWebsite.Entities
{
	public class SpawnsetFile : IEntity
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public int PlayerId { get; set; }

		[ForeignKey(nameof(PlayerId))]
		public Player Player { get; set; } = null!;

		public int? MaxDisplayWaves { get; set; }

		public string? HtmlDescription { get; set; }

		public DateTime LastUpdated { get; set; }

		public bool IsPractice { get; set; }
	}
}
