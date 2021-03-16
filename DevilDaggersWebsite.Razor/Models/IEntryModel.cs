using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Entities;

namespace DevilDaggersWebsite.Razor.Models
{
	public interface IEntryModel
	{
		public Entry Entry { get; }
		public Player? Player { get; }

		public string[] Titles { get; }
		public string DaggerColor { get; }
		public string BanString { get; }
	}
}
