using DevilDaggersInfo.Web.ApiSpec.Admin.Users;

namespace DevilDaggersInfo.Web.Client.StateObjects.Admin.Users;

public class AssignPlayerState : IStateObject<AssignPlayer>
{
	public int PlayerId { get; set; }

	public AssignPlayer ToModel()
	{
		return new AssignPlayer
		{
			PlayerId = PlayerId,
		};
	}
}
