using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Authentication;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
	private readonly IUserService _userService;
	private readonly ILogger<AuthenticationController> _logger;

	public AuthenticationController(IUserService userService, ILogger<AuthenticationController> logger)
	{
		_userService = userService;
		_logger = logger;
	}

	[HttpPost("authenticate")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<AuthenticationResponse> Authenticate([FromBody] AuthenticationRequest authenticationRequest)
	{
		UserEntity? user = _userService.GetUserByJwt(authenticationRequest.Jwt);
		if (user == null)
			return BadRequest("Failed to authenticate. The token is invalid.");

		return new AuthenticationResponse
		{
			Id = user.Id,
			Name = user.Name,
			RoleNames = user.UserRoles?.Select(ur => ur.Role?.Name).Where(s => s != null).ToList()!,
		};
	}

	[HttpPost("login")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<LoginResponse> Login([FromBody] LoginRequest loginRequest)
	{
		UserEntity? user = _userService.Authenticate(loginRequest.Name, loginRequest.Password);
		if (user == null)
		{
			_logger.LogInformation("User '{name}' failed to login.", loginRequest.Name);
			return BadRequest("Username or password is incorrect.");
		}

		string tokenString = _userService.GenerateJwt(user);

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
		try
		{
			_userService.Create(registrationRequest.Name, registrationRequest.Password);
			_logger.LogInformation("User '{name}' registered successfully.", registrationRequest.Name);
			return Ok();
		}
		catch (Exception ex)
		{
			_logger.LogInformation(ex, "User '{name}' failed to register.", registrationRequest.Name);
			return BadRequest(ex.Message);
		}
	}

	[HttpPost("update-name")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult UpdateName([FromBody] UpdateNameRequest updateNameRequest)
	{
		if (updateNameRequest.NewName == updateNameRequest.CurrentName)
			return BadRequest("The same username was entered.");

		UserEntity? user = _userService.Authenticate(updateNameRequest.CurrentName, updateNameRequest.CurrentPassword);
		if (user == null)
		{
			_logger.LogInformation("User '{name}' failed to authenticate while attempting to update name.", updateNameRequest.CurrentName);
			return BadRequest("Username or password is incorrect.");
		}

		try
		{
			_userService.UpdateName(user.Id, updateNameRequest.NewName);
			_logger.LogInformation("User '{oldName}' changed their name to '{newName}'.", updateNameRequest.CurrentName, updateNameRequest.NewName);
			return Ok();
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "User '{name}' failed to update name.", updateNameRequest.CurrentName);
			return BadRequest(ex.Message);
		}
	}

	[HttpPost("update-password")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult UpdatePassword([FromBody] UpdatePasswordRequest updatePasswordRequest)
	{
		if (updatePasswordRequest.NewPassword == updatePasswordRequest.CurrentPassword)
			return BadRequest("The same password was entered.");

		UserEntity? user = _userService.Authenticate(updatePasswordRequest.CurrentName, updatePasswordRequest.CurrentPassword);
		if (user == null)
		{
			_logger.LogInformation("User '{name}' failed to authenticate while attempting to update password.", updatePasswordRequest.CurrentName);
			return BadRequest("Username or password is incorrect.");
		}

		try
		{
			_userService.UpdatePassword(user.Id, updatePasswordRequest.NewPassword);
			return Ok();
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "User '{name}' failed to update password.", updatePasswordRequest.CurrentName);
			return BadRequest(ex.Message);
		}
	}
}
