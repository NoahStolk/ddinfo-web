namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Authentication;

public class LoginResponse
{
	public int Id { get; init; }

	public string Name { get; init; } = null!;

	public string Token { get; init; } = null!;
}
