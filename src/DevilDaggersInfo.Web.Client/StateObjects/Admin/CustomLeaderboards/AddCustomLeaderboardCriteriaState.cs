using DevilDaggersInfo.Web.ApiSpec.Admin.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Client.StateObjects.Admin.CustomLeaderboards;

public class AddCustomLeaderboardCriteriaState : IStateObject<AddCustomLeaderboardCriteria>
{
	public CustomLeaderboardCriteriaOperator Operator { get; set; }

	public string? Expression { get; set; }

	public AddCustomLeaderboardCriteria ToModel()
	{
		return new()
		{
			Expression = Expression,
			Operator = Operator,
		};
	}
}
