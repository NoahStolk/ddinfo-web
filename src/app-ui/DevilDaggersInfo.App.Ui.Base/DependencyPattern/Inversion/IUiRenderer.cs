using System.Numerics;
using Warp.Numerics;

namespace DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion;

public interface IUiRenderer
{
	void RenderRectangleTopLeft(Vector2i<int> scale, Vector2i<int> topLeft, float depth, Vector3 color);

	void RenderRectangleCenter(Vector2i<int> scale, Vector2i<int> center, float depth, Vector3 color);

	void RenderCircleCenter(Vector2i<int> center, float radius, float depth, Vector3 color);

	void RenderHollowRectangleTopLeft(Vector2 scale, Vector2 topLeft, float depth, Vector3 color);
}
