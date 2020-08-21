namespace DevilDaggersWebsite.Core.Dto
{
	public class CustomLeaderboardCategory
	{
		public string Name { get; set; }
		public string SortingPropertyName { get; set; }
		public bool Ascending { get; set; }
		public string LayoutPartialName { get; set; }
	}
}