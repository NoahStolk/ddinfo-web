using DevilDaggersInfo.Api.Main.Authentication;

namespace DevilDaggersInfo.Web.Client.StateObjects.Authentication;

public class RegistrationRequestState : IStateObject<RegistrationRequest>
{
	public string Name { get; set; } = string.Empty;

	public string Password { get; set; } = string.Empty;

	public string PasswordRepeated { get; set; } = string.Empty;

	public RegistrationRequest ToModel() => new()
	{
		Name = Name,
		Password = Password,
		PasswordRepeated = PasswordRepeated,
	};
}
