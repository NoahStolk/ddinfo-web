namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FormatAttribute : Attribute
{
	public FormatAttribute(string? format)
	{
		Format = format;
	}

	public string? Format { get; init; }
}
