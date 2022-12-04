using System.Diagnostics;

namespace DevilDaggersInfo.App.Core.NativeInterface.Services.Linux;

public class LinuxMemoryService : INativeMemoryService
{
	public void WriteMemory(Process process, long address, byte[] bytes, int offset, int size)
	{
		throw new NotImplementedException();
	}

	public void ReadMemory(Process process, long address, byte[] bytes, int offset, int size)
	{
		throw new NotImplementedException();
	}

	public Process? GetDevilDaggersProcess()
	{
		throw new NotImplementedException();
	}
}
