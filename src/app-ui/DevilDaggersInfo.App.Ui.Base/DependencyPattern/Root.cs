namespace DevilDaggersInfo.App.Ui.Base.DependencyPattern;

public static class Root
{
	private static IDependencyContainer? _game;

	public static IDependencyContainer Game
	{
		get => _game ?? throw new InvalidOperationException("Game is not initialized.");
		set => _game = value;
	}
}
