using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Main.Authentication;

public record UpdatePasswordRequest
{
	public required string CurrentName { get; init; }

	public required string CurrentPassword { get; init; }

	[RegularExpression(AuthenticationConstants.PasswordRegex, ErrorMessage = AuthenticationConstants.PasswordValidation)]
	public required string NewPassword { get; init; }

	[Compare(nameof(NewPassword), ErrorMessage = "Repeated password does not match.")]
	public required string PasswordRepeated { get; init; }
}
