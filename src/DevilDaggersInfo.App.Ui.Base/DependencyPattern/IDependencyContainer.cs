using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.SurvivalEditor;
using System.Numerics;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.Base.DependencyPattern;

public interface IDependencyContainer
{
	Vector2 ViewportOffset { get; }

	int InitialWindowWidth { get; }
	int InitialWindowHeight { get; }

	string? CursorText { get; set; }

	ILayout? ActiveLayout { get; set; }

	#region Main dependencies

	IUiRenderer UiRenderer { get; }
	IMonoSpaceFontRenderer MonoSpaceFontRenderer { get; }
	IMonoSpaceFontRenderer MonoSpaceSmallFontRenderer { get; }

	#endregion Main dependencies

	#region Main screen

	Layout MainLayout { get; }

	#endregion Main screen

	#region DDSE

	ISurvivalEditorMainLayout SurvivalEditorMainLayout { get; }
	Layout SurvivalEditorOpenLayout { get; }
	Layout SurvivalEditorSaveLayout { get; }

	#endregion DDSE
}
