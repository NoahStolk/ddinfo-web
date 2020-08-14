using System.Collections.Generic;

namespace DevilDaggersWebsite.Code.Database
{
	public class Title
	{
		public int Id { get; set; }

		public string Name { get; set; }
		public List<PlayerTitle> PlayerTitles { get; set; }
	}
}