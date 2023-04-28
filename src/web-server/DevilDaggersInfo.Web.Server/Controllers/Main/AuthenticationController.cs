using DevilDaggersInfo.Api.Main.Authentication;
using DevilDaggersInfo.Web.Server.Domain.Main.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/authentication")]
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
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
	public async Task<ActionResult> Register([FromBody] RegistrationRequest registrationRequest)
	{
		await _authenticationService.RegisterAsync(registrationRequest);
		return Ok();
	}

	[Authorize]
	[HttpPost("update-name")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> UpdateName([FromBody] UpdateNameRequest updateNameRequest)
	{
		await _authenticationService.UpdateNameAsync(updateNameRequest);
		return Ok();
	}

	[Authorize]
	[HttpPost("update-password")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> UpdatePassword([FromBody] UpdatePasswordRequest updatePasswordRequest)
	{
		await _authenticationService.UpdatePasswordAsync(updatePasswordRequest);
		return Ok();
	}
}
