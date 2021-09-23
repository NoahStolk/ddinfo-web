namespace DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Generators.ApiHttpClient;

internal class Parameter
{
	public Parameter(string type, string name)
	{
		Type = type;
		Name = name;
	}

	public string Type { get; }
	public string Name { get; }

	public override string ToString()
		=> $"{Type} {Name}";
}
