using DevilDaggersInfo.Web.ApiSpec.Admin.Mods;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Client.StateObjects.Admin.Mods;

public class BinaryDataState : IStateObject<BinaryData>
{
	/// <summary>
	/// This name should not contain the type prefix or the mod name.
	/// </summary>
	[StringLength(64, MinimumLength = 1)]
	public required string Name { get; set; }

	public required byte[] Data { get; set; }

	public BinaryData ToModel()
	{
		return new BinaryData
		{
			Name = Name,
			Data = Data,
		};
	}
}
