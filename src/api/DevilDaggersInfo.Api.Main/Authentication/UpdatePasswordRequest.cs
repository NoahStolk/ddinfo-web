using DevilDaggersInfo.Api.Main;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Shared.Dto.Public.Authentication;

public record UpdatePasswordRequest
{
	[Required]
	public string CurrentName { get; set; } = null!;

	[Required]
	public string CurrentPassword { get; set; } = null!;

	[Required]
	[RegularExpression(AuthenticationConstants.PasswordRegex, ErrorMessage = AuthenticationConstants.PasswordValidation)]
	public string NewPassword { get; set; } = null!;

	[Required]
	[Compare(nameof(NewPassword), ErrorMessage = "Repeated password does not match.")]
	public string PasswordRepeated { get; set; } = null!;
}
