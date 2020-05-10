using Lb = DevilDaggersCore.Leaderboards.Leaderboard;

namespace DevilDaggersWebsite.Code.PageModels
{
	public interface IDefaultLeaderboardPage
	{
		public Lb Leaderboard { get; set; }
	}
}