using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.SurvivalEditor;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.Base.DependencyPattern;

public interface IDependencyContainer
{
	Vector2 ViewportOffset { get; }
	Vector2 UiScale { get; }

	int InitialWindowWidth { get; }
	int InitialWindowHeight { get; }

	string? TooltipText { get; set; }

	IExtendedLayout? ActiveLayout { get; set; }

	#region Main dependencies

	IUiRenderer UiRenderer { get; }
	IMonoSpaceFontRenderer FontRenderer12X12 { get; }
	IMonoSpaceFontRenderer FontRenderer8X8 { get; }
	IMonoSpaceFontRenderer FontRenderer4X6 { get; }

	#endregion Main dependencies

	#region Main screen

	IConfigLayout ConfigLayout { get; }
	IMainLayout MainLayout { get; }

	#endregion Main screen

	#region DDSE

	ISurvivalEditorMainLayout SurvivalEditorMainLayout { get; }
	IFileDialogLayout SurvivalEditorOpenLayout { get; }
	IFileDialogLayout SurvivalEditorSaveLayout { get; }

	#endregion DDSE
}
