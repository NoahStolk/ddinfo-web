namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Authentication;

public class LoginRequest
{
	[Required]
	public string Name { get; set; } = null!;

	[Required]
	public string Password { get; set; } = null!;
}
