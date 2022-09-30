using System.Numerics;
using Warp.Numerics;

namespace DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion;

public interface IUiRenderer
{
	void RenderTopLeft(Vector2i<int> scale, Vector2i<int> topLeft, float depth, Vector3 color);

	void RenderCenter(Vector2i<int> scale, Vector2i<int> center, float depth, Vector3 color);

	void RenderCircle(Vector2i<int> center, float radius, float depth, Vector3 color);
}
