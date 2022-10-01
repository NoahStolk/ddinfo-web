using Warp.Numerics;

namespace DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion;

public interface IUiRenderer
{
	void RenderRectangleTopLeft(Vector2i<int> scale, Vector2i<int> topLeft, float depth, Color color);

	void RenderRectangleCenter(Vector2i<int> scale, Vector2i<int> center, float depth, Color color);

	void RenderCircleCenter(Vector2i<int> center, float radius, float depth, Color color);

}
