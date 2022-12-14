using DevilDaggersInfo.Api.Admin.Tools;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Client.StateObjects.Admin.Tools;

public class AddDistributionState : IStateObject<AddDistribution>
{
	public required string Name { get; set; }

	public ToolPublishMethod PublishMethod { get; set; }

	public ToolBuildType BuildType { get; set; }

	public required string Version { get; set; }

	public required byte[] ZipFileContents { get; set; }

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
