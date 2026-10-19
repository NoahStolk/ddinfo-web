using DevilDaggersInfo.Core.Mod.Builders;
using DevilDaggersInfo.Web.ApiSpec.Admin.Mods;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Data;

internal static class ModFixtures
{
	internal static byte[] Png { get; } = File.ReadAllBytes(Path.Combine("Resources", "Textures", "green.png"));

	/// <summary>
	/// A minimal valid DD mod binary: one object binding, which is enough for the archive validation to accept it.
	/// </summary>
	internal static BinaryData Binary(string name)
	{
		DdModBinaryBuilder builder = new();
		builder.AddObjectBinding($"{name}-binding", [.. "shader = \"boid\""u8]);
		return new BinaryData { Name = name, Data = builder.Compile() };
	}
}
