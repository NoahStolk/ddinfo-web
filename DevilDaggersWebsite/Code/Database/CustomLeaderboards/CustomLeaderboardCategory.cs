using DevilDaggersCore.CustomLeaderboards;

namespace DevilDaggersWebsite.Code.Database.CustomLeaderboards
{
	public class CustomLeaderboardCategory : CustomLeaderboardCategoryBase
	{
		public int ID { get; set; }

		public CustomLeaderboardCategory(string name, string sortingPropertyName, bool ascending, string layoutPartialName)
			: base(name, sortingPropertyName, ascending, layoutPartialName)
		{
		}
	}
}