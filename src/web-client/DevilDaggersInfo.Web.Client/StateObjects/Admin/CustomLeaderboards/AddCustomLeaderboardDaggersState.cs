using DevilDaggersInfo.Api.Admin.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Client.StateObjects.Admin.CustomLeaderboards;

public class AddCustomLeaderboardDaggersState : IStateObject<AddCustomLeaderboardDaggers>
{
	public double Bronze { get; set; }

	public double Silver { get; set; }

	public double Golden { get; set; }

	public double Devil { get; set; }

	public double Leviathan { get; set; }

	public AddCustomLeaderboardDaggers ToModel() => new()
	{
		Bronze = Bronze,
		Silver = Silver,
		Golden = Golden,
		Devil = Devil,
		Leviathan = Leviathan,
	};
}
