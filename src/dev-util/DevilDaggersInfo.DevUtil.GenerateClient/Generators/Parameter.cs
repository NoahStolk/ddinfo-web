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

	public string BuildAsQueryParameter()
	{
		// TODO: Probably need to do Uri.EscapeDataString for all types. Need to test later.
		string value = Type == "byte[]" ? $"Uri.EscapeDataString(Convert.ToBase64String({Name}))" : Name;
		return $"{{ nameof({Name}), {value} }}";
	}

	public override string ToString()
		=> $"{Type} {Name}";
}
