namespace DevilDaggersInfo.Core.Replay;

// TODO: Make non-static.
public static class ReplayBinary
{
	public static byte[] GetSpawnsetBuffer(byte[] contents)
	{
		const int preUsernameLength = 50;
		const int preSpawnsetLength = 26;

		if (contents.Length < preUsernameLength + sizeof(int))
			throw new InvalidReplayBinaryException("Premature end of replay data. Expected to be able to read until username length.");

		using MemoryStream ms = new(contents);
		using BinaryReader br = new(ms);
		br.BaseStream.Seek(preUsernameLength, SeekOrigin.Begin);
		int usernameLength = br.ReadInt32();
		if (usernameLength < 0)
			throw new InvalidReplayBinaryException($"Username length '{usernameLength}' cannot be negative.");

		if (contents.Length < preUsernameLength + sizeof(int) + usernameLength + preSpawnsetLength + sizeof(int))
			throw new InvalidReplayBinaryException("Premature end of replay data. Expected to be able to read until spawnset length.");

		br.BaseStream.Seek(usernameLength + preSpawnsetLength, SeekOrigin.Current);

		int spawnsetLength = br.ReadInt32();
		if (spawnsetLength < 0)
			throw new InvalidReplayBinaryException($"Spawnset length '{spawnsetLength}' cannot be negative.");

		if (preUsernameLength + sizeof(int) + usernameLength + preSpawnsetLength + sizeof(int) + spawnsetLength > contents.Length)
			throw new InvalidReplayBinaryException($"Spawnset length '{spawnsetLength}' exceeds the length of the replay data.");

		return br.ReadBytes(spawnsetLength);
	}
}
