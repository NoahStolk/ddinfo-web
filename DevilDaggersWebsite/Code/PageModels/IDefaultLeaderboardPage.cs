using Lb = DevilDaggersWebsite.Code.DataTransferObjects.Leaderboard;

namespace DevilDaggersWebsite.Code.PageModels
{
	public interface IDefaultLeaderboardPage
	{
		public Lb Leaderboard { get; set; }
	}
}