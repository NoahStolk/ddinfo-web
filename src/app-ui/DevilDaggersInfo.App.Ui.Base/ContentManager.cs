using DevilDaggersInfo.App.Core.AssetInterop;
using DevilDaggersInfo.App.Ui.Base.Exceptions;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.App.Ui.Base.Utils;
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
		get => _content ?? throw new InvalidOperationException("Content is not initialized.");
		private set => _content = value;
	}

	public static void Initialize()
	{
		string dir = UserSettings.DevilDaggersInstallationDirectory;
		if (!Directory.Exists(dir))
			throw new MissingContentException("Installation directory does not exist.");

		// TODO: Use correct Linux file name for executable.
		// string ddExe = Path.Combine(dir, "dd.exe");
		string survival = Path.Combine(dir, "dd", "survival");
		string resDd = Path.Combine(dir, "res", "dd");

		// if (!File.Exists(ddExe))
		// 	throw new MissingContentException("Executable does not exist.");

		if (!File.Exists(survival))
			throw new MissingContentException("File 'dd/survival' does not exist.");

		if (!File.Exists(resDd))
			throw new MissingContentException("File 'res/dd' does not exist.");

		// TODO: Also verify survival hash.
		if (!SpawnsetBinary.TryParse(File.ReadAllBytes(Path.Combine(UserSettings.DevilDaggersInstallationDirectory, "dd", "survival")), out SpawnsetBinary? defaultSpawnset))
			throw new MissingContentException("File 'dd/survival' could not be parsed.");

		if (Directory.Exists(Path.Combine(UserSettings.DevilDaggersInstallationDirectory, "mods", "survival")))
			throw new MissingContentException("There must not be a directory named 'survival' in the 'mods' directory. You must delete the directory, or mods will not work.");

		ModBinary modBinary = new(File.ReadAllBytes(Path.Combine(UserSettings.DevilDaggersInstallationDirectory, "res", "dd")), ModBinaryReadComprehensiveness.All);

		Content = new(
			DefaultSpawnset: defaultSpawnset,
			IconDaggerTexture: GetTexture(modBinary, "iconmaskdagger"),
			DaggerMesh: GetMesh(modBinary, "dagger"),
			DaggerSilverTexture: GetTexture(modBinary, "daggersilver"),
			Skull4Mesh: GetMesh(modBinary, "boid4"),
			Skull4Texture: GetTexture(modBinary, "boid4"),
			Skull4JawMesh: GetMesh(modBinary, "boid4jaw"),
			Skull4JawTexture: GetTexture(modBinary, "boid4jaw"),
			TileMesh: GetMesh(modBinary, "tile"),
			TileTexture: GetTexture(modBinary, "tile"),
			PillarMesh: TallTilesBuilder.CreateTallTiles(GetMesh(modBinary, "pillar"), 16),
			PillarTexture: GetTexture(modBinary, "pillar"));
	}

	private static Mesh GetMesh(ModBinary modBinary, string meshName)
	{
		if (!modBinary.AssetMap.TryGetValue(new(AssetType.Mesh, meshName), out AssetData? meshData))
			throw new MissingContentException($"Required mesh '{meshName}' from 'res/dd' was not found.");

		return MeshConverter.ToWarpMesh(meshData.Buffer);
	}

	private static Texture GetTexture(ModBinary modBinary, string textureName)
	{
		if (!modBinary.AssetMap.TryGetValue(new(AssetType.Texture, textureName), out AssetData? textureData))
			throw new MissingContentException($"Required texture '{textureName}' from 'res/dd' was not found.");

		return TextureConverter.ToWarpTexture(textureData.Buffer);
	}
}
