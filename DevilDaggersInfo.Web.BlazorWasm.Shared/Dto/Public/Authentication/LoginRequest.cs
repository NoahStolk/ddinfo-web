namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Authentication;

public class LoginRequest
{
	public string Name { get; init; } = null!;

	public string Password { get; init; } = null!;
}
