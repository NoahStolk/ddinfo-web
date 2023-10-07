using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.ApiSpec.Main.Authentication;

public record RegistrationRequest
{
	[StringLength(32, MinimumLength = 2)]
	public required string Name { get; init; }

	[RegularExpression(AuthenticationConstants.PasswordRegex, ErrorMessage = AuthenticationConstants.PasswordValidation)]
	public required string Password { get; init; }

	[Compare(nameof(Password), ErrorMessage = "Repeated password does not match.")]
	public required string PasswordRepeated { get; init; }
}
