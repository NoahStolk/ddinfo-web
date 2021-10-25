using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Entities
{
	public class Title
	{
		[Key]
		public int Id { get; init; }

		[StringLength(16)]
		public string Name { get; set; } = null!;

		public List<PlayerTitle> PlayerTitles { get; set; } = new();
	}
}
