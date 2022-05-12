using DevilDaggersCustomLeaderboards.Clients;
using DevilDaggersInfo.Core.CustomLeaderboards.Memory;
using System.Diagnostics;
using System.Text;

namespace DevilDaggersInfo.Core.CustomLeaderboards.Services;

public class ReaderService
{
	private const int _bufferSize = 319;
	private const int _statesBufferSize = 112;

	private long _memoryBlockAddress;

	private readonly INativeMemoryService _nativeMemoryService;

	public ReaderService(INativeMemoryService nativeMemoryService)
	{
		_nativeMemoryService = nativeMemoryService;
	}

	public bool IsInitialized { get; set; }

	public Process? Process { get; private set; }

	public byte[] Buffer { get; } = new byte[_bufferSize];

	public MainBlock MainBlockPrevious { get; private set; }
	public MainBlock MainBlock { get; private set; }

	public void FindWindow()
	{
		Process = _nativeMemoryService.GetDevilDaggersProcess();
	}

	public void Initialize(long ddstatsMarkerOffset)
	{
		if (IsInitialized || Process?.MainModule == null)
			return;

		long? memoryBlockAddress = GetMemoryBlockAddress(Process, ddstatsMarkerOffset);
		if (!memoryBlockAddress.HasValue)
			return;

		_memoryBlockAddress = memoryBlockAddress.Value;

		IsInitialized = true;
	}

	public void Scan()
	{
		if (Process == null)
			return;

		_nativeMemoryService.ReadMemory(Process, _memoryBlockAddress, Buffer, 0, _bufferSize);

		MainBlockPrevious = MainBlock;
		MainBlock = new(Buffer);
	}

	public AddGameData GetGameDataForUpload()
	{
		if (Process == null)
			return new();

		byte[] buffer = new byte[_statesBufferSize * MainBlock.StatsCount];
		_nativeMemoryService.ReadMemory(Process, MainBlock.StatsBase, buffer, 0, buffer.Length);

		AddGameData gameData = new();

		using MemoryStream ms = new(buffer);
		using BinaryReader br = new(ms);
		for (int i = 0; i < MainBlock.StatsCount; i++)
		{
			(gameData.GemsCollected ??= new()).Add(br.ReadInt32());
			(gameData.EnemiesKilled ??= new()).Add(br.ReadInt32());
			(gameData.DaggersFired ??= new()).Add(br.ReadInt32());
			(gameData.DaggersHit ??= new()).Add(br.ReadInt32());
			(gameData.EnemiesAlive ??= new()).Add(br.ReadInt32());
			_ = br.ReadInt32(); // Skip level gems.
			(gameData.HomingStored ??= new()).Add(br.ReadInt32());
			(gameData.GemsDespawned ??= new()).Add(br.ReadInt32());
			(gameData.GemsEaten ??= new()).Add(br.ReadInt32());
			(gameData.GemsTotal ??= new()).Add(br.ReadInt32());
			(gameData.HomingEaten ??= new()).Add(br.ReadInt32());

			(gameData.Skull1sAlive ??= new()).Add(br.ReadInt16());
			(gameData.Skull2sAlive ??= new()).Add(br.ReadInt16());
			(gameData.Skull3sAlive ??= new()).Add(br.ReadInt16());
			(gameData.SpiderlingsAlive ??= new()).Add(br.ReadInt16());
			(gameData.Skull4sAlive ??= new()).Add(br.ReadInt16());
			(gameData.Squid1sAlive ??= new()).Add(br.ReadInt16());
			(gameData.Squid2sAlive ??= new()).Add(br.ReadInt16());
			(gameData.Squid3sAlive ??= new()).Add(br.ReadInt16());
			(gameData.CentipedesAlive ??= new()).Add(br.ReadInt16());
			(gameData.GigapedesAlive ??= new()).Add(br.ReadInt16());
			(gameData.Spider1sAlive ??= new()).Add(br.ReadInt16());
			(gameData.Spider2sAlive ??= new()).Add(br.ReadInt16());
			(gameData.LeviathansAlive ??= new()).Add(br.ReadInt16());
			(gameData.OrbsAlive ??= new()).Add(br.ReadInt16());
			(gameData.ThornsAlive ??= new()).Add(br.ReadInt16());
			(gameData.GhostpedesAlive ??= new()).Add(br.ReadInt16());
			(gameData.SpiderEggsAlive ??= new()).Add(br.ReadInt16());

			(gameData.Skull1sKilled ??= new()).Add(br.ReadInt16());
			(gameData.Skull2sKilled ??= new()).Add(br.ReadInt16());
			(gameData.Skull3sKilled ??= new()).Add(br.ReadInt16());
			(gameData.SpiderlingsKilled ??= new()).Add(br.ReadInt16());
			(gameData.Skull4sKilled ??= new()).Add(br.ReadInt16());
			(gameData.Squid1sKilled ??= new()).Add(br.ReadInt16());
			(gameData.Squid2sKilled ??= new()).Add(br.ReadInt16());
			(gameData.Squid3sKilled ??= new()).Add(br.ReadInt16());
			(gameData.CentipedesKilled ??= new()).Add(br.ReadInt16());
			(gameData.GigapedesKilled ??= new()).Add(br.ReadInt16());
			(gameData.Spider1sKilled ??= new()).Add(br.ReadInt16());
			(gameData.Spider2sKilled ??= new()).Add(br.ReadInt16());
			(gameData.LeviathansKilled ??= new()).Add(br.ReadInt16());
			(gameData.OrbsKilled ??= new()).Add(br.ReadInt16());
			(gameData.ThornsKilled ??= new()).Add(br.ReadInt16());
			(gameData.GhostpedesKilled ??= new()).Add(br.ReadInt16());
			(gameData.SpiderEggsKilled ??= new()).Add(br.ReadInt16());
		}

		return gameData;
	}

	public bool IsReplayValid()
	{
		if (Process == null || MainBlock.ReplayLength <= 0 || MainBlock.ReplayLength > 30 * 1024 * 1024)
			return false;

		byte[] headerBytes = new byte[6];
		_nativeMemoryService.ReadMemory(Process, MainBlock.ReplayBase, headerBytes, 0, headerBytes.Length);
		string header = Encoding.Default.GetString(headerBytes);
		return header == "ddrpl.";
	}

	public byte[] GetReplayForUpload()
	{
		if (Process == null)
			return Array.Empty<byte>();

		byte[] buffer = new byte[MainBlock.ReplayLength];
		_nativeMemoryService.ReadMemory(Process, MainBlock.ReplayBase, buffer, 0, buffer.Length);

		return buffer;
	}

	public void WriteReplayToMemory(byte[] replay)
	{
		if (Process == null)
			return;

		_nativeMemoryService.WriteMemory(Process, MainBlock.ReplayBase, replay, 0, replay.Length);
		_nativeMemoryService.WriteMemory(Process, _memoryBlockAddress + 312, BitConverter.GetBytes(replay.Length), 0, sizeof(int));
		_nativeMemoryService.WriteMemory(Process, _memoryBlockAddress + 316, new byte[] { 1 }, 0, 1);
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
