namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Authentication;

public class AuthenticationRequest
{
	[Required]
	public string Jwt { get; init; } = null!;
}
