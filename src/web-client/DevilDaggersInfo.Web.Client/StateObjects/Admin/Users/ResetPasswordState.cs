using DevilDaggersInfo.Api.Admin.Users;

namespace DevilDaggersInfo.Web.Client.StateObjects.Admin.Users;

public class ResetPasswordState : IStateObject<ResetPassword>
{
	public string NewPassword { get; set; } = null!;

	public ResetPassword ToModel() => new()
	{
		NewPassword = NewPassword,
	};
}
