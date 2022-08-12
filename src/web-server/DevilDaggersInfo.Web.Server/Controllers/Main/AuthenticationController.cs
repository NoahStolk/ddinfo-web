using DevilDaggersInfo.Api.Main.Authentication;
using DevilDaggersInfo.Web.Server.Domain.Main.Services;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
	private readonly AuthenticationService _authenticationService;

	public AuthenticationController(AuthenticationService authenticationService)
	{
		_authenticationService = authenticationService;
	}

	[HttpPost("authenticate")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<AuthenticationResponse> Authenticate([FromBody] AuthenticationRequest authenticationRequest)
		=> _authenticationService.Authenticate(authenticationRequest);

	[HttpPost("login")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<LoginResponse> Login([FromBody] LoginRequest loginRequest)
		=> _authenticationService.Login(loginRequest);

	[HttpPost("register")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult Register([FromBody] RegistrationRequest registrationRequest)
	{
		_authenticationService.Register(registrationRequest);
		return Ok();
	}

	[Authorize]
	[HttpPost("update-name")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult UpdateName([FromBody] UpdateNameRequest updateNameRequest)
	{
		_authenticationService.UpdateName(updateNameRequest);
		return Ok();
	}

	[Authorize]
	[HttpPost("update-password")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult UpdatePassword([FromBody] UpdatePasswordRequest updatePasswordRequest)
	{
		_authenticationService.UpdatePassword(updatePasswordRequest);
		return Ok();
	}
}
