using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Entities
{
	public class Title
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; }
		public List<PlayerTitle> PlayerTitles { get; set; }
	}
}
