using DevilDaggersInfo.Web.ApiSpec.Main.Authentication;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Services;
using Microsoft.Extensions.Logging;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Services;

public sealed class AuthenticationService(UserManager userManager, ILogger<AuthenticationService> logger)
{
	public AuthenticationResponse Authenticate(AuthenticationRequest authenticationRequest)
	{
		UserEntity? user = userManager.GetUserByJwt(authenticationRequest.Jwt);
		if (user == null)
			throw new BadRequestException("Failed to authenticate. The token is invalid.");

		// ! LINQ filters out null values.
		return new AuthenticationResponse
		{
			Id = user.Id,
			Name = user.Name,
			RoleNames = user.UserRoles?.Select(ur => ur.Role?.Name).Where(s => s != null).ToList()!,
			PlayerId = user.PlayerId,
		};
	}

	public LoginResponse Login(LoginRequest loginRequest)
	{
		UserEntity? user = userManager.Authenticate(loginRequest.Name, loginRequest.Password);
		if (user == null)
		{
			logger.LogInformation("User '{Name}' failed to login.", loginRequest.Name);
			throw new BadRequestException("Username or password is incorrect.");
		}

		string tokenString = userManager.GenerateJwt(user);

		logger.LogInformation("User '{Name}' logged in successfully.", loginRequest.Name);
		return new LoginResponse
		{
			Id = user.Id,
			Name = user.Name,
			Token = tokenString,
		};
	}

	public async Task RegisterAsync(RegistrationRequest registrationRequest)
	{
		if (registrationRequest.Password != registrationRequest.PasswordRepeated)
			throw new BadRequestException("Passwords don't match.");

		try
		{
			await userManager.CreateAsync(registrationRequest.Name, registrationRequest.Password);
			logger.LogInformation("User '{Name}' registered successfully.", registrationRequest.Name);
		}
		catch (Exception ex)
		{
			logger.LogInformation(ex, "User '{Name}' failed to register.", registrationRequest.Name);
			throw new BadRequestException(ex.Message);
		}
	}

	public async Task UpdateNameAsync(UpdateNameRequest updateNameRequest)
	{
		if (updateNameRequest.NewName == updateNameRequest.CurrentName)
			throw new BadRequestException("The same username was entered.");

		UserEntity? user = userManager.Authenticate(updateNameRequest.CurrentName, updateNameRequest.CurrentPassword);
		if (user == null)
		{
			logger.LogInformation("User '{Name}' failed to authenticate while attempting to update name.", updateNameRequest.CurrentName);
			throw new BadRequestException("Username or password is incorrect.");
		}

		try
		{
			await userManager.UpdateNameAsync(user.Id, updateNameRequest.NewName);
			logger.LogInformation("User '{OldName}' changed their name to '{NewName}'.", updateNameRequest.CurrentName, updateNameRequest.NewName);
		}
		catch (Exception ex)
		{
			logger.LogWarning(ex, "User '{Name}' failed to update name.", updateNameRequest.CurrentName);
			throw new BadRequestException(ex.Message);
		}
	}

	public async Task UpdatePasswordAsync(UpdatePasswordRequest updatePasswordRequest)
	{
		if (updatePasswordRequest.NewPassword != updatePasswordRequest.PasswordRepeated)
			throw new BadRequestException("Passwords don't match.");

		if (updatePasswordRequest.NewPassword == updatePasswordRequest.CurrentPassword)
			throw new BadRequestException("The same password was entered.");

		UserEntity? user = userManager.Authenticate(updatePasswordRequest.CurrentName, updatePasswordRequest.CurrentPassword);
		if (user == null)
		{
			logger.LogInformation("User '{Name}' failed to authenticate while attempting to update password.", updatePasswordRequest.CurrentName);
			throw new BadRequestException("Username or password is incorrect.");
		}

		try
		{
			await userManager.UpdatePasswordAsync(user.Id, updatePasswordRequest.NewPassword);
		}
		catch (Exception ex)
		{
			logger.LogWarning(ex, "User '{Name}' failed to update password.", updatePasswordRequest.CurrentName);
			throw new BadRequestException(ex.Message);
		}
	}
}
