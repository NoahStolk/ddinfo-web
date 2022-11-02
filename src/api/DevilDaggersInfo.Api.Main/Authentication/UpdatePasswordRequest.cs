using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Main.Authentication;

public record UpdatePasswordRequest
{
	[Required]
	public required string CurrentName { get; init; }

	[Required]
	public required string CurrentPassword { get; init; }

	[Required]
	[RegularExpression(AuthenticationConstants.PasswordRegex, ErrorMessage = AuthenticationConstants.PasswordValidation)]
	public required string NewPassword { get; init; }

	[Required]
	[Compare(nameof(NewPassword), ErrorMessage = "Repeated password does not match.")]
	public required string PasswordRepeated { get; init; }
}
