using DevilDaggersInfo.App.Core.NativeInterface.Services;
using System.Diagnostics;
using System.Text;

namespace DevilDaggersInfo.App.Core.GameMemory;

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

		long? GetMemoryBlockAddress(Process process, long ddstatsMarkerOffset)
		{
			if (process.MainModule == null)
				return null;

			byte[] pointerBytes = new byte[sizeof(long)];
			_nativeMemoryService.ReadMemory(process, process.MainModule.BaseAddress.ToInt64() + ddstatsMarkerOffset, pointerBytes, 0, sizeof(long));
			return BitConverter.ToInt64(pointerBytes);
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
		if (_process == null || MainBlock.ReplayLength <= 0 || MainBlock.ReplayLength > 30 * 1024 * 1024)
			return false;

		// TODO: Probably want to read more data and parse LocalReplayBinaryHeader. That way we could validate more data based on the recorded run.
		byte[] headerBytes = new byte[6];
		_nativeMemoryService.ReadMemory(_process, MainBlock.ReplayBase, headerBytes, 0, headerBytes.Length);
		string header = Encoding.Default.GetString(headerBytes);
		return header == "ddrpl.";
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
