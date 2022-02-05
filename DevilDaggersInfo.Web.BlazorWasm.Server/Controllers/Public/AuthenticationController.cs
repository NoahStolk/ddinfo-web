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

	[HttpPost("update")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult UpdateUser([FromBody] UpdateUserRequest updateUserRequest)
	{
		UserEntity? user = _userService.Authenticate(updateUserRequest.CurrentName, updateUserRequest.CurrentPassword);
		if (user == null)
		{
			_logger.LogWarning("User '{name}' failed to authenticate while attempting to update.", updateUserRequest.CurrentName);
			return BadRequest("Username or password is incorrect.");
		}

		try
		{
			_userService.Update(user.Id, updateUserRequest.NewName, updateUserRequest.NewPassword);
			if (updateUserRequest.CurrentName == updateUserRequest.NewName)
				_logger.LogWarning("User '{oldName}' updated successfully. Name was not updated.", updateUserRequest.CurrentName);
			else
				_logger.LogWarning("User '{oldName}' updated successfully. Name was changed to {newName}.", updateUserRequest.CurrentName, updateUserRequest.NewName);
			return Ok();
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "User '{name}' failed to update.", updateUserRequest.CurrentName);
			return BadRequest(ex.Message);
		}
	}
}
