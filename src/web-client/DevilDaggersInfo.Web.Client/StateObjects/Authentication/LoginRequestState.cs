using DevilDaggersInfo.Api.Main.Authentication;

namespace DevilDaggersInfo.Web.Client.StateObjects.Authentication;

public class LoginRequestState : IStateObject<LoginRequest>
{
	public string Name { get; set; } = string.Empty;

	public string Password { get; set; } = string.Empty;

	public LoginRequest ToModel() => new()
	{
		Name = Name,
		Password = Password,
	};
}
