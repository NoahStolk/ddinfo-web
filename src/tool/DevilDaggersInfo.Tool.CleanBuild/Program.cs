DirectoryInfo root = new FileInfo(Directory.GetCurrentDirectory()).Directory!.Parent!.Parent!.Parent!.Parent!.Parent!;

Console.ForegroundColor = ConsoleColor.Gray;

foreach (string subDirectory in Directory.GetDirectories(Path.Combine(root.ToString(), "src")))
{
	string directory = new DirectoryInfo(subDirectory).Name;
	if (directory.StartsWith('.') || directory.ToLower() != directory)
		continue;

	foreach (string projectDirectory in Directory.GetDirectories(subDirectory))
	{
		string bin = Path.Combine(projectDirectory, "bin");
		string obj = Path.Combine(projectDirectory, "obj");

		Delete(bin);
		Delete(obj);
	}
}

Console.ForegroundColor = ConsoleColor.Gray;

static void Delete(string directory)
{
	if (Directory.Exists(directory))
	{
		try
		{
			Directory.Delete(directory, true);
		}
		catch (Exception ex)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"Failed: {directory} {ex.Message}");
		}

		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine($"Deleted: {directory}");
	}
	else
	{
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine($"Skipped: {directory}");
	}
}
