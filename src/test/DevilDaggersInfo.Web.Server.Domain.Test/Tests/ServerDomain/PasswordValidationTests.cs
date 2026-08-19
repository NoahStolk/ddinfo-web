// ReSharper disable StringLiteralTypo
using DevilDaggersInfo.Web.Server.Domain.Utils;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

internal sealed class PasswordValidationTests
{
	[Test]
	[Arguments("asdfasdfasD1")]
	[Arguments("ASDF 3asdfasdf")]
	[Arguments("0123123 1 23489 C a")]
	[Arguments("0123123 A 23489 c a")]
	[Arguments("0123123 A 234898 a")]
	[Arguments("qQ2qQ2qQ2qQ2qQ2qQ2")]
	public async Task TestValidPasswords(string password)
	{
		PasswordValidator.CreatePasswordHash(password, out byte[] hash, out byte[] salt);
		await Assert.That(hash.Length).IsEqualTo(64);
		await Assert.That(salt.Length).IsEqualTo(128);
	}

	[Test]
	[Arguments("asdfasdfasdf")]
	[Arguments("ASDFasdfasdf")]
	[Arguments("ASDFasdfasd")]
	[Arguments("ABC abc abc ABC")]
	[Arguments("AAAAAAAAAAAA")]
	[Arguments("AAAAAAAAAAAAAA")]
	[Arguments("AAAAAAAAAAAAAA?")]
	[Arguments("12345")]
	[Arguments("")]
	[Arguments("0123123 1 234898 a")]
	[Arguments("0123123 A a")]
	[Arguments("qQ2")]
	[Arguments("qQ2qQ2qQ2")]
	public async Task TestInvalidPasswords(string password)
	{
		await Assert.That(() => PasswordValidator.CreatePasswordHash(password, out _, out _)).Throws<ArgumentException>();
	}
}
