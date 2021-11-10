namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services.Scoped;

public class UserService : IUserService
{
	private readonly ApplicationDbContext _dbContext;

	public UserService(ApplicationDbContext context)
	{
		_dbContext = context;
	}

	public UserEntity? Authenticate(string name, string password)
	{
		if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
			return null;

		UserEntity? user = _dbContext.Users.Include(u => u.UserRoles)!.ThenInclude(ur => ur.Role).SingleOrDefault(u => u.Name == name);
		if (user == null)
			return null;

		if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
			return null;

		return user;
	}

	public List<UserEntity> GetAll() => _dbContext.Users.ToList();

	public UserEntity? GetById(int id) => _dbContext.Users.Find(id);

	public UserEntity Create(string name, string password)
	{
		if (string.IsNullOrWhiteSpace(password))
			throw new("Password is required.");

		if (_dbContext.Users.Any(u => u.Name == name))
			throw new($"Name '{name}' is already taken.");

		CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

		UserEntity user = new()
		{
			Name = name,
			PasswordHash = passwordHash,
			PasswordSalt = passwordSalt,
		};

		_dbContext.Users.Add(user);
		_dbContext.SaveChanges();

		return user;
	}

	public void Update(int id, string? name, string? password)
	{
		UserEntity? user = _dbContext.Users.Find(id);
		if (user == null)
			throw new("User not found.");

		if (!string.IsNullOrWhiteSpace(name) && name != user.Name)
		{
			if (_dbContext.Users.Any(u => u.Name == name))
				throw new($"Name '{user.Name}' is already taken.");

			user.Name = name;
		}

		if (!string.IsNullOrWhiteSpace(password))
		{
			CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

			user.PasswordHash = passwordHash;
			user.PasswordSalt = passwordSalt;
		}

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

	private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
	{
		if (string.IsNullOrWhiteSpace(password))
			throw new ArgumentException("Value cannot be empty or whitespace-only string.", nameof(password));

		using HMACSHA512 hmac = new();
		passwordSalt = hmac.Key;
		passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
	}

	private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
	{
		if (string.IsNullOrWhiteSpace(password))
			throw new ArgumentException("Value cannot be empty or whitespace-only string.", nameof(password));
		if (storedHash.Length != 64)
			throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(storedHash));
		if (storedSalt.Length != 128)
			throw new ArgumentException("Invalid length of password salt (128 bytes expected).", nameof(storedSalt));

		using (HMACSHA512 hmac = new(storedSalt))
		{
			byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
			for (int i = 0; i < computedHash.Length; i++)
			{
				if (computedHash[i] != storedHash[i])
					return false;
			}
		}

		return true;
	}
}
