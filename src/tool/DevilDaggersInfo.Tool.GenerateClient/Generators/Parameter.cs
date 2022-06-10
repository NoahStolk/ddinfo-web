namespace DevilDaggersInfo.Tool.GenerateClient.Generators;

internal class Parameter
{
	public Parameter(string type, string name)
	{
		Type = type;
		Name = name;
	}

	public string Type { get; }
	public string Name { get; }

	public string Build()
	{
		string value = Type == "byte[]" ? $"Convert.ToBase64String({Name})" : Name;
		return $"{{ nameof({Name}), {value} }}";
	}

	public override string ToString()
		=> $"{Type} {Name}";
}
