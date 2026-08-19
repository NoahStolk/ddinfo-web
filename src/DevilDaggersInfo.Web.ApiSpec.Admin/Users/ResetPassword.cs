namespace DevilDaggersInfo.Web.ApiSpec.Admin.Users;

public sealed record ResetPassword
{
	public required string NewPassword { get; init; }
}
