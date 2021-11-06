namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Authentication;

public class RegistrationRequest
{
	[StringLength(32, MinimumLength = 2)]
	public string Name { get; set; } = null!;

	[StringLength(128, MinimumLength = 8)]
	public string Password { get; set; } = null!;
}
