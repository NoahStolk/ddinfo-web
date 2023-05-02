namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.Recording;

public class RecordingIcon : AbstractComponent
{
	private readonly TextureContent _texture;
	private readonly Color _color;

	private readonly Vector2 _size;

	public RecordingIcon(IBounds bounds, TextureContent texture, Color color)
		: base(bounds)
	{
		_texture = texture;
		_color = color;

		_size = new(_texture.Width, _texture.Height);
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		Root.Game.SpriteRenderer.Schedule(_size, (Bounds.Center + scrollOffset).ToVector2(), Depth, _texture, _color);
	}
}
