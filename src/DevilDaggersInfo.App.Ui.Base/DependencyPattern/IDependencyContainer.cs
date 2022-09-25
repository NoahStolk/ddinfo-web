using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.SurvivalEditor;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.Base.DependencyPattern;

public interface IDependencyContainer
{
	Vector2 ViewportOffset { get; }

	int InitialWindowWidth { get; }
	int InitialWindowHeight { get; }

	string? TooltipText { get; set; }

	IExtendedLayout? ActiveLayout { get; set; }

	#region Main dependencies

	IUiRenderer UiRenderer { get; }
	IMonoSpaceFontRenderer MonoSpaceFontRenderer { get; }
	IMonoSpaceFontRenderer MonoSpaceSmallFontRenderer { get; }

	#endregion Main dependencies

	#region Main screen

	IMainLayout MainLayout { get; }

	#endregion Main screen

	#region DDSE

	ISurvivalEditorMainLayout SurvivalEditorMainLayout { get; }
	IFileDialogLayout SurvivalEditorOpenLayout { get; }
	IFileDialogLayout SurvivalEditorSaveLayout { get; }

	#endregion DDSE
}
