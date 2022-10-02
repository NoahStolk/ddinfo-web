using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using System.Numerics;
using Warp;
using Warp.Extensions;
using Warp.Numerics;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Rendering;

public record Scissor(int X, int Y, uint Width, uint Height)
{
	public static Scissor FromComponent(Rectangle metric, Vector2i<int> parentPosition)
	{
		Vector2 viewportOffset = Root.Game.ViewportOffset;
		Vector2i<int> scaledSize = (metric.Size.ToVector2() * Root.Game.UiScale).RoundToVector2Int32();
		Vector2i<int> scaledTopLeft = (metric.TopLeft.ToVector2() * Root.Game.UiScale).RoundToVector2Int32();
		Vector2i<int> scaledParentPosition = (parentPosition.ToVector2() * Root.Game.UiScale).RoundToVector2Int32();
		return new(
			scaledTopLeft.X + (int)viewportOffset.X + scaledParentPosition.X,
			Graphics.WindowHeight - (scaledSize.Y + scaledParentPosition.Y) - (int)viewportOffset.Y,
			(uint)scaledSize.X,
			(uint)scaledSize.Y);
	}
}
