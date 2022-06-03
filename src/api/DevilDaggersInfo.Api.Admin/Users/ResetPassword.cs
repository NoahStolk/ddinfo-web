namespace DevilDaggersInfo.Api.Admin.Users;

public record ResetPassword
{
	public string NewPassword { get; set; } = null!;
}
