using DevilDaggersInfo.App.Ui.SurvivalEditor.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering.Text;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;

public static class SpawnsetComponentBuilder
{
	public static SpawnsetTextInput CreateSpawnsetTextInput(IBounds bounds, Action<string> onChange)
	{
		TextInputStyle textInputStyle = new(Color.Black, Color.Gray(0.75f), Color.Gray(0.25f), Color.White, Color.White, Color.Green, Color.Gray(0.5f), 2, 8, FontSize.H12);
		return new(bounds, true, onChange, onChange, null, textInputStyle);
	}
}
