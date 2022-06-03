namespace DevilDaggersInfo.Api.Admin.FileSystem;

public class GetFileSystemEntry
{
	public GetFileSystemEntry(string name, int count, long size)
	{
		Name = name;
		Count = count;
		Size = size;
	}

	public string Name { get; }

	public int Count { get; }

	public long Size { get; }
}
