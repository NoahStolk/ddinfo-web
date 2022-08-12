using DevilDaggersInfo.Api.Admin.Users;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Admin.Services;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/users")]
[ApiController]
public class UsersController : ControllerBase
{
	private readonly UserRepository _userRepository;
	private readonly UserService _userService;

	public UsersController(UserRepository userRepository, UserService userService)
	{
		_userRepository = userRepository;
		_userService = userService;
	}

	[HttpGet]
	[Authorize(Roles = Roles.Players)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetUser>>> GetUsers()
		=> await _userRepository.GetUsersAsync();

	[HttpGet("{id}")]
	[Authorize(Roles = Roles.Players)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetUser>> GetUserById(int id)
		=> await _userRepository.GetUserAsync(id);

	[HttpPatch("{id}/toggle-role")]
	[Authorize(Roles = Roles.Admin)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> ToggleRole(int id, ToggleRole toggleRole)
	{
		await _userService.ToggleRoleAsync(id, toggleRole);
		return Ok();
	}

	[HttpPut("{id}/assign-player")]
	[Authorize(Roles = Roles.Players)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> AssignPlayer(int id, AssignPlayer assignPlayer)
	{
		await _userService.AssignPlayerAsync(id, assignPlayer);
		return Ok();
	}

	[HttpPut("{id}/reset-password")]
	[Authorize(Roles = Roles.Admin)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> ResetPasswordForUserById(int id, ResetPassword resetPassword)
	{
		await _userService.ResetPasswordForUser(id, resetPassword);
		return Ok();
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = Roles.Admin)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> DeleteUserById(int id)
	{
		await _userService.DeleteUser(id);
		return Ok();
	}
}
