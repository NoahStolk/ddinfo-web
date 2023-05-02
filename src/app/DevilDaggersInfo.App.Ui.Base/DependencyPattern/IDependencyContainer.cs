using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.App.Core.NativeInterface.Services;
using DevilDaggersInfo.App.Ui.Base.Platforms;
using Serilog.Core;

namespace DevilDaggersInfo.App.Ui.Base.DependencyPattern;

public interface IDependencyContainer
{
	#region Main dependencies

	Logger Log { get; }
	IPlatformSpecificValues PlatformSpecificValues { get; }
	INativeFileSystemService NativeFileSystemService { get; }
	INativeDialogService NativeDialogService { get; }

	#endregion Main dependencies

	#region DDCL dependencies

	GameMemoryService GameMemoryService { get; }

	#endregion DDCL dependencies
}
