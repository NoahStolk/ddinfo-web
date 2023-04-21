namespace Warp.NET;

public static class WarpBase
{
	private static GameBase? _game;

	public static GameBase Game
	{
		get => _game ?? throw new InvalidOperationException("Game is not initialized.");
		set
		{
			if (_game != null)
				throw new InvalidOperationException("Game is already initialized.");

			_game = value;
		}
	}
}
