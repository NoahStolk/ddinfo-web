using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.SurvivalEditor;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.Scene;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;
using DevilDaggersInfo.Core.Spawnset;
using Silk.NET.GLFW;
using Warp.NET;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditor3dLayout : Layout, ISurvivalEditor3dLayout
{
	private readonly ShrinkSlider _shrinkSlider;
	private readonly ArenaScene _arenaScene = new();

	private int _currentTick;

	public SurvivalEditor3dLayout()
	{
		_shrinkSlider = new(new PixelBounds(0, 752, 1024, 16), f => _currentTick = (int)(f * 60), true, 0, 0, 0.1f, 0, GlobalStyles.DefaultSliderStyle);
		NestingContext.Add(_shrinkSlider);
	}

	public void BuildScene(SpawnsetBinary spawnset)
	{
		_currentTick = 0;

		_shrinkSlider.Max = spawnset.GetSliderMaxSeconds();
		_shrinkSlider.CurrentValue = Math.Clamp(_shrinkSlider.CurrentValue, 0, _shrinkSlider.Max);

		_arenaScene.BuildArena(spawnset);
	}

	public unsafe void Update()
	{
		_currentTick++;
		_shrinkSlider.CurrentValue = _currentTick / 60f;

		_arenaScene.Update(_currentTick);

		if (Input.IsKeyPressed(Keys.Escape))
		{
			Graphics.Glfw.SetInputMode(Window, CursorStateAttribute.Cursor, CursorModeValue.CursorNormal);
			LayoutManager.ToSurvivalEditorMainLayout();
		}
	}

	public void Render3d()
	{
		_arenaScene.Render();
	}

	public void Render()
	{
	}
}
