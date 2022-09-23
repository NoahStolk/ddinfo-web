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

	#region DDSE

	ISurvivalEditorMainLayout MainLayout { get; }
	Layout OpenLayout { get; }
	Layout SaveLayout { get; }

	#endregion DDSE
}
