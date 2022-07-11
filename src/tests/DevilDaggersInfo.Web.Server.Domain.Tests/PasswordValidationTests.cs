using DevilDaggersInfo.Web.Server.Domain.Utils;

namespace DevilDaggersInfo.Web.Server.Domain.Tests;

[TestClass]
public class PasswordValidationTests
{
	[DataTestMethod]
	[DataRow("asdfasdfasD1")]
	[DataRow("ASDF 3asdfasdf")]
	[DataRow("0123123 1 23489 C a")]
	[DataRow("0123123 A 23489 c a")]
	[DataRow("0123123 A 234898 a")]
	[DataRow("qQ2qQ2qQ2qQ2qQ2qQ2")]
	public void TestValidPasswords(string password)
	{
		PasswordValidator.CreatePasswordHash(password, out byte[] hash, out byte[] salt);
		Assert.AreEqual(64, hash.Length);
		Assert.AreEqual(128, salt.Length);
	}

	[DataTestMethod]
	[DataRow("asdfasdfasdf")]
	[DataRow("ASDFasdfasdf")]
	[DataRow("ASDFasdfasd")]
	[DataRow("ABC abc abc ABC")]
	[DataRow("AAAAAAAAAAAA")]
	[DataRow("AAAAAAAAAAAAAA")]
	[DataRow("AAAAAAAAAAAAAA?")]
	[DataRow("12345")]
	[DataRow("")]
	[DataRow("0123123 1 234898 a")]
	[DataRow("0123123 A a")]
	[DataRow("qQ2")]
	[DataRow("qQ2qQ2qQ2")]
	public void TestInvalidPasswords(string password)
	{
		Assert.ThrowsException<ArgumentException>(() => PasswordValidator.CreatePasswordHash(password, out byte[] hash, out byte[] salt));
	}
}
