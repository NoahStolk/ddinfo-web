using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.SurvivalEditor;
using DevilDaggersInfo.Core.Versioning;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.Base.DependencyPattern;

public interface IDependencyContainer
{
	AppVersion AppVersion { get; }

	Vector2 ViewportOffset { get; }
	Vector2 UiScale { get; }
	Vector2 MousePositionWithOffset { get; }

	int InitialWindowWidth { get; }
	int InitialWindowHeight { get; }

	float Dt { get; }
	float Tt { get; }

	string? TooltipText { get; set; }

	IExtendedLayout? ActiveLayout { get; set; }

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

	IExtendedLayout CustomLeaderboardsRecorderMainLayout { get; }

	#endregion DDCL screen
}
