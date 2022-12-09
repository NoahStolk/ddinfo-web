using DevilDaggersInfo.Api.App.ProcessMemory;
using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.CustomLeaderboardsRecorder;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.SurvivalEditor;
using DevilDaggersInfo.Core.Versioning;
using DevilDaggersInfo.Types.Web;
using Warp.NET.RenderImpl.Ui.Rendering.Renderers;

namespace DevilDaggersInfo.App.Ui.Base.DependencyPattern;

public interface IDependencyContainer
{
	ToolBuildType BuildType { get; }
	SupportedOperatingSystem SupportedOperatingSystem { get; }
	AppVersion AppVersion { get; }

	float Dt { get; }
	float Tt { get; }

	string? TooltipText { get; set; }

	IExtendedLayout? ActiveLayout { get; set; }

	#region Renderers

	MonoSpaceFontRenderer MonoSpaceFontRenderer8 { get; }
	MonoSpaceFontRenderer MonoSpaceFontRenderer12 { get; }
	MonoSpaceFontRenderer MonoSpaceFontRenderer16 { get; }
	MonoSpaceFontRenderer MonoSpaceFontRenderer24 { get; }
	MonoSpaceFontRenderer MonoSpaceFontRenderer32 { get; }
	MonoSpaceFontRenderer MonoSpaceFontRenderer64 { get; }

	SpriteRenderer SpriteRenderer { get; }
	RectangleRenderer RectangleRenderer { get; }
	CircleRenderer CircleRenderer { get; }

	#endregion Renderers

	#region Main screen

	IConfigLayout ConfigLayout { get; }
	IMainLayout MainLayout { get; }

	#endregion Main screen

	#region DDSE screen

	ISurvivalEditorMainLayout SurvivalEditorMainLayout { get; }
	ISurvivalEditor3dLayout SurvivalEditor3dLayout { get; }
	IFileDialogLayout SurvivalEditorOpenLayout { get; }
	IFileDialogLayout SurvivalEditorSaveLayout { get; }

	#endregion DDSE screen

	#region DDCL screen

	ICustomLeaderboardsRecorderMainLayout CustomLeaderboardsRecorderMainLayout { get; }
	IReplayViewer3dLayout CustomLeaderboardsRecorderReplayViewer3dLayout { get; }

	#endregion DDCL screen

	#region DDCL dependencies

	GameMemoryService GameMemoryService { get; }

	#endregion DDCL dependencies
}
