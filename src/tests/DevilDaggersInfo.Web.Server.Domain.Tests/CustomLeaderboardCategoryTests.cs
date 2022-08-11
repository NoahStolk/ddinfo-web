using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Extensions;

namespace DevilDaggersInfo.Web.Server.Domain.Tests;

[TestClass]
public class CustomLeaderboardCategoryTests
{
	[TestMethod]
	public void TestIsAscending()
	{
		Assert.IsFalse(CustomLeaderboardCategory.Survival.IsAscending());
		Assert.IsFalse(CustomLeaderboardCategory.Pacifist.IsAscending());

		Assert.IsTrue(CustomLeaderboardCategory.TimeAttack.IsAscending());
		Assert.IsTrue(CustomLeaderboardCategory.Speedrun.IsAscending());
		Assert.IsTrue(CustomLeaderboardCategory.Race.IsAscending());
		Assert.IsTrue(CustomLeaderboardCategory.RaceNoShooting.IsAscending());
	}
}
