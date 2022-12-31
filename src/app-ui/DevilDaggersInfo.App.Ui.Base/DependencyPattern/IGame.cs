using DevilDaggersInfo.Core.Versioning;
using Warp.NET.RenderImpl.Ui.Rendering.Renderers;

namespace DevilDaggersInfo.App.Ui.Base.DependencyPattern;

public interface IGame
{
	AppVersion AppVersion { get; }

	float Dt { get; }
	float Tt { get; }

	string? TooltipText { get; set; }

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
}
