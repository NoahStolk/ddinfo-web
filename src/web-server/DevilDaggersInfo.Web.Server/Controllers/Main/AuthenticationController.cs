using DevilDaggersInfo.Api.Main.Authentication;
using DevilDaggersInfo.Web.Server.Domain.Services;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
	private readonly UserManager _userManager;
	private readonly ILogger<AuthenticationController> _logger;

	public AuthenticationController(UserManager userManager, ILogger<AuthenticationController> logger)
	{
		_userManager = userManager;
		_logger = logger;
	}

	[HttpPost("authenticate")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<AuthenticationResponse> Authenticate([FromBody] AuthenticationRequest authenticationRequest)
	{
		UserEntity? user = _userManager.GetUserByJwt(authenticationRequest.Jwt);
		if (user == null)
			return BadRequest("Failed to authenticate. The token is invalid.");

		return new AuthenticationResponse
		{
			Id = user.Id,
			Name = user.Name,
			RoleNames = user.UserRoles?.Select(ur => ur.Role?.Name).Where(s => s != null).ToList()!,
			PlayerId = user.PlayerId,
		};
	}

	[HttpPost("login")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<LoginResponse> Login([FromBody] LoginRequest loginRequest)
	{
		UserEntity? user = _userManager.Authenticate(loginRequest.Name, loginRequest.Password);
		if (user == null)
		{
			_logger.LogInformation("User '{name}' failed to login.", loginRequest.Name);
			return BadRequest("Username or password is incorrect.");
		}

		string tokenString = _userManager.GenerateJwt(user);

		_logger.LogInformation("User '{name}' logged in successfully.", loginRequest.Name);
		return new LoginResponse
		{
			Id = user.Id,
			Name = user.Name,
			Token = tokenString,
		};
	}

	[HttpPost("register")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult Register([FromBody] RegistrationRequest registrationRequest)
	{
		if (registrationRequest.Password != registrationRequest.PasswordRepeated)
			return BadRequest("Passwords don't match.");

		try
		{
			_userManager.Create(registrationRequest.Name, registrationRequest.Password);
			_logger.LogInformation("User '{name}' registered successfully.", registrationRequest.Name);
			return Ok();
		}
		catch (Exception ex)
		{
			_logger.LogInformation(ex, "User '{name}' failed to register.", registrationRequest.Name);
			return BadRequest(ex.Message);
		}
	}

	[Authorize]
	[HttpPost("update-name")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult UpdateName([FromBody] UpdateNameRequest updateNameRequest)
	{
		if (updateNameRequest.NewName == updateNameRequest.CurrentName)
			return BadRequest("The same username was entered.");

		UserEntity? user = _userManager.Authenticate(updateNameRequest.CurrentName, updateNameRequest.CurrentPassword);
		if (user == null)
		{
			_logger.LogInformation("User '{name}' failed to authenticate while attempting to update name.", updateNameRequest.CurrentName);
			return BadRequest("Username or password is incorrect.");
		}

		try
		{
			_userManager.UpdateName(user.Id, updateNameRequest.NewName);
			_logger.LogInformation("User '{oldName}' changed their name to '{newName}'.", updateNameRequest.CurrentName, updateNameRequest.NewName);
			return Ok();
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "User '{name}' failed to update name.", updateNameRequest.CurrentName);
			return BadRequest(ex.Message);
		}
	}

	[Authorize]
	[HttpPost("update-password")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult UpdatePassword([FromBody] UpdatePasswordRequest updatePasswordRequest)
	{
		if (updatePasswordRequest.NewPassword != updatePasswordRequest.PasswordRepeated)
			return BadRequest("Passwords don't match.");

		if (updatePasswordRequest.NewPassword == updatePasswordRequest.CurrentPassword)
			return BadRequest("The same password was entered.");

		UserEntity? user = _userManager.Authenticate(updatePasswordRequest.CurrentName, updatePasswordRequest.CurrentPassword);
		if (user == null)
		{
			_logger.LogInformation("User '{name}' failed to authenticate while attempting to update password.", updatePasswordRequest.CurrentName);
			return BadRequest("Username or password is incorrect.");
		}

		try
		{
			_userManager.UpdatePassword(user.Id, updatePasswordRequest.NewPassword);
			return Ok();
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "User '{name}' failed to update password.", updatePasswordRequest.CurrentName);
			return BadRequest(ex.Message);
		}
	}
}
