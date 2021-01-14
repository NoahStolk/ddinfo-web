using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersWebsite.Entities
{
	public class SpawnsetFile
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int PlayerId { get; set; }

		[ForeignKey(nameof(PlayerId))]
		public Player Player { get; set; }

		public int? MaxDisplayWaves { get; set; }

		public string? HtmlDescription { get; set; }

		public DateTime LastUpdated { get; set; }
	}
}
