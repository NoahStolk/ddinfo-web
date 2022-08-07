using DevilDaggersInfo.Web.Server.Domain.Constants;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace DevilDaggersInfo.Web.Server.Domain.Utils;

public static class PasswordValidator
{
	public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
	{
		if (!Regex.IsMatch(password, AuthenticationConstants.PasswordRegex))
			throw new ArgumentException("Password does not meet the requirements.", nameof(password));

		using HMACSHA512 hmac = new();
		passwordSalt = hmac.Key;
		passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
	}

	public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
	{
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
