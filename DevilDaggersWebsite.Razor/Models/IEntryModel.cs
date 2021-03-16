using DevilDaggersWebsite.Entities;

namespace DevilDaggersWebsite.Razor.Models
{
	public interface IEntryModel
	{
		public Player? Player { get; }

		public string Username { get; }
		public string FlagCode { get; }
		public string CountryName { get; }

		public string[] Titles { get; }
		public string DaggerColor { get; }
		public string BanString { get; }
	}
}
