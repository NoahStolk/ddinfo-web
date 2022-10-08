using DevilDaggersInfo.App.Core.AssetInterop;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Types.Core.Assets;
using Warp.Content;

namespace DevilDaggersInfo.App.Ui.Base;

public static class ContentManager
{
	private static ContentContainer? _content;

	public static ContentContainer Content
	{
		get => _content ?? throw new InvalidOperationException("Content does not exist.");
		private set => _content = value;
	}

	public static string? Initialize()
	{
		string dir = UserSettings.DevilDaggersInstallationDirectory;
		if (!Directory.Exists(dir))
			return "Installation directory does not exist.";

		// TODO: Linux paths.
		string ddExe = Path.Combine(dir, "dd.exe");
		string survival = Path.Combine(dir, "dd", "survival");
		string resAudio = Path.Combine(dir, "res", "audio");
		string resDd = Path.Combine(dir, "res", "dd");

		if (!File.Exists(ddExe))
			return "Executable does not exist.";

		if (!File.Exists(survival))
			return "File 'dd/survival' does not exist.";

		if (!File.Exists(resAudio))
			return "File 'res/audio' does not exist.";

		if (!File.Exists(resDd))
			return "File 'res/dd' does not exist.";

		// TODO: Also verify survival hash.
		if (!SpawnsetBinary.TryParse(File.ReadAllBytes(Path.Combine(UserSettings.DevilDaggersInstallationDirectory, "dd", "survival")), out SpawnsetBinary? defaultSpawnset))
			return "File 'dd/survival' could not be parsed.";

		if (Directory.Exists(Path.Combine(UserSettings.DevilDaggersInstallationDirectory, "mods", "survival")))
			return "There must not be a directory named 'survival' in the 'mods' directory. You must delete the directory, or mods will not work.";

		ModBinary modBinary = new(File.ReadAllBytes(Path.Combine(UserSettings.DevilDaggersInstallationDirectory, "res", "dd")), ModBinaryReadComprehensiveness.All);
		Texture? iconDaggerTexture = GetTexture(modBinary, "iconmaskdagger");
		Mesh? skull4Mesh = GetMesh(modBinary, "boid4");
		Texture? skull4Texture = GetTexture(modBinary, "boid4");
		Mesh? tileMesh = GetMesh(modBinary, "tile");
		Texture? tileTexture = GetTexture(modBinary, "tile");

		if (iconDaggerTexture == null || skull4Mesh == null || skull4Texture == null || tileMesh == null || tileTexture == null)
			return "Not all required assets from 'res/dd' were found.";

		Content = new(defaultSpawnset, iconDaggerTexture, skull4Mesh, skull4Texture, tileMesh, tileTexture);
		return null;
	}

	private static Mesh? GetMesh(ModBinary modBinary, string meshName)
	{
		if (!modBinary.AssetMap.TryGetValue(new(AssetType.Mesh, meshName), out AssetData? meshData))
			return null;

		Mesh mesh = MeshConverter.ToWarpMesh(meshData.Buffer);
		return mesh;
	}

	private static Texture? GetTexture(ModBinary modBinary, string textureName)
	{
		if (!modBinary.AssetMap.TryGetValue(new(AssetType.Texture, textureName), out AssetData? textureData))
			return null;

		Texture texture = TextureConverter.ToWarpTexture(textureData.Buffer);
		texture.Load();
		return texture;
	}
}
