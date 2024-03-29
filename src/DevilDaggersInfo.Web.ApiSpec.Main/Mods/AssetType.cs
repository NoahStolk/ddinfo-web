namespace DevilDaggersInfo.Web.ApiSpec.Main.Mods;

#pragma warning disable CA1027 // Not a flag enum.
public enum AssetType : byte
#pragma warning restore CA1027
{
	Mesh = 0x01,
	Texture = 0x02,
	Shader = 0x10,
	Audio = 0x20,
	ObjectBinding = 0x80,
}
