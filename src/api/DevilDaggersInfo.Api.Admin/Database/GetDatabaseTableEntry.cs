namespace DevilDaggersInfo.Api.Admin.Database;

public record GetDatabaseTableEntry
{
	public GetDatabaseTableEntry(string name, int count, int dataSize, int indexSize)
	{
		Name = name;
		Count = count;
		DataSize = dataSize;
		IndexSize = indexSize;
	}

	public string Name { get; }

	public int Count { get; }

	public int DataSize { get; }

	public int IndexSize { get; }
}
