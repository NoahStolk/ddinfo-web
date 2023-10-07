namespace DevilDaggersInfo.Api.Admin.Users;

public record ResetPassword
{
	public required string NewPassword { get; init; }
}
