using Lb = DevilDaggersWebsite.Core.Dto.Leaderboard;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public interface IDefaultLeaderboardPage
	{
		public Lb? Leaderboard { get; set; }
	}
}