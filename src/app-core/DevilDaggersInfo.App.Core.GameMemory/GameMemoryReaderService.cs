using DevilDaggersInfo.App.Core.NativeInterface.Services;
using DevilDaggersInfo.Core.Replay;
using System.Diagnostics;

namespace DevilDaggersInfo.App.Core.GameMemory;

// TODO: Rename to GameMemoryService.
public class GameMemoryReaderService
{
	private const int _bufferSize = 319;
	private const int _statesBufferSize = 112;

	private long _memoryBlockAddress;
	private Process? _process;
	private bool _isInitialized;

	private readonly INativeMemoryService _nativeMemoryService;

	public GameMemoryReaderService(INativeMemoryService nativeMemoryService)
	{
		_nativeMemoryService = nativeMemoryService;
	}

	public byte[] Buffer { get; } = new byte[_bufferSize];

	public MainBlock MainBlockPrevious { get; private set; }
	public MainBlock MainBlock { get; private set; }

	public bool HasProcess => _process != null;
	public bool IsInitialized => _isInitialized;

	public bool Initialize(long ddstatsMarkerOffset)
	{
		_process = _nativeMemoryService.GetDevilDaggersProcess();
		if (_process == null || _process.MainModule == null)
		{
			_isInitialized = false;
			return _isInitialized;
		}

		byte[] pointerBytes = new byte[sizeof(long)];
		_nativeMemoryService.ReadMemory(_process, _process.MainModule.BaseAddress.ToInt64() + ddstatsMarkerOffset, pointerBytes, 0, sizeof(long));
		_memoryBlockAddress = BitConverter.ToInt64(pointerBytes);
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
		if (_process == null || MainBlock.ReplayLength <= 0 || MainBlock.ReplayLength > 30 * 1024 * 1024)
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
}
