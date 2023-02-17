using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.Base.Actions;
using DevilDaggersInfo.App.Ui.Base.Styling;
using Warp.NET.Text;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Layouts;

public class SettingsLayout : Layout, IExtendedLayout
{
	public SettingsLayout()
	{
		const int headerHeight = 24;
		MainLayoutBackButton backButton = new(new PixelBounds(0, 0, 24, headerHeight), () => StateManager.Dispatch(new SetLayout(Root.Dependencies.MainLayout)));
		NestingContext.Add(backButton);

		NestingContext.Add(new Label(new PixelBounds(32, 384, 256, 32), "Scale UI to window", LabelStyles.DefaultLeft));
		NestingContext.Add(new Label(new PixelBounds(32, 448, 256, 32), "Show debug output", LabelStyles.DefaultLeft));
		NestingContext.Add(new Label(new PixelBounds(32, 512, 256, 32), "Render while window is inactive", LabelStyles.DefaultLeft));
		NestingContext.Add(new Label(new PixelBounds(32, 640, 256, 32), "Max FPS", LabelStyles.DefaultLeft));

		Checkbox scaleUiToWindowCheckbox = new(new PixelBounds(48, 416, 24, 24), OnChangeScaleUiToWindow);
		Checkbox showDebugOutputCheckbox = new(new PixelBounds(48, 480, 24, 24), b => UserSettings.Model = UserSettings.Model with { ShowDebugOutput = b });
		Checkbox renderWhileWindowIsInactiveCheckbox = new(new PixelBounds(48, 544, 24, 24), b => UserSettings.Model = UserSettings.Model with { RenderWhileWindowIsInactive = b });
		Slider maxFpsSlider = new(new PixelBounds(48, 608, 256, 24), f => UserSettings.Model = UserSettings.Model with { MaxFps = (int)f }, false, 60, 300, 1, UserSettings.Model.MaxFps, SliderStyles.Default with { ValueFormat = "0" });

		NestingContext.Add(scaleUiToWindowCheckbox);
		NestingContext.Add(showDebugOutputCheckbox);
		NestingContext.Add(renderWhileWindowIsInactiveCheckbox);
		NestingContext.Add(maxFpsSlider);

		StateManager.Subscribe<UserSettingsLoaded>(OnUserSettingsLoaded);

		void OnUserSettingsLoaded()
		{
			scaleUiToWindowCheckbox.CurrentValue = UserSettings.Model.ScaleUiToWindow;
			showDebugOutputCheckbox.CurrentValue = UserSettings.Model.ShowDebugOutput;
			renderWhileWindowIsInactiveCheckbox.CurrentValue = UserSettings.Model.RenderWhileWindowIsInactive;
			maxFpsSlider.CurrentValue = UserSettings.Model.MaxFps;
		}
	}

	private static void OnChangeScaleUiToWindow(bool value)
	{
		UserSettings.Model = UserSettings.Model with { ScaleUiToWindow = value };
		ViewportState.UpdateViewports(CurrentWindowState.Width, CurrentWindowState.Height);
	}

	public void Update()
	{
	}

	public void Render3d()
	{
	}

	public void Render()
	{
		Vector2i<int> windowScale = new(CurrentWindowState.Width, CurrentWindowState.Height);
		Game.Self.RectangleRenderer.Schedule(windowScale, windowScale / 2, -100, Color.Gray(0.1f));

		Game.Self.MonoSpaceFontRenderer32.Schedule(Vector2i<int>.One, new(512, 64), 0, Color.White, "Settings", TextAlign.Middle);
	}
}
