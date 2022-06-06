using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
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
