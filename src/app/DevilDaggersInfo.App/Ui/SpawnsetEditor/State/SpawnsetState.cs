using DevilDaggersInfo.Core.Spawnset;
using System.Security.Cryptography;

namespace DevilDaggersInfo.App.Ui.SpawnsetEditor.State;

public static class SpawnsetState
{
	public const string UntitledName = "<untitled>";

	private static SpawnsetBinary _spawnset = SpawnsetBinary.CreateDefault();
	private static byte[] _memorySpawnsetMd5Hash = Array.Empty<byte>();
	private static byte[] _fileSpawnsetMd5Hash = Array.Empty<byte>();

	public static SpawnsetBinary Spawnset
	{
		get => _spawnset;
		set
		{
			_spawnset = value;

			_memorySpawnsetMd5Hash = MD5.HashData(_spawnset.ToBytes());
			IsSpawnsetModified = !_fileSpawnsetMd5Hash.SequenceEqual(_memorySpawnsetMd5Hash);
		}
	}

	public static string? SpawnsetName { get; private set; }

	public static string? SpawnsetPath { get; private set; }

	public static bool IsSpawnsetModified { get; private set; }

	public static void SetFile(string? path, string? name)
	{
		SpawnsetPath = path;
		SpawnsetName = name;
		_fileSpawnsetMd5Hash = _memorySpawnsetMd5Hash;
		IsSpawnsetModified = !_fileSpawnsetMd5Hash.SequenceEqual(_memorySpawnsetMd5Hash);
	}
}
