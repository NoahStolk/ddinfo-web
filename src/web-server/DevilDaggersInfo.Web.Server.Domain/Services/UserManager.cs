using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public class UserManager
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IConfiguration _configuration;
	private readonly ILogger<UserManager> _logger;

	public UserManager(ApplicationDbContext context, IConfiguration configuration, ILogger<UserManager> logger)
	{
		_dbContext = context;
		_configuration = configuration;
		_logger = logger;
	}

	public UserEntity? Authenticate(string name, string password)
	{
		name = name.Trim();

		if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
			return null;

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

	public UserEntity Create(string name, string password)
	{
		name = name.Trim();

		if (_dbContext.Users.Any(u => u.Name == name))
			throw new($"Name '{name}' is already taken.");

		PasswordValidator.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

		UserEntity user = new()
		{
			Name = name,
			PasswordHash = passwordHash,
			PasswordSalt = passwordSalt,
			DateRegistered = DateTime.UtcNow,
		};

		_dbContext.Users.Add(user);
		_dbContext.SaveChanges();

		return user;
	}

	public void UpdateName(int id, string name)
	{
		name = name.Trim();

		UserEntity? user = _dbContext.Users.Find(id);
		if (user == null)
			throw new("User not found.");

		if (_dbContext.Users.Any(u => u.Name == name))
			throw new($"Name '{user.Name}' is already taken.");

		user.Name = name;

		_dbContext.SaveChanges();
	}

	public void UpdatePassword(int id, string password)
	{
		UserEntity? user = _dbContext.Users.Find(id);
		if (user == null)
			throw new($"User with ID '{id}' not found.");

		PasswordValidator.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

		user.PasswordHash = passwordHash;
		user.PasswordSalt = passwordSalt;

		_dbContext.SaveChanges();
	}

	public void Delete(int id)
	{
		UserEntity? user = _dbContext.Users.Find(id);
		if (user != null)
		{
			_dbContext.Users.Remove(user);
			_dbContext.SaveChanges();
		}
	}

	public string GenerateJwt(UserEntity userEntity)
	{
		string keyString = _configuration["JwtKey"];
		byte[] keyBytes = Encoding.ASCII.GetBytes(keyString);

		SecurityTokenDescriptor tokenDescriptor = new()
		{
			Subject = ClaimsIdentityUtils.CreateClaimsIdentity(
				id: userEntity.Id,
				name: userEntity.Name,
				roles: (userEntity.UserRoles?.Select(ur => ur.Role?.Name).Where(s => s != null).ToList() ?? new())!,
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
			string keyString = _configuration["JwtKey"];
			byte[] keyBytes = Encoding.ASCII.GetBytes(keyString);

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
