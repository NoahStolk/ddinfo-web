using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
	private readonly IUserService _userService;

	public AuthenticationController(IUserService userService)
	{
		_userService = userService;
	}

	[ApiExplorerSettings(IgnoreApi = true)]
	[HttpPost("login")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<LoginResponse> Login([FromBody] LoginRequest loginRequest)
	{
		UserEntity? user = _userService.Authenticate(loginRequest.Name, loginRequest.Password);
		if (user == null)
			return BadRequest("Username or password is incorrect.");

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

		return Ok(new LoginResponse
		{
			Id = user.Id,
			Name = user.Name,
			Token = tokenString,
		});
	}

	[ApiExplorerSettings(IgnoreApi = true)]
	[HttpPost("register")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult Register([FromBody] RegistrationRequest registrationRequest)
	{
		try
		{
			_userService.Create(registrationRequest.Name, registrationRequest.Password);
			return Ok();
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}
}
