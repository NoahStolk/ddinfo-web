namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Authentication;

public class RegistrationRequest
{
	public string Name { get; init; } = null!;

	public string Password { get; init; } = null!;
}
