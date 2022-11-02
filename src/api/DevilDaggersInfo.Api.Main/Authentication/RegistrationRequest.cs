using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Main.Authentication;

public record RegistrationRequest
{
	[Required]
	[StringLength(32, MinimumLength = 2)]
	public required string Name { get; init; }

	[Required]
	[RegularExpression(AuthenticationConstants.PasswordRegex, ErrorMessage = AuthenticationConstants.PasswordValidation)]
	public required string Password { get; init; }

	[Required]
	[Compare(nameof(Password), ErrorMessage = "Repeated password does not match.")]
	public required string PasswordRepeated { get; init; }
}
