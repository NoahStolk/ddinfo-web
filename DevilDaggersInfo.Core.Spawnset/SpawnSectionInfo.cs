namespace DevilDaggersInfo.Core.Spawnset
{
	public struct SpawnSectionInfo
	{
		public SpawnSectionInfo(int loopSpawnCount, float? nonLoopLength)
		{
			LoopSpawnCount = loopSpawnCount;
			NonLoopLength = nonLoopLength;
		}

		public int LoopSpawnCount { get; }
		public float? NonLoopLength { get; }
	}
}
