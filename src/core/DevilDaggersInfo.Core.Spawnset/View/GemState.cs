namespace DevilDaggersInfo.Core.Spawnset.View;

public readonly record struct GemState(HandLevel HandLevel, int Value, int TotalGemsCollected)
{
	public GemState Add(int gems)
	{
		HandLevel newHandLevel = HandLevel;
		int newValue = Value + gems;
		int newTotalGemsCollected = TotalGemsCollected + gems;

		if (newHandLevel == HandLevel.Level1 && newValue >= 10)
			newHandLevel = HandLevel.Level2;

		if (newHandLevel == HandLevel.Level2 && newValue >= 70)
		{
			newHandLevel = HandLevel.Level3;
			newValue -= 70;
		}

		if (newHandLevel == HandLevel.Level3 && newValue >= 150)
		{
			newHandLevel = HandLevel.Level4;
			newValue -= 150;
		}

		return new(newHandLevel, newValue, newTotalGemsCollected);
	}
}
