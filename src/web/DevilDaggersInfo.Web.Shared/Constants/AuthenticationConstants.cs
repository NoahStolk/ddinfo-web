namespace DevilDaggersInfo.Web.Shared.Constants;

public static class AuthenticationConstants
{
	public const string PasswordRegex = "^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{12,40}$";

	public const string PasswordValidation = "Password must be at least 12 characters in length (max 40), contain at least one uppercase letter, contain at least one lowercase letter, and contain at least one digit.";
}
