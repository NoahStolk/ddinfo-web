using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Configuration;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public class UserManager
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IOptions<AuthenticationOptions> _authenticationOptions;
	private readonly ILogger<UserManager> _logger;

	public UserManager(ApplicationDbContext context, IOptions<AuthenticationOptions> authenticationOptions, ILogger<UserManager> logger)
	{
		_dbContext = context;
		_authenticationOptions = authenticationOptions;
		_logger = logger;
	}

	public UserEntity? Authenticate(string name, string password)
	{
		name = name.Trim();

		if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
			return null;

		// ! Navigation property.
		UserEntity? user = _dbContext.Users
			.Include(u => u.UserRoles)!
			.ThenInclude(ur => ur.Role)
			.SingleOrDefault(u => u.Name == name);
		if (user == null)
			return null;

		if (!PasswordValidator.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
			return null;

		return user;
	}

	public async Task CreateAsync(string name, string password)
	{
		name = name.Trim();

		if (await _dbContext.Users.AnyAsync(u => u.Name == name))
			throw new BadRequestException($"Name '{name}' is already taken.");

		PasswordValidator.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

		UserEntity user = new()
		{
			Name = name,
			PasswordHash = passwordHash,
			PasswordSalt = passwordSalt,
			DateRegistered = DateTime.UtcNow,
		};

		_dbContext.Users.Add(user);
		await _dbContext.SaveChangesAsync();
	}

	public async Task UpdateNameAsync(int id, string name)
	{
		name = name.Trim();

		UserEntity? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
		if (user == null)
			throw new NotFoundException("User not found.");

		if (await _dbContext.Users.AnyAsync(u => u.Name == name))
			throw new BadRequestException($"Name '{user.Name}' is already taken.");

		user.Name = name;

		await _dbContext.SaveChangesAsync();
	}

	public async Task UpdatePasswordAsync(int id, string password)
	{
		UserEntity? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
		if (user == null)
			throw new NotFoundException($"User with ID '{id}' not found.");

		PasswordValidator.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

		user.PasswordHash = passwordHash;
		user.PasswordSalt = passwordSalt;

		await _dbContext.SaveChangesAsync();
	}

	public string GenerateJwt(UserEntity userEntity)
	{
		byte[] keyBytes = Encoding.ASCII.GetBytes(_authenticationOptions.Value.JwtKey);

		// ! LINQ filters out null values.
		SecurityTokenDescriptor tokenDescriptor = new()
		{
			Subject = ClaimsIdentityUtils.CreateClaimsIdentity(
				id: userEntity.Id,
				name: userEntity.Name,
				roles: (userEntity.UserRoles?.Select(ur => ur.Role?.Name).Where(s => s != null).ToList() ?? [])!,
				playerId: userEntity.PlayerId),
			Expires = DateTime.UtcNow.AddDays(7),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature),
		};

		JwtSecurityTokenHandler tokenHandler = new();
		SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);
	}

	public UserEntity? GetUserByJwt(string jwt)
	{
		try
		{
			byte[] keyBytes = Encoding.ASCII.GetBytes(_authenticationOptions.Value.JwtKey);

			TokenValidationParameters tokenValidationParameters = new()
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
				ValidateIssuer = false,
				ValidateAudience = false,
			};

			JwtSecurityTokenHandler tokenHandler = new();
			ClaimsPrincipal principal = tokenHandler.ValidateToken(jwt, tokenValidationParameters, out SecurityToken securityToken);
			if (securityToken is not JwtSecurityToken jwtSecurityToken)
				return null;

			if (DateTime.UtcNow >= jwtSecurityToken.ValidTo)
				return null;

			if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
				return null;

			string? name = principal.GetName();

			// ! Navigation property.
			return _dbContext.Users
				.Include(u => u.UserRoles)!
					.ThenInclude(ur => ur.Role)
				.FirstOrDefault(u => u.Name == name);
		}
		catch (Exception ex)
		{
			ex.Data.Add("JWT", jwt);
			if (ex is not SecurityTokenExpiredException)
				_logger.LogWarning(ex, "Exception occurred in GetUserByJwt.");

			return null;
		}
	}
}
