using DevilDaggersCore.CustomLeaderboards;

namespace DevilDaggersWebsite.Code.Database
{
	public class CustomLeaderboardCategory : CustomLeaderboardCategoryBase
	{
		public CustomLeaderboardCategory(string name, string sortingPropertyName, bool ascending, string layoutPartialName)
			: base(name, sortingPropertyName, ascending, layoutPartialName)
		{
		}

		public int Id { get; set; }
	}
}