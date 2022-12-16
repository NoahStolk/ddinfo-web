using DevilDaggersInfo.Api.Admin.Users;

namespace DevilDaggersInfo.Web.Client.StateObjects.Admin.Users;

public class AssignPlayerState : IStateObject<AssignPlayer>
{
	public int PlayerId { get; set; }

	public AssignPlayer ToModel() => new()
	{
		PlayerId = PlayerId,
	};
}
