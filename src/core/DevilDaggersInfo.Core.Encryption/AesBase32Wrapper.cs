using System.Security.Cryptography;
using System.Text;

namespace DevilDaggersInfo.Core.Encryption;

/// <summary>
/// Taken from <see href="http://mjremijan.blogspot.com/2014/08/aes-encryption-between-java-and-c.html"/>.
/// </summary>
public class AesBase32Wrapper
{
	private readonly string _initializationVector;
	private readonly string _password;
	private readonly string _salt;

	public AesBase32Wrapper(string initializationVector, string password, string salt)
	{
		_initializationVector = initializationVector;
		_password = password;
		_salt = salt;
	}

	private enum TransformType
	{
		Encrypt,
		Decrypt,
	}

	public string EncryptAndEncode(string inputRaw)
	{
		// The raw input is in UTF-8 format.
		byte[] buffer = Encoding.UTF8.GetBytes(inputRaw);

		using Aes csp = Aes.Create();
		ICryptoTransform encryptTransform = GetCryptoTransform(csp, TransformType.Encrypt);
		byte[] encrypted = encryptTransform.TransformFinalBlock(buffer, 0, buffer.Length);

		// The encrypted output is in Base32 format.
		return Base32Encoding.ToString(encrypted);
	}

	public string DecodeAndDecrypt(string inputEncrypted)
	{
		// The encrypted input is in Base32 format.
		byte[] buffer = Base32Encoding.ToBytes(inputEncrypted);

		using Aes aes = Aes.Create();
		ICryptoTransform decryptTransform = GetCryptoTransform(aes, TransformType.Decrypt);
		byte[] decrypted = decryptTransform.TransformFinalBlock(buffer, 0, buffer.Length);

		// The decrypted output is in UTF-8 format.
		return Encoding.UTF8.GetString(decrypted);
	}

	private ICryptoTransform GetCryptoTransform(SymmetricAlgorithm symmetricAlgorithm, TransformType transformType)
	{
		symmetricAlgorithm.Mode = CipherMode.CBC;
		symmetricAlgorithm.Padding = PaddingMode.PKCS7;
		symmetricAlgorithm.IV = Encoding.UTF8.GetBytes(_initializationVector);

		using Rfc2898DeriveBytes spec = new(Encoding.UTF8.GetBytes(_password), Encoding.UTF8.GetBytes(_salt), 65536, HashAlgorithmName.SHA1);
		symmetricAlgorithm.Key = spec.GetBytes(16);

		return transformType switch
		{
			TransformType.Encrypt => symmetricAlgorithm.CreateEncryptor(),
			TransformType.Decrypt => symmetricAlgorithm.CreateDecryptor(),
			_ => throw new($"Unknown {nameof(TransformType)} '{transformType}'."),
		};
	}
}
