using DevilDaggersInfo.App.Ui.Base.Enums;
using Warp.Numerics;
using Warp.Text;

namespace DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion;

public interface IMonoSpaceFontRenderer
{
	void SetFont(MonoSpaceFont font);

	void Render(Vector2i<int> scale, Vector2i<int> position, float depth, Color color, object? obj, TextAlign textAlign);

	int GetCharWidthInPixels();
}
