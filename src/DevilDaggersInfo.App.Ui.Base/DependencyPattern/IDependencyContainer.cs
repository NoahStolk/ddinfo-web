using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.SurvivalEditor;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.Base.DependencyPattern;

public interface IDependencyContainer
{
	Vector2 ViewportOffset { get; }

	int InitialWindowWidth { get; }
	int InitialWindowHeight { get; }

	string? CursorText { get; set; }

	IExtendedLayout? ActiveLayout { get; set; }

	#region Main dependencies

	IUiRenderer UiRenderer { get; }
	IMonoSpaceFontRenderer MonoSpaceFontRenderer { get; }
	IMonoSpaceFontRenderer MonoSpaceSmallFontRenderer { get; }

	#endregion Main dependencies

	#region Main screen

	IExtendedLayout MainLayout { get; }

	#endregion Main screen

	#region DDSE

	ISurvivalEditorMainLayout SurvivalEditorMainLayout { get; }
	IExtendedLayout SurvivalEditorOpenLayout { get; }
	IExtendedLayout SurvivalEditorSaveLayout { get; }

	#endregion DDSE
}
