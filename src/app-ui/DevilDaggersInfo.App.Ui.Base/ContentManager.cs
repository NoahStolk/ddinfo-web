using DevilDaggersInfo.App.Core.AssetInterop;
using DevilDaggersInfo.App.Ui.Base.Exceptions;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Types.Core.Assets;
using Warp.NET.Content;
using Warp.NET.Content.Conversion.Data;
using Warp.NET.Content.Conversion.Parsers;

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
		if (!Directory.Exists(UserSettings.DevilDaggersInstallationDirectory))
			throw new MissingContentException("Installation directory does not exist.");

		// TODO: Use correct Linux file name for executable.
		// string ddExe = Path.Combine(dir, "dd.exe");
		// if (!File.Exists(ddExe))
		// 	throw new MissingContentException("Executable does not exist.");

		if (!File.Exists(UserSettings.DdSurvivalPath))
			throw new MissingContentException("File 'dd/survival' does not exist.");

		if (!File.Exists(UserSettings.ResAudioPath))
			throw new MissingContentException("File 'res/audio' does not exist.");

		if (!File.Exists(UserSettings.ResDdPath))
			throw new MissingContentException("File 'res/dd' does not exist.");

		// TODO: Also verify survival hash.
		if (!SpawnsetBinary.TryParse(File.ReadAllBytes(UserSettings.DdSurvivalPath), out SpawnsetBinary? defaultSpawnset))
			throw new MissingContentException("File 'dd/survival' could not be parsed.");

		if (Directory.Exists(UserSettings.ModsSurvivalPath))
			throw new MissingContentException("There must not be a directory named 'survival' in the 'mods' directory. You must delete the directory, or mods will not work.");

		ModBinaryReadFilter ddReadFilter = ModBinaryReadFilter.Assets(
			new(AssetType.Texture, "iconmaskdagger"),
			new(AssetType.Mesh, "dagger"),
			new(AssetType.Texture, "daggersilver"),
			new(AssetType.Mesh, "boid4"),
			new(AssetType.Texture, "boid4"),
			new(AssetType.Mesh, "boid4jaw"),
			new(AssetType.Texture, "boid4jaw"),
			new(AssetType.Mesh, "tile"),
			new(AssetType.Texture, "tile"),
			new(AssetType.Mesh, "pillar"),
			new(AssetType.Texture, "pillar"));

		ModBinaryReadFilter audioReadFilter = ModBinaryReadFilter.Assets(
			new(AssetType.Audio, "jump1"),
			new(AssetType.Audio, "jump2"),
			new(AssetType.Audio, "jump3"));

		ModBinary ddBinary;
		using (FileStream fs = new(UserSettings.ResDdPath, FileMode.Open))
			ddBinary = new(fs, ddReadFilter);

		ModBinary audioBinary;
		using (FileStream fs = new(UserSettings.ResAudioPath, FileMode.Open))
			audioBinary = new(fs, audioReadFilter);

		Content = new(
			DefaultSpawnset: defaultSpawnset,
			IconDaggerTexture: GetTexture(ddBinary, "iconmaskdagger"),
			DaggerMesh: GetMesh(ddBinary, "dagger"),
			DaggerSilverTexture: GetTexture(ddBinary, "daggersilver"),
			Skull4Mesh: GetMesh(ddBinary, "boid4"),
			Skull4Texture: GetTexture(ddBinary, "boid4"),
			Skull4JawMesh: GetMesh(ddBinary, "boid4jaw"),
			Skull4JawTexture: GetTexture(ddBinary, "boid4jaw"),
			TileMesh: GetMesh(ddBinary, "tile"),
			TileTexture: GetTexture(ddBinary, "tile"),
			PillarMesh: GetMesh(ddBinary, "pillar"),
			PillarTexture: GetTexture(ddBinary, "pillar"),
			SoundJump1: GetSound(audioBinary, "jump1"),
			SoundJump2: GetSound(audioBinary, "jump2"),
			SoundJump3: GetSound(audioBinary, "jump3"));
	}

	private static Mesh GetMesh(ModBinary ddBinary, string meshName)
	{
		if (!ddBinary.AssetMap.TryGetValue(new(AssetType.Mesh, meshName), out AssetData? meshData))
			throw new MissingContentException($"Required mesh '{meshName}' from 'res/dd' was not found.");

		return MeshConverter.ToWarpMesh(meshData.Buffer);
	}

	private static Texture GetTexture(ModBinary ddBinary, string textureName)
	{
		if (!ddBinary.AssetMap.TryGetValue(new(AssetType.Texture, textureName), out AssetData? textureData))
			throw new MissingContentException($"Required texture '{textureName}' from 'res/dd' was not found.");

		return TextureConverter.ToWarpTexture(textureData.Buffer);
	}

	private static Sound GetSound(ModBinary audioBinary, string soundName)
	{
		if (!audioBinary.AssetMap.TryGetValue(new(AssetType.Audio, soundName), out AssetData? audioData))
			throw new MissingContentException($"Required audio '{soundName}' from 'res/audio' was not found.");

		SoundData waveData = WaveParser.Parse(audioData.Buffer);
		return new(waveData.Channels, waveData.SampleRate, waveData.BitsPerSample, waveData.Data.Length, waveData.Data);
	}
}
