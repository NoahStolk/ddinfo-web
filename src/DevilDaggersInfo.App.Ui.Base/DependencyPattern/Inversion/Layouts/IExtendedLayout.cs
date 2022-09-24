using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;

public interface IExtendedLayout : ILayout
{
	void Update();

	void Render3d();

	void Render();

	void RenderText();
}
