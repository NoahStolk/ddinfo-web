using DevilDaggersInfo.App.Ui.Base.Rendering.Scissors;
using DevilDaggersInfo.App.Ui.Base.Utils;
using Silk.NET.OpenGL;

namespace DevilDaggersInfo.App.Ui.Base.Rendering.Renderers;

public class CircleRenderer
{
	private const int _circleSubdivisionCount = 40;

	private readonly uint _vaoCircleLines = VertexArrayObjectUtils.CreateFromVertices(UiVertexBuilder.CircleLines(_circleSubdivisionCount));
	private readonly List<CircleLine> _collection = new();

	public void Schedule(Vector2i<int> center, float radius, float depth, Color color)
	{
		_collection.Add(new(center, radius, depth, color, ScissorScheduler.GetCalculatedScissor()));
	}

	public void Render()
	{
		if (_collection.Count == 0)
			return;

		Gl.BindVertexArray(_vaoCircleLines);

		foreach (CircleLine cl in _collection)
		{
			ScissorActivator.SetScissor(cl.Scissor);

			Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(cl.Radius, cl.Radius, 1);

			Shader.SetMatrix4x4(UiUniforms.Model, scaleMatrix * Matrix4x4.CreateTranslation(cl.CenterPosition.X, cl.CenterPosition.Y, cl.Depth));
			Shader.SetVector4(UiUniforms.Color, cl.Color);
			Gl.DrawArrays(PrimitiveType.LineStrip, 0, _circleSubdivisionCount + 1);
		}

		Gl.BindVertexArray(0);

		_collection.Clear();
	}

	private readonly record struct CircleLine(Vector2i<int> CenterPosition, float Radius, float Depth, Color Color, Scissor? Scissor);
}
