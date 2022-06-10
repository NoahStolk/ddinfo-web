using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Core.CustomLeaderboard.Enums;
using DevilDaggersInfo.Core.CustomLeaderboard.Memory;
using System.Diagnostics;
using System.Text;

namespace DevilDaggersInfo.Core.CustomLeaderboard.Services;

public class ReaderService
{
	private const int _bufferSize = 319;
	private const int _statesBufferSize = 112;

	private long _memoryBlockAddress;
	private Process? _process;
	private bool _isInitialized;

	private readonly INativeMemoryService _nativeMemoryService;

	public ReaderService(INativeMemoryService nativeMemoryService)
	{
		_nativeMemoryService = nativeMemoryService;
	}

	public byte[] Buffer { get; } = new byte[_bufferSize];

	public MainBlock MainBlockPrevious { get; private set; }
	public MainBlock MainBlock { get; private set; }

	public bool HasProcess => _process != null;
	public bool IsInitialized => _isInitialized;

	public bool FindWindow()
	{
		_process = _nativeMemoryService.GetDevilDaggersProcess();
		if (_process == null)
			_isInitialized = false;

		return _process != null;
	}

	public bool Initialize(long ddstatsMarkerOffset)
	{
		if (_isInitialized || _process?.MainModule == null)
			return _isInitialized;

		long? memoryBlockAddress = GetMemoryBlockAddress(_process, ddstatsMarkerOffset);
		if (!memoryBlockAddress.HasValue)
			return _isInitialized;

		_memoryBlockAddress = memoryBlockAddress.Value;

		_isInitialized = true;
		return _isInitialized;
	}

	public void Scan()
	{
		if (_process == null)
			return;

		_nativeMemoryService.ReadMemory(_process, _memoryBlockAddress, Buffer, 0, _bufferSize);

		MainBlockPrevious = MainBlock;
		MainBlock = new(Buffer);
	}

	public AddGameData GetGameDataForUpload()
	{
		if (_process == null)
			return new();

		byte[] buffer = new byte[_statesBufferSize * MainBlock.StatsCount];
		_nativeMemoryService.ReadMemory(_process, MainBlock.StatsBase, buffer, 0, buffer.Length);

		AddGameData gameData = new();

		using MemoryStream ms = new(buffer);
		using BinaryReader br = new(ms);
		for (int i = 0; i < MainBlock.StatsCount; i++)
		{
			gameData.GemsCollected.Add(br.ReadInt32());
			gameData.EnemiesKilled.Add(br.ReadInt32());
			gameData.DaggersFired.Add(br.ReadInt32());
			gameData.DaggersHit.Add(br.ReadInt32());
			gameData.EnemiesAlive.Add(br.ReadInt32());
			_ = br.ReadInt32(); // Skip level gems.
			gameData.HomingStored.Add(br.ReadInt32());
			gameData.GemsDespawned.Add(br.ReadInt32());
			gameData.GemsEaten.Add(br.ReadInt32());
			gameData.GemsTotal.Add(br.ReadInt32());
			gameData.HomingEaten.Add(br.ReadInt32());

			gameData.Skull1sAlive.Add(br.ReadUInt16());
			gameData.Skull2sAlive.Add(br.ReadUInt16());
			gameData.Skull3sAlive.Add(br.ReadUInt16());
			gameData.SpiderlingsAlive.Add(br.ReadUInt16());
			gameData.Skull4sAlive.Add(br.ReadUInt16());
			gameData.Squid1sAlive.Add(br.ReadUInt16());
			gameData.Squid2sAlive.Add(br.ReadUInt16());
			gameData.Squid3sAlive.Add(br.ReadUInt16());
			gameData.CentipedesAlive.Add(br.ReadUInt16());
			gameData.GigapedesAlive.Add(br.ReadUInt16());
			gameData.Spider1sAlive.Add(br.ReadUInt16());
			gameData.Spider2sAlive.Add(br.ReadUInt16());
			gameData.LeviathansAlive.Add(br.ReadUInt16());
			gameData.OrbsAlive.Add(br.ReadUInt16());
			gameData.ThornsAlive.Add(br.ReadUInt16());
			gameData.GhostpedesAlive.Add(br.ReadUInt16());
			gameData.SpiderEggsAlive.Add(br.ReadUInt16());

			gameData.Skull1sKilled.Add(br.ReadUInt16());
			gameData.Skull2sKilled.Add(br.ReadUInt16());
			gameData.Skull3sKilled.Add(br.ReadUInt16());
			gameData.SpiderlingsKilled.Add(br.ReadUInt16());
			gameData.Skull4sKilled.Add(br.ReadUInt16());
			gameData.Squid1sKilled.Add(br.ReadUInt16());
			gameData.Squid2sKilled.Add(br.ReadUInt16());
			gameData.Squid3sKilled.Add(br.ReadUInt16());
			gameData.CentipedesKilled.Add(br.ReadUInt16());
			gameData.GigapedesKilled.Add(br.ReadUInt16());
			gameData.Spider1sKilled.Add(br.ReadUInt16());
			gameData.Spider2sKilled.Add(br.ReadUInt16());
			gameData.LeviathansKilled.Add(br.ReadUInt16());
			gameData.OrbsKilled.Add(br.ReadUInt16());
			gameData.ThornsKilled.Add(br.ReadUInt16());
			gameData.GhostpedesKilled.Add(br.ReadUInt16());
			gameData.SpiderEggsKilled.Add(br.ReadUInt16());
		}

		return gameData;
	}

	public bool IsReplayValid()
	{
		if (_process == null || MainBlock.ReplayLength <= 0 || MainBlock.ReplayLength > 30 * 1024 * 1024)
			return false;

		byte[] headerBytes = new byte[6];
		_nativeMemoryService.ReadMemory(_process, MainBlock.ReplayBase, headerBytes, 0, headerBytes.Length);
		string header = Encoding.Default.GetString(headerBytes);
		return header == "ddrpl.";
	}

	public byte[] GetReplayForUpload()
	{
		if (_process == null)
			return Array.Empty<byte>();

		byte[] buffer = new byte[MainBlock.ReplayLength];
		_nativeMemoryService.ReadMemory(_process, MainBlock.ReplayBase, buffer, 0, buffer.Length);

		return buffer;
	}

	public void WriteReplayToMemory(byte[] replay)
	{
		if (_process == null)
			return;

		_nativeMemoryService.WriteMemory(_process, MainBlock.ReplayBase, replay, 0, replay.Length);
		_nativeMemoryService.WriteMemory(_process, _memoryBlockAddress + 312, BitConverter.GetBytes(replay.Length), 0, sizeof(int));
		_nativeMemoryService.WriteMemory(_process, _memoryBlockAddress + 316, new byte[] { 1 }, 0, 1);
	}

	private long? GetMemoryBlockAddress(Process process, long ddstatsMarkerOffset)
	{
		if (process.MainModule == null)
			return null;

		byte[] pointerBytes = new byte[sizeof(long)];
		_nativeMemoryService.ReadMemory(process, process.MainModule.BaseAddress.ToInt64() + ddstatsMarkerOffset, pointerBytes, 0, sizeof(long));
		return BitConverter.ToInt64(pointerBytes);
	}
}
