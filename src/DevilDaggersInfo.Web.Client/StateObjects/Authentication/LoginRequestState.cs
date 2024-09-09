using DevilDaggersInfo.Web.ApiSpec.Main.Authentication;

namespace DevilDaggersInfo.Web.Client.StateObjects.Authentication;

public class LoginRequestState : IStateObject<LoginRequest>
{
	public string Name { get; set; } = string.Empty;

	public string Password { get; set; } = string.Empty;

	public LoginRequest ToModel()
	{
		return new LoginRequest
		{
			Name = Name,
			Password = Password,
		};
	}
}
