using Lb = DevilDaggersCore.Leaderboards.Leaderboard;

namespace DevilDaggersWebsite.Code.PageModels
{
	public interface IGlobalLeaderboardPage
	{
		public Lb Leaderboard { get; set; }
	}
}