using DevilDaggersInfo.Api.Admin.CustomLeaderboards;
using DevilDaggersInfo.Types.Core.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Client.StateObjects.Admin.CustomLeaderboards;

public class AddCustomLeaderboardCriteriaState : IStateObject<AddCustomLeaderboardCriteria>
{
	public CustomLeaderboardCriteriaOperator Operator { get; set; }

	public string? Expression { get; set; }

	public AddCustomLeaderboardCriteria ToModel() => new()
	{
		Expression = Expression,
		Operator = Operator,
	};
}
