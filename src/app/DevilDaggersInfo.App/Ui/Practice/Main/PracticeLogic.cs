using DevilDaggersInfo.App.Engine.Maths;
using DevilDaggersInfo.App.Ui.Practice.Main.Data;
using DevilDaggersInfo.App.User.Settings;
using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Ui.Practice.Main;

public static class PracticeLogic
{
	public const int SpawnVersion = 6;

#pragma warning disable SA1401, S1104, S2223, CA2211
	public static PracticeState State = PracticeState.Default;
#pragma warning restore CA2211, S2223, S1104, SA1401

	public static void Apply()
	{
		State.TimerStart = Math.Clamp(State.TimerStart, 0, 1400);

		SpawnsetBinary spawnset = ContentManager.Content.DefaultSpawnset;
		float shrinkStart = MathUtils.Lerp(spawnset.ShrinkStart, spawnset.ShrinkEnd, State.TimerStart / ((spawnset.ShrinkStart - spawnset.ShrinkEnd) / spawnset.ShrinkRate));

		SpawnsetBinary generatedSpawnset = spawnset.GetWithHardcodedEndLoop(70).GetWithTrimmedStart(State.TimerStart) with
		{
			HandLevel = State.HandLevel,
			AdditionalGems = State.AdditionalGems,
			TimerStart = State.TimerStart,
			SpawnVersion = SpawnVersion,
			ShrinkStart = shrinkStart,
		};
		File.WriteAllBytes(UserSettings.ModsSurvivalPath, generatedSpawnset.ToBytes());
	}
}
