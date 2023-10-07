using DevilDaggersInfo.Web.ApiSpec.Admin.Users;

namespace DevilDaggersInfo.Web.Client.StateObjects.Admin.Users;

public class ResetPasswordState : IStateObject<ResetPassword>
{
	public string NewPassword { get; set; } = string.Empty;

	public ResetPassword ToModel() => new()
	{
		NewPassword = NewPassword,
	};
}
