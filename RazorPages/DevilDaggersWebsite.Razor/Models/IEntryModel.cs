namespace DevilDaggersWebsite.Razor.Models
{
	public interface IEntryModel
	{
		public int PlayerId { get; }
		public int Rank { get; }

		public bool IsBanned { get; }
		public string? BanDescription { get; }

		public string Username { get; }
		public string FlagCode { get; }
		public string CountryName { get; }

		public string[] Titles { get; }
		public string DaggerColor { get; }
		public string BanString { get; }
	}
}
