namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Authentication;

public record AuthenticationRequest
{
	[Required]
	public string Jwt { get; init; } = null!;
}
