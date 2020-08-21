using System.Collections.Generic;

namespace DevilDaggersWebsite.Core.Entities
{
	public class Title
	{
		public int Id { get; set; }

		public string Name { get; set; }
		public List<PlayerTitle> PlayerTitles { get; set; }
	}
}