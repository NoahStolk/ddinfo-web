using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
	private readonly IUserService _userService;
	private readonly IWebHostEnvironment _environment;
	private readonly ILogger<AuthenticationController> _logger;

	public AuthenticationController(IUserService userService, IWebHostEnvironment environment, ILogger<AuthenticationController> logger)
	{
		_userService = userService;
		_environment = environment;
		_logger = logger;
	}

	[HttpPost("login")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<LoginResponse> Login([FromBody] LoginRequest loginRequest)
	{
		UserEntity? user = _userService.Authenticate(loginRequest.Name, loginRequest.Password);
		if (user == null)
		{
			_logger.LogWarning("User {user} failed to login.", loginRequest.Name);
			return BadRequest("Username or password is incorrect.");
		}

		JwtSecurityTokenHandler tokenHandler = new();
		SecurityTokenDescriptor tokenDescriptor = new()
		{
			Subject = new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.Name, user.Name),
				new Claim(ClaimTypes.Role, string.Join(",", user.UserRoles!.ConvertAll(r => r.Role!.Name))),
			}),
			Expires = DateTime.UtcNow.AddDays(7),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(AuthSecrets.AuthKey), SecurityAlgorithms.HmacSha256Signature),
		};
		SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
		string tokenString = tokenHandler.WriteToken(token);

		_logger.LogWarning("User {user} logged in successfully.", loginRequest.Name);
		return Ok(new LoginResponse
		{
			Id = user.Id,
			Name = user.Name,
			Token = tokenString,
		});
	}

	[HttpPost("register")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult Register([FromBody] RegistrationRequest registrationRequest)
	{
		if (!_environment.IsDevelopment())
			return BadRequest("Registering is currently not allowed.");

		try
		{
			_userService.Create(registrationRequest.Name, registrationRequest.Password);
			_logger.LogWarning("User {user} registered successfully.", registrationRequest.Name);
			return Ok();
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "User {user} failed to register.", registrationRequest.Name);
			return BadRequest(ex.Message);
		}
	}
}
