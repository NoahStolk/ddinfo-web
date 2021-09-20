using Lb = DevilDaggersWebsite.Clients.Leaderboard;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public interface IDefaultLeaderboardPageModel
	{
		public Lb? Leaderboard { get; set; }
	}
}
