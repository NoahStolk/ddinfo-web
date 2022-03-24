namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Users;

public record ResetPassword
{
	public string NewPassword { get; set; } = null!;
}
