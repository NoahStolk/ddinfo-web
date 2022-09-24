using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.Base;

public interface IExtendedLayout : ILayout
{
	void Update();

	void Render3d();

	void Render();

	void RenderText();
}
