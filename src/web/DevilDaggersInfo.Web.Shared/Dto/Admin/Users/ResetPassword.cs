namespace DevilDaggersInfo.Web.Shared.Dto.Admin.Users;

public record ResetPassword
{
	public string NewPassword { get; set; } = null!;
}
