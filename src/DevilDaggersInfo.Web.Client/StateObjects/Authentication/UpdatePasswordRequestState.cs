using DevilDaggersInfo.Web.ApiSpec.Main.Authentication;

namespace DevilDaggersInfo.Web.Client.StateObjects.Authentication;

public class UpdatePasswordRequestState : IStateObject<UpdatePasswordRequest>
{
	public string CurrentName { get; set; } = string.Empty;

	public string CurrentPassword { get; set; } = string.Empty;

	public string NewPassword { get; set; } = string.Empty;

	public string PasswordRepeated { get; set; } = string.Empty;

	public UpdatePasswordRequest ToModel()
	{
		return new()
		{
			CurrentName = CurrentName,
			CurrentPassword = CurrentPassword,
			NewPassword = NewPassword,
			PasswordRepeated = PasswordRepeated,
		};
	}
}
