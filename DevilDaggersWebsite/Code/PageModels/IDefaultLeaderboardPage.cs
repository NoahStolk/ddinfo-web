using Lb = DevilDaggersWebsite.Core.Dto.Leaderboard;

namespace DevilDaggersWebsite.Code.PageModels
{
	public interface IDefaultLeaderboardPage
	{
		public Lb? Leaderboard { get; set; }
	}
}