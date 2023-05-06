namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditor3dLayout : Layout, IExtendedLayout
{
	private readonly ShrinkSlider _shrinkSlider;
	private readonly EditorArenaScene _arenaScene = new();

	private int _currentTick;

	public SurvivalEditor3dLayout()
	{
		_shrinkSlider = new(new PixelBounds(0, 752, 1024, 16), f => _currentTick = (int)(f * 60), true, 0, 0, 0.1f, 0, SliderStyles.Default);
		NestingContext.Add(_shrinkSlider);

		StateManager.Subscribe<SetLayout>(BuildScene);
	}

	private void BuildScene()
	{
		if (StateManager.LayoutState.CurrentLayout != Root.Dependencies.SurvivalEditor3dLayout)
			return;

		_arenaScene.Update(0);
		_currentTick = 0;

		_shrinkSlider.Max = StateManager.SpawnsetState.Spawnset.GetSliderMaxSeconds();
		_shrinkSlider.CurrentValue = Math.Clamp(_shrinkSlider.CurrentValue, 0, _shrinkSlider.Max);

		_arenaScene.BuildSpawnset(StateManager.SpawnsetState.Spawnset);
	}

	public unsafe void Update()
	{
		_arenaScene.Update(_currentTick);

		_shrinkSlider.Max = StateManager.SpawnsetState.Spawnset.GetSliderMaxSeconds();
		_shrinkSlider.CurrentValue = _currentTick / 60f;

		_currentTick = 0;

		if (Input.IsKeyPressed(Keys.Escape))
		{
			Graphics.Glfw.SetInputMode(Window, CursorStateAttribute.Cursor, CursorModeValue.CursorNormal);
			StateManager.Dispatch(new SetLayout(Root.Dependencies.SurvivalEditorMainLayout));
		}
	}

	public void Render3d()
	{
		_arenaScene.Render(_currentTick);
	}

	public void Render()
	{
		Root.Game.MonoSpaceFontRenderer24.Schedule(Vector2i<int>.One, new(8), 1000, Color.HalfTransparentWhite, StringResources.SurvivalEditor3d, TextAlign.Left);
	}
}
