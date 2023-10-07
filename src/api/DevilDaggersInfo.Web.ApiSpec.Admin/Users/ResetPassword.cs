namespace DevilDaggersInfo.Web.ApiSpec.Admin.Users;

public record ResetPassword
{
	public required string NewPassword { get; init; }
}
