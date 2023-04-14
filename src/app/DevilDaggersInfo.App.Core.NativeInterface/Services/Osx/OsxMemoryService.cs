using System.Diagnostics;

namespace DevilDaggersInfo.App.Core.NativeInterface.Services.Osx;

public class OsxMemoryService : INativeMemoryService
{
	public void WriteMemory(Process process, long address, byte[] bytes, int offset, int size)
	{
		// TODO: Implement.
	}

	public void ReadMemory(Process process, long address, byte[] bytes, int offset, int size)
	{
		// TODO: Implement.
	}

	public Process? GetDevilDaggersProcess()
	{
		return Array.Find(Process.GetProcesses(), p => p.ProcessName.StartsWith("devildaggers"));
	}
}
