using DevilDaggersInfo.Api.Admin.Tools;

namespace DevilDaggersInfo.Web.Client.StateObjects.Admin.Tools;

public class AddDistributionState : IStateObject<AddDistribution>
{
	public string Name { get; set; } = string.Empty;

	public ToolPublishMethod PublishMethod { get; set; }

	public ToolBuildType BuildType { get; set; }

	public string Version { get; set; } = string.Empty;

	public byte[] ZipFileContents { get; set; } = Array.Empty<byte>();

	public bool UpdateVersion { get; set; }

	public bool UpdateRequiredVersion { get; set; }

	public AddDistribution ToModel() => new()
	{
		Name = Name,
		PublishMethod = PublishMethod,
		BuildType = BuildType,
		Version = Version,
		ZipFileContents = ZipFileContents,
		UpdateVersion = UpdateVersion,
		UpdateRequiredVersion = UpdateRequiredVersion,
	};
}
