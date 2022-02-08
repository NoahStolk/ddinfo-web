namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Authentication;

public class RegistrationRequest
{
	[Required]
	[StringLength(32, MinimumLength = 2)]
	public string Name { get; set; } = null!;

	[Required]
	[StringLength(AuthenticationConstants.MaximumPasswordLength, MinimumLength = AuthenticationConstants.MinimumPasswordLength)]
	public string Password { get; set; } = null!;

	[Required]
	[StringLength(AuthenticationConstants.MaximumPasswordLength, MinimumLength = AuthenticationConstants.MinimumPasswordLength)]
	public string PasswordRepeated { get; set; } = null!;
}
