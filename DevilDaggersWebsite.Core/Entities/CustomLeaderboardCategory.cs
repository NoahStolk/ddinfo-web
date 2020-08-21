namespace DevilDaggersWebsite.Core.Entities
{
	public class CustomLeaderboardCategory
	{
		public int Id { get; set; }

		public string Name { get; set; }
		public string SortingPropertyName { get; set; }
		public bool Ascending { get; set; }
		public string LayoutPartialName { get; set; }
	}
}