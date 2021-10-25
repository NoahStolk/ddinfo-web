using Lb = DevilDaggersWebsite.Dto.Leaderboard;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public interface IDefaultLeaderboardPageModel
	{
		public Lb? Leaderboard { get; set; }
	}
}
