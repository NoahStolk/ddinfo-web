using DevilDaggersInfo.Web.ApiSpec.Main.Authentication;

namespace DevilDaggersInfo.Web.Client.StateObjects.Authentication;

public class UpdateNameRequestState : IStateObject<UpdateNameRequest>
{
	public string CurrentName { get; set; } = string.Empty;

	public string CurrentPassword { get; set; } = string.Empty;

	public string NewName { get; set; } = string.Empty;

	public UpdateNameRequest ToModel() => new()
	{
		CurrentName = CurrentName,
		CurrentPassword = CurrentPassword,
		NewName = NewName,
	};
}
