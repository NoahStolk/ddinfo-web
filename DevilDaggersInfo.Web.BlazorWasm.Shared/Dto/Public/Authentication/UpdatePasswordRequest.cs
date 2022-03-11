namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Authentication;

public class UpdatePasswordRequest
{
	[Required]
	public string CurrentName { get; set; } = null!;

	[Required]
	public string CurrentPassword { get; set; } = null!;

	[Required]
	[RegularExpression(AuthenticationConstants.PasswordRegex, ErrorMessage = AuthenticationConstants.PasswordValidation)]
	public string NewPassword { get; set; } = null!;

	[Required]
	[RegularExpression(AuthenticationConstants.PasswordRegex, ErrorMessage = AuthenticationConstants.PasswordValidation)]
	public string PasswordRepeated { get; set; } = null!;
}
