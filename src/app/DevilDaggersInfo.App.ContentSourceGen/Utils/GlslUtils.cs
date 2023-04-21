using Warp.NET.SourceGen.Generators.ShaderUniform;

namespace Warp.NET.SourceGen.Utils;

internal static class GlslUtils
{
	public static ShaderUniform? GetFromGlslLine(string line)
	{
		const string uniform = nameof(uniform);
		string trimmedLine = line.Trim();
		if (!trimmedLine.StartsWith(uniform))
			return null;

		StringBuilder uniformBuilder = new();
		bool foundSecondSpace = false;
		for (int i = uniform.Length + 1; i < trimmedLine.Length; i++)
		{
			char c = trimmedLine[i];
			if (!foundSecondSpace)
			{
				if (c == ' ')
					foundSecondSpace = true;
			}
			else
			{
				if (char.IsLetterOrDigit(c) || c == '_')
					uniformBuilder.Append(c);
				else
					break;
			}
		}

		if (uniformBuilder.Length == 0)
			return null;

		return new(uniformBuilder.ToString());
	}
}
