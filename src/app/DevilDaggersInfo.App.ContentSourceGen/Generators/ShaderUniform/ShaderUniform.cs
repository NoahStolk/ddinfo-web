using DevilDaggersInfo.App.ContentSourceGen.Extensions;

namespace DevilDaggersInfo.App.ContentSourceGen.Generators.ShaderUniform;

public sealed record ShaderUniform
{
	public ShaderUniform(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentException("Uniform name cannot be empty or consist of only white-space characters.", nameof(name));

		Name = name;
		PropertyName = name.FirstCharToUpperCase();
	}

	public string Name { get; }

	public string PropertyName { get; }
}
