using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Main.Authentication;

public record RegistrationRequest
{
	[Required]
	[StringLength(32, MinimumLength = 2)]
	public string Name { get; init; } = null!;

	[Required]
	[RegularExpression(AuthenticationConstants.PasswordRegex, ErrorMessage = AuthenticationConstants.PasswordValidation)]
	public string Password { get; init; } = null!;

	[Required]
	[Compare(nameof(Password), ErrorMessage = "Repeated password does not match.")]
	public string PasswordRepeated { get; init; } = null!;
}
