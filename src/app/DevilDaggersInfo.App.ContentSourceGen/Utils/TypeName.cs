namespace DevilDaggersInfo.App.ContentSourceGen.Utils;

public record TypeName(string Type, string Namespace = Constants.RootNamespace)
{
	public string Type { get; } = Type;

	public string Namespace { get; } = Namespace;

	public string FullName => $"{Namespace}.{Type}";
}
