namespace DevilDaggersWebsite.Razor.Models
{
	public class DonatorModel
	{
		public bool HideDonations { get; init; }
		public int PlayerId { get; init; }
		public string PlayerName { get; init; } = null!;
	}
}
