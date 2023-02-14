using DevilDaggersInfo.App.Ui.Base.Settings;
using Warp.NET;

namespace DevilDaggersInfo.App.Ui.Base.Rendering;

public static class ViewportState
{
	public static Viewport Viewport3d { get; set; }

	public static Viewport Viewport { get; set; }

	public static Vector2 Offset { get; set; }

	public static Vector2 Scale { get; set; }

	public static Vector2 MousePosition => (Input.GetMousePosition() - Offset) / Scale;

	public static void UpdateViewports(int width, int height)
	{
		Viewport3d = new(0, 0, width, height);

		const float nativeAspectRatio = Constants.NativeWidth / (float)Constants.NativeHeight;

		if (UserSettings.Model.ScaleUiToWindow)
		{
			float adjustedWidth = height * nativeAspectRatio; // Adjusted for aspect ratio

			int leftOffset = (int)((width - adjustedWidth) / 2);
			Offset = new(leftOffset, 0);
			Viewport = new(leftOffset, 0, (int)adjustedWidth, height); // Fix viewport to maintain aspect ratio
			Scale = new(Viewport.Width / (float)Constants.NativeWidth, Viewport.Height / (float)Constants.NativeHeight);
		}
		else
		{
			int minDimension = (int)Math.Min(height, width / nativeAspectRatio);
			int clampedHeight = Math.Max(Constants.NativeHeight, minDimension / Constants.NativeHeight * Constants.NativeHeight);

			float adjustedWidth = clampedHeight * nativeAspectRatio; // Adjusted for aspect ratio

			int leftOffset = (int)((width - adjustedWidth) / 2);
			int bottomOffset = (height - clampedHeight) / 2;
			Offset = new(leftOffset, bottomOffset);
			Viewport = new(leftOffset, bottomOffset, (int)adjustedWidth, clampedHeight); // Fix viewport to maintain aspect ratio
			Scale = new(Viewport.Width / (float)Constants.NativeWidth, Viewport.Height / (float)Constants.NativeHeight);
		}
	}
}
