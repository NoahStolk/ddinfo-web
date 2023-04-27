using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Extensions;

namespace DevilDaggersInfo.Web.Server.Domain.Tests.DomainTests;

[TestClass]
public class CustomLeaderboardCategoryTests
{
	[TestMethod]
	public void TestIsAscending()
	{
		Assert.IsFalse(CustomLeaderboardCategory.Survival.IsAscending());
		Assert.IsTrue(CustomLeaderboardCategory.TimeAttack.IsAscending());
		Assert.IsTrue(CustomLeaderboardCategory.Speedrun.IsAscending());
		Assert.IsTrue(CustomLeaderboardCategory.Race.IsAscending());
	}
}
