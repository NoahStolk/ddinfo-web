using DevilDaggersInfo.App.Core.NativeInterface.Services;
using DevilDaggersInfo.Core.Replay;
using System.Diagnostics;

namespace DevilDaggersInfo.App.Core.GameMemory;

public class GameMemoryService
{
	private const int _bufferSize = 319;
	private const int _statesBufferSize = 112;

	private long _memoryBlockAddress;
	private Process? _process;

	private readonly INativeMemoryService _nativeMemoryService;

	public GameMemoryService(INativeMemoryService nativeMemoryService)
	{
		_nativeMemoryService = nativeMemoryService;
	}

	public byte[] Buffer { get; } = new byte[_bufferSize];

	public MainBlock MainBlockPrevious { get; private set; }
	public MainBlock MainBlock { get; private set; }

	public bool HasProcess => _process != null;
	public bool IsInitialized { get; private set; }

	public void Initialize(long ddstatsMarkerOffset)
	{
		_process = _nativeMemoryService.GetDevilDaggersProcess();
		if (_process?.MainModule == null)
		{
			IsInitialized = false;
		}
		else
		{
			byte[] pointerBytes = new byte[sizeof(long)];
			_nativeMemoryService.ReadMemory(_process, _process.MainModule.BaseAddress.ToInt64() + ddstatsMarkerOffset, pointerBytes, 0, sizeof(long));
			_memoryBlockAddress = BitConverter.ToInt64(pointerBytes);
			IsInitialized = true;
		}
	}

	public void Scan()
	{
		if (_process == null)
			return;

		_nativeMemoryService.ReadMemory(_process, _memoryBlockAddress, Buffer, 0, _bufferSize);

		MainBlockPrevious = MainBlock;
		MainBlock = new(Buffer);
	}

	public byte[] GetStatsBuffer()
	{
		if (_process == null)
			throw new InvalidOperationException("Cannot get stats buffer while the process is unavailable.");

		byte[] buffer = new byte[_statesBufferSize * MainBlock.StatsCount];
		_nativeMemoryService.ReadMemory(_process, MainBlock.StatsBase, buffer, 0, buffer.Length);
		return buffer;
	}

	public bool IsReplayValid()
	{
		if (_process == null || MainBlock.ReplayLength is <= 0 or > 30 * 1024 * 1024)
			return false;

		byte[] headerBytes = new byte[LocalReplayBinaryHeader.IdentifierLength];
		_nativeMemoryService.ReadMemory(_process, MainBlock.ReplayBase, headerBytes, 0, headerBytes.Length);
		return LocalReplayBinaryHeader.IdentifierIsValid(headerBytes, out _);
	}

	public byte[] ReadReplayFromMemory()
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

	public string? GetPathToSurvivalFile()
	{
		if (_process?.MainModule?.FileName == null)
			return null;

		string? dir = Path.GetDirectoryName(_process.MainModule.FileName);
		if (dir == null)
			return null;

		return Path.Combine(dir, "mods", "survival");
	}
}
