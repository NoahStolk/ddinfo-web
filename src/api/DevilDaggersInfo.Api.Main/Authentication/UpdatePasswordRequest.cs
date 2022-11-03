using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Main.Authentication;

public record UpdatePasswordRequest
{
	[Required]
	public string CurrentName { get; init; } = null!;

	[Required]
	public string CurrentPassword { get; init; } = null!;

	[Required]
	[RegularExpression(AuthenticationConstants.PasswordRegex, ErrorMessage = AuthenticationConstants.PasswordValidation)]
	public string NewPassword { get; init; } = null!;

	[Required]
	[Compare(nameof(NewPassword), ErrorMessage = "Repeated password does not match.")]
	public string PasswordRepeated { get; init; } = null!;
}
