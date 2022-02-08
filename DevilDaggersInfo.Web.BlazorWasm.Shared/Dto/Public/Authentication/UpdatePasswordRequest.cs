namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Authentication;

public class UpdatePasswordRequest
{
	[Required]
	public string CurrentName { get; set; } = null!;

	[Required]
	public string CurrentPassword { get; set; } = null!;

	[Required]
	[StringLength(AuthenticationConstants.MaximumPasswordLength, MinimumLength = AuthenticationConstants.MinimumPasswordLength)]
	public string NewPassword { get; set; } = null!;

	[Required]
	[StringLength(AuthenticationConstants.MaximumPasswordLength, MinimumLength = AuthenticationConstants.MinimumPasswordLength)]
	public string PasswordRepeated { get; set; } = null!;
}
