using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.SurvivalEditor;
using DevilDaggersInfo.Core.Versioning;
using System.Numerics;
using Warp.NET.RenderImpl.Ui.Rendering.Renderers;

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

	IExtendedLayout CustomLeaderboardsRecorderMainLayout { get; }

	#endregion DDCL screen
}
