namespace DevilDaggersInfo.Web.Client;

[AttributeUsage(AttributeTargets.Assembly)]
internal sealed class BuildTimeAttribute : Attribute
{
	public BuildTimeAttribute(string buildTime)
	{
		BuildTime = buildTime;
	}

	public string BuildTime { get; }
}
